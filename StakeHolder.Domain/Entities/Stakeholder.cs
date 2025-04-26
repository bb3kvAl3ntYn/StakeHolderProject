using StakeholderCommon.Enums;
using System.ComponentModel.DataAnnotations;

namespace StakeHolderProject.Models
{
    public class Stakeholder
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        
        [Required]
        public string Name { get; set; } = string.Empty;
        
        public DateTime? DateOfBirth { get; set; }
        
        public string? Phone { get; set; }
        
        public string? Designation { get; set; }
        
        public string? Organization { get; set; }
        
        public OrganizationType OrganizationType { get; set; }
        
        public DateTime? JoiningDate { get; set; }
        
        public string CreatedBy { get; set; } = string.Empty;
        
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        
        public string Status { get; set; } = "Active";
        
        public SeniorityLevel SeniorityLevel { get; set; }
        
        public List<Visit> Visits { get; set; } = new List<Visit>();
    }
} 