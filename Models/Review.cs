using System.ComponentModel.DataAnnotations;

namespace test.Models
{
    public class Review
    {
        public int Id { get; set; }

        [MaxLength(250)]
        public string Name { get; set; }
        public double Rate { get; set; }

        [MaxLength(2500)]
        public string Description { get; set; }

    }
}
