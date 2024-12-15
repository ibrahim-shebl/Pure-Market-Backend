using test.Models;

namespace test.Services
{
    public interface IContactUs
    {
        Task<IEnumerable<Contact>> GetAll();
        Task<Contact> GetById(int id);
        Task<Contact> Add(Contact contact);
        Contact Delete(Contact contact);
    }
}
