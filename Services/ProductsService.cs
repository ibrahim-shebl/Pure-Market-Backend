using Microsoft.EntityFrameworkCore;
using test.Data;

namespace test.Services
{
    public class ProductsService : IProductsService
    {
        private readonly ApplicationDbContext _context;

        public ProductsService(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<Product> Add(Product product)
        {
            await _context.AddAsync(product);
            _context.SaveChanges();

            return product;
        }

        public Product Delete(Product product)
        {
            _context.Remove(product);
            _context.SaveChanges();

            return product;
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            return await _context.Products.ToListAsync();

        }

        public async Task<Product> GetById(int id)
        {
            return await _context.Products.SingleOrDefaultAsync(p => p.Id == id);
        }

        public Product Update(Product product)
        {
            _context.Update(product);
            _context.SaveChanges();

            return product;
        }
    }
}
