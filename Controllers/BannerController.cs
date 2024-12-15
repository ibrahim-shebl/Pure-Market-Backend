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
    public class BannerController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IBannerProduct _bannerProduct;
        private readonly string _imageFolderPath;

        private new List<string> _allowedExtensions = new List<string> { ".jpg", ".png", ".webp" };
        private long _maxAllowedPosterSize = 5 * 1024 * 1024; // 5MB

        public BannerController(IBannerProduct bannerProduct, IMapper mapper)
        {
            _bannerProduct = bannerProduct;
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
            var products = await _bannerProduct.GetAll();
            var baseUrl = $"{Request.Scheme}://{Request.Host}/UploadedImages/";
            var data = _mapper.Map<IEnumerable<BannerProductDetailsDto>>(products);

            foreach (var productDto in data)
            {
                productDto.CatImgPath = $"{baseUrl}{productDto.CatImgPath}";
            }

            return Ok(data);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var product = await _bannerProduct.GetById(id);
            if (product == null)
                return NotFound();

            var baseUrl = $"{Request.Scheme}://{Request.Host}/UploadedImages/";
            var dto = _mapper.Map<BannerProductDetailsDto>(product);
            dto.CatImgPath = $"{baseUrl}{product.CatImgPath}";

            return Ok(dto);
        }


        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] BannerProductDto dto)
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

            var banner = _mapper.Map<Banner>(dto);
            banner.CatImgPath = newFileName;  

            _bannerProduct.Add(banner);

            return Ok(banner);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromForm] BannerProductDto dto)
        {
            var product = await _bannerProduct.GetById(id);
            if (product == null)
                return NotFound($"No product was found with ID {id}");

            if (dto.CatImg != null)
            {
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

                
                if (!string.IsNullOrEmpty(product.CatImgPath))
                {
                    var oldFilePath = Path.Combine(_imageFolderPath, product.CatImgPath);
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                product.CatImgPath = newFileName;  
            }

            product.ProductName = dto.ProductName;
            product.Description = dto.Description;
            

            _bannerProduct.Update(product);

            return Ok(product);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var product = await _bannerProduct.GetById(id);
            if (product == null)
                return NotFound($"No product was found with ID {id}");

            if (!string.IsNullOrEmpty(product.CatImgPath))
            {
                var filePath = Path.Combine(_imageFolderPath, product.CatImgPath);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            _bannerProduct.Delete(product);

            return Ok(product);
        }
    }
}
