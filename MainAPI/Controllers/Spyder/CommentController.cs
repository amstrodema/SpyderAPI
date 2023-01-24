using MainAPI.Business.Spyder;
using MainAPI.Models.Comment.Spyder;
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
    public class CommentController : ControllerBase
    {
        private readonly CommentBusiness commentBusiness;
        private readonly JWTService _jwtService;

        public CommentController(CommentBusiness commentBusiness, JWTService jWTService)
        {
            this.commentBusiness = commentBusiness;
            _jwtService = jWTService;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var countries = await commentBusiness.GetComments();
            return Ok(countries);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            var country = await commentBusiness.GetCommentByID(id);
            return Ok(country);
        }
        [HttpGet("GetCommentVMsByItemID")]
        public async Task<ActionResult> GetCommentVMsByItemID(Guid itemID)
        {
            var country = await commentBusiness.GetCommentVMsByItemID(itemID);
            return Ok(country);
        }
        [HttpPost]
        public async Task<ActionResult> Post(Comment comment)
        {

            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            var res = await commentBusiness.Comment(comment);
            return Ok(res);
        }
    }
}
