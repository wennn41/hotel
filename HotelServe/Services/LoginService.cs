
using HotelServe.Models;
using HotelServe.Repositories;
using System.Security.Cryptography;
using System.Text;


namespace HotelServe.Services
{
    public class LoginService : ILoginService
    {
        private readonly LoginRepository _loginRepository;

        public LoginService(LoginRepository loginRepository)
        {
            _loginRepository = loginRepository;
        }

        public bool CheckUserExists(string email)
        {
            return _loginRepository.CheckUserExists(email);
        }

        // 根據 Email 獲取使用者資料
        public LoginUser? GetUserByEmail(string email)
        {
            return _loginRepository.GetUserByEmail(email);
        }

        // 生成隨機 Salt
        public string GenerateSalt()
        {
            var saltBytes = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }

        // 使用 Salt 進行密碼哈希
        public string HashPassword(string password, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                var saltedPassword = $"{salt}{password}"; // 將 Salt 與密碼組合
                var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
                return Convert.ToBase64String(hashBytes);
            }
        }


        // 註冊新使用者
        public bool RegisterUser(LoginUser user)
        {
            // 生成 Salt
            user.Salt = GenerateSalt();

            // 使用 Salt 加密密碼
            user.PasswordHash = HashPassword(user.PasswordHash, user.Salt);

            // 保存到資料庫
            return _loginRepository.AddUser(user);
        }

        // 驗證使用者密碼
        public bool ValidateUser(string email, string password)
        {
            var user = _loginRepository.GetUserByEmail(email);
            if (user == null || user.Locked)
            {
                return false; // 使用者不存在或帳戶已鎖定
            }

            // 驗證密碼是否匹配
            var hashedPassword = HashPassword(password, user.Salt);
            return hashedPassword == user.PasswordHash;
        }

        // Google 註冊專用方法
        public bool RegisterGoogleUser(string email, string fullName)
        {
            // 確保 Email 和 FullName 不為空
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(fullName))
            {
                throw new ArgumentException("Google 用戶的 Email 和 FullName 是必填的！");
            }

            // 檢查用戶是否已存在
            if (CheckUserExists(email))
            {
                return false; // 用戶已存在，返回 false 表示不需再次註冊
            }

            // 準備 Google 用戶資料
            var newUser = new LoginUser
            {
                Email = email,
                FullName = fullName,
                Role = "4", // 默認 Google 用戶角色
                CreatedAt = DateTime.UtcNow,
                Locked = false, // Google 用戶默認不鎖定
                PasswordHash = null, // 確保密碼為 null
                Salt = null // 確保 Salt 為 null
            };

            // 新增用戶到資料庫
            return _loginRepository.AddGoogleUser(newUser);
        }
    }
}
