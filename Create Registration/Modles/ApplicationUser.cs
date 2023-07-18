using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Create_Registration.Modles
{
    public class ApplicationUser : IdentityUser
    {
        [Required , MinLength(50)]
        public string FirstName { get; set; }
        [Required , MinLength(50)]  
        public string LastName { get; set; } = string.Empty;
    }
}
