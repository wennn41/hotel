using HotelServe.Models;
using System.Data;

namespace HotelServe.Repositories
{
	public class RoomTypeRepository : BaseRepository<RoomType>
	{
		public RoomTypeRepository(IDbConnection dbConnection) : base(dbConnection)
		{
		}

		public List<RoomType> GetAllRoomTypes()
		{
			// 使用通用的 GetAll 方法
			var query = "SELECT Id, Name, PricePerNight, Capacity, Description, Stock, image FROM RoomType";
			return GetAll(query).ToList();
		}
	}
}
