using System.ComponentModel.DataAnnotations;

namespace test.DTOs
{
    public class BannerProductDto
    {
        [MaxLength(250)]
        public string ProductName { get; set; }

        public IFormFile? CatImg { get; set; }

        [MaxLength(2500)]
        public string Description { get; set; }
    }
}
