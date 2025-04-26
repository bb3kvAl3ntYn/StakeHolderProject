using System;
using StakeholderCommon.Enums;

namespace StakeholderCommon.DTOs
{
    public class VisitDto
    {
        public Guid Id { get; set; }
        
        public Guid StakeholderId { get; set; }
        
        public string VisitedPlace { get; set; } = string.Empty;
        
        public TimeSpan VisitedTime { get; set; }
        
        public DateTime VisitedDate { get; set; }
        
        public Gift Gift { get; set; }
    }

    public class VisitResponseDto : VisitDto
    {
        public string CreatedBy { get; set; } = string.Empty;
        
        public DateTime CreatedDate { get; set; }
    }
} 