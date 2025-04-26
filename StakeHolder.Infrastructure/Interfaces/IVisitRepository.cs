using StakeHolderProject.Models;

namespace StakeHolderProject.Repositories.Interfaces
{
    public interface IVisitRepository
    {
        IEnumerable<Visit> GetAll();
        IEnumerable<Visit> GetByStakeholderId(Guid stakeholderId);
        Visit? GetById(Guid id);
        Visit Add(Visit visit);
    }
} 