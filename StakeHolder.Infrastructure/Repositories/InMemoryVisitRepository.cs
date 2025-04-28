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
        
        public Visit Update(Visit visit)
        {
            var existingVisit = GetById(visit.Id);
            if (existingVisit == null)
            {
                throw new KeyNotFoundException($"Visit with ID {visit.Id} not found");
            }
            
            // Remove old visit from the list
            _visits.Remove(existingVisit);
            
            // If the stakeholder ID changed, update the stakeholder references
            if (existingVisit.StakeholderId != visit.StakeholderId)
            {
                // Remove from the old stakeholder's visits list
                var oldStakeholder = _stakeholderRepository.GetById(existingVisit.StakeholderId);
                if (oldStakeholder != null)
                {
                    var visitToRemove = oldStakeholder.Visits.FirstOrDefault(v => v.Id == visit.Id);
                    if (visitToRemove != null)
                    {
                        oldStakeholder.Visits.Remove(visitToRemove);
                    }
                }
                
                // Add to the new stakeholder's visits list
                var newStakeholder = _stakeholderRepository.GetById(visit.StakeholderId);
                if (newStakeholder != null && !newStakeholder.Visits.Any(v => v.Id == visit.Id))
                {
                    newStakeholder.Visits.Add(visit);
                }
            }
            
            // Add the updated visit back to the list
            _visits.Add(visit);
            
            return visit;
        }
    }
} 