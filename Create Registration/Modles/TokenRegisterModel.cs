using System.ComponentModel.DataAnnotations;

namespace Create_Registration.Modles
{
    public class TokenRegisterModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }    
    }
}
