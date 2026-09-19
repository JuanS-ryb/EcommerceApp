using Ecommerce.server.Dto;
using Ecommerce.server.Models;
using Ecommerce.server.services.interfaces;
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
        public async Task<ActionResult<AuthDto>> Login(UserDto request)
        {
            AuthDto? lgdResponse = await authService.LoginAsync(request);
            if (lgdResponse is null) return BadRequest("Credenciales invalidas");

            return Ok(lgdResponse);
        }
    }
}
