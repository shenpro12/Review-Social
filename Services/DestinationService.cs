using CloudinaryDotNet.Actions;
using Microsoft.EntityFrameworkCore;
using review.Common.Constatnts;
using review.Common.Entities;
using review.Common.Exceptions;
using review.Common.Extensions;
using review.Common.ReqModels;
using review.Common.ResModels;
using review.Data;

namespace review.Services
{
    public interface IDestinationService
    {
        Task Add(DestinationReqModel req);

        Task Update(DestinationReqModel req, string id);

        Task Delete(string id);

        Task<List<DestinationResModel>> GetForMap();
        Task<List<DestinationResModel>> GetByKeyword(string keyword);

        Task<List<DestinationResModel>> GetAll();

        Task<DestinationResModel> GetById(string id);
    }
    public class DestinationService : IDestinationService
    {
        private readonly DataContext _dataContext;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _userID;
        private readonly bool _isAdmin;

        public DestinationService(DataContext dataContext, ICloudinaryService cloudinaryService, IHttpContextAccessor httpContextAccessor)
        {
            _dataContext = dataContext;
            _cloudinaryService = cloudinaryService;
            _httpContextAccessor = httpContextAccessor;
            _userID = IdentityExtensions.GetUserId(_httpContextAccessor);
            _isAdmin = IdentityExtensions.IsAdmin(_httpContextAccessor);
        }

        public async Task Add(DestinationReqModel req)
        {
            var province = _dataContext.ProvinceEntitys.FirstOrDefault(p => p.ID == req.ProvinceID);
            if (province == null)
            {
                throw new NotFoundException($"Tỉnh thành ID '{req.ProvinceID}' không tồn tại");
            }
            var checkName = _dataContext.DestinationEntitys.FirstOrDefault(n => n.Name == req.Name);
            if (checkName != null)
            {
                throw new DuplicatedException($"Tên địa điểm {req.Name} đã tồn tại!");
            }

            ImageUploadResult image = new ImageUploadResult();
            image = await _cloudinaryService.UploadImage(req.Thumb);
            DestinationEntity data = new DestinationEntity()
            {
                ID = Guid.NewGuid().ToString(),
                Name = req.Name,
                Address = req.Address,
                Phone = req.Phone,
                MinPrice = req.MinPrice,
                MaxPrice = req.MaxPrice,
                Open = req.Open,
                Closed = req.Closed,
                Thumb = image.SecureUrl.OriginalString,
                Lat = req.Lat,
                Long = req.Long,
                UserID = _userID,
                ProvinceID = province.ID,
                IsAdmin = _isAdmin ? 1 : 0
            };
            _dataContext.DestinationEntitys.Add(data);
            await _dataContext.SaveChangesAsync();
        }

        public async Task Delete(string id)
        {
            var destination = _dataContext.DestinationEntitys.FirstOrDefault(g => (g.ID == id && ((g.UserID == _userID) || _isAdmin)));
            if(destination == null)
            {
                throw new NotFoundException($"Địa điểm '{id}' không tồn tại!");
            }
            if (destination.Thumb != null)
            {
                await _cloudinaryService.DeleteImage(destination.Thumb);
            }
            _dataContext.DestinationEntitys.Remove(destination);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<List<DestinationResModel>> GetForMap()
        {
            var listDestination = _dataContext.DestinationEntitys.Where(l => l.IsAdmin == 1);
            var data = new List<DestinationResModel>();
            if (listDestination is not null) {
                data = listDestination.Select(s => new DestinationResModel
                {
                    ID = s.ID,
                    Name = s.Name,
                    Address = s.Address,
                    Phone = s.Phone,
                    MinPrice = s.MinPrice,
                    MaxPrice = s.MaxPrice,
                    Open = s.Open,
                    Closed = s.Closed,
                    Thumb = s.Thumb,
                    Lat = s.Lat,
                    Long = s.Long,
                    IsAdmin = s.IsAdmin,
                    ProvinceID = s.ProvinceID,
                    UserID = s.UserID,
                }).ToList();
            }
            return data;
        }

        public async Task<List<DestinationResModel>> GetAll()
        {
            var listDestination = _dataContext.DestinationEntitys.Where(l => true);
            var data = new List<DestinationResModel>();
            if (listDestination is not null)
            {
                data = listDestination.Select(s => new DestinationResModel
                {
                    ID = s.ID,
                    Name = s.Name,
                    Address = s.Address,
                    Phone = s.Phone,
                    MinPrice = s.MinPrice,
                    MaxPrice = s.MaxPrice,
                    Open = s.Open,
                    Closed = s.Closed,
                    Thumb = s.Thumb,
                    Lat = s.Lat,
                    Long = s.Long,
                    IsAdmin = s.IsAdmin,
                    ProvinceID = s.ProvinceID,
                    UserID = s.UserID,
                }).ToList();
            }
            return data;
        }

        public async Task<DestinationResModel> GetById(string id)
        {
            var data = _dataContext.DestinationEntitys.FirstOrDefault(g => (g.ID == id && g.UserID == _userID) || _isAdmin);
            if (data == null)
            {
                throw new NotFoundException($"Địa điểm ID '{id}' không tồn tại!");
            }
            return new DestinationResModel
            {
                ID = data.ID,
                Name = data.Name,
                Address = data.Address,
                Phone = data.Phone,
                MinPrice = data.MinPrice,
                MaxPrice = data.MaxPrice,
                Open = data.Open,
                Closed = data.Closed,
                Thumb = data.Thumb,
                Lat = data.Lat,
                Long = data.Long,
                IsAdmin = data.IsAdmin,
                ProvinceID = data.ProvinceID,
                UserID = data.UserID,
            };
        }

        public async Task Update(DestinationReqModel req, string id)
        {
            var data = _dataContext.DestinationEntitys.FirstOrDefault(g => (g.ID == id && g.UserID == _userID) || _isAdmin);
            if (data == null)
            {
                throw new NotFoundException("Địa điểm ID {id} không tồn tại!");
            }

            var nameDestination = _dataContext.DestinationEntitys.FirstOrDefault(n => n.Name == req.Name);
            if (nameDestination != null)
            {
                throw new DuplicatedException($"Tên {req.Name} đã tồn tại!");
            }

            ImageUploadResult image = new ImageUploadResult();
            if (req.Thumb is not null)
            {
                image = await _cloudinaryService.UploadImage(req.Thumb);
                if (data.Thumb != null)
                {
                    await _cloudinaryService.DeleteImage(data.Thumb);
                }
            }

            data.Name = req.Name is not null ? req.Name : data.Name;
            data.ProvinceID = req.ProvinceID is not null ? req.ProvinceID : data.ProvinceID;
            data.Address = req.Address is not null ? req.Address : data.Address;
            data.Phone = req.Phone is not null ? req.Phone : data.Phone;
            data.MinPrice = req.MinPrice is not null ? req.MinPrice : data.MinPrice;
            data.MaxPrice = req.MaxPrice is not null ? req.MaxPrice : data.MaxPrice;
            data.Open = req.Open is not null ? req.Open : data.Open;
            data.Closed = req.Closed is not null ? req.Closed : data.Closed;
            data.Thumb = image.SecureUrl.OriginalString != null ? image.SecureUrl.OriginalString : data.Thumb;
            data.Lat = req.Lat is not null ? req.Lat : data.Lat;
            data.Long = req.Long is not null ? req.Long : data.Long;

            _dataContext.DestinationEntitys.Update(data);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<List<DestinationResModel>> GetByKeyword(string keyword)
        {
            var listDestination = _dataContext.DestinationEntitys.Where(l => l.Name.ToLower().Contains(keyword.ToLower()));
            var data = new List<DestinationResModel>();
            if (listDestination is not null)
            {
                data = listDestination.Select(s => new DestinationResModel
                {
                    ID = s.ID,
                    Name = s.Name,
                    Address = s.Address,
                    Phone = s.Phone,
                    MinPrice = s.MinPrice,
                    MaxPrice = s.MaxPrice,
                    Open = s.Open,
                    Closed = s.Closed,
                    Thumb = s.Thumb,
                    Lat = s.Lat,
                    Long = s.Long,
                    IsAdmin = s.IsAdmin,
                    ProvinceID = s.ProvinceID,
                    UserID = s.UserID,
                }).ToList();
            }
            return data;
        }
    }
    
}
