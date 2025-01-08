namespace HotelServe.Models
{
    public class UpdateProfileRequest
    {
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public DateTime? Birth { get; set; }
        public string? NewPassword { get; set; } // 新增 NewPassword 屬性
    }
}
