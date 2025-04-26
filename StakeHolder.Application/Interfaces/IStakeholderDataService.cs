using StakeHolderProject.Models;

namespace StakeHolderProject.Services.Interfaces
{
    public interface IStakeholderDataService
    {
        IEnumerable<Stakeholder> GetAllStakeholders();
        Stakeholder? GetStakeholderById(Guid id);
        Stakeholder AddStakeholder(Stakeholder stakeholder, string adminUsername);
        bool UpdateStakeholder(Guid id, Stakeholder updatedStakeholder, string adminUsername);
        bool DeleteStakeholder(Guid id, string adminUsername);
    }
} 