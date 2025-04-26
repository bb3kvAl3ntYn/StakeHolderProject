using StakeHolderProject.Models;

namespace StakeHolderProject.Repositories.Interfaces
{
    public interface IUserRepository
    {
        User? GetByUsername(string username);
        bool ValidateCredentials(string username, string password);
        User Create(User user);
        IEnumerable<User> GetAll();
        void SeedDefaultUsers();
    }
} 