using test.Models;

namespace test.Services
{
    public interface IProductsService
    {
        Task<IEnumerable<Product>> GetAll();
        Task<Product> GetById(int id);
        Task<Product> Add(Product movie);
        Product Update(Product movie);
        Product Delete(Product movie);
    }
}
