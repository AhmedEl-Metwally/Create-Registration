using Create_Registration.Interface;
using Create_Registration.Modles;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;

namespace Create_Registration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync ([FromBody]RegisterModel model)
        {
            if (!ModelState.IsValid) 
                return BadRequest(ModelState);  

            var regist = await _authService.RegisterAsync(model);
            if (regist.IsAuthenticated)
                return BadRequest(regist.Message);

            return Ok(regist);  
        }

        [HttpPost("token")]
        public async Task<IActionResult> GetTokenAsync([FromBody] TokenRegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var regist = await _authService.GetTokenAsync(model);
            if (regist.IsAuthenticated)
                return BadRequest(regist.Message);

            return Ok(regist);
        }

        [HttpPost("addroles")]
        public async Task<IActionResult> AddRolesAsync([FromBody] AddRolesModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var regist = await _authService.AddRolesAsync(model);
            if (!string.IsNullOrEmpty(regist))
                return BadRequest(regist);

            return Ok(model);
        }

    }
}
