using Dapper;
using HotelServe.Models;
using System.Data;

namespace HotelServe.Repositories
{
    public class MemberRepository : BaseRepository<Member>
    {
        public MemberRepository(IDbConnection dbConnection) : base(dbConnection)
        {
        }

        //// 獲取所有會員資料
        //public IEnumerable<Member> GetAllMembers()
        //{
        //    string sql = @"SELECT Id, FullName, Email, Countries, Phone, Birth, CreatedAt, Locked
        //           FROM Login"; // 確保 Id 被選取
        //    return GetAll(sql);
        //}

        // 獲取 role = 0 的會員
        public IEnumerable<Member> GetMembersByRole0()
        {
            string sql = @"SELECT Id, FullName, Email, Countries, Phone, Birth, CreatedAt, Locked, Role
                           FROM Login
                           WHERE Role = 0";
            return GetAll(sql);
        }

        // 獲取 role = 1 的會員
        public IEnumerable<Member> GetMembersByRole1()
        {
            string sql = @"SELECT Id, FullName, Email, Countries, Phone, Birth, CreatedAt, Locked, Role
                           FROM Login
                           WHERE Role = 1";
            return GetAll(sql);
        }

        // 新增會員
        public int AddMember(Member member)
        {
            string sql = @"INSERT INTO Login (FullName, Email, Countries, Phone, Birth, CreatedAt, Locked)
                           VALUES (@FullName, @Email, @Countries, @Phone, @Birth, @CreatedAt, @Locked)";
            return Add(sql, member);
        }

        // 搜尋會員
        public IEnumerable<Member> SearchMembers(string keyword)
        {
            string sql = @"SELECT Id, FullName, Email, Countries, Phone, Birth, CreatedAt, Locked
                           FROM Login
                           WHERE Email LIKE @Keyword";
            return GetAll(sql, new { Keyword = $"%{keyword}%" });
        }

        // 更新會員
        public bool UpdateMember(Member member)
        {
            string query = @"UPDATE Login 
                             SET FullName = @FullName, 
Email=@Email,                                 
Phone = @Phone, 
                                 Countries = @Countries, 
                                 Birth = @Birth, 
                                 
                                 Locked = @Locked 
                             WHERE Id = @Id";
            return Update(query, member) > 0;
        }

        // 刪除會員
        public bool DeleteMember(int memberId)
        {
            string query = "DELETE FROM Login WHERE Id = @Id";
            int affectedRows = Delete(query, new { Id = memberId });
            return affectedRows > 0; // 回傳是否有刪除成功
        }

        // 檢查是否存在 Email
        public bool EmailExists(string email)
        {
            string query = "SELECT COUNT(1) FROM Login WHERE Email = @Email";
            return ExecuteScalar<int>(query, new { Email = email }) > 0;
        }

    }
}
