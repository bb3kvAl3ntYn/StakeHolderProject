using StakeHolderProject.Models;
using StakeHolderProject.Repositories.Interfaces;

namespace StakeHolderProject.Repositories
{
    public class InMemoryStakeholderRepository : IStakeholderRepository
    {
        private readonly List<Stakeholder> _stakeholders = new();

        public IEnumerable<Stakeholder> GetAll()
        {
            return _stakeholders.Where(s => s.Status != "Deleted");
        }

        public Stakeholder? GetById(Guid id)
        {
            return _stakeholders.FirstOrDefault(s => s.Id == id && s.Status != "Deleted");
        }

        public Stakeholder Add(Stakeholder stakeholder)
        {
            _stakeholders.Add(stakeholder);
            return stakeholder;
        }

        public bool Update(Guid id, Stakeholder updatedStakeholder)
        {
            var existingStakeholder = GetById(id);
            if (existingStakeholder == null)
            {
                return false;
            }

            existingStakeholder.Name = updatedStakeholder.Name;
            existingStakeholder.DateOfBirth = updatedStakeholder.DateOfBirth;
            existingStakeholder.Phone = updatedStakeholder.Phone;
            existingStakeholder.Designation = updatedStakeholder.Designation;
            existingStakeholder.Organization = updatedStakeholder.Organization;
            existingStakeholder.OrganizationType = updatedStakeholder.OrganizationType;
            existingStakeholder.JoiningDate = updatedStakeholder.JoiningDate;
            existingStakeholder.SeniorityLevel = updatedStakeholder.SeniorityLevel;

            return true;
        }

        public bool Delete(Guid id)
        {
            var stakeholder = GetById(id);
            if (stakeholder == null)
            {
                return false;
            }

            stakeholder.Status = "Deleted";
            return true;
        }

        public DateTime? GetOldestStakeholderCreationDate()
        {
            return _stakeholders.Any() ? _stakeholders.Min(s => s.CreatedDate) : null;
        }
    }
} 