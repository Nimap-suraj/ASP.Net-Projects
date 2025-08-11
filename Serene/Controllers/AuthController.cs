using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serene.DTO;
using Serene.Entity;
using Serene.Repository.Interface;

namespace Serene.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuth _authService;
        public AuthController(IAuth service)
        {
            _authService = service;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto userDto)
        {
            var user = await _authService.Register(userDto);

            return Ok(new
            {
                user.Id,
                user.Email,
                user.Roles,
            });
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {
            var user = _authService.Authenticate(login.Email, login.Password);
            if(user == null)
            {
                return Unauthorized();
            }
            var token = _authService.GenerateToken(user);
            return Ok(token);
        }
    }
}
