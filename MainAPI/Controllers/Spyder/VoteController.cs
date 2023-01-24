using MainAPI.Business.Spyder;
using MainAPI.Models.Spyder;
using MainAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainAPI.Controllers.Spyder
{
    [ApiKeyAuth]
    [Route("api/[controller]")]
    [ApiController]
    public class VoteController : ControllerBase
    {
        private readonly VoteBusiness voteBusiness;
        private readonly JWTService _jwtService;

        public VoteController(VoteBusiness voteBusiness, JWTService jWTService)
        {
            this.voteBusiness = voteBusiness;
            _jwtService = jWTService;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var halls = await voteBusiness.GetVotes();
            return Ok(halls);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            var hall = await voteBusiness.GetVoteByID(id);
            return Ok(hall);
        }
        [HttpPost]
        public async Task<ActionResult> Post(Vote vote)
        {

            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            var res = await voteBusiness.Vote(vote);
            return Ok(res);
        }
    }
}
