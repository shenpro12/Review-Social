using CloudinaryDotNet.Actions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using review.Common.Entities;
using review.Common.Exceptions;
using review.Common.ReqModels;
using review.Common.ResModels;
using review.Data;

namespace review.Services
{
    public interface IProvinceCategoryService
    {
        Task Add(ProvinceCategoryReqModel req);

        Task Update(ProvinceCategoryReqModel req, string id);

        Task Delete(string id);

        Task<List<ProvinceCategoryResModel>> GetAll();

        Task<ProvinceCategoryGroupResModel> GetByProvince(string provineId);
    }
    public class ProvinceCategoryService : IProvinceCategoryService
    {
        private readonly DataContext _dataContext;
        private readonly ICloudinaryService _cloudinaryService;

        public ProvinceCategoryService(DataContext dataContext, ICloudinaryService cloudinaryService)
        {
            _dataContext = dataContext;
            _cloudinaryService = cloudinaryService;
        }
        public async Task Add(ProvinceCategoryReqModel req)
        {
            //B1: Kiểm tra xem provinceID này có bên Province chưa
            var province = _dataContext.ProvinceEntitys.FirstOrDefault(p => p.ID == req.ProvinceID);
            if(province == null)
            {
                throw new NotFoundException($"Tỉnh ID '{req.ProvinceID}' không tồn tại!");
            }
            //B2: nếu có r xét tới tới Name của ProvinceCategory xem có chưa
            var prCateName = _dataContext.ProvinceCategoryEntitys.FirstOrDefault(p => p.Name == req.Name && province.ID == req.ProvinceID);
            if (prCateName != null)
            {
                throw new DuplicatedException($"Tên category {req.Name} đã tồn tại!");
            }

            ImageUploadResult image = new ImageUploadResult();
            image = await _cloudinaryService.UploadImage(req.Thumb);

            ProvinceCategoryEntity data = new ProvinceCategoryEntity()
            {
                ID = Guid.NewGuid().ToString(),
                Name = req.Name,
                Thumb = image.SecureUrl.OriginalString,
                Province = province,
            };

            _dataContext.ProvinceCategoryEntitys.Add(data);
            await _dataContext.SaveChangesAsync();
        }

        public async Task Delete(string id)
        {
            var provinceCategory = _dataContext.ProvinceCategoryEntitys.FirstOrDefault(p => p.ID == id);
            if (provinceCategory == null)
            {
                throw new NotFoundException($"Category ID '{id}' không tồn tại!");
            }
            if (provinceCategory.Thumb != null)
            {
                await _cloudinaryService.DeleteImage(provinceCategory.Thumb);
            }
            _dataContext.ProvinceCategoryEntitys.Remove(provinceCategory);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<List<ProvinceCategoryResModel>> GetAll()
        {
            var listProvinceCategory = _dataContext.ProvinceCategoryEntitys.Include(s => s.Province).Where(s => true);

            var data = new List<ProvinceCategoryResModel>();

            if(listProvinceCategory is not null)
            {
                data = listProvinceCategory.Select(s => new ProvinceCategoryResModel
                {
                    ID = s.ID,
                    Name = s.Name,
                    Thumb = s.Thumb,
                    ProvinceID = s.ProvinceID,
                }).ToList();
            }
            return data;
        }

        public async Task<ProvinceCategoryGroupResModel> GetByProvince(string provineId)
        {

            var provinceCategory = _dataContext.ProvinceCategoryEntitys.Include(s => s.Province).Where(p => p.ProvinceID == provineId);

            var data = new ProvinceCategoryGroupResModel();

            if (provinceCategory.Count() > 0)
            {
                data = new ProvinceCategoryGroupResModel()
                {
                    Items = provinceCategory.Select(s => new ProvinceCategoryResModel
                    {
                        ID = s.ID,
                        Name = s.Name,
                        Thumb = s.Thumb,
                        ProvinceID = s.ProvinceID,
                    }),
                    CategorySlug = provinceCategory.FirstOrDefault().Province.Slug,
                    CategoryThumb = provinceCategory.FirstOrDefault().Province.CategoryThumb,
                };
            }
            return data;
        }

        public async Task Update(ProvinceCategoryReqModel req, string id)
        {
            //B1: trước tiên bạn cần nhập provinceID muốn update
            var province = _dataContext.ProvinceEntitys.FirstOrDefault(p => p.ID == req.ProvinceID);
            if (province == null)
            {
                throw new NotFoundException($"ID tỉnh '{req.ProvinceID}' không tồn tại!");
            }
            //B2: Có provinceID đúng r thì kiểm tra coi provinceCategoryID có chưa
            var provinceCategory = _dataContext.ProvinceCategoryEntitys.FirstOrDefault(p => p.ID == id);
            if (provinceCategory == null)
            {
                throw new NotFoundException($"Category ID '{id}' không tồn tại!");
            }
                //B3: nếu cũng có r thì kiểm tra cais province nayf ddax cos category nayf chuaw
                var provinceCategoryName = _dataContext.ProvinceCategoryEntitys.FirstOrDefault(p => p.Name == req.Name && p.ProvinceID == req.ProvinceID);
            if (provinceCategoryName != null)
            {
                throw new DuplicatedException($"Tỉnh đã tồn tại category {req.Name}!");
            }

            ImageUploadResult image = new ImageUploadResult();
            if (req.Thumb is not null)
            {
                image = await _cloudinaryService.UploadImage(req.Thumb);
                if (provinceCategory.Thumb != null)
                {
                    await _cloudinaryService.DeleteImage(provinceCategory.Thumb);
                }
            }
            provinceCategory.Name = req.Name;
            provinceCategory.Thumb = image.SecureUrl.OriginalString;
            provinceCategory.ProvinceID = req.ProvinceID;
            _dataContext.ProvinceCategoryEntitys.Update(provinceCategory);
            await _dataContext.SaveChangesAsync();
        }
    }
}
