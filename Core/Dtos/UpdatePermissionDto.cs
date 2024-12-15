using System.ComponentModel.DataAnnotations;

namespace test.Core.Dtos
{
    public class UpdatePermissionDto
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
    }
}
