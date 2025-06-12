// Crie este arquivo: Middleware/FirebaseAuthHandler.cs
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using backend.Services;

namespace backend.Middleware
{
    public class FirebaseAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IAuthService _authService;

        public FirebaseAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IAuthService authService)
            : base(options, logger, encoder, clock)
        {
            _authService = authService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var authHeader = Request.Headers["Authorization"].FirstOrDefault();

            if (authHeader == null || !authHeader.StartsWith("Bearer "))
            {
                return AuthenticateResult.Fail("Token de autorização necessário");
            }

            var token = authHeader.Substring("Bearer ".Length).Trim();
            var result = await _authService.VerifyTokenAsync(token);

            if (!result.Success || result.User == null)
            {
                return AuthenticateResult.Fail("Token inválido ou expirado");
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, result.User.Uid),
                new Claim(ClaimTypes.Name, result.User.DisplayName ?? ""),
                new Claim(ClaimTypes.Email, result.User.Email ?? "")
            };

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            // Adicionar informações ao contexto para compatibilidade com middleware existente
            Context.Items["User"] = result.User;
            Context.Items["UserId"] = result.User.Uid;

            return AuthenticateResult.Success(ticket);
        }
    }
}