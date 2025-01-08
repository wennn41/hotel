namespace HotelServe.Models
{
    public class User
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; } // 用於存儲哈希密碼
        public string Salt { get; set; } // 用於存儲密碼的 Salt 值
        public string Phone { get; set; }
        public DateTime? Birth { get; set; }
        public string FullName { get; set; }

       
    }
}
