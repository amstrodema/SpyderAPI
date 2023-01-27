using MainAPI.Business.Spyder.Feature;
using MainAPI.Data.Interface;
using MainAPI.Generics;
using MainAPI.Models;
using MainAPI.Models.Spyder;
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
        private readonly IUnitOfWork unitOfWork;

        public FeatureGroupController(FeatureGroupBusiness featureGroupBusiness, JWTService jWTService, IUnitOfWork unitOfWork)
        {
            this.featureGroupBusiness = featureGroupBusiness;
            _jwtService = jWTService;
            this.unitOfWork = unitOfWork;
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
        public async Task<ActionResult> Post(RequestObject<FeatureGroup> requestObject)
        {
            FeatureGroup featureGroup = requestObject.Data;
            var rez = await ValidateLogIn.Validate(unitOfWork, requestObject.AppID, featureGroup.CreatedBy);
            if (rez.StatusCode != 200)
            {
                return Ok(rez);
            }

            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            var res = await featureGroupBusiness.Create(featureGroup);
            return Ok(res);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> Put([FromBody] RequestObject<FeatureGroup> requestObject, Guid id)
        {
            FeatureGroup featureGroup = requestObject.Data;
            var rez = await ValidateLogIn.Validate(unitOfWork, requestObject.AppID, featureGroup.ModifiedBy);
            if (rez.StatusCode != 200)
            {
                return Ok(rez);
            }

            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            if (featureGroup.ID != id)
                return BadRequest("Invalid record!");

            ResponseMessage<string> responseMessage = new ResponseMessage<string>();
            int res = await featureGroupBusiness.Update(featureGroup);

            if (res >= 1)
            {
                responseMessage.Message = "Record updated!";
                responseMessage.StatusCode = 200;
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
