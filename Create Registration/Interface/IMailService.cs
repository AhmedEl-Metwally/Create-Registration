namespace Create_Registration.Interface
{
    public interface IMailService
    {
        Task SendEmailAsync(string mailTo, string subject , string body , IList<IFormFile> attachments = null);
    }
}
