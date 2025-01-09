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
        public int Role { get; set; }                // 角色
    }

    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public int MemberId { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentStatus { get; set; }
        public string OrderStatus { get; set; }
        public string Notes { get; set; }

        public int OrderId { get; set; }
  
        public string RoomTypeName { get; set; }
    }

    public class OrderDetail
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int RoomTypeId { get; set; }
        public string RoomNumber { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Subtotal { get; set; }
    }
}
