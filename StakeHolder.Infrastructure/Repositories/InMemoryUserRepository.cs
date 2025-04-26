using StakeHolderProject.Models;
using StakeHolderProject.Repositories.Interfaces;

namespace StakeHolderProject.Repositories
{
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly List<User> _users = new();

        public InMemoryUserRepository()
        {
            SeedDefaultUsers();
        }

        public User? GetByUsername(string username)
        {
            return _users.FirstOrDefault(u => u.Username == username);
        }

        public bool ValidateCredentials(string username, string password)
        {
            return _users.Any(u => u.Username == username && u.Password == password);
        }

        public User Create(User user)
        {
            user.Id = Guid.NewGuid();
            _users.Add(user);
            return user;
        }

        public IEnumerable<User> GetAll()
        {
            return _users;
        }

        public void SeedDefaultUsers()
        {
            if (!_users.Any())
            {
                _users.Add(new User 
                { 
                    Username = "admin1", 
                    Password = "admin1", 
                    Role = UserRole.Admin 
                });
                
                _users.Add(new User 
                { 
                    Username = "admin2", 
                    Password = "admin2", 
                    Role = UserRole.Admin 
                });
                
                _users.Add(new User 
                { 
                    Username = "user1", 
                    Password = "user1", 
                    Role = UserRole.Regular 
                });
            }
        }
    }
} 