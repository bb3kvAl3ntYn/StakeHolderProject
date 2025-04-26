namespace StakeHolderProject.Models
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        
        public string Username { get; set; } = string.Empty;
        
        public string Password { get; set; } = string.Empty;
        
        public UserRole Role { get; set; }
    }

    public enum UserRole
    {
        Regular,
        Admin
    }
} 