using System.Data;
using Dapper;

namespace HotelServe.Repositories
{
	public abstract class BaseRepository<T> where T : class
	{
		protected readonly IDbConnection _dbConnection;

		public BaseRepository(IDbConnection dbConnection)
		{
			_dbConnection = dbConnection;
		}

		// 通用的新增資料方法
		public virtual int Add(string insertQuery, object parameters)
		{
			return _dbConnection.Execute(insertQuery, parameters);
		}

        // 通用的 ExecuteScalar 方法，返回單一值
        protected T ExecuteScalar<T>(string query, object parameters = null)
        {
            return _dbConnection.ExecuteScalar<T>(query, parameters);
        }

        // 通用的取得單筆資料方法
        public virtual T? Get(string selectQuery, object parameters)
		{
			return _dbConnection.QueryFirstOrDefault<T>(selectQuery, parameters);
		}

		// 通用的取得多筆資料方法
		public virtual IEnumerable<T> GetAll(string selectQuery, object? parameters = null)
		{
			return _dbConnection.Query<T>(selectQuery, parameters);
		}

		// 通用的更新資料方法
		public virtual int Update(string updateQuery, object parameters)
		{
			return _dbConnection.Execute(updateQuery, parameters);
		}

		// 通用的刪除資料方法
		public virtual int Delete(string deleteQuery, object parameters)
		{
			return _dbConnection.Execute(deleteQuery, parameters);
		}

        protected int Execute(string query, object parameters = null)
        {
            return _dbConnection.Execute(query, parameters);
        }
    }
}
