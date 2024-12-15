using System.ComponentModel.DataAnnotations;

namespace test.Dtos
{
    public class ProductDetailsDto
    {
        public int Id { get; set; }

        public string ProductName { get; set; }

        public double Price { get; set; }

        public double OldPrice { get; set; }

        public double Rate { get; set; }

        public double DiscountPercentage { get; set; }

        public string Category { get; set; }

        public string Brand { get; set; }

        public string CatImgPath { get; set; } 


        [MaxLength(2500)]
        public string Description { get; set; }
    }
}
