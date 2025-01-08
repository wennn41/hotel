using HotelServe.Models;
using HotelServe.Repositories;

namespace HotelServe.Services
{
    public class ContactUsService : IContactUsService
    {
        private readonly IContactUsRepository _contactUsRepository;
        public ContactUsService(IContactUsRepository contactUsRepository)
        {
            _contactUsRepository = contactUsRepository;
        }

        public void SubmitContactForm(ContactUs contact)
        {
            _contactUsRepository.AddContactUs(contact);
        }
    }
}
