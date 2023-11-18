﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using review.Common.Models;
using review.Common.ReqModels;
using review.Services;
using System.Security.Claims;

namespace review.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]//khai báo controller này không cần đăng nhập 
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        { 
            _authService = authService;//
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromForm] SignUpReqModel req)
        {
            await _authService.SignUp(req);
            return Ok();
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromForm] SignInReqModel req)
        {
            var response = await _authService.SignIn(req);
            return Ok(response);
        }

        [HttpPost("refeshtoken")]
        public async Task<IActionResult> RefeshToken([FromForm] string refeshToken)
        {
            var response = await _authService.RefeshToken(refeshToken);
            return Ok(response);
        }
    }
}
