using MainAPI.Business.Spyder;
using MainAPI.Data.Interface;
using MainAPI.Generics;
using MainAPI.Models.Comment.Spyder;
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
    public class CommentController : ControllerBase
    {
        private readonly CommentBusiness commentBusiness;
        private readonly JWTService _jwtService;
        private readonly IUnitOfWork _unitOfWork;

        public CommentController(CommentBusiness commentBusiness, JWTService jWTService, IUnitOfWork unitOfWork)
        {
            this.commentBusiness = commentBusiness;
            _jwtService = jWTService;
            _unitOfWork = unitOfWork;
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
        public async Task<ActionResult> Post(RequestObject<Comment> requestObject)
        {
            var rez = await ValidateLogIn.Validate(_unitOfWork, requestObject.AppID, requestObject.Data.UserID);
            if (rez.StatusCode != 200)
            {
                return Ok(rez);
            }

            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            var res = await commentBusiness.Comment(requestObject);
            return Ok(res);
        }
    }
}
