using MainAPI.Business.Examina;
using MainAPI.Models.Examina;
using MainAPI.Models.ViewModel.Examina;
using MainAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainAPI.Controllers.Examina
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandidateGroupController : ControllerBase
    {
        private readonly CandidateGroupBusiness _CandidateGroupBusiness;
        private readonly JWTService _jwtService;

        public CandidateGroupController(CandidateGroupBusiness CandidateGroupBusiness, JWTService jWTService)
        {
            _CandidateGroupBusiness = CandidateGroupBusiness;
            _jwtService = jWTService;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var CandidateGroups = await _CandidateGroupBusiness.GetCandidateGroups();
            return Ok(CandidateGroups);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            var CandidateGroup = await _CandidateGroupBusiness.GetCandidateGroupByID(id);
            return Ok(CandidateGroup);
        }

        [HttpGet("GetCandidateGroupsByGroupID")]
        public async Task<ActionResult> GetCandidateGroupsByGroupID(Guid groupID)
        {
            var res = await _CandidateGroupBusiness.GetCandidateGroupsByGroupID(groupID);
            return Ok(res);
        }

        [HttpGet("GetCandidateGroupsByCandidateID")]
        public async Task<ActionResult> GetCandidateGroupsByCandidateID(Guid candidateID)
        {
            var res = await _CandidateGroupBusiness.GetCandidateGroupsByCandidateID(candidateID);
            return Ok(res);
        }

        [HttpPost]
        public async Task<ActionResult> Post(CandidateGroup CandidateGroup)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            await _CandidateGroupBusiness.Create(CandidateGroup);
            return Ok(CandidateGroup);
        }

        [HttpPost("AttachCandidatesToGroup")]
        public async Task<ActionResult> AttachCandidatesToGroup(CandidateGroupVM candidateVM)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            var res = await _CandidateGroupBusiness.AttachCandidatesToGroup(candidateVM);
            return Ok(res);
        }

        [HttpPost("AttachGroupsToCandidate")]
        public async Task<ActionResult> AttachGroupsToCandidate(CandidateGroupVM candidateVM)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            var res = await _CandidateGroupBusiness.AttachGroupsToCandidate(candidateVM);
            return Ok(res);
        }

        [HttpPut("EditCandidateGroup")]
        public async Task<ActionResult> EditCandidateGroup(CandidateGroupVM candidateGroupVM)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

           var res = await _CandidateGroupBusiness.Update(candidateGroupVM);
            return Ok(res);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _CandidateGroupBusiness.Delete(id);
            return Ok("Record deleted successfully");
        }
    }
}
