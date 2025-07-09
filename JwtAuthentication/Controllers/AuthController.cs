using JwtAuthentication.Entity;
using JwtAuthentication.Models;
using JwtAuthentication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JwtAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDTO dto) // Fixed return type to Task<ActionResult<User>>
        {
            var user = await _userService.RegisterAsync(dto);
            if (user == null)
            {
                return BadRequest("User already exists or registration failed.");
            }
            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserDTO dto) // Fixed return type to Task<ActionResult<string>>
        {
            var token = await _userService.LoginAsync(dto);
            if (token  == null)
            {
                return BadRequest("Invalid username or password");
            }
            return Ok(token);
        }

        [Authorize]
        [HttpGet]
        public IActionResult AuthenticatedOnlyEndpoint()
        {
            return Ok("You are authenticated");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin-only")]
        public IActionResult AdminOnlyEndpoint()
        {
            return Ok("You are Admin");
        }
    }
}
