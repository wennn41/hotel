using HotelServe.Models;
using System.Data;

namespace HotelServe.Repositories
{
	public class LoginRepository : BaseRepository<LoginUser>
	{
		public LoginRepository(IDbConnection dbConnection) : base(dbConnection)
		{
		}

		// 專屬方法：根據 Email 取得使用者資料
		public LoginUser? GetUserByEmail(string email)
		{
			const string query = "SELECT * FROM Login WHERE Email = @Email";
			return Get(query, new { Email = email });
		}

        // 檢查用戶是否存在
        public bool CheckUserExists(string email)
        {
            const string query = "SELECT COUNT(1) FROM Login WHERE Email = @Email";
            var count = ExecuteScalar<int>(query, new { Email = email });
            return count > 0; // 如果查詢結果大於 0，則表示用戶存在
        }

        

        // 新增專屬方法：新增使用者
        public bool AddUser(LoginUser user)
        {
            if (CheckUserExists(user.Email))
            {
                // 如果信箱已存在，返回 false
                return false;
            }

            const string query = @"
        INSERT INTO Login (Email, PasswordHash, Salt, FullName, Countries, Phone, Birth, Role, CreatedAt, Locked)
        VALUES (@Email, @PasswordHash, @Salt, @FullName, @Countries, @Phone, 
                @Birth, @Role, @CreatedAt, @Locked)";

            // 檢查並修正日期值
            user.CreatedAt = user.CreatedAt == DateTime.MinValue ? DateTime.UtcNow : user.CreatedAt;
            user.Birth = user.Birth == DateTime.MinValue ? (DateTime?)null : user.Birth;

            return Execute(query, new
            {
                user.Email,
                user.PasswordHash,
                user.Salt,
                user.FullName,
                user.Countries,
                user.Phone,
                user.Birth,
                user.Role,
                user.CreatedAt,
                user.Locked
            }) > 0; // 成功執行後受影響的行數大於 0 表示成功
        }

        // 新增 Google 使用者專屬方法
        public bool AddGoogleUser(LoginUser user)
        {
            const string query = @"
            INSERT INTO Login (Email, PasswordHash, Salt, FullName, Countries, Phone, Birth, Role, CreatedAt, Locked)
            VALUES (@Email, NULL, NULL, @FullName, NULL, NULL, NULL, @Role, @CreatedAt, @Locked)";

            // 確保 Google 使用者的日期值正確
            user.CreatedAt = user.CreatedAt == DateTime.MinValue ? DateTime.UtcNow : user.CreatedAt;

            return Execute(query, new
            {
                user.Email,
                user.FullName,
                user.Role,
                user.CreatedAt,
                user.Locked
            }) > 0; // 成功執行後受影響的行數大於 0 表示成功
        }
    }
}
