using System.ComponentModel.DataAnnotations;

namespace StakeholderCommon.DTOs
{
    public class LoginDto
    {
        [Required]
        public string Username { get; set; } = string.Empty;
        
        [Required]
        public string Password { get; set; } = string.Empty;
    }

    public class AuthResponseDto
    {
        public string Username { get; set; } = string.Empty;
        
        public string Role { get; set; } = string.Empty;
        
        public string Token { get; set; } = string.Empty;
    }
} 