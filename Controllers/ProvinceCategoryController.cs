using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using review.Common.ReqModels;
using review.Services;

namespace review.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProvinceCategoryController : ControllerBase
    {
        private readonly IProvinceCategoryService _provinceCategoryService;

        public ProvinceCategoryController(IProvinceCategoryService provinceCategoryService )
        {
            _provinceCategoryService = provinceCategoryService;
        }
        //[Authorize(Roles = "Admin")]//những route chỉ cho admin xài thì khai báo ntn 
        [HttpPost]
        public async Task<IActionResult> Add([FromForm] ProvinceCategoryReqModel req)
        {
            await _provinceCategoryService.Add(req);
            return Ok();
        }

        //[Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromForm] ProvinceCategoryReqModel req, string id)
        {
            await _provinceCategoryService.Update(req, id);
            return Ok();
        }

        //[Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _provinceCategoryService.Delete(id);
            return Ok();
        }

        //route này danh cho tất cả nguoiqf dùng xài
        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _provinceCategoryService.GetAll();
            return Ok(response);
        }

        [HttpGet("getByProvince/{id}")]
        public async Task<IActionResult> GetByProvince(string id)
        {
            var response = await _provinceCategoryService.GetByProvince(id);
            return Ok(response);
        }
    }
}
