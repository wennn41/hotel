using Dapper;
using HotelServe.Models;
using System.Data;

namespace HotelServe.Repositories
{
    public class ContactUsRepository : IContactUsRepository
    {
        private readonly IDbConnection _dbConnection;
        public ContactUsRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }
        public void AddContactUs(ContactUs contact)
        {
            const string sql = @"
                INSERT INTO ContactUs (Name, Email, Phone, Message, CreateDate)
                VALUES (@Name, @Email, @Phone, @Message, @CreateDate)";
            _dbConnection.Execute(sql, contact);
        }
    }
}
