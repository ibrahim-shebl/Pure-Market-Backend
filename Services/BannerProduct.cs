using Microsoft.EntityFrameworkCore;
using test.Data;
using test.Models;

namespace test.Services
{
    public class BannerProduct : IBannerProduct
    {
        private readonly ApplicationDbContext _context;

        public BannerProduct(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<Banner> Add(Banner banner)
        {
            await _context.AddAsync(banner);
            _context.SaveChanges();

            return banner;
        }

        public Banner Delete(Banner banner)
        {
            _context.Remove(banner);
            _context.SaveChanges();

            return banner;
        }

        public async Task<IEnumerable<Banner>> GetAll()
        {
            return await _context.Banners.ToListAsync();

        }

        public async Task<Banner> GetById(int id)
        {
            return await _context.Banners.SingleOrDefaultAsync(p => p.Id == id);
        }

        public Banner Update(Banner banner)
        {
            _context.Update(banner);
            _context.SaveChanges();

            return banner;
        }
    }
}
