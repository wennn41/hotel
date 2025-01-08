namespace HotelServe.Models
{
	public class RoomType
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public decimal PricePerNight { get; set; }
		public int Capacity { get; set; }
		public string Description { get; set; }
		public int Stock { get; set; }
		public string Image { get; set; }
	}
}
