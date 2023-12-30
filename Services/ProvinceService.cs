using CloudinaryDotNet.Actions;
using review.Common.Entities;
using review.Common.Exceptions;
using review.Common.ReqModels;
using review.Common.ResModels;
using review.Data;

namespace review.Services
{
    public interface IProvinceService
    {
        Task Add(ProvinceReqModel req);

        Task Update(ProvinceReqModel req, string id);

        Task Delete(string id);

        Task<List<ProvinceResModel>> GetAll();

    }
    public class ProvinceService : IProvinceService
    {
        private readonly DataContext _dataContext;
        private readonly ICloudinaryService _cloudinaryService;

        public ProvinceService(DataContext dataContext, ICloudinaryService cloudinaryService)
        {
            _dataContext = dataContext;
            _cloudinaryService = cloudinaryService;

        }
        public async Task Add(ProvinceReqModel req)
        {
            var province = _dataContext.ProvinceEntitys.FirstOrDefault(p => p.Name == req.Name);
            if (province != null)
            {
                throw new DuplicatedException($"Tỉnh thành {req.Name} đã tồn tại");
            }

            ImageUploadResult image = new ImageUploadResult();
            image = await _cloudinaryService.UploadImage(req.CategoryThumb);

            ProvinceEntity provinceEntity = new ProvinceEntity()
            {
                ID = Guid.NewGuid().ToString(),
                Name = req.Name,
                Slug = req.Slug,
                CategoryThumb = image.SecureUrl.OriginalString
            };
            _dataContext.ProvinceEntitys.Add(provinceEntity);
            await _dataContext.SaveChangesAsync();  
        }

        public async Task Delete(string id)
        {
            var province = _dataContext.ProvinceEntitys.FirstOrDefault(p => p.ID == id);
            if (province == null)
            {
                throw new NotFoundException($"Tỉnh thành ID {id} không tồn tại!");
            }
            if (province.CategoryThumb != null)
            {
                await _cloudinaryService.DeleteImage(province.CategoryThumb);
            }
            _dataContext.ProvinceEntitys.Remove(province);
            await _dataContext.SaveChangesAsync();
        }

        public async Task Update(ProvinceReqModel req, string id)
        {
            var province = _dataContext.ProvinceEntitys.FirstOrDefault(p => p.ID == id);
            if (province == null)
            {
                throw new NotFoundException($"Tỉnh thành ID {id} khoogn tồn tại!");
            }
            var provinceName = _dataContext.ProfileEntitys.FirstOrDefault(p => p.Name == req.Name);
            if (provinceName != null)
            {
                throw new DuplicatedException($"Tên tỉnh {req.Name} đã tồn tại!");
            }
            ImageUploadResult image = new ImageUploadResult();
            if (req.CategoryThumb is not null)
            {
                image = await _cloudinaryService.UploadImage(req.CategoryThumb);
                if (province.CategoryThumb != null)
                {
                    await _cloudinaryService.DeleteImage(province.CategoryThumb);
                }
            }
            province.Name = province.Name;
            province.CategoryThumb = image.SecureUrl.OriginalString;
            province.Slug = req.Slug;
            _dataContext.ProvinceEntitys.Update(province);
            await _dataContext.SaveChangesAsync();
        }
        public async Task<List<ProvinceResModel>> GetAll()
        {
            var listprovince = _dataContext.ProvinceEntitys.Where(l => true);

            var list = new List<ProvinceResModel>();
            if(listprovince is not null)
            {
                list = listprovince.Select(s => new ProvinceResModel
                {
                    Name = s.Name,
                    ID = s.ID
                }).ToList();
            }    
            return list;
        }
    }
}
