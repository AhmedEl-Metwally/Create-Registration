using Create_Registration.Interface;
using Create_Registration.Setting;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Net.Mime;
using ContentType = MimeKit.ContentType;

namespace Create_Registration.Services
{
    public class MailService : IMailService
    {
        private readonly MailSetting _mailSetting;

        public MailService(IOptions<MailSetting> mailSetting)
        {
            _mailSetting = mailSetting.Value;   
        }


        public async Task SendEmailAsync(string mailTo, string subject, string body, IList<IFormFile> attachments = null)
        {
            var email = new MimeMessage
            {
                Sender = MailboxAddress.Parse(mailTo),
                Subject = subject,
            };

            email.To.Add(MailboxAddress.Parse(mailTo));

            var builder = new BodyBuilder();

            if (attachments != null)
            {
                byte[] fileBytes;
                foreach (var file in attachments) 
                {
                    if(file.Length > 0)
                    {
                        using var ms = new MemoryStream();
                        file.CopyTo(ms);
                        fileBytes = ms.ToArray();

                        builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                    }
                }
            }

            builder.HtmlBody = body;
            email.Body = builder.ToMessageBody();
            email.From.Add(new MailboxAddress (_mailSetting.DisplayName , _mailSetting.Email));

            using var smtp = new SmtpClient();
            smtp.Connect(_mailSetting.Host, _mailSetting.Port , SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSetting.Email , _mailSetting.Password);
            await smtp.SendAsync(email);

            smtp.Disconnect(true);
        }
    }
}
