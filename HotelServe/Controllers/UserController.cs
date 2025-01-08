using HotelServe.Models;
using HotelServe.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelServe.Controllers
{
    [ApiController]
    [Route("api/user")]
    [Authorize] // 需要授權
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILoginService _loginService;

        public UserController(IUserService userService, ILoginService loginService)
        {
            _userService = userService;
            _loginService = loginService; // 注入 LoginService
        }

        [HttpGet("profile")]
        public IActionResult GetUserProfile()
        {
            var email = User.Identity.Name; // 從 Token 中取得 Email
            if (string.IsNullOrEmpty(email))
                return Unauthorized(new { success = false, message = "未登入或 Token 無效" });

            var user = _userService.GetUserByEmail(email);
            if (user == null)
                return NotFound(new { success = false, message = "用戶不存在" });

            return Ok(new
            {
                success = true,
                user = new
                {
                    email = user.Email,
                    name = user.FullName,
                    phone = user.Phone,
                    birthday = user.Birth.HasValue
            ? user.Birth.Value.ToString("yyyy-MM-dd")
            : null // 如果為 null，則返回 null
                }
            });
        }

        [HttpPost("change-password")]
        public IActionResult ChangePassword(ChangePasswordRequest request)
        {
            var user = _userService.GetUserByEmail(request.Email);
            if (user == null)
            {
                return NotFound(new { message = "找不到該使用者。" });
            }

            // 驗證舊密碼
            var isValidOldPassword = _loginService.ValidateUser(request.Email, request.OldPassword);
            if (!isValidOldPassword)
            {
                return Unauthorized(new { message = "舊密碼不正確。" });
            }

            // 更新密碼和其他屬性
            var isUpdated = _userService.UpdateUser(
    request.Email,
    new UpdateProfileRequest
    {
        Phone = request.Phone,
        Birth = request.Birth,
        NewPassword = request.NewPassword
    });

            if (!isUpdated)
            {
                return StatusCode(500, new { message = "伺服器錯誤，無法更新使用者資料。" });
            }

            return Ok(new { success = true, message = "資料更新成功！" });
        }
    }

}
