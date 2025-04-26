using StakeholderCommon.Enums;

namespace StakeHolderProject.Models
{
    public class Visit
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        
        public Guid StakeholderId { get; set; }
        
        public string VisitedPlace { get; set; } = string.Empty;
        
        public TimeSpan VisitedTime { get; set; }
        
        public DateTime VisitedDate { get; set; }
        
        public Gift Gift { get; set; }
        
        public string CreatedBy { get; set; } = string.Empty;
        
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        
        public Stakeholder? Stakeholder { get; set; }
    }
} 