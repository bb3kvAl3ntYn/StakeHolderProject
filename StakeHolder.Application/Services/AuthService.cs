using StakeHolderProject.Models;
using StakeHolderProject.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StakeHolderProject.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly IClaimsService _claimsService;

        public AuthService(IUserService userService, IConfiguration configuration, IClaimsService claimsService)
        {
            _userService = userService;
            _configuration = configuration;
            _claimsService = claimsService;
        }

        public bool IsAdmin(string username)
        {
            var user = _userService.GetUserByUsername(username);
            return user != null && user.Role == UserRole.Admin;
        }

        public bool IsAdmin(ClaimsPrincipal principal)
        {
            _claimsService.Initialize(principal);
            return _claimsService.IsAdmin;
        }

        public bool Authenticate(string username, string password)
        {
            return _userService.ValidateUser(username, password);
        }

        public string GenerateJwtToken(string username)
        {
            var user = _userService.GetUserByUsername(username);
            if (user == null)
                return string.Empty;

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpiryInMinutes"])),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
} 