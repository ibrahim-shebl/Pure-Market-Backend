using Microsoft.EntityFrameworkCore;
using test.Data;
using test.Models;

namespace test.Services
{
    public class ContactUs : IContactUs
    {
        private readonly ApplicationDbContext _context;

        public ContactUs(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Contact> Add(Contact contact)
        {
            await _context.AddAsync(contact);
            _context.SaveChanges();

            return contact;
        }

        public Contact Delete(Contact contact)
        {
            _context.Remove(contact);
            _context.SaveChanges();

            return contact;
        }

        public async Task<IEnumerable<Contact>> GetAll()
        {
            return await _context.Contacts.ToListAsync();

        }

        public async Task<Contact> GetById(int id)
        {
            return await _context.Contacts.SingleOrDefaultAsync(p => p.Id == id);
        }

        public Contact Update(Contact contact)
        {
            _context.Update(contact);
            _context.SaveChanges();

            return contact;
        }
    }
}
