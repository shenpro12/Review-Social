using review.Common.Entities;
using review.Common.Exceptions;
using review.Common.ReqModels;
using review.Common.ResModels;
using review.Data;

namespace review.Services
{
    public interface ICategoryService
    {
        Task Add(CategoryReqModel req);

        Task Update(CategoryReqModel req, string id);

        Task Delete(string id);

        Task<List<CategoryResModel>> GetAll();

    }   
    public class CategoryService : ICategoryService
    {
        private readonly DataContext _dataContext;

        public CategoryService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task Add(CategoryReqModel req)
        {
            var category = _dataContext.CategoryEntitys.FirstOrDefault(p => p.Name == req.Name);
            if (category != null)
            {
                throw new DuplicatedException($"Tên Category {req.Name} đã tòn tại!");
            }
            CategoryEntity cate = new CategoryEntity()
            {
                ID = Guid.NewGuid().ToString(),
                Name = req.Name,
                Slug = req.Slug,
            };
            _dataContext.CategoryEntitys.Add(cate);
            await _dataContext.SaveChangesAsync();
        }

        public async Task Delete(string id)
        {
            var category = _dataContext.CategoryEntitys.FirstOrDefault(p => p.ID == id);
            if (category == null)
            {
                throw new NotFoundException("This Category is not exits!");
            }
            _dataContext.CategoryEntitys.Remove(category);
            await _dataContext.SaveChangesAsync();
                
        }

        public async Task<List<CategoryResModel>> GetAll()
        {
            var listCategory = _dataContext.CategoryEntitys.Select(l => new CategoryResModel()
            {
                Name = l.Name,
                ID = l.ID,
                Slug = l.Slug
            });

            var list = new List<CategoryResModel>();

            if (listCategory is not null)
            {
                list = listCategory.ToList();
            }

            return list;
        }

        public async Task Update(CategoryReqModel req, string id)
        {
            var category = _dataContext.CategoryEntitys.FirstOrDefault(p => p.ID == id);

            if (category == null)
            {
                throw new NotFoundException($"Category {id} không tồn tại!");
            }

            var categoryName = _dataContext.CategoryEntitys.FirstOrDefault(p => p.Name == req.Name);
            if (categoryName != null)
            {
                throw new DuplicatedException($"Tên {req.Name} đã tồn tại!");
            }
            category.Name = req.Name is not null ? req.Name : category.Name;
            category.Slug = req.Slug is not null ? req.Slug : category.Slug;
            _dataContext.CategoryEntitys.Update(category);
            await _dataContext.SaveChangesAsync();
        }
    }
}
