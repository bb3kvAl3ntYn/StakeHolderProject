using System.Security.Claims;

namespace StakeHolderProject.Services.Interfaces
{
    public interface IClaimsService
    {
        string UserId { get; }
        string Username { get; }
        string Role { get; }
        bool IsAdmin { get; }
        void Initialize(ClaimsPrincipal user);
        void Initialize(string token);
        bool IsUserInRole(string role);
        string GetUserId(ClaimsPrincipal user);
        string GetUsername(ClaimsPrincipal user);
        string GetUserRole(ClaimsPrincipal user);
        bool IsUserInRole(ClaimsPrincipal user, string role);
        bool IsAdminCheck(ClaimsPrincipal user);
        ClaimsPrincipal GetPrincipalFromToken(string token);
    }
} 