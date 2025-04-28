using Microsoft.AspNetCore.Mvc;
using StakeholderCommon.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StakeHolderProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnumsController : ControllerBase
    {
        [HttpGet("SeniorityLevels")]
        public IActionResult GetSeniorityLevels()
        {
            var levels = Enum.GetValues(typeof(SeniorityLevel))
                .Cast<SeniorityLevel>()
                .Select(e => new { Id = (int)e, Name = e.ToString() })
                .ToList();
                
            return Ok(levels);
        }

        [HttpGet("OrganizationTypes")]
        public IActionResult GetOrganizationTypes()
        {
            var types = Enum.GetValues(typeof(OrganizationType))
                .Cast<OrganizationType>()
                .Select(e => new { Id = (int)e, Name = e.ToString() })
                .ToList();
                
            return Ok(types);
        }
        
        [HttpGet("Gifts")]
        public IActionResult GetGifts()
        {
            var gifts = Enum.GetValues(typeof(Gift))
                .Cast<Gift>()
                .Select(e => new { Id = (int)e, Name = e.ToString() })
                .ToList();
                
            return Ok(gifts);
        }
    }
} 