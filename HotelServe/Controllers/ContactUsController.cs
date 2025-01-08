using HotelServe.Models;
using HotelServe.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotelServe.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactUsController : ControllerBase
    {
        private readonly IContactUsService _contactUsService;
        public ContactUsController(IContactUsService contactUsService)
        {
            _contactUsService = contactUsService;
        }

        [HttpPost]
        public IActionResult SubmitContactForm([FromBody] ContactUs contact)
        {
            if (contact == null) {
                return BadRequest("無效的表單資料");
            }
            _contactUsService.SubmitContactForm(contact);
            return Ok("表單提交成功");
        }

    }
}
