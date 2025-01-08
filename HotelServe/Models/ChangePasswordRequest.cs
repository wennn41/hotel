namespace HotelServe.Models
{
    public class ChangePasswordRequest
    {
        public string Email { get; set; } // 用戶 Email
        public string OldPassword { get; set; } // 舊密碼
        public string NewPassword { get; set; } // 新密碼
        public string Phone { get; set; } // 電話
        public DateTime? Birth { get; set; } // 出生日期
    }
}
