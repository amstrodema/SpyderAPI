using MainAPI.Business.Spyder;
using MainAPI.Data.Interface;
using MainAPI.Generics;
using MainAPI.Models;
using MainAPI.Models.Spyder;
using MainAPI.Models.Spyder.Hall;
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
    public class HallController : ControllerBase
    {
        private readonly HallBusiness hallBusiness;
        private readonly JWTService _jwtService;
        private readonly IUnitOfWork unitOfWork;

        public HallController(HallBusiness hallBusiness, JWTService jWTService, IUnitOfWork unitOfWork)
        {
            this.hallBusiness = hallBusiness;
            _jwtService = jWTService;
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var halls = await hallBusiness.GetHalls();
            return Ok(halls);
        }

        [HttpPost("GetHallMemberVMs")]
        public async Task<ActionResult> GetHallMemberVMs(RequestObject<string> requestObject)
        {
            var halls = await hallBusiness.GetHallMemberVMs(requestObject);
            return Ok(halls);
        }

        //[HttpGet("GetHallVMs_LoggedIn")]
        //public async Task<ActionResult> GetHallVMs_LoggedIn(Guid userID, string route)
        //{
        //    var halls = await hallBusiness.GetHallMemeberVMs_LoggedIn(userID, route);
        //    return Ok(halls);
        //}

        [HttpPost("GetHallMemberDetailsVM")]
        public async Task<ActionResult> GetHallMemberDetailsVM(RequestObject<string> requestObject)
        {

            var halls = await hallBusiness.GetHallMemberDetailsVM(requestObject);
            return Ok(halls);
        }
        //[HttpGet("GetHallMemberDetailsVM_Logged")]
        //public async Task<ActionResult> GetHallMemberDetailsVM_Logged(RequestObject<string> requestObject)
        //{
        //    var halls = await hallBusiness.GetHallMemberDetailsVM_Logged(userID, recordID);
        //    return Ok(halls);
        //}

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            var hall = await hallBusiness.GetHallByID(id);
            return Ok(hall);
        }
        [HttpPost]
        public async Task<ActionResult> Post(RequestObject<Hall> requestObject)
        {
            var rez = await ValidateLogIn.Validate(unitOfWork, requestObject.AppID, requestObject.Data.CreatedBy);
            if (rez.StatusCode != 200)
            {
                return Ok(rez);
            }

            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            var res = await hallBusiness.Create(requestObject.Data);
            return Ok(res);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> Put([FromBody] RequestObject<Hall> requestObject, Guid id)
        {
            var rez = await ValidateLogIn.Validate(unitOfWork, requestObject.AppID, requestObject.Data.ModifiedBy);
            if (rez.StatusCode != 200)
            {
                return Ok(rez);
            }

            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            if (requestObject.Data.ID != id)
                return BadRequest("Invalid record!");

            int res = await hallBusiness.Update(requestObject.Data);
            ResponseMessage<Hall> responseMessage = new ResponseMessage<Hall>();
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
        //    int res = await hallBusiness.Delete(id);
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
