// Controllers/AuthController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using backend.Services;
using backend.Models;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("verify")]
        public async Task<ActionResult<AuthResponse>> VerifyToken([FromBody] AuthRequest request)
        {
            if (string.IsNullOrEmpty(request.Token))
            {
                return BadRequest(new AuthResponse
                {
                    Success = false,
                    Message = "Token é obrigatório"
                });
            }

            var userRecord = await _authService.VerifyTokenAsync(request.Token);

            if (userRecord == null)
            {
                return Unauthorized(new AuthResponse
                {
                    Success = false,
                    Message = "Token inválido"
                });
            }

            var userInfo = new UserInfo
            {
                Uid = userRecord.Uid,
                Email = userRecord.Email
            };

            return Ok(new AuthResponse
            {
                Success = true,
                Message = "Token válido",
                User = userInfo
            });
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<ActionResult<UserInfo>> GetCurrentUser()
        {
            var authHeader = Request.Headers["Authorization"].FirstOrDefault();
            if (authHeader == null || !authHeader.StartsWith("Bearer "))
            {
                return Unauthorized();
            }

            var token = authHeader.Substring("Bearer ".Length).Trim();
            var uid = await _authService.GetUserIdFromTokenAsync(token);

            if (uid == null)
            {
                return Unauthorized();
            }

            var userInfo = await _authService.GetUserInfoAsync(uid);
            if (userInfo == null)
            {
                return NotFound();
            }

            return Ok(userInfo);
        }
    }
}