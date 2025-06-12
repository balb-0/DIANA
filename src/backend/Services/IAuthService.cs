using backend.Models;

namespace backend.Services
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterUserAsync(RegisterRequest request);
        Task<AuthResponse> LoginUserAsync(LoginRequest request);
        Task<AuthResponse> VerifyTokenAsync(string idToken);
    }
}