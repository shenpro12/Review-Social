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

        public ProvinceService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task Add(ProvinceReqModel req)
        {
            var province = _dataContext.ProvinceEntitys.FirstOrDefault(p => p.Name == req.Name);
            if (province != null)
            {
                throw new DuplicatedException($"Tỉnh thành {req.Name} đã tồn tại");
            }
            ProvinceEntity provinceEntity = new ProvinceEntity()
            {
                ID = Guid.NewGuid().ToString(),
                Name = req.Name
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
            province.Name = province.Name;
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
