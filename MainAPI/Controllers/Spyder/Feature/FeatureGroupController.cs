using MainAPI.Business.Spyder.Feature;
using MainAPI.Models;
using MainAPI.Models.Spyder.Feature;
using MainAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainAPI.Controllers.Spyder.Feature
{
    [ApiKeyAuth]
    [Route("api/[controller]")]
    [ApiController]
    public class FeatureGroupController : ControllerBase
    {
        private readonly FeatureGroupBusiness featureGroupBusiness;
        private readonly JWTService _jwtService;

        public FeatureGroupController(FeatureGroupBusiness featureGroupBusiness, JWTService jWTService)
        {
            this.featureGroupBusiness = featureGroupBusiness;
            _jwtService = jWTService;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var featureGroups = await featureGroupBusiness.GetFeatureGroups();
            return Ok(featureGroups);
        }
        [HttpGet("GetFeatureGroupByGroupNo")]
        public async Task<ActionResult> GetFeatureGroupByGroupNo(int groupNo)
        {
            var featureGroups = await featureGroupBusiness.GetFeatureGroupByGroupNo(groupNo);
            return Ok(featureGroups);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            var featureGroup = await featureGroupBusiness.GetFeatureGroupByID(id);
            return Ok(featureGroup);
        }
        [HttpPost]
        public async Task<ActionResult> Post(FeatureGroup featureGroup)
        {

            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            var res = await featureGroupBusiness.Create(featureGroup);
            return Ok(res);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> Put([FromBody] FeatureGroup featureGroup, Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            if (featureGroup.ID != id)
                return BadRequest("Invalid record!");

            int res = await featureGroupBusiness.Update(featureGroup);
            ResponseMessage<FeatureGroup> responseMessage = new ResponseMessage<FeatureGroup>();
            if (res >= 1)
            {
                responseMessage.Message = "Record updated!";
                responseMessage.StatusCode = 200;
                responseMessage.Data = featureGroup;
            }
            else
            {
                responseMessage.Message = "No record updated!";
                responseMessage.StatusCode = 201;
            }
            return Ok(responseMessage);
        }

        //[HttpDelete("{id}")]
        //public async Task<ActionResult> Delete(Guid id)
        //{
        //    int res = await featureGroupBusiness.Delete(id);
        //    ResponseMessage<string> responseMessage = new ResponseMessage<string>();
        //    if (res >= 1)
        //    {
        //        responseMessage.Message = "Record deleted!";
        //        responseMessage.StatusCode = 200;
        //    }
        //    else
        //    {
        //        responseMessage.Message = "No record deleted!";
        //        responseMessage.StatusCode = 201;
        //    }
        //    return Ok(responseMessage);
        //}
    }
}
