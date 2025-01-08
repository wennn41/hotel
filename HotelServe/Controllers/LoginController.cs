using HotelServe.Models;
using HotelServe.Services;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration; // 引入命名空間

namespace HotelServe.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // API 路徑: api/Login
    public class LoginController : ControllerBase // 繼承 ControllerBase 用於 API
    {
        private readonly LoginService _loginService;
        private readonly LineService  _lineService;
        
        private readonly IConfiguration _configuration; // 添加 IConfiguration

        public LoginController(LoginService loginService, LineService lineService, IConfiguration configuration)
        {
            _loginService = loginService;
            _lineService = lineService;
            _configuration = configuration; // 注入 IConfiguration
        }

        // 登入 API
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            Console.WriteLine($"[Controller] Email: {request.Email}, Password: {request.Password}");

            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                Console.WriteLine("[Controller] Invalid input data");
                return BadRequest(new { message = "請求資料錯誤：請輸入 Email 和密碼。" });
            }


            // 獲取使用者資料
            var user = _loginService.GetUserByEmail(request.Email); 
            if (user == null)
            {
                Console.WriteLine($"[Controller] User not found: {request.Email}");
                return Unauthorized(new { message = "登入失敗：帳號或密碼錯誤。" });
            }


            // 驗證使用者密碼
            var isValid = _loginService.ValidateUser(request.Email, request.Password);
            if (!isValid)
            {
                Console.WriteLine($"[Controller] Login failed for user: {request.Email}");
                return Unauthorized(new { message = "登入失敗：帳號或密碼錯誤。" });
            }

            // 生成 JWT Token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]); // 從配置中讀取密鑰
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.Name, user.Email), // 設定使用者的 Email
            new Claim(ClaimTypes.Role, user.Role.ToString()) // 設定使用者的角色
        }),
                Expires = DateTime.UtcNow.AddHours(1), // 設定 Token 過期時間
                Issuer = _configuration["Jwt:Issuer"], // 設定 Token 的發行者
                Audience = _configuration["Jwt:Audience"], // 設定 Token 的受眾
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            Console.WriteLine($"[Controller] Login successful for user: {request.Email}");

            // 返回登入成功結果與 JWT Token
            return Ok(new
            {
                success = true,
                role = user.Role,
                token = tokenString // 返回 JWT Token
            }



                  );
        }

        // 註冊 API
        [HttpPost("register")]
        public IActionResult Register([FromBody] LoginUser user)
        {
            if (!ModelState.IsValid)
            {
                // 打印驗證錯誤
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                                .Select(e => e.ErrorMessage)
                                                .ToList();
                Console.WriteLine("驗證錯誤: " + string.Join(", ", errors));
                return BadRequest(new { message = "請求資料驗證失敗", errors });
            }

            user.CreatedAt = DateTime.UtcNow;
            user.Locked = false;
            user.Role = "0";

            var isRegistered = _loginService.RegisterUser(user);
            if (!isRegistered)
            {
                return Conflict(new { message = "註冊失敗：Email 已被使用。" });
            }

            return Ok(new { success = true, message = "註冊成功！" });

        }


        [HttpPost("google-register")]
        public IActionResult GoogleRegister([FromBody] GoogleUserRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.FullName))
            {
                return BadRequest(new { success = false, message = "缺少必要的參數！" });
            }

            // 檢查用戶是否已存在
            var existingUser = _loginService.GetUserByEmail(request.Email);
            if (existingUser != null)
            {
                // 如果已存在，用戶的角色直接返回給前端
                return Ok(new
                {
                    success = true,
                    message = "用戶已經存在，跳轉到對應頁面。",
                    role = existingUser.Role // 返回用戶角色
                });
            }

            // 如果用戶不存在，進行註冊
            var isRegistered = _loginService.RegisterGoogleUser(request.Email, request.FullName);
            if (!isRegistered)
            {
                return Conflict(new { success = false, message = "註冊失敗，請稍後再試。" });
            }

            // 確保回傳資料中包含角色
            return Ok(new { success = true, message = "Google 註冊成功！", role = "0" });
        }


        


        // 定義登入請求的資料模型
        public class LoginRequest
        {
            public string Email { get; set; } // 必須是非空字串
            public string Password { get; set; } // 必須是非空字串
        }

        // 定義 Google 註冊請求的資料模型
        public class GoogleUserRequest
        {
            public string Email { get; set; } // 必須是非空字串
            public string FullName { get; set; } // 必須是非空字串
        }

        
    }
}

