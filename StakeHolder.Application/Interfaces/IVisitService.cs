using StakeHolderProject.Models;

namespace StakeHolderProject.Services.Interfaces
{
    public interface IVisitService
    {
        IEnumerable<Visit> GetAllVisits();
        IEnumerable<Visit> GetVisitsByStakeholderId(Guid stakeholderId);
        Visit? GetVisitById(Guid id);
        Visit AddVisit(Visit visit, string adminUsername);
        Visit UpdateVisit(Visit visit, string adminUsername);
    }
} 