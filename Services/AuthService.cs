using Microsoft.Extensions.Options;
using review.Common;
using review.Common.Entities;
using review.Common.Helpers;
using review.Common.Models;
using review.Common.ReqModels;
using review.Data;
using System;
using System.Security.Principal;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using review.Common.ResModels;
using Microsoft.AspNetCore.Http;
using review.Common.Constatnts;
using review.Common.Exceptions;

namespace review.Services
{
    public interface IAuthService
    {
        Task SignUp(SignUpReqModel req);
        Task<TokenModel> SignIn(SignInReqModel req);

        Task<TokenModel> RefeshToken(string refeshToken);
    }
    public class AuthService : IAuthService
    {
        private readonly DataContext _dataContext;
        private readonly JwtTokensOptionsModel _jwtTokensOptions;
        public AuthService(DataContext dataContext, IOptions<JwtTokensOptionsModel> jwtTokensOptions) 
        {
            _dataContext = dataContext;
            _jwtTokensOptions = jwtTokensOptions.Value;
        }

        public async Task<TokenModel> RefeshToken(string refeshToken)
        {
            var token = _dataContext.RefeshTokenEntitys.Include(tk => tk.Account).FirstOrDefault(tk => tk.RefeshToken == refeshToken);
            if (token == null || DateTime.Compare(token.ExpiredAt, DateTime.Now) < 0)//nếu không tồn tại rftoken hoặc rftoken hết hạn
            {
                throw new BadRequestException("Hết phiên đăng nhập! Hãy đăng nhập lại!");
            }
            //Tạo accesstoken mới
            TokenModel newToken = new JwtTokensHelper(_jwtTokensOptions).GenerateJSONWebToken(token.Account, false);

            return newToken;
        }

        public async Task<TokenModel> SignIn(SignInReqModel req)
        {
            var user = _dataContext.AccountEntitys.FirstOrDefault(u => u.UserName == req.UserName || u.Email == req.UserName);
            if (user == null)
            {
                throw new NotFoundException("UserName không tồn tại!");
            }

            if (!PasswordHelper.VerifyHashedPassword(user.Password, req.Password))//giải mã mật khẩu 
            {
                throw new BadRequestException("Mật khẩu không đúng!");
            }
            //Tạo token
            TokenModel token = new JwtTokensHelper(_jwtTokensOptions).GenerateJSONWebToken(user);
            //Lưu refesh token
            var refeshToken = new RefeshTokenEntity()
            {
                ID = Guid.NewGuid().ToString(),
                RefeshToken = token.RefeshToken,
                ExpiredAt = DateTime.Now.AddDays(30),//ngày hiện tại cộng thêm 30 ngày
                Account = user//tự động lưu account id khi lưu rftoken
            };
            _dataContext.RefeshTokenEntitys.Add(refeshToken);
            await _dataContext.SaveChangesAsync();

            return token;
        }

        public async Task SignUp(SignUpReqModel req)
        {
            var user = _dataContext.AccountEntitys.FirstOrDefault(u => u.UserName == req.UserName || u.Email == req.Email);
            if (user != null) 
            {
                throw new DuplicatedException($"UserName {req.UserName} đã tồn tại!");
            }
            AccountEntity account = new AccountEntity()
            {
                ID = Guid.NewGuid().ToString(),
                UserName = req.UserName,
                IsAdmin = RolesConstant.Admin,
                Password = PasswordHelper.HashPassword(req.Password),//mã hóa mật khẩu 
                Email = req.Email,
                Profile = new ProfileEntity() //Tạo profile mặc định
                {
                    ID = Guid.NewGuid().ToString(),
                    Name = req.UserName,
                    Phone = null,
                    Gender = null,
                    Avatar = "",
                    Identify = $"@{req.UserName}.{DateTime.Now.Month}{DateTime.Now.Year}"
                }
            };
            _dataContext.AccountEntitys.Add(account);
            await _dataContext.SaveChangesAsync();
        }
    }
}
