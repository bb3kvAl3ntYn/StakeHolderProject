using StakeHolderProject.Models;
using StakeHolderProject.Repositories.Interfaces;
using StakeHolderProject.Services.Interfaces;
using System.Linq;

namespace StakeHolderProject.Services
{
    public class StakeholderService : IStakeholderDataService
    {
        private readonly IStakeholderRepository _stakeholderRepository;
        private readonly IAuthService _authService;

        public StakeholderService(IStakeholderRepository stakeholderRepository, IAuthService authService)
        {
            _stakeholderRepository = stakeholderRepository;
            _authService = authService;
        }

        public IEnumerable<Stakeholder> GetAllStakeholders()
        {
            var stakeholders = _stakeholderRepository.GetAll();
            
            // Sort stakeholders based on the specified criteria:
            // 1. SeniorityLevel (higher level is newer)
            // 2. Creation Year (later year is newer)
            // 3. Lexical name comparison (alphabetically later is newer)
            return stakeholders.OrderByDescending(s => s.SeniorityLevel)
                             .ThenBy(s => s.CreatedDate.Year)
                             .ThenBy(s => s.Name);
        }

        public Stakeholder? GetStakeholderById(Guid id)
        {
            return _stakeholderRepository.GetById(id);
        }

        public Stakeholder AddStakeholder(Stakeholder stakeholder, string adminUsername)
        {
            if (!_authService.IsAdmin(adminUsername))
            {
                throw new UnauthorizedAccessException("Only admins can create stakeholders");
            }
            
            stakeholder.CreatedBy = adminUsername;
            stakeholder.CreatedDate = DateTime.UtcNow;
            return _stakeholderRepository.Add(stakeholder);
        }

        public bool UpdateStakeholder(Guid id, Stakeholder updatedStakeholder, string adminUsername)
        {
            if (!_authService.IsAdmin(adminUsername))
            {
                throw new UnauthorizedAccessException("Only admins can update stakeholders");
            }

            var existingStakeholder = GetStakeholderById(id);
            if (existingStakeholder == null)
            {
                return false;
            }

            // Get the oldest stakeholder based on our custom sorting criteria
            var stakeholders = _stakeholderRepository.GetAll();
            var oldestStakeholder = stakeholders
                .OrderBy(s => s.SeniorityLevel)
                .ThenBy(s => s.CreatedDate.Year)
                .ThenBy(s => s.Name)
                .FirstOrDefault();

            // Prevent updating the oldest stakeholder
            if (oldestStakeholder != null && oldestStakeholder.Id == existingStakeholder.Id)
            {
                throw new InvalidOperationException("Cannot update the oldest stakeholder in the system");
            }

            return _stakeholderRepository.Update(id, updatedStakeholder);
        }

        public bool DeleteStakeholder(Guid id, string adminUsername)
        {
            if (!_authService.IsAdmin(adminUsername))
            {
                throw new UnauthorizedAccessException("Only admins can delete stakeholders");
            }

            var existingStakeholder = GetStakeholderById(id);
            if (existingStakeholder == null)
            {
                return false;
            }

            // Get the oldest stakeholder based on our custom sorting criteria
            var stakeholders = _stakeholderRepository.GetAll();
            var oldestStakeholder = stakeholders
                .OrderBy(s => s.SeniorityLevel)
                .ThenBy(s => s.CreatedDate.Year)
                .ThenBy(s => s.Name)
                .FirstOrDefault();

            // Prevent deleting the oldest stakeholder
            if (oldestStakeholder != null && oldestStakeholder.Id == existingStakeholder.Id)
            {
                throw new InvalidOperationException("Cannot delete the oldest stakeholder in the system");
            }

            return _stakeholderRepository.Delete(id);
        }
    }
} 