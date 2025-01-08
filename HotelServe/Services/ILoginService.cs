using HotelServe.Models;

namespace HotelServe.Services
{
    public interface ILoginService
    {
        string GenerateSalt();
        string HashPassword(string password, string salt);
        bool ValidateUser(string email, string password);
    }
}
