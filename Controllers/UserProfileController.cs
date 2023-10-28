using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using review.Common.Helpers;
using review.Common.ReqModels;
using review.Services;
using System.Security.Claims;

namespace review.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]//khai báo controller này cần đăng nhập nhưng không cần phân quyền
    public class UserProfileController : ControllerBase
    {
        private readonly IUserProfileService _userProfileService;

        public UserProfileController(IUserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUserProfile([FromForm] UserProfileReqModel req)
        {
            await _userProfileService.UpdateUserProfile(req);
            return Ok();
        }

        [HttpPut("changePassword")]
        public async Task<IActionResult> ChangePassword([FromForm] ChangePasswordReqModel req) 
        {
            await _userProfileService.ChangePassword(req);
            return Ok();
        }
        //all route follow xét theo ID trong tài Account
        [HttpPost("follow/{id}")]
        public async Task<IActionResult> Follow(string id)
        {
            await _userProfileService.Follow(id);
            return Ok();
        }

        [HttpDelete("unFollow/{id}")]
        public async Task<IActionResult> UnFollow(string id)
        {
            await _userProfileService.UnFollow(id);
            return Ok();
        }

        [HttpGet("followCountInfo")]
        public async Task<IActionResult> MyFollowCountInfo()
        {
            var response = await _userProfileService.GetMyFollowCountInfo();
            return Ok(response);
        }

        [HttpGet("followerInfo")]//lay thong tin nguoi dang theo doi minh
        public async Task<IActionResult> MyFollowerInfo(int page = 1)
        {
            var response = await _userProfileService.MyFollowerInfo(page);
            return Ok(response);
        }

        [HttpGet("followingInfo")]//lay thong tin nguoi minh dang theo doi
        public async Task<IActionResult> MyFollowingInfo(int page = 1)
        {
            var response = await _userProfileService.MyFollowingInfo(page);
            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            var response = await _userProfileService.GetProfile();
            return Ok(response);
        }
    }
}
