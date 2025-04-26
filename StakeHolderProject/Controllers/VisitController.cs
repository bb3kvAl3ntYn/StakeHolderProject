using Microsoft.AspNetCore.Mvc;
using StakeHolderProject.Models;
using StakeholderCommon.DTOs;
using StakeholderCommon.Enums;
using StakeHolderProject.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace StakeHolderProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class VisitController : ControllerBase
    {
        private readonly IVisitService _visitService;
        private readonly IStakeholderDataService _stakeholderService;
        private readonly IAuthService _authService;
        private readonly IClaimsService _claimsService;

        public VisitController(
            IVisitService visitService, 
            IStakeholderDataService stakeholderService, 
            IAuthService authService,
            IClaimsService claimsService)
        {
            _visitService = visitService;
            _stakeholderService = stakeholderService;
            _authService = authService;
            _claimsService = claimsService;
        }

        [HttpGet]
        public IActionResult GetAllVisits()
        {
            var visits = _visitService.GetAllVisits();
            var visitDtos = visits.Select(MapToResponseDto).ToList();
            return Ok(visitDtos);
        }

        [HttpGet("stakeholder/{stakeholderId}")]
        public IActionResult GetVisitsByStakeholderId(Guid stakeholderId)
        {
            var stakeholder = _stakeholderService.GetStakeholderById(stakeholderId);
            if (stakeholder == null)
            {
                return NotFound("Stakeholder not found");
            }

            var visits = _visitService.GetVisitsByStakeholderId(stakeholderId);
            var visitDtos = visits.Select(MapToResponseDto).ToList();
            return Ok(visitDtos);
        }

        [HttpGet("{id}")]
        public IActionResult GetVisitById(Guid id)
        {
            var visit = _visitService.GetVisitById(id);
            if (visit == null)
            {
                return NotFound();
            }

            return Ok(MapToResponseDto(visit));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateVisit([FromBody] VisitDto visitDto)
        {
            try
            {
                var visit = new Visit
                {
                    StakeholderId = visitDto.StakeholderId,
                    VisitedPlace = visitDto.VisitedPlace,
                    VisitedTime = visitDto.VisitedTime,
                    VisitedDate = visitDto.VisitedDate,
                    Gift = visitDto.Gift
                };

                var createdVisit = _visitService.AddVisit(visit, _claimsService.Username);
                return CreatedAtAction(nameof(GetVisitById), new { id = createdVisit.Id }, MapToResponseDto(createdVisit));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
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

        private VisitResponseDto MapToResponseDto(Visit visit)
        {
            return new VisitResponseDto
            {
                Id = visit.Id,
                StakeholderId = visit.StakeholderId,
                VisitedPlace = visit.VisitedPlace,
                VisitedTime = visit.VisitedTime,
                VisitedDate = visit.VisitedDate,
                Gift = visit.Gift,
                CreatedBy = visit.CreatedBy,
                CreatedDate = visit.CreatedDate
            };
        }
    }
} 