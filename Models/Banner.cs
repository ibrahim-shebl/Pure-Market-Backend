using System.ComponentModel.DataAnnotations;

namespace test.Models
{
    public class Banner
    {
        public int Id { get; set; }

        [MaxLength(250)]
        public string ProductName { get; set; }
 
        [MaxLength(2500)]
        public string Description { get; set; }

        public string CatImgPath { get; set; }  
    }
}
