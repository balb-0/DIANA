using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        /// <summary>
        /// Registra um novo usuário no Firebase
        /// </summary>
        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest request)
        {   
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthResponse
                {
                    Success = false,
                    Message = "Dados inválidos"
                });
            }

            var result = await _authService.RegisterUserAsync(request);

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        /// <summary>
        /// Verifica o ID Token do Firebase (usado como "login")
        /// O cliente deve autenticar no Firebase e enviar o ID Token
        /// </summary>
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthResponse
                {
                    Success = false,
                    Message = "Credenciais inválidas"
                });
            }

            var result = await _authService.LoginUserAsync(request);

            if (result.Success)
                return Ok(result);

            return Unauthorized(result);
        }

        /// <summary>
        /// Verifica se um token é válido
        /// </summary>
        [HttpPost("verify")]
        public async Task<ActionResult<AuthResponse>> VerifyToken([FromBody] VerifyTokenRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthResponse
                {
                    Success = false,
                    Message = "Token é obrigatório"
                });
            }

            var result = await _authService.VerifyTokenAsync(request.Token);

            if (result.Success)
                return Ok(result);

            return Unauthorized(result);
        }
    }
}
