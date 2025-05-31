using backend.Services;

namespace backend.Middleware
{
    public class FirebaseAuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<FirebaseAuthMiddleware> _logger;

        public FirebaseAuthMiddleware(RequestDelegate next, ILogger<FirebaseAuthMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, IAuthService authService)
        {
            // Verificar se a rota requer autenticação
            var endpoint = context.GetEndpoint();
            var requiresAuth = endpoint?.Metadata?.GetMetadata<Microsoft.AspNetCore.Authorization.AuthorizeAttribute>() != null;

            if (requiresAuth)
            {
                var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();

                if (authHeader != null && authHeader.StartsWith("Bearer "))
                {
                    var token = authHeader.Substring("Bearer ".Length).Trim();
                    var result = await authService.VerifyTokenAsync(token);

                    if (result.Success && result.User != null)
                    {
                        // Adicionar informações do usuário ao contexto
                        context.Items["User"] = result.User;
                        context.Items["UserId"] = result.User.Uid;
                    }
                    else
                    {
                        context.Response.StatusCode = 401;
                        await context.Response.WriteAsync("Token inválido ou expirado");
                        return;
                    }
                }
                else
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Token de autorização necessário");
                    return;
                }
            }

            await _next(context);
        }
    }
}