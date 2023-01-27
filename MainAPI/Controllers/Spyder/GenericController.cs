using MainAPI.Business.Spyder;
using MainAPI.Data.Interface;
using MainAPI.Generics;
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
    public class GenericController : ControllerBase
    {
        private readonly GenericBusiness genericBusiness;
        private readonly JWTService _jwtService;
        private readonly IUnitOfWork unitOfWork;

        public GenericController(GenericBusiness genericBusiness, JWTService jWTService, IUnitOfWork unitOfWork)
        {
            this.genericBusiness = genericBusiness;
            _jwtService = jWTService;
            this.unitOfWork = unitOfWork;
        }

        //[HttpGet]
        //public async Task<ActionResult> Get()
        //{
        //    var paramz = await paramsBusiness.GetParams();
        //    return Ok(paramz);
        //}

        //[HttpGet("{id}")]
        //public async Task<ActionResult> Get(Guid id)
        //{
        //    var param = await paramsBusiness.GetParamsByID(id);
        //    return Ok(param);
        //}
        [HttpPost("GetTrending")]
        public async Task<ActionResult> GetTrending(RequestObject<string> requestObject)
        {
            var trends = await genericBusiness.GetTrending(requestObject);
            return Ok(trends);
        }
        [HttpPost("GetSearchResult")]
        public async Task<ActionResult> GetSearchResult(RequestObject<string> requestObject)
        {
            var searchResult = await genericBusiness.GetSearchResult(requestObject);
            return Ok(searchResult);
        }
        [HttpGet("GetProfileContent")]
        public async Task<ActionResult> GetProfileContent(Guid userID, Guid appID, Guid profileUserID)
        {
            var rez = await ValidateLogIn.Validate(unitOfWork, appID, userID);
            bool isUser = true;

            if (rez.StatusCode != 200)
            {
                isUser = false;
            }

            var profileData = await genericBusiness.GetProfileContent(profileUserID, isUser);
            return Ok(profileData);
        }
        [HttpGet("GetProfileContentWithComment")]
        public async Task<ActionResult> GetProfileContentWithComment(Guid userID)
        {
            var profileData = await genericBusiness.GetProfileContentWithComment(userID);
            return Ok(profileData);
        }
        [HttpGet("GetProfileContentWithReaction")]
        public async Task<ActionResult> GetProfileContentWithReaction(Guid userID)
        {

            var profileData = await genericBusiness.GetProfileContentWithReaction(userID);
            return Ok(profileData);
        }
    }
}
