using review.Common.Entities;
using review.Common.Exceptions;
using review.Common.ReqModels;
using review.Common.ResModels;
using review.Data;

namespace review.Services
{
    public interface IRatingTypeService
    {
        Task Add(RatingTypeReqModel req);

        Task Update(RatingTypeReqModel req, string id);

        Task Delete(string id);

        Task<List<RatingTypeResModel>> GetAll();
    }
    public class RatingTypeService : IRatingTypeService
    {
        private readonly DataContext _dataContext;

        public RatingTypeService (DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task Add(RatingTypeReqModel req)
        {
            var ratingType = _dataContext.RatingTypeEntitys.FirstOrDefault(p => p.Name == req.Name);
            if (ratingType != null)
            {
                throw new DuplicatedException($"Tên rating {req.Name} đã tồn tại!");
            }
            RatingTypeEntity data = new RatingTypeEntity()
            {
                ID = Guid.NewGuid().ToString(),
                Name = req.Name
            };
            _dataContext.RatingTypeEntitys.Add(data);
            await _dataContext.SaveChangesAsync();
               
        }

        public async Task Delete(string id)
        {
            var ratingType = _dataContext.RatingTypeEntitys.FirstOrDefault(p => p.ID == id);
            if (ratingType == null)
            {
                throw new NotFoundException($"ID {id} không tồn tại!");
            }
            _dataContext.RatingTypeEntitys.Remove(ratingType);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<List<RatingTypeResModel>> GetAll()
        {
            var listRatingType = _dataContext.RatingTypeEntitys.Where(l => true);
            var list = new List<RatingTypeResModel>();
            if (listRatingType is not null)
            {
                list = listRatingType.Select(s => new RatingTypeResModel
                {
                    Name = s.Name,
                    ID = s.ID
                }).ToList();
            }

            return list;
        }

        public async Task Update(RatingTypeReqModel req, string id)
        {
            var ratingType = _dataContext.RatingTypeEntitys.FirstOrDefault(p => p.ID == id);
            if (ratingType == null)
            {
                throw new NotFoundException($"ID {id} không tồn tại!");
            }
            var ratingTypeName = _dataContext.RatingTypeEntitys.FirstOrDefault(p => p.Name == req.Name);
            if (ratingTypeName != null)
            {
                throw new DuplicatedException($"Tên {req.Name} đã tồn tại!");
            }
            ratingType.Name = ratingType.Name;
            _dataContext.RatingTypeEntitys.Update(ratingType);
            await _dataContext.SaveChangesAsync();
        }
    }
}
