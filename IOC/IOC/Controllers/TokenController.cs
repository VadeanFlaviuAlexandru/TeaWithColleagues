using IOC.Models;
using IOC.Services;
using IOC.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IOC.Controllers
{
    public class TokenController : ControllerBase
    {
        private readonly ITokenService _tokenService;

        public TokenController(ITokenService tokenService)
        {
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        }

        [HttpPost]
        [Route("refresh")]
        public async Task<IActionResult> RefreshAsync([FromBody] TokenAPI tokenAPI)
        {
            if (tokenAPI is null)
                return BadRequest("Invalid client request");
            var result = await _tokenService.RefreshToken(tokenAPI);
            return (result == null) ? BadRequest("Invalid client request") : Ok(result);
        }

        [HttpPost, Authorize]
        [Route("revoke")]
        public async Task<IActionResult> Revoke()
        {
            var mail = User.Identity.Name;
            var result = await _tokenService.Revoke(mail);
            if (!result)
                return BadRequest();
            return NoContent();
        }
    }
}
