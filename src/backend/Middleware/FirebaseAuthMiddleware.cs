// Middleware/FirebaseAuthMiddleware.cs
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace backend.Middleware
{
    public class FirebaseAuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<FirebaseAuthMiddleware> _logger;
        private const string FirebaseProjectId = "diana-446a1";

        public FirebaseAuthMiddleware(RequestDelegate next, ILogger<FirebaseAuthMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();

            if (authHeader != null && authHeader.StartsWith("Bearer "))
            {
                var token = authHeader.Substring("Bearer ".Length).Trim();

                try
                {
                    var handler = new JwtSecurityTokenHandler();
                    var jsonToken = handler.ReadJwtToken(token);

                    // Validações básicas
                    if (jsonToken.Issuer == $"https://securetoken.google.com/{FirebaseProjectId}" &&
                        jsonToken.Audiences.Contains(FirebaseProjectId) &&
                        jsonToken.ValidTo > DateTime.UtcNow)
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier, GetClaimValue(jsonToken, "user_id") ?? GetClaimValue(jsonToken, "sub") ?? ""),
                            new Claim(ClaimTypes.Email, GetClaimValue(jsonToken, "email") ?? ""),
                            new Claim("firebase_uid", GetClaimValue(jsonToken, "user_id") ?? GetClaimValue(jsonToken, "sub") ?? "")
                        };

                        var identity = new ClaimsIdentity(claims, "Firebase");
                        context.User = new ClaimsPrincipal(identity);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Erro ao processar token Firebase");
                    // Token inválido - continua sem autenticação
                }
            }

            await _next(context);
        }

        private string? GetClaimValue(JwtSecurityToken token, string claimType)
        {
            return token.Claims.FirstOrDefault(c => c.Type == claimType)?.Value;
        }
    }

    public static class FirebaseAuthMiddlewareExtensions
    {
        public static IApplicationBuilder UseFirebaseAuth(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<FirebaseAuthMiddleware>();
        }
    }
}