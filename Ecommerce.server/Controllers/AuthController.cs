using Ecommerce.server.Dto;
using Ecommerce.server.Models;
using Ecommerce.server.services.auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController (IAuthService authService) : ControllerBase
    {

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDto auth)
        {
            var user = await authService.RegisterAsync(auth);

            if (user is null) return BadRequest("El usuario ya existe");
            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserDto request)
        {
            var token = await authService.LoginAsync(request);
            if (token is null) return BadRequest("Credenciales invalidas");
            return Ok(token);
        }

        [Authorize]
        [HttpGet("check")]
        public IActionResult AuthenticationCheck()
        {
            return Ok("You Are logged in!!!");
        }
    }
}
