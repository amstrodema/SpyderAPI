using MainAPI.Business.Spyder;
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
    [Route("api/[controller]")]
    [ApiController]
    public class MissingController : ControllerBase
    {
        private readonly MissingBusiness missingBusiness;
        private readonly JWTService _jwtService;

        public MissingController(MissingBusiness missingBusiness, JWTService jWTService)
        {
            this.missingBusiness = missingBusiness;
            _jwtService = jWTService;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var missings = await missingBusiness.GetMissings();
            return Ok(missings);
        }


        [HttpPost("GetMissingByItemTypeID")]
        public async Task<ActionResult> GetMissingByItemTypeID(RequestObject<int> requestObject)
        {
            var missings = await missingBusiness.GetMissingByItemTypeID(requestObject);
            return Ok(missings);
        }

        [HttpPost("GetMissingDetails")]
        public async Task<ActionResult> GetMissingDetails(RequestObject<int> requestObject)
        {
            var res = await missingBusiness.GetMissingDetails(requestObject);
            return Ok(res);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            var missing = await missingBusiness.GetMissingByID(id);
            return Ok(missing);
        }
        [HttpPost]
        public async Task<ActionResult> Post(MissingVM missingVM)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            var res = await missingBusiness.Create(missingVM);
            return Ok(res);
        }
        //[HttpPut("{id}")]
        //public async Task<ActionResult> Put([FromBody] Feature feature, Guid id)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest("Invalid entries!");

        //    if (missing.ID != id)
        //        return BadRequest("Invalid record!");

        //    int res = await missingBusiness.Update(feature);
        //    ResponseMessage<Hall> responseMessage = new ResponseMessage<Hall>();
        //    if (res >= 1)
        //    {
        //        responseMessage.Message = "Record updated!";
        //        responseMessage.StatusCode = 200;
        //        responseMessage.Data = missing;
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
        //    int res = await missingBusiness.Delete(id);
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
