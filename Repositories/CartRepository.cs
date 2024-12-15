using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using test.Data;
using test.Models;

namespace test.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _context;

        public CartRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cart>> GetAllCartsAsync()
        {
            return await _context.Carts.Include(c => c.CartItems).ThenInclude(ci => ci.Product).ToListAsync();
        }

        public async Task<Cart> GetCartByIdAsync(int id)
        {
            return await _context.Carts.Include(c => c.CartItems).ThenInclude(ci => ci.Product).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Cart> CreateCartAsync(Cart cart)
        {
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
            return cart;
        }

        public async Task UpdateCartAsync(Cart cart)
        {
            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCartAsync(int id)
        {
            var cart = await _context.Carts.FindAsync(id);
            if (cart != null)
            {
                _context.Carts.Remove(cart);
                await _context.SaveChangesAsync();
            }
        }

        public async Task ClearAllCartsAsync()  
        {
            var carts = await _context.Carts.ToListAsync();
            _context.Carts.RemoveRange(carts);
            await _context.SaveChangesAsync();
        }
    }
}
