using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StakeHolderProject.Models;
using StakeholderCommon.DTOs;
using StakeholderCommon.Enums;
using StakeHolderProject.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace StakeHolderProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StakeholderController : ControllerBase
    {
        private readonly IStakeholderDataService _dataService;
        private readonly IAuthService _authService;
        private readonly IClaimsService _claimsService;

        public StakeholderController(
            IStakeholderDataService dataService, 
            IAuthService authService,
            IClaimsService claimsService)
        {
            _dataService = dataService;
            _authService = authService;
            _claimsService = claimsService;
        }

        [HttpGet]
        public IActionResult GetAllStakeholders()
        {
            var stakeholders = _dataService.GetAllStakeholders();
            var stakeholderDtos = stakeholders.Select(s => MapToResponseDto(s)).ToList();
            return Ok(stakeholderDtos);
        }

        [HttpGet("{id}")]
        public IActionResult GetStakeholderById(Guid id)
        {
            var stakeholder = _dataService.GetStakeholderById(id);
            if (stakeholder == null)
            {
                return NotFound();
            }

            return Ok(MapToResponseDto(stakeholder));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateStakeholder([FromBody] StakeholderDto stakeholderDto)
        {
            try
            {
                var stakeholder = new Stakeholder
                {
                    Name = stakeholderDto.Name,
                    DateOfBirth = stakeholderDto.DateOfBirth,
                    Phone = stakeholderDto.Phone,
                    Designation = stakeholderDto.Designation,
                    Organization = stakeholderDto.Organization,
                    OrganizationType = stakeholderDto.OrganizationType,
                    JoiningDate = stakeholderDto.JoiningDate,
                    SeniorityLevel = stakeholderDto.SeniorityLevel
                };

                var createdStakeholder = _dataService.AddStakeholder(stakeholder, _claimsService.Username);
                return CreatedAtAction(nameof(GetStakeholderById), new { id = createdStakeholder.Id }, MapToResponseDto(createdStakeholder));
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateStakeholder(Guid id, [FromBody] StakeholderDto stakeholderDto)
        {
            try
            {
                var stakeholder = new Stakeholder
                {
                    Name = stakeholderDto.Name,
                    DateOfBirth = stakeholderDto.DateOfBirth,
                    Phone = stakeholderDto.Phone,
                    Designation = stakeholderDto.Designation,
                    Organization = stakeholderDto.Organization,
                    OrganizationType = stakeholderDto.OrganizationType,
                    JoiningDate = stakeholderDto.JoiningDate,
                    SeniorityLevel = stakeholderDto.SeniorityLevel
                };

                var result = _dataService.UpdateStakeholder(id, stakeholder, _claimsService.Username);
                if (!result)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteStakeholder(Guid id)
        {
            var result = _dataService.DeleteStakeholder(id, _claimsService.Username);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        private StakeholderResponseDto MapToResponseDto(Stakeholder stakeholder)
        {
            return new StakeholderResponseDto
            {
                Id = stakeholder.Id,
                Name = stakeholder.Name,
                DateOfBirth = stakeholder.DateOfBirth,
                Phone = stakeholder.Phone,
                Designation = stakeholder.Designation,
                Organization = stakeholder.Organization,
                OrganizationType = stakeholder.OrganizationType,
                JoiningDate = stakeholder.JoiningDate,
                SeniorityLevel = stakeholder.SeniorityLevel,
                CreatedBy = stakeholder.CreatedBy,
                CreatedDate = stakeholder.CreatedDate,
                Status = stakeholder.Status,
                Visits = stakeholder.Visits.Select(v => new VisitResponseDto
                {
                    Id = v.Id,
                    StakeholderId = v.StakeholderId,
                    VisitedPlace = v.VisitedPlace,
                    VisitedTime = v.VisitedTime,
                    VisitedDate = v.VisitedDate,
                    Gift = v.Gift,
                    CreatedBy = v.CreatedBy,
                    CreatedDate = v.CreatedDate
                }).ToList()
            };
        }
    }
}
