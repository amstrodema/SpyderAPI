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
    public class FeatureTypeController : ControllerBase
    {
        private readonly FeatureTypeBusiness featureTypeBusiness;
        private readonly JWTService _jwtService;
        private readonly IUnitOfWork unitOfWork;

        public FeatureTypeController(FeatureTypeBusiness featureTypeBusiness, JWTService jWTService, IUnitOfWork unitOfWork)
        {
            this.featureTypeBusiness = featureTypeBusiness;
            _jwtService = jWTService;
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var featureTypes = await featureTypeBusiness.GetFeatureTypes();
            return Ok(featureTypes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            var featureType = await featureTypeBusiness.GetFeatureTypeByID(id);
            return Ok(featureType);
        }

        [HttpGet("GetFeatureTypesByGroupCode")]
        public async Task<ActionResult> GetFeatureTypesByGroupCode(string code)
        {
            var featureType = await featureTypeBusiness.GetFeatureTypesByGroupCode(code);
            return Ok(featureType);
        }
        [HttpPost]
        public async Task<ActionResult> Post(RequestObject<FeatureType> requestObject)
        {
            FeatureType featureType = requestObject.Data;
            var rez = await ValidateLogIn.Validate(unitOfWork, requestObject.AppID, featureType.CreatedBy);
            if (rez.StatusCode != 200)
            {
                return Ok(rez);
            }

            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            var res = await featureTypeBusiness.Create(featureType);
            return Ok(res);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> Put([FromBody] RequestObject<FeatureType> requestObject, Guid id)
        {
            FeatureType featureType = requestObject.Data;
            var rez = await ValidateLogIn.Validate(unitOfWork, requestObject.AppID, featureType.ModifiedBy);
            if (rez.StatusCode != 200)
            {
                return Ok(rez);
            }

            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            if (featureType.ID != id)
                return BadRequest("Invalid record!");

            ResponseMessage<string> responseMessage = new ResponseMessage<string>();
            int res = await featureTypeBusiness.Update(featureType);

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
        //    int res = await featureTypeBusiness.Delete(id);
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
