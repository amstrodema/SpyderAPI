﻿using MainAPI.Business.Spyder;
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
    public class ItemTypeController : ControllerBase
    {
        private readonly ItemTypeBusiness itemTypeBusiness;
        private readonly JWTService _jwtService;

        public ItemTypeController(ItemTypeBusiness itemTypeBusiness, JWTService jWTService)
        {
            this.itemTypeBusiness = itemTypeBusiness;
            _jwtService = jWTService;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var itemTypes = await itemTypeBusiness.GetItemTypes();
            return Ok(itemTypes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            var itemType = await itemTypeBusiness.GetItemTypeByID(id);
            return Ok(itemType);
        }
        [HttpPost]
        public async Task<ActionResult> Post(ItemType itemType)
        {

            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            var res = await itemTypeBusiness.Create(itemType);
            return Ok(res);
        }
        //[HttpPut("{id}")]
        //public async Task<ActionResult> Put([FromBody] Feature feature, Guid id)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest("Invalid entries!");

        //    if (itemType.ID != id)
        //        return BadRequest("Invalid record!");

        //    int res = await itemTypeBusiness.Update(feature);
        //    ResponseMessage<Hall> responseMessage = new ResponseMessage<Hall>();
        //    if (res >= 1)
        //    {
        //        responseMessage.Message = "Record updated!";
        //        responseMessage.StatusCode = 200;
        //        responseMessage.Data = itemType;
        //    }
        //    else
        //    {
        //        responseMessage.Message = "No record updated!";
        //        responseMessage.StatusCode = 201;
        //    }
        //    return Ok(responseMessage);
        //}

        //[HttpDelete("{id}")]
        //public async Task<ActionResult> Delete(Guid id)
        //{
        //    int res = await itemTypeBusiness.Delete(id);
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
