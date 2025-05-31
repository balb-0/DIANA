using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class RegisterRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;

        public string? DisplayName { get; set; }
    }

    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }

    public class LoginWithTokenRequest
    {
        [Required]
        public string IdToken { get; set; } = string.Empty; // Token vem do cliente Firebase
    }

    public class AuthResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public string? IdToken { get; set; } // Token do Firebase
        public string? RefreshToken { get; set; } // Para renovar o token
        public UserInfo? User { get; set; }
        public int ExpiresIn { get; set; } // Tempo de expiração em segundos

    }

    public class UserInfo
    {
        public string Uid { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? DisplayName { get; set; }
        public bool EmailVerified { get; set; }
    }

    public class VerifyTokenRequest
    {
        [Required]
        public string Token { get; set; } = string.Empty;
    }
}