using HotelServe.Models;

namespace HotelServe.Services
{
    public interface IUserRepository
    {
        User GetUserByEmail(string email);
        bool UpdateUser(User user);
    }
}
