using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using HotelServe.Models;
using HotelServe.Repositories;

namespace HotelServe.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewsController : Controller
    {
        private readonly INewsRepository _newsRepository;

        public NewsController(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        // GET: api/news
        [HttpGet]
        public async Task<ActionResult<IEnumerable<News>>> GetAllNews()
        {
            var news = await _newsRepository.GetAllNewsAsync();
            return Ok(news);
        }

		// 新增 GET: api/news/latest
		[HttpGet("latest")]
		public async Task<ActionResult<IEnumerable<object>>> GetLatestNews([FromQuery] int limit = 6)
		{
			var news = await _newsRepository.GetLatestNewsAsync(limit);

			// 格式化返回資料
			var result = news.Select(n => new
			{
				n.Title,
				n.Category,
				Starttime = n.Starttime.ToString("yyyy-MM-dd"), // 格式化日期
				Endtime = n.Endtime.ToString("yyyy-MM-dd"),
				n.Image
			});

			return Ok(result);
		}
	}

}
