using HotelServe.Models;


namespace HotelServe.Services
{
    public interface IUserService
    {
        User GetUserByEmail(string email);
        public bool UpdateUser(string email, UpdateProfileRequest request);
        bool ValidateUser(string email, string password);
        string EncryptPassword(string password);
    }
}