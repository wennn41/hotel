namespace HotelServe.Models
{
	public class LoginUser
	{
        public int Id { get; set; } // 唯一標識符
        public string Email { get; set; } // 信箱
        public string PasswordHash { get; set; } // 密碼哈希值
        public string Salt { get; set; } = ""; // 提供預設空字串
        public string FullName { get; set; } // 姓名
        public string Countries { get; set; } // 國籍
        public string Phone { get; set; } // 手機號碼
        public DateTime? Birth { get; set; } // 可空型別
        public string Role { get; set; } = "User"; // 默認角色
        public DateTime CreatedAt { get; set; } = DateTime.Now; // 註冊時間
        public bool Locked { get; set; } = false; // 帳號是否被鎖定
    }
}
