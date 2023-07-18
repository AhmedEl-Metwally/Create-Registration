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

            var result = await _authService.RegisterAsync(model);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

            return Ok(result);
        
        }

        [HttpPost("token")]
        public async Task<IActionResult> GetTokenAsync([FromBody] TokenRegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var regist = await _authService.GetTokenAsync(model);
            if (regist.IsAuthenticated)
                return BadRequest(regist.Message);

            if (!string.IsNullOrEmpty(regist.RefreshToken))
                SetRefreshTokenInCookie(regist.RefreshToken, regist.RefreshTokenExpiration);

            return Ok(new { Token = regist.Token, Exception = regist.ExpiresOn });

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

        private void SetRefreshTokenInCookie(string refreshToken, DateTime expires)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = expires.ToLocalTime(),
            };

            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }

        [HttpGet("refreshToken")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            var result = await _authService.RefreshTokenAsync(refreshToken);

            if (!result.IsAuthenticated)
                return BadRequest(result);

            SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

            return Ok(result);
        }

        [HttpPost("revokeToken")]
        public async Task<IActionResult> RevokeToken([FromBody] RevokeToken model)
        {
            var token = model.Token ?? Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest("Token is required!");

            var result = await _authService.RevokeTokenAsync(token);

            if (!result)
                return BadRequest("Token is invalid!");

            return Ok();
        }

    }
}
