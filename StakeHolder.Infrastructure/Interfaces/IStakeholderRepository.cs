using StakeHolderProject.Models;

namespace StakeHolderProject.Repositories.Interfaces
{
    public interface IStakeholderRepository
    {
        IEnumerable<Stakeholder> GetAll();
        Stakeholder? GetById(Guid id);
        Stakeholder Add(Stakeholder stakeholder);
        bool Update(Guid id, Stakeholder updatedStakeholder);
        bool Delete(Guid id);
        DateTime? GetOldestStakeholderCreationDate();
    }
} 