using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace test.Models
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
        public ICollection<Cart> Carts { get; set; } = new List<Cart>();
         
    }
}
