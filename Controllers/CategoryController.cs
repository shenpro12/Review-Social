using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using review.Common.ReqModels;
using review.Services;

namespace review.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [Authorize(Roles = "Admin")]//những route chỉ cho admin xài thì khai báo ntn 
        [HttpPost]
        public async Task<IActionResult> Add([FromForm] CategoryReqModel req)
        {
            await _categoryService.Add(req);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromForm] CategoryReqModel req, string id)
        {
            await _categoryService.Update(req, id);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _categoryService.Delete(id);
            return Ok();
        }

        //route này danh cho tất cả người dùng xài
        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _categoryService.GetAll();
            return Ok(response);
        }
    }
}
