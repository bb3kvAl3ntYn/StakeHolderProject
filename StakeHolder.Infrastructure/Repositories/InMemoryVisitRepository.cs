using StakeHolderProject.Models;
using StakeHolderProject.Repositories.Interfaces;

namespace StakeHolderProject.Repositories
{
    public class InMemoryVisitRepository : IVisitRepository
    {
        private readonly List<Visit> _visits = new();
        private readonly IStakeholderRepository _stakeholderRepository;

        public InMemoryVisitRepository(IStakeholderRepository stakeholderRepository)
        {
            _stakeholderRepository = stakeholderRepository;
        }

        public IEnumerable<Visit> GetAll()
        {
            return _visits;
        }

        public IEnumerable<Visit> GetByStakeholderId(Guid stakeholderId)
        {
            return _visits.Where(v => v.StakeholderId == stakeholderId);
        }

        public Visit? GetById(Guid id)
        {
            return _visits.FirstOrDefault(v => v.Id == id);
        }

        public Visit Add(Visit visit)
        {
            _visits.Add(visit);
            
            var stakeholder = _stakeholderRepository.GetById(visit.StakeholderId);
            if (stakeholder != null && !stakeholder.Visits.Any(v => v.Id == visit.Id))
            {
                stakeholder.Visits.Add(visit);
            }
            
            return visit;
        }
    }
} 