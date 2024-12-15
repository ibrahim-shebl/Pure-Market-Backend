using System.ComponentModel.DataAnnotations;

namespace test.DTOs
{
    public class BannerProductDetailsDto
    {
        public int Id { get; set; }

        public string ProductName { get; set; }

        public string CatImgPath { get; set; }


        [MaxLength(2500)]
        public string Description { get; set; }
    }
}
