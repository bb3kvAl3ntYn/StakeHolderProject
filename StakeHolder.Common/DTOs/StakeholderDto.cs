using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using StakeholderCommon.Enums;

namespace StakeholderCommon.DTOs
{
    public class StakeholderDto
    {
        public Guid Id { get; set; }
        
        [Required]
        public string Name { get; set; } = string.Empty;
        
        public DateTime? DateOfBirth { get; set; }
        
        public string? Phone { get; set; }
        
        public string? Designation { get; set; }
        
        public string? Organization { get; set; }
        
        public OrganizationType OrganizationType { get; set; }
        
        public DateTime? JoiningDate { get; set; }
        
        public SeniorityLevel SeniorityLevel { get; set; }
    }

    public class StakeholderResponseDto : StakeholderDto
    {
        public string CreatedBy { get; set; } = string.Empty;
        
        public DateTime CreatedDate { get; set; }
        
        public string Status { get; set; } = string.Empty;
        
        public ICollection<VisitResponseDto> Visits { get; set; } = new List<VisitResponseDto>();
    }
} 