using Create_Registration.Dto;
using Create_Registration.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Create_Registration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailingController : ControllerBase
    {
        private readonly IMailService _mailService;

        public MailingController (IMailService mailService)
        {
            _mailService = mailService;
        }


        [HttpPost("send")]
        public async Task<IActionResult> SendEmailAsync([FromForm] MailRequestDto dto)
        {
            await _mailService.SendEmailAsync(dto.ToEmail, dto.Subject, dto.Body,  dto.Attachments);

            return Ok();
        }
    }
}
