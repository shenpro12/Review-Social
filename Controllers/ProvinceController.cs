using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using review.Common.ReqModels;
using review.Services;

namespace review.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProvinceController : ControllerBase
    {
        private readonly IProvinceService _provinceService;

        public ProvinceController(IProvinceService provinceService)
        {
            _provinceService = provinceService;
        }

        //[Authorize(Roles = "Admin")]//những route chỉ cho admin xài thì khai báo ntn 
        [HttpPost]
        public async Task<IActionResult> Add([FromForm] ProvinceReqModel req)
        {
            await _provinceService.Add(req);
            return Ok();
        }

        //[Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromForm] ProvinceReqModel req, string id)
        {
            await _provinceService.Update(req, id);
            return Ok();
        }

        //[Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _provinceService.Delete(id);
            return Ok();
        }

        //route này danh cho tất cả nguoiqf dùng xài
        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _provinceService.GetAll();
            return Ok(response);
        }
    }
}
