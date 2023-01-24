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
    [Route("api/[controller]")]
    [ApiController]
    public class LikeController : ControllerBase
    {
        private readonly LikeBusiness likeBusiness;
        private readonly JWTService _jwtService;

        public LikeController(LikeBusiness likeBusiness, JWTService jWTService)
        {
            this.likeBusiness = likeBusiness;
            _jwtService = jWTService;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var halls = await likeBusiness.GetLikes();
            return Ok(halls);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            var hall = await likeBusiness.GetLikeByID(id);
            return Ok(hall);
        }
        [HttpPost]
        public async Task<ActionResult> Post(Like like)
        {

            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            var res = await likeBusiness.Like(like);
            return Ok(res);
        }
    }
}
