using System.Collections.Generic;
using System.Threading.Tasks;
using test.Models;

namespace test.Repositories
{
    public interface ICartRepository
    {
        Task<IEnumerable<Cart>> GetAllCartsAsync();
        Task<Cart> GetCartByIdAsync(int id);
        Task<Cart> CreateCartAsync(Cart cart);
        Task UpdateCartAsync(Cart cart);
        Task DeleteCartAsync(int id);
        Task ClearAllCartsAsync();  
    }
}
