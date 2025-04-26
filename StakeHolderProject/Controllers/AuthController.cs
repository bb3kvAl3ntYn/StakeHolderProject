using Microsoft.AspNetCore.Mvc;
using StakeholderCommon.DTOs;
using StakeHolderProject.Services.Interfaces;

namespace StakeHolderProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly IClaimsService _claimsService;

        public AuthController(
            IAuthService authService, 
            IUserService userService,
            IClaimsService claimsService)
        {
            _authService = authService;
            _userService = userService;
            _claimsService = claimsService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            if (_authService.Authenticate(loginDto.Username, loginDto.Password))
            {
                var user = _userService.GetUserByUsername(loginDto.Username);
                if (user == null)
                {
                    return Unauthorized("Invalid credentials");
                }
                
                var token = _authService.GenerateJwtToken(loginDto.Username);
                var role = user.Role.ToString();

                _claimsService.Initialize(token);

                return Ok(new AuthResponseDto
                {
                    Username = user.Username,
                    Role = role,
                    Token = token
                });
            }

            return Unauthorized("Invalid credentials");
        }
    }
} 