using Microsoft.AspNetCore.Mvc;

namespace HotelServe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LineController : ControllerBase
    {
        private readonly string clientId = "2006706094"; // 替換為您的 LINE Channel ID
        private readonly string clientSecret = "d8cfcb21294b6fd45460f91a7533aedb"; // 替換為您的 LINE Channel Secret
        private readonly string redirectUri = "http://localhost:8080/LineCallback"; // 替換為您的 Redirect URI

        [HttpGet("login")]
        public IActionResult Login()
        {
            var state = Guid.NewGuid().ToString(); // 生成唯一的狀態碼，防止 CSRF 攻擊
            var loginUrl = $"https://access.line.me/oauth2/v2.1/authorize?" +
                           $"response_type=code&client_id={clientId}&redirect_uri={redirectUri}" +
                           $"&state={state}&scope=profile%20openid%20email";
            return Redirect(loginUrl); // 將用戶導向 LINE 登入頁面
        }

        // 2. 處理 LINE 登入回呼
        [HttpGet("callback")]
        public async Task<IActionResult> Callback([FromQuery] string code, [FromQuery] string state)
        {
            // 構建交換 access token 的請求
            var tokenRequest = new HttpClient();
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>("code", code),
                new KeyValuePair<string, string>("redirect_uri", redirectUri),
                new KeyValuePair<string, string>("client_id", clientId),
                new KeyValuePair<string, string>("client_secret", clientSecret),
            });

            var response = await tokenRequest.PostAsync("https://api.line.me/oauth2/v2.1/token", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest("Failed to exchange token with LINE.");
            }

            // 解析 token 回應
            var tokenResult = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(responseContent);
            var accessToken = (string)tokenResult.access_token;

            // 使用 Access Token 獲取用戶資料
            var profileRequest = new HttpClient();
            profileRequest.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
            var profileResponse = await profileRequest.GetAsync("https://api.line.me/v2/profile");

            if (!profileResponse.IsSuccessStatusCode)
            {
                return BadRequest("Failed to fetch user profile.");
            }

            var profileContent = await profileResponse.Content.ReadAsStringAsync();
            var profileData = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(profileContent);

            return Ok(profileData); // 返回用戶資料
        }
    }
}
