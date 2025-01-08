using System.Data;
using Dapper;
using HotelServe.Models;
using HotelServe.Services;

namespace HotelServe.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnection _dbConnection;

        public UserRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        // 根據 Email 獲取用戶
        public User GetUserByEmail(string email)
        {
            const string sql = "SELECT * FROM Login WHERE Email = @Email"; // 假設資料表名稱為 Users
            return _dbConnection.QueryFirstOrDefault<User>(sql, new { Email = email });
        }

        // 更新用戶資料
        public bool UpdateUser(User user)
        {
            const string sql = @"
    UPDATE Login
    SET FullName = @FullName,
        Phone = @Phone,
        Birth = @Birth,
        PasswordHash = @PasswordHash,
        Salt = @Salt
    WHERE Email = @Email";
            var rowsAffected = _dbConnection.Execute(sql, user);
            return rowsAffected > 0;
        }
    }
}
