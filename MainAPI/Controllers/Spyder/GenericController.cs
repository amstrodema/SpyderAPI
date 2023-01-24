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
    public class GenericController : ControllerBase
    {
        private readonly GenericBusiness genericBusiness;
        private readonly JWTService _jwtService;

        public GenericController(GenericBusiness genericBusiness, JWTService jWTService)
        {
            this.genericBusiness = genericBusiness;
            _jwtService = jWTService;
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
        public async Task<ActionResult> GetProfileContent(Guid userID)
        {
            var profileData = await genericBusiness.GetProfileContent(userID);
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
