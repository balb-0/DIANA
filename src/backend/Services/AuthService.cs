// Services/AuthService.cs
using FirebaseAdmin.Auth;
using backend.Models;

namespace backend.Services
{
    public interface IAuthService
    {
        Task<UserRecord> VerifyTokenAsync(string token);
        Task<string> GetUserIdFromTokenAsync(string token);
        Task<UserInfo> GetUserInfoAsync(string uid);
    }

    public class AuthService : IAuthService
    {
        public async Task<UserRecord> VerifyTokenAsync(string token)
        {
            try
            {
                var decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(token);
                var userRecord = await FirebaseAuth.DefaultInstance.GetUserAsync(decodedToken.Uid);
                return userRecord;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<string> GetUserIdFromTokenAsync(string token)
        {
            try
            {
                var decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(token);
                return decodedToken.Uid;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<UserInfo> GetUserInfoAsync(string uid)
        {
            try
            {
                var userRecord = await FirebaseAuth.DefaultInstance.GetUserAsync(uid);
                return new UserInfo
                {
                    Uid = userRecord.Uid,
                    Email = userRecord.Email,
                };
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}