using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using test.Dtos;
using test.DTOs;
using test.Models;
using test.Services;

namespace test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IReviewsProduct _reviewsProduct;
        public ReviewsController(IReviewsProduct reviewsProduct, IMapper mapper)
        {
            _reviewsProduct = reviewsProduct;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var reviews = await _reviewsProduct.GetAll();
            var data = _mapper.Map<IEnumerable<ReviewsDetailsDto>>(reviews);

            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] ReviewDto dto)
        {           

            var review = _mapper.Map<Review>(dto);  
            _reviewsProduct.Add(review);

            return Ok(review);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var review = await _reviewsProduct.GetById(id);
            if (review == null)
                return NotFound($"No product was found with ID {id}");

            _reviewsProduct.Delete(review);

            return Ok(review);
        }
    }
}
