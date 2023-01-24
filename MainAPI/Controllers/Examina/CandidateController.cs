using MainAPI.Business.Examina;
using MainAPI.Models;
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
    public class CandidateController : ControllerBase
    {
        private readonly CandidateBusiness _CandidateBusiness;
        private readonly JWTService _jwtService;
        private readonly UserBusiness _userBusiness;

        public CandidateController(CandidateBusiness CandidateBusiness, JWTService jWTService, UserBusiness userBusiness)
        {
            _CandidateBusiness = CandidateBusiness;
            _jwtService = jWTService;
            _userBusiness = userBusiness;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var Candidates = await _CandidateBusiness.GetCandidates();
            return Ok(Candidates);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            var Candidate = await _CandidateBusiness.GetCandidateByID(id);
            return Ok(Candidate);
        }

        [HttpGet("GetCandidatesByNodeID")]
        public async Task<ActionResult> GetCandidatesByNodeID(Guid nodeID)
        {
            var res = await _CandidateBusiness.GetCandidatesByNodeID(nodeID);
            return Ok(res);
        }

        [HttpPost]
        public async Task<ActionResult> Post(Candidate Candidate)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            await _CandidateBusiness.Create(Candidate);
            return Ok(Candidate);
        }

        [HttpPost("PostMultiple")]
        public async Task<ActionResult> PostMultiple(Candidate[] Candidates)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            var res = await _CandidateBusiness.CreateMultiple(Candidates);
            return Ok(res);
        }

        [HttpPut("EditCandidate")]
        public async Task<ActionResult> EditCandidate(Candidate Candidate)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            var res = await _CandidateBusiness.Update(Candidate);
            return Ok(res);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _CandidateBusiness.Delete(id);
            return Ok("Record deleted successfully");
        }

        [HttpPost("CandidateLogin")]
        public async Task<ActionResult> CandidateLogin(LoginVM login)
        {
            var res = await _userBusiness.ValidateUser(login);
            if (res.StatusCode == 1018)
            {
                var response = await _CandidateBusiness.ValidateCandidate(login);
                response.Flag = 2;
                return Ok(response);
            }
            
            return Ok(res);
        }
    }
}
