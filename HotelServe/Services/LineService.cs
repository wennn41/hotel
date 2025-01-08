using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace HotelServe.Services
{
    public class LineService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public LineService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        // 生成 JWT Token
        public string GenerateJwtToken(string email, string role)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
        new Claim(ClaimTypes.Email, email),
        new Claim(ClaimTypes.Role, role)
    };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public LineUserInfo ValidateLineCode(string code)
        {
            var clientId = _configuration["Line:ChannelId"];
            var clientSecret = _configuration["Line:ChannelSecret"];
            var redirectUri = _configuration["Line:RedirectUri"];

            using var client = _httpClientFactory.CreateClient();
            var tokenResponse = client.PostAsync("https://api.line.me/oauth2/v2.1/token", new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "grant_type", "authorization_code" },
            { "code", code },
            { "redirect_uri", redirectUri },
            { "client_id", clientId },
            { "client_secret", clientSecret }
        })).Result;

            if (!tokenResponse.IsSuccessStatusCode) return null;

            var tokenContent = tokenResponse.Content.ReadAsStringAsync().Result;
            var tokenData = JsonSerializer.Deserialize<LineTokenResponse>(tokenContent);

            // 使用 Access Token 獲取用戶資料
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenData.AccessToken);
            var profileResponse = client.GetAsync("https://api.line.me/v2/profile").Result;

            if (!profileResponse.IsSuccessStatusCode) return null;

            var profileContent = profileResponse.Content.ReadAsStringAsync().Result;
            var userInfo = JsonSerializer.Deserialize<LineUserInfo>(profileContent);
            return userInfo;
        }
    }

    public class LineUserInfo
    {
        public string UserId { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; } = "Customer"; // 預設角色
    }

    public class LineTokenResponse
    {
        public string AccessToken { get; set; }
        public string IdToken { get; set; }
    }
}
