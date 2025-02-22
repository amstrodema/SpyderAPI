﻿using MainAPI.Business.Spyder.Feature;
using MainAPI.Data.Interface;
using MainAPI.Generics;
using MainAPI.Models;
using MainAPI.Models.Spyder;
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
    public class FeatureController : ControllerBase
    {
        private readonly FeatureBusiness featureBusiness;
        private readonly JWTService _jwtService;
        private readonly IUnitOfWork unitOfWork;

        public FeatureController(FeatureBusiness featureBusiness, JWTService jWTService, IUnitOfWork unitOfWork)
        {
            this.featureBusiness = featureBusiness;
            _jwtService = jWTService;
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var features = await featureBusiness.GetFeatures();
            return Ok(features);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            var feature = await featureBusiness.GetFeatureByID(id);
            return Ok(feature);
        }
        [HttpPost]
        public async Task<ActionResult> Post(RequestObject<Models.Spyder.Feature.Feature> requestObject)
        {
            Models.Spyder.Feature.Feature feature = requestObject.Data;

            var rez = await ValidateLogIn.Validate(unitOfWork, requestObject.AppID, feature.CreatedBy);
            if (rez.StatusCode != 200)
            {
                return Ok(rez);
            }

            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            var res = await featureBusiness.Create(feature);
            return Ok(res);
        }

        [HttpGet("GetFeaturesByItemID")]
        public async Task<ActionResult> GetFeaturesByItemID(Guid itemID)
        {
            var features = await featureBusiness.GetFeaturesByItemID(itemID);
            return Ok(features);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> Put([FromBody]RequestObject<Models.Spyder.Feature.Feature> requestObject, Guid id)
        {
            Models.Spyder.Feature.Feature feature = requestObject.Data;

            var rez = await ValidateLogIn.Validate(unitOfWork, requestObject.AppID, feature.ModifiedBy);
            if (rez.StatusCode != 200)
            {
                return Ok(rez);
            }

            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            if (feature.ID != id)
                return BadRequest("Invalid record!");

            ResponseMessage<string> responseMessage = new ResponseMessage<string>();
            int res = await featureBusiness.Update(feature);
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
        //    int res = await featureBusiness.Delete(id);
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
