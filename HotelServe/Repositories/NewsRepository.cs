using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using HotelServe.Models;
using System.Data;

namespace HotelServe.Repositories
{
    public class NewsRepository : BaseRepository<News>, INewsRepository
    {
        public NewsRepository(IDbConnection dbConnection) : base(dbConnection)
        {
        }

        public async Task<IEnumerable<News>> GetAllNewsAsync()
        {
            var query = "SELECT * FROM News";
            return await Task.Run(() => GetAll(query)); // 使用 BaseRepository 的 GetAll 方法
        }

		// 實現 GetLatestNewsAsync 方法
		public async Task<IEnumerable<News>> GetLatestNewsAsync(int limit)
		{
			var query = @"
        SELECT TOP (@Limit) Title, Category, Starttime, Endtime, Image
        FROM News
        WHERE Enabled = 1
        ORDER BY Createdate DESC";
			return await Task.Run(() => GetAll(query, new { Limit = limit })); // 使用 BaseRepository 的 GetAll 方法
		}
	}
}
