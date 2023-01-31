using MainAPI.Business.Spyder;
using MainAPI.Data.Interface;
using MainAPI.Data.Repository;
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
    public class DeathController : ControllerBase
    {
        private readonly DeathBusiness deathBusiness;
        private readonly JWTService _jwtService;
        private readonly IUnitOfWork unitOfWork;

        public DeathController(DeathBusiness deathBusiness, JWTService jWTService, IUnitOfWork unitOfWork)
        {
            this.deathBusiness = deathBusiness;
            _jwtService = jWTService;
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var deaths = await deathBusiness.GetDeaths();
            return Ok(deaths);
        }
        
        [HttpGet("GetDeathsWithCountry")]
        public async Task<ActionResult> GetDeathsWithCountry(string countryID)
        {
            var deaths = await deathBusiness.GetDeaths(countryID);
            return Ok(deaths);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            var death = await deathBusiness.GetDeathByID(id);
            return Ok(death);
        }
        [HttpPost]
        public async Task<ActionResult> Post(RequestObject<Death> requestObject)
        {
            var rez = await ValidateLogIn.Validate(unitOfWork, requestObject.AppID, requestObject.Data.CreatedBy);
            if (rez.StatusCode != 200)
            {
                return Ok(rez);
            }
            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            var res = await deathBusiness.Create(requestObject.Data);
            return Ok(res);
        }
        //[HttpPut("{id}")]
        //public async Task<ActionResult> Put([FromBody] Feature feature, Guid id)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest("Invalid entries!");

        //    if (death.ID != id)
        //        return BadRequest("Invalid record!");

        //    int res = await deathBusiness.Update(feature);
        //    ResponseMessage<Hall> responseMessage = new ResponseMessage<Hall>();
        //    if (res >= 1)
        //    {
        //        responseMessage.Message = "Record updated!";
        //        responseMessage.StatusCode = 200;
        //        responseMessage.Data = death;
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
        //    int res = await deathBusiness.Delete(id);
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
