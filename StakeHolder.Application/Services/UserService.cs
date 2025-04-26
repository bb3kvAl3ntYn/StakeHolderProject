using StakeHolderProject.Models;
using StakeHolderProject.Repositories.Interfaces;
using StakeHolderProject.Services.Interfaces;

namespace StakeHolderProject.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User? GetUserByUsername(string username)
        {
            return _userRepository.GetByUsername(username);
        }

        public bool ValidateUser(string username, string password)
        {
            return _userRepository.ValidateCredentials(username, password);
        }

        public User CreateUser(User user)
        {
            return _userRepository.Create(user);
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _userRepository.GetAll();
        }

        public void SeedDefaultUsers()
        {
            _userRepository.SeedDefaultUsers();
        }
    }
} 