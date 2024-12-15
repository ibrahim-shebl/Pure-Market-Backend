using test.Models;

namespace test.Services
{
    public interface IReviewsProduct
    {
        Task<IEnumerable<Review>> GetAll();
        Task<Review> GetById(int id);
        Task<Review> Add(Review review);
        Review Delete(Review review);
    }
}
