using MainAPI.Business.Spyder;
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

namespace MainAPI.Controllers.Spyder
{
    [ApiKeyAuth]
    [Route("api/[controller]")]
    [ApiController]
    public class ConfessionController : ControllerBase
    {
        private readonly ConfessionBusiness confessionBusiness;
        private readonly JWTService _jwtService;
        private readonly IUnitOfWork unitOfWork;

        public ConfessionController(ConfessionBusiness confessionBusiness, JWTService jWTService, IUnitOfWork unitOfWork)
        {
            this.confessionBusiness = confessionBusiness;
            _jwtService = jWTService;
            this.unitOfWork = unitOfWork;
        }
     
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var confessions = await confessionBusiness.GetConfessions();
            return Ok(confessions);
        }
    
        [HttpPost("GetHeadLines")]
        public async Task<ActionResult> GetHeadLines(RequestObject<int> requestObject)
        {
            var confessions = await confessionBusiness.GetConfessionHeaders(requestObject);
            return Ok(confessions);
        }
        [HttpPost("GetConfessionDetails")]
        public async Task<ActionResult> GetConfessionDetails(RequestObject<string> requestObject)
        {
            var confessions = await confessionBusiness.GetConfessionDetails(requestObject);
            return Ok(confessions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            var confession = await confessionBusiness.GetConfessionByID(id);
            return Ok(confession);
        }
        [HttpPost]
        public async Task<ActionResult> Post(RequestObject<Confession> requestObject)
        {
            Confession confession = requestObject.Data;
            ResponseMessage<string> responseMessage = new ResponseMessage<string>();

            var rez = await ValidateLogIn.Validate(unitOfWork, requestObject.AppID, confession.CreatedBy);
            if (rez.StatusCode != 200)
            {
                return Ok(rez);
            }

            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            var res = await confessionBusiness.Create(confession);
            return Ok(res);
        }
        //[HttpPut("{id}")]
        //public async Task<ActionResult> Put([FromBody] Confession confession, Guid id)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest("Invalid entries!");

        //    if (confession.ID != id)
        //        return BadRequest("Invalid record!");

        //    int res = await confessionBusiness.Update(confession);
        //    ResponseMessage<Hall> responseMessage = new ResponseMessage<Hall>();
        //    if (res >= 1)
        //    {
        //        responseMessage.Message = "Record updated!";
        //        responseMessage.StatusCode = 200;
        //        responseMessage.Data = confession;
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
        //    int res = await confessionBusiness.Delete(id);
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
