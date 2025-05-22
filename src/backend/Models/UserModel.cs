// Models/UserModels.cs
namespace backend.Models
{
    public class UserInfo
    {
        public string Uid { get; set; }
        public string Email { get; set; }

    }

    public class AuthRequest
    {
        public string Token { get; set; }
    }

    public class AuthResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public UserInfo User { get; set; }
    }
}