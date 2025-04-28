using StakeHolderProject.Models;
using StakeHolderProject.Repositories.Interfaces;
using StakeHolderProject.Services.Interfaces;

namespace StakeHolderProject.Services
{
    public class VisitService : IVisitService
    {
        private readonly IVisitRepository _visitRepository;
        private readonly IStakeholderRepository _stakeholderRepository;
        private readonly IAuthService _authService;

        public VisitService(
            IVisitRepository visitRepository,
            IStakeholderRepository stakeholderRepository,
            IAuthService authService)
        {
            _visitRepository = visitRepository;
            _stakeholderRepository = stakeholderRepository;
            _authService = authService;
        }

        public IEnumerable<Visit> GetAllVisits()
        {
            return _visitRepository.GetAll();
        }

        public IEnumerable<Visit> GetVisitsByStakeholderId(Guid stakeholderId)
        {
            return _visitRepository.GetByStakeholderId(stakeholderId);
        }

        public Visit? GetVisitById(Guid id)
        {
            return _visitRepository.GetById(id);
        }

        public Visit AddVisit(Visit visit, string adminUsername)
        {
            if (!_authService.IsAdmin(adminUsername))
            {
                throw new UnauthorizedAccessException("Only admins can add visits");
            }

            var stakeholder = _stakeholderRepository.GetById(visit.StakeholderId);
            if (stakeholder == null)
            {
                throw new KeyNotFoundException("Stakeholder not found");
            }

            if (stakeholder.CreatedBy != adminUsername)
            {
                throw new UnauthorizedAccessException("Admins can only add visits for stakeholders they created");
            }

            visit.Id = Guid.NewGuid();
            visit.CreatedBy = adminUsername;
            visit.CreatedDate = DateTime.UtcNow;
            return _visitRepository.Add(visit);
        }

        public Visit UpdateVisit(Visit visit, string adminUsername)
        {
            if (!_authService.IsAdmin(adminUsername))
            {
                throw new UnauthorizedAccessException("Only admins can update visits");
            }

            var stakeholder = _stakeholderRepository.GetById(visit.StakeholderId);
            if (stakeholder == null)
            {
                throw new KeyNotFoundException("Stakeholder not found");
            }

            if (stakeholder.CreatedBy != adminUsername)
            {
                throw new UnauthorizedAccessException("Admins can only update visits for stakeholders they created");
            }

            return _visitRepository.Update(visit);
        }
    }
} 