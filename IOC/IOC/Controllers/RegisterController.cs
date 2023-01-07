using IOC.RequestModels;
using IOC.Models;
using IOC.Services;
using Microsoft.AspNetCore.Mvc;
using IOC.Services.Interfaces;

namespace IOC.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegisterController : ControllerBase
    {
        private readonly IRegisterService _registerService;

        public RegisterController(IRegisterService registerService)
        {
            _registerService = registerService;
        }

        [HttpPost("sign-up-user")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterRequest user)
        {
            User u = new User();
            u.Name = user.Name;
            u.PhoneNumber = user.PhoneNumber;
            u.Surname = user.Surname;
            u.MailAddress = user.MailAddress;
            u.Password = user.Password;

            if (user is null)
                return BadRequest("Invalid client request");            

            bool result = await _registerService.CheckIfUserExists(u);

            if (result == true)
                return BadRequest("User already exists");
            else
                return Ok(user);
        }
    }
}
