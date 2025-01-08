using HotelServe.Models;
using System.Text;
using System.Security.Cryptography;
using Dapper;
using HotelServe.Services; // 確保引入了 LoginService 的命名空間

namespace HotelServe.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILoginService _loginService; // 新增 LoginService

        public UserService(IUserRepository userRepository, ILoginService loginService)
        {
            _userRepository = userRepository;
            _loginService = loginService; // 注入 LoginService
        }
   

        // 依據 Email 獲取用戶
        public User GetUserByEmail(string email)
        {
            return _userRepository.GetUserByEmail(email);
        }


        public bool UpdateUser(string email, UpdateProfileRequest request)
        {
            var user = _userRepository.GetUserByEmail(email);
            if (user == null) return false;

            // 更新密碼（如果提供了新密碼）
            if (!string.IsNullOrEmpty(request.NewPassword))
            {
                user.Salt = _loginService.GenerateSalt(); // 使用 LoginService 的 GenerateSalt 方法
                user.PasswordHash = _loginService.HashPassword(request.NewPassword, user.Salt); // Hash 新密碼
            }

            // 更新其他屬性
            user.FullName = request.Name ?? user.FullName;
            user.Phone = request.Phone ?? user.Phone;
            user.Birth = request.Birth ?? user.Birth;

            return _userRepository.UpdateUser(user); // 更新到資料庫
        }

        public bool ValidateUser(string email, string password)
        {
            var user = GetUserByEmail(email);
            if (user == null) return false;

            var encryptedPassword = EncryptPassword(password);
            return user.PasswordHash == encryptedPassword;
        }

        public string EncryptPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }



    }
}
