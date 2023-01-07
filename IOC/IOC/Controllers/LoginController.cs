using IOC.RequestModels;
using IOC.Services;
using IOC.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IOC.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest user)
        {
            if (user is null)
                return BadRequest("Invalid client request");
            var loginResponse = await _loginService.CreateTokenString(user);
            return (loginResponse.GetType() == typeof(string)) ? Unauthorized(loginResponse) : Ok(loginResponse);
        }
    }
}
