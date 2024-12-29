using System.ComponentModel.DataAnnotations;

namespace test.DTOs
{
    public class ReviewDto
    {
        [MaxLength(250)]
        public string Name { get; set; }

        public double Rate { get; set; }

        [MaxLength(2500)]
        public string Description { get; set; }
    }
}
