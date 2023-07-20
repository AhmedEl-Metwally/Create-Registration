using System.ComponentModel.DataAnnotations;

namespace Create_Registration.Dto
{
    public class MailRequestDto
    {
        [Required]
        public string ToEmail { get; set; }
        [Required]
        public string Subject { get; set; }
        public string Body { get; set; }
        public IList<IFormFile> Attachments { get; set; }
    }
}
