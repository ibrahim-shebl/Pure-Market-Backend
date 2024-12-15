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
        private readonly string _imageFolderPath;

        private new List<string> _allowedExtensions = new List<string> { ".jpg", ".png", ".webp" };
        private long _maxAllowedPosterSize = 5 * 1024 * 1024; // 5MB

        public ReviewsController(IReviewsProduct reviewsProduct, IMapper mapper)
        {
            _reviewsProduct = reviewsProduct;
            _mapper = mapper;
            _imageFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadedImages");

            if (!Directory.Exists(_imageFolderPath))
            {
                Directory.CreateDirectory(_imageFolderPath);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var reviews = await _reviewsProduct.GetAll();
            var baseUrl = $"{Request.Scheme}://{Request.Host}/UploadedImages/";
            var data = _mapper.Map<IEnumerable<ReviewsDetailsDto>>(reviews);

            foreach (var reviewDto in data)
            {
                reviewDto.CatImgPath = $"{baseUrl}{reviewDto.CatImgPath}";
            }

            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] ReviewDto dto)
        {
            if (dto.CatImg == null)
                return BadRequest("Image is required!");

            if (!_allowedExtensions.Contains(Path.GetExtension(dto.CatImg.FileName).ToLower()))
                return BadRequest("Only .png, .jpg and .webp images are allowed!");

            if (dto.CatImg.Length > _maxAllowedPosterSize)
                return BadRequest("Max allowed size for image is 5MB!");

            var fileName = Path.GetFileNameWithoutExtension(dto.CatImg.FileName);
            var extension = Path.GetExtension(dto.CatImg.FileName);
            var newFileName = $"{fileName}_{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(_imageFolderPath, newFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.CatImg.CopyToAsync(stream);
            }

            var review = _mapper.Map<Review>(dto);
            review.CatImgPath = newFileName;  

            _reviewsProduct.Add(review);

            return Ok(review);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var review = await _reviewsProduct.GetById(id);
            if (review == null)
                return NotFound($"No product was found with ID {id}");

            if (!string.IsNullOrEmpty(review.CatImgPath))
            {
                var filePath = Path.Combine(_imageFolderPath, review.CatImgPath);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            _reviewsProduct.Delete(review);

            return Ok(review);
        }
    }
}
