using HotelServe.Models;

namespace HotelServe.Repositories
 


{
	public interface INewsRepository
	{
		Task<IEnumerable<News>> GetAllNewsAsync();

		// 新增方法：獲取最新的新聞（限制筆數）
		Task<IEnumerable<News>> GetLatestNewsAsync(int limit);
	}
}