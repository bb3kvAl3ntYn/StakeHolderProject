using StakeHolderProject.Models;

namespace StakeHolderProject.Services.Interfaces
{
    public interface IUserService
    {
        User? GetUserByUsername(string username);
        bool ValidateUser(string username, string password);
        User CreateUser(User user);
        IEnumerable<User> GetAllUsers();
        void SeedDefaultUsers();
    }
} 