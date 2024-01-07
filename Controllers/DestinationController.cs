using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using review.Common.ReqModels;
using review.Services;

namespace review.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DestinationController : ControllerBase
    {
        private readonly IDestinationService _destinationService;

        public DestinationController(IDestinationService destinationService)
        {
            _destinationService = destinationService;
        }
        [Authorize]//những route chỉ cho admin xài thì khai báo ntn 
        [HttpPost]
        public async Task<IActionResult> Add([FromForm] DestinationReqModel req)
        {
            await _destinationService.Add(req);
            return Ok();
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromForm] DestinationReqModel req, string id)
        {
            await _destinationService.Update(req, id);
            return Ok();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _destinationService.Delete(id);
            return Ok();
        }

        //route này danh cho tất cả người dùng xài
        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetSlect(string id)
        {
            var response = await _destinationService.GetById(id);
            return Ok(response);
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _destinationService.GetAll();
            return Ok(response);
        }

        [HttpGet("getForMap")]
        public async Task<IActionResult> GetForMap()
        {
            var response = await _destinationService.GetForMap();
            return Ok(response);
        }

        [HttpGet("getByKeyword")]
        public async Task<IActionResult> GetByKeyword([FromQuery] string keyword)
        {
            var response = await _destinationService.GetByKeyword(keyword);
            return Ok(response);
        }
    }
}
