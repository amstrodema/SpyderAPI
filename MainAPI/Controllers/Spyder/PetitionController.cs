using MainAPI.Business.Spyder;
using MainAPI.Data.Interface;
using MainAPI.Generics;
using MainAPI.Models;
using MainAPI.Models.Petition.Spyder;
using MainAPI.Models.Spyder;
using MainAPI.Services;
using Microsoft.AspNetCore.Hosting;
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
    public class PetitionController : ControllerBase
    {
        private readonly PetitionBusiness petitionBusiness;
        private readonly JWTService _jwtService;
        private readonly IUnitOfWork unitOfWork;

        public PetitionController(PetitionBusiness petitionBusiness, JWTService jWTService, IUnitOfWork unitOfWork)
        {
            this.petitionBusiness = petitionBusiness;
            _jwtService = jWTService;
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var petitions = await petitionBusiness.GetPetitions();
            return Ok(petitions);
        }

        //[HttpGet("GetPetitionVMs_LoggedIn")]
        //public async Task<ActionResult> GetPetitionVMs_LoggedIn(Guid userID)
        //{
        //    var petitions = await petitionBusiness.GetPetitionVMs_LoggedIn(userID);
        //    return Ok(petitions);
        //}


        [HttpPost("GetPetitionVMs")]
        public async Task<ActionResult> GetPetitionVMs(RequestObject<string> requestObject)
        {
            var petitions = await petitionBusiness.GetPetitionVMs(requestObject);
            return Ok(petitions);
        }

        [HttpPost("GetPetitionDetailsVM")]
        public async Task<ActionResult> GetPetitionDetailsVM(RequestObject<string> requestObject)
        {
            var petitions = await petitionBusiness.GetPetitionDetailsVM(requestObject);
            return Ok(petitions);
        }

        //[HttpGet("GetPetitionDetailsVM_Logged")]
        //public async Task<ActionResult> GetPetitionDetailsVM(Guid userID, Guid recordID)
        //{
        //    var petitions = await petitionBusiness.GetPetitionDetailsVM_Logged(userID,recordID);
        //    return Ok(petitions);
        //}

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            var petition = await petitionBusiness.GetPetitionByID(id);
            return Ok(petition);
        }
        [HttpPost]
        public async Task<ActionResult> Post(RequestObject<Petition> requestObject)
        {
            var rez = await ValidateLogIn.Validate(unitOfWork, requestObject.AppID, requestObject.Data.PetitionerID);
            if (rez.StatusCode != 200)
            {
                return Ok(rez);
            }

            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            var res = await petitionBusiness.Create(requestObject.Data);
            return Ok(res);
        }
        //[HttpPut("{id}")]
        //public async Task<ActionResult> Put([FromBody] Petition petition, Guid id)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest("Invalid entries!");

        //    if (petition.ID != id)
        //        return BadRequest("Invalid record!");

        //    int res = await petitionBusiness.Update(petition);
        //    ResponseMessage<Petition> responseMessage = new ResponseMessage<Petition>();
        //    if (res >= 1)
        //    {
        //        responseMessage.Message = "Record updated!";
        //        responseMessage.StatusCode = 200;
        //        responseMessage.Data = petition;
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
        //    int res = await petitionBusiness.Delete(id);
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
