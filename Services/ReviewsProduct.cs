using Microsoft.EntityFrameworkCore;
using test.Data;
using test.Models;

namespace test.Services
{
    public class ReviewsProduct : IReviewsProduct
    {
        private readonly ApplicationDbContext _context;

        public ReviewsProduct(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<Review> Add(Review review)
        {
            await _context.AddAsync(review);
            _context.SaveChanges();

            return review;
        }

        public Review Delete(Review review)
        {
            _context.Remove(review);
            _context.SaveChanges();

            return review;
        }

        public async Task<IEnumerable<Review>> GetAll()
        {
            return await _context.Reviews.ToListAsync();

        }
        public async Task<Review> GetById(int id)
        {
            return await _context.Reviews.FindAsync(id);
        }
    }
}
