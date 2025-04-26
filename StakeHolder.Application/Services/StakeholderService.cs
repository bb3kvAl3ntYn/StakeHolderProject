using StakeHolderProject.Models;
using StakeHolderProject.Repositories.Interfaces;
using StakeHolderProject.Services.Interfaces;

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
            return _stakeholderRepository.GetAll();
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

            var oldestDate = _stakeholderRepository.GetOldestStakeholderCreationDate();
            if (oldestDate.HasValue && oldestDate.Value == existingStakeholder.CreatedDate)
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

            return _stakeholderRepository.Delete(id);
        }
    }
} 