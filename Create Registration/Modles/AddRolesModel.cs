using System.ComponentModel.DataAnnotations;

namespace Create_Registration.Modles
{
    public class AddRolesModel
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string Role { get; set; } 
    }
}
