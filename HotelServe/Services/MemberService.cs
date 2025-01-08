using HotelServe.Models;
using HotelServe.Repositories;
using HotelServe.Services;


namespace HotelServe.Services
{
    public class MemberService
    {
        private readonly MemberRepository _repository;

        public MemberService(MemberRepository repository)
        {
            _repository = repository;
        }

        // 獲取所有會員
        //public List<Member> GetAllMembers()
        //{
        //    return _repository.GetAllMembers().ToList(); // 將 IEnumerable 轉為 List
        //}

        // 獲取 role = 0 的會員
        public List<Member> GetMembersByRole0()
        {
            return _repository.GetMembersByRole0().ToList();
        }

        // 獲取 role = 1 的會員
        public List<Member> GetMembersByRole1()
        {
            return _repository.GetMembersByRole1().ToList();
        }

        // 新增會員
        public bool RegisterMember(Member member)
        {
            // 檢查業務邏輯 (例如：是否重複註冊)
            if (string.IsNullOrEmpty(member.Email))
                return false;

            _repository.AddMember(member);
            return true;
        }

        // 搜尋會員
        public List<Member> SearchMembers(string keyword)
        {
            // 如果搜尋關鍵字為空，返回空列表
            if (string.IsNullOrWhiteSpace(keyword))
                return new List<Member>();

            return _repository.SearchMembers(keyword).ToList();
        }

        public bool UpdateMember(Member member)
        {
            return _repository.UpdateMember(member);
        }

        public bool DeleteMember(int memberId)
        {
            if (memberId <= 0)
                throw new ArgumentException("會員 ID 無效");

            return _repository.DeleteMember(memberId);
        }

        private string HashPassword(string password, string salt)
        {
            // 模擬的雜湊處理
            return Convert.ToBase64String(
                System.Security.Cryptography.SHA256.Create().ComputeHash(
                    System.Text.Encoding.UTF8.GetBytes(password + salt)
                )
            );
        }

    }
}
