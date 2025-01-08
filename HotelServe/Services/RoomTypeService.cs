using HotelServe.Repositories;
using HotelServe.Models;

namespace HotelServe.Services

{
	public class RoomTypeService
	{

		private readonly RoomTypeRepository _roomTypeRepository;

		public RoomTypeService(RoomTypeRepository roomTypeRepository)
		{
			_roomTypeRepository = roomTypeRepository;
		}

		public List<RoomType> GetAllRoomTypes()
		{
			return _roomTypeRepository.GetAllRoomTypes();
		}
	}
}
