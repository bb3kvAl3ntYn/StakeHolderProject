using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StakeHolderProject.Models;
using StakeHolderProject.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Principal;

namespace StakeHolderProject.Services
{
    public class ClaimsService : IClaimsService
    {
        private readonly IConfiguration _configuration;
        private ClaimsPrincipal _currentUser;

        public ClaimsService(IConfiguration configuration)
        {
            _configuration = configuration;
            _currentUser = null;
        }

        public string UserId => _currentUser != null ? GetUserId(_currentUser) : string.Empty;
        public string Username => _currentUser != null ? GetUsername(_currentUser) : string.Empty;
        public string Role => _currentUser != null ? GetUserRole(_currentUser) : string.Empty;
        public bool IsAdmin => _currentUser != null && IsAdminCheck(_currentUser);

        public void Initialize(ClaimsPrincipal user)
        {
            _currentUser = user;
        }

        public void Initialize(string token)
        {
            _currentUser = GetPrincipalFromToken(token);
        }

        public bool IsUserInRole(string role)
        {
            if (_currentUser == null)
                return false;

            var userRole = GetUserRole(_currentUser);
            return !string.IsNullOrEmpty(userRole) && userRole.Equals(role, StringComparison.OrdinalIgnoreCase);
        }

        public string GetUserId(ClaimsPrincipal user)
        {
            return user?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        }

        public string GetUsername(ClaimsPrincipal user)
        {
            return user?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        }

        public string GetUserRole(ClaimsPrincipal user)
        {
            return user?.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;
        }

        public bool IsUserInRole(ClaimsPrincipal user, string role)
        {
            var userRole = GetUserRole(user);
            return !string.IsNullOrEmpty(userRole) && userRole.Equals(role, StringComparison.OrdinalIgnoreCase);
        }

        public bool IsAdminCheck(ClaimsPrincipal user)
        {
            return IsUserInRole(user, UserRole.Admin.ToString());
        }

        public ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidAudience = _configuration["Jwt:Audience"],
                ValidateLifetime = false // Don't validate lifetime here as we may want to inspect expired tokens
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            
            try
            {
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
                var jwtSecurityToken = securityToken as JwtSecurityToken;
                
                if (jwtSecurityToken == null || 
                    !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    return null;
                }
                
                return principal;
            }
            catch
            {
                return null;
            }
        }
    }
} 