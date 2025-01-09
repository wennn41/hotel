using HotelServe.Models;
using HotelServe.Repositories;
using HotelServe.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotelServe.Controllers.BackgroundControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MemberController : ControllerBase
    {
        private readonly MemberRepository _repository;
        private readonly MemberService _service;

        public MemberController(MemberService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }



        // 獲取所有會員
        //[HttpGet]
        //public ActionResult<IEnumerable<Member>> GetMembers()
        //{
        //    var members = _service.GetAllMembers();
        //    return Ok(members);
        //}

        // 獲取 role = 0 的會員
        [HttpGet("role0")]
        public ActionResult<IEnumerable<Member>> GetMembersByRole0()
        {
            var members = _service.GetMembersByRole0();
            return Ok(members);
        }

        // 獲取 role = 1 的會員
        [HttpGet("role1")]
        public ActionResult<IEnumerable<Member>> GetMembersByRole1()
        {
            var members = _service.GetMembersByRole1();
            return Ok(members);
        }

        // 新增員工
        [HttpPost]
        public ActionResult AddMember(Member member)
        {
            member.CreatedAt = DateTime.Now; // 設置當前時間
            member.Role = 1; // 默認設置 role = 1
            bool isRegistered = _service.RegisterMember(member);

            if (isRegistered)
                return Ok("會員添加成功");
            else
                return BadRequest("會員添加失敗");
        }

        // 查詢會員
        [HttpGet("search")]
        public ActionResult<IEnumerable<Member>> SearchMembers([FromQuery] string keyword)
        {
            // 檢查 keyword 是否為空  
            if (string.IsNullOrWhiteSpace(keyword))
                return BadRequest("搜尋欄不得為空");

            var members = _service.SearchMembers(keyword);
            if (members == null || !members.Any())
                return NotFound("沒有找到符合條件的會員");

            return Ok(members);
        }

        // 更新會員
        [HttpPut("{id}")]
        public ActionResult UpdateMember(int id, Member member)
        {
            if (id != member.Id)
                return BadRequest("ID 不一致");

            var updated = _service.UpdateMember(member);
            if (updated)
                return Ok("會員更新成功");
            else
                return NotFound("會員更新失敗，找不到此會員");
        }

        // 刪除會員
        [HttpDelete("{id}")]
        public ActionResult DeleteMember(int id)
        {
            if (id <= 0)
                return BadRequest("ID 無效");

            var deleted = _service.DeleteMember(id);
            if (deleted)
                return Ok("會員刪除成功");
            else
                return NotFound("找不到要刪除的會員");
        }

        // 在 MemberController 中新增 API，用於獲取會員的訂單歷史紀錄
        [HttpGet("{memberId}/order-history")]  
        public ActionResult<IEnumerable<dynamic>> GetOrderHistory(int memberId)
        {
            // 檢查會員 ID 是否有效，避免處理無效請求
            if (memberId <= 0)
                return BadRequest("會員 ID 無效");

            // 調用 Service 層，獲取會員的訂單歷史紀錄
            var orderHistory = _service.GetOrderHistory(memberId);

            // 如果結果為空或不存在，返回 404 Not Found
            if (orderHistory == null || !orderHistory.Any())
                return NotFound("該會員沒有訂單記錄");

            // 返回 200 OK，並附帶訂單歷史紀錄數據
            return Ok(orderHistory);
        }
    }
}
