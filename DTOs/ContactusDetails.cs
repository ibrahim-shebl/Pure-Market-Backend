using System.ComponentModel.DataAnnotations;

namespace test.DTOs
{
    public class ContactusDetails
    {
        public int Id { get; set; }

        [MaxLength(250)]
        public string Name { get; set; }
        public string Email { get; set; }
        [MaxLength(2500)]
        public string Description { get; set; }
    }
}
