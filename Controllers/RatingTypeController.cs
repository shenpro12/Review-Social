using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using review.Common.ReqModels;
using review.Services;

namespace review.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingTypeController : ControllerBase
    {
        private readonly IRatingTypeService _ratingTypeService;

        public RatingTypeController(IRatingTypeService ratingTypeService)
        {
            _ratingTypeService = ratingTypeService;
        }

        [Authorize(Roles = "Admin")]//những route chỉ cho admin xài thì khai báo ntn 
        [HttpPost]
        public async Task<IActionResult> Add([FromForm] RatingTypeReqModel req)
        {
            await _ratingTypeService.Add(req);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromForm] RatingTypeReqModel req, string id)
        {
            await _ratingTypeService.Update(req, id);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _ratingTypeService.Delete(id);
            return Ok();
        }

        //route này danh cho tất cả nguoiqf dùng xài
        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _ratingTypeService.GetAll();
            return Ok(response);
        }
    }
}
