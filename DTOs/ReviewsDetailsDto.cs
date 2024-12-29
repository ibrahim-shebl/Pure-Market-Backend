using System.ComponentModel.DataAnnotations;

namespace test.DTOs
{
    public class ReviewsDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Rate { get; set; }

        [MaxLength(2500)]
        public string Description { get; set; }
    }
}
