using Microsoft.AspNetCore.Mvc;
using HotelServe.Services;

namespace HotelServe.Controllers
{

	[ApiController]
	[Route("api/[controller]")]
	public class RoomTypeController : ControllerBase
	{
		private readonly RoomTypeService _roomTypeService;

		public RoomTypeController(RoomTypeService roomTypeService)
		{
			_roomTypeService = roomTypeService;
		}

		[HttpGet]
		public IActionResult GetAllRoomTypes()
		{
			try
			{
				var roomTypes = _roomTypeService.GetAllRoomTypes();
				return Ok(roomTypes);
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { Message = "An error occurred while retrieving room types.", Details = ex.Message });
			}
		}
	}
}
