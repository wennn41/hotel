namespace HotelServe.Models
{
    public class Member
    {
        public int Id { get; set; }
        public string FullName { get; set; }          // 會員姓名
        public string Email { get; set; }         // 電子郵件
        public string Countries { get; set; }     // 國家
        public string Phone { get; set; }         // 手機號碼
        public DateTime? Birth { get; set; }      // 出生日期
        public DateTime CreatedAt { get; set; }   // 建立日期
        public bool Locked { get; set; }          // 是否鎖定
    }
}
