using MainAPI.Business.Spyder;
using MainAPI.Data.Interface;
using MainAPI.Generics;
using MainAPI.Models.Spyder;
using MainAPI.Models.ViewModel.Spyder;
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
    public class MarriageController : ControllerBase
    {
        private readonly MarriageBusiness marriageBusiness;
        private readonly JWTService _jwtService;
        private readonly IUnitOfWork unitOfWork;

        public MarriageController(MarriageBusiness marriageBusiness, JWTService jWTService, IUnitOfWork unitOfWork)
        {
            this.marriageBusiness = marriageBusiness;
            _jwtService = jWTService;
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var marriages = await marriageBusiness.GetMarriages();
            return Ok(marriages);
        }

        [HttpGet("GetMarriagesWithCountry")]
        public async Task<ActionResult> GetMarriagesWithCountry(string countryID)
        {
            return Ok(await marriageBusiness.GetMarriages(countryID));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            var marriage = await marriageBusiness.GetMarriageByID(id);
            return Ok(marriage);
        }

        [HttpGet("GetMarriageVMByID")]
        public async Task<ActionResult> GetMarriageVMByID(Guid id)
        {
            var marriage = await marriageBusiness.GetMarriageVMByID(id);
            return Ok(marriage);
        }
        [HttpPost]
        public async Task<ActionResult> Post(RequestObject<MarriageVM> requestObject)
        {
            var rez = await ValidateLogIn.Validate(unitOfWork, requestObject.AppID, requestObject.Data.marriage.CreatedBy);
            if (rez.StatusCode != 200)
            {
                return Ok(rez);
            }

            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            var res = await marriageBusiness.Create(requestObject.Data);
            return Ok(res);
        }
        //[HttpPut("{id}")]
        //public async Task<ActionResult> Put([FromBody] Feature feature, Guid id)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest("Invalid entries!");

        //    if (marriage.ID != id)
        //        return BadRequest("Invalid record!");

        //    int res = await marriageBusiness.Update(feature);
        //    ResponseMessage<Hall> responseMessage = new ResponseMessage<Hall>();
        //    if (res >= 1)
        //    {
        //        responseMessage.Message = "Record updated!";
        //        responseMessage.StatusCode = 200;
        //        responseMessage.Data = marriage;
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
        //    int res = await marriageBusiness.Delete(id);
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
