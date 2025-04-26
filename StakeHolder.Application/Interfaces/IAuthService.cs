using System.Security.Claims;

namespace StakeHolderProject.Services.Interfaces
{
    public interface IAuthService
    {
        bool IsAdmin(string username);
        bool IsAdmin(ClaimsPrincipal principal);
        bool Authenticate(string username, string password);
        string GenerateJwtToken(string username);
    }
} 