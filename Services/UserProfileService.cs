using CloudinaryDotNet.Actions;
using review.Common.Entities;
using review.Common.Helpers;
using review.Common.ReqModels;
using review.Controllers;
using review.Data;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using review.Common.ResModels;
using Microsoft.AspNetCore.Http;
using review.Common.Constatnts;
using System.Reflection;
using review.Common.Extensions;
using review.Common.Exceptions;

namespace review.Services
{
    public interface IUserProfileService
    {
        Task UpdateUserProfile(UserProfileReqModel req);

        Task ChangePassword(ChangePasswordReqModel req);

        Task Follow(string id);

        Task UnFollow(string id);

        Task<object> GetMyFollowCountInfo();

        Task<ProfileEntity> GetProfile();

        Task<PagingResModel> MyFollowerInfo(int page);

        Task<PagingResModel> MyFollowingInfo(int page);
    }

    public class UserProfileService : IUserProfileService
    {
        private readonly DataContext _dataContext;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _userid;

        public UserProfileService(DataContext dataContext, ICloudinaryService cloudinaryService, IHttpContextAccessor httpContextAccessor)
        {
            _dataContext = dataContext;
            _cloudinaryService = cloudinaryService;
            _httpContextAccessor = httpContextAccessor;
            _userid = IdentityExtensions.GetUserId(_httpContextAccessor);
        }

        public async Task UpdateUserProfile(UserProfileReqModel req)
        {
            var userProfile = _dataContext.ProfileEntitys.FirstOrDefault(u => u.AccountID == _userid);
            if (userProfile == null)
            {
                throw new NotFoundException("User không tồn tại!");
            }
            ImageUploadResult image = new ImageUploadResult();
            if (req.Avatar is not null) 
            {
                image = await _cloudinaryService.UploadImage(req.Avatar);
                if(userProfile.Avatar != null)
                {
                    await _cloudinaryService.DeleteImage(userProfile.Avatar);
                }    
            }
            userProfile.Name = req.Name is not null ? req.Name : userProfile.Name;
            userProfile.Phone = req.Phone is not null ? req.Phone : userProfile.Phone;
            userProfile.Gender = req.Gender is not null ? req.Gender : userProfile.Gender;
            userProfile.Avatar = image.PublicId is not null ? image.PublicId : userProfile.Avatar;
            _dataContext.ProfileEntitys.Update(userProfile);
            await _dataContext.SaveChangesAsync();
        }
        public async Task ChangePassword(ChangePasswordReqModel req)
        {
            var userAccount = _dataContext.AccountEntitys.FirstOrDefault(u => u.ID == _userid);
            if(userAccount == null)
            {
                throw new NotFoundException("User không tồn tại!");
            }
            if(!PasswordHelper.VerifyHashedPassword(userAccount.Password, req.OldPassword))
            {
                throw new BadRequestException("Mật khẩu không đúng!");
            }
            if (req.NewPassword == req.OldPassword)
            {
                throw new BadRequestException("Mật khẩu mới không được trùng với mật khẩu cũ!");
            }
            if (req.ConfirmPassword != req.NewPassword)
            {
                throw new BadRequestException("Mật khẩu không khớp!");
            }
            var hash = PasswordHelper.HashPassword(req.NewPassword);
            userAccount.Password = hash;
            _dataContext.AccountEntitys.Update(userAccount);
            await _dataContext.SaveChangesAsync();
        }

        public async Task Follow(string id)
        {
            var followerProfile = _dataContext.ProfileEntitys.FirstOrDefault(u => u.AccountID == _userid);//my profile info
            var followingProfile = _dataContext.ProfileEntitys.FirstOrDefault(u => u.AccountID == id);//other info who i will follow
            if (_userid == id)
            {
                throw new BadRequestException("Bạn không thể follow bản thân!");
            }
                if (followingProfile == null)
            {
                throw new BadRequestException("Người bạn cần follow không tồn tại!");
            }
            var followInfo = _dataContext.ProfileFollowEntitys.FirstOrDefault(f => f.FollowerID == followerProfile.ID && f.FollowingID == followingProfile.ID);
            if (followInfo != null)
            {
                throw new BadRequestException("Bạn đã follow người này!");
            }
            ProfileFollowEntity data = new ProfileFollowEntity()
            {
                ID = Guid.NewGuid().ToString(),
                FollowerID = followerProfile.ID,
                FollowingID = followingProfile.ID
            };
            _dataContext.ProfileFollowEntitys.Add(data);
            await _dataContext.SaveChangesAsync();
        }

        public async Task UnFollow(string id)
        {
            var followerProfile = _dataContext.AccountEntitys.Include(u => u.Profile).FirstOrDefault(u => u.ID == _userid);//my profile info
            var followingProfile = _dataContext.AccountEntitys.Include(u => u.Profile).FirstOrDefault(u => u.ID == id);//other info who i will follow
            if (followingProfile == null || followerProfile == null)
            {
                throw new BadRequestException("User bạn unfollow không tồn tại!");
            }
            var followInfo = _dataContext.ProfileFollowEntitys.FirstOrDefault(f => f.FollowerID == followerProfile.Profile.ID && f.FollowingID == followingProfile.Profile.ID);
            if (followInfo == null)
            {
                throw new NotFoundException("Bạn không theo dõi người này!");
            }
            _dataContext.ProfileFollowEntitys.Remove(followInfo);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<object> GetMyFollowCountInfo()
        {
            var userData = _dataContext.AccountEntitys.Include(a => a.Profile).FirstOrDefault(a => a.ID == _userid);
            var followerData = _dataContext.ProfileFollowEntitys.Where(p => p.FollowingID == userData.Profile.ID).Include(p => p.Follower).ToList();
            var followingData = _dataContext.ProfileFollowEntitys.Where(p => p.FollowerID == userData.Profile.ID).Include(p => p.Following).ToList();

            return new
            {
                followerCount = followerData.Count(),
                followingCount = followingData.Count(),
            };
        }

        public async Task<PagingResModel> MyFollowerInfo(int page)
        {
            var userData = _dataContext.AccountEntitys.Include(a => a.Profile).FirstOrDefault(a => a.ID == _userid);
            var followerData = _dataContext.ProfileFollowEntitys.Where(p => p.FollowingID == userData.Profile.ID).Include(p => p.Follower).ToList();
            var followerList = followerData.Select(f => new
            {
                f.Follower.ID,
                f.Follower.Name,
                f.Follower.Phone,
                f.Follower.Gender,
                f.Follower.Identify,
                f.Follower.AccountID,
            }).Skip((page - 1) * PagingConstant.PageSize).Take(PagingConstant.PageSize);
            return new PagingResModel()
            {
                Data = new { followerList },
                TotalData = followerData.Count(),
                CurrentPage = page,
                TotalPage = Math.Ceiling((decimal)followerData.Count() / PagingConstant.PageSize)

            };
        }

        public async Task<PagingResModel> MyFollowingInfo(int page)
        {
            var userData = _dataContext.AccountEntitys.Include(a => a.Profile).FirstOrDefault(a => a.ID == _userid);
            var followingData = _dataContext.ProfileFollowEntitys.Where(p => p.FollowerID == userData.Profile.ID).Include(p => p.Following).ToList();
            var followingList = followingData.Select(f => new
            {
                f.Following.ID,
                f.Following.Name,
                f.Following.Phone,
                f.Following.Gender,
                f.Following.Identify,
                f.Following.AccountID,
            }).Skip((page - 1) * PagingConstant.PageSize).Take(PagingConstant.PageSize);
            return new PagingResModel()
            {
                Data = new { followingList },
                TotalData = followingData.Count(),
                CurrentPage = page,
                TotalPage = Math.Ceiling((decimal)followingData.Count() / PagingConstant.PageSize)

            };
            
        }

        public async Task<ProfileEntity> GetProfile()
        {
            var account = _dataContext.AccountEntitys.Include(a => a.Profile).FirstOrDefault(u => u.ID == _userid);

            return new ProfileEntity()
            {
                ID = account.Profile.ID,
                Name = account.Profile.Name,
                Phone = account.Profile.Phone,
                Gender = account.Profile.Gender,
                Avatar = account.Profile.Avatar == "" ? "https://res.cloudinary.com/dbey8svpl/image/upload/v1694912519/user_tidw2g.png" : $"https://res.cloudinary.com/dbey8svpl/image/upload/v1696056834/{account.Profile.Avatar}.jpg",
                Identify = account.Profile.Identify,
                AccountID = account.Profile.AccountID,
            };
            
        }
    }
}
