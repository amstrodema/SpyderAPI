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
    public class ParamsController : ControllerBase
    {
        private readonly ParamsBusiness paramsBusiness;
        private readonly JWTService _jwtService;
        private readonly IUnitOfWork unitOfWork;

        public ParamsController(ParamsBusiness paramsBusiness, JWTService jWTService, IUnitOfWork unitOfWork)
        {
            this.paramsBusiness = paramsBusiness;
            _jwtService = jWTService;
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var paramz = await paramsBusiness.GetParams();
            return Ok(paramz);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            var param = await paramsBusiness.GetParamsByID(id);
            return Ok(param);
        }
        [HttpPost("GetParamsByCode")]
        public async Task<ActionResult> GetParamsByCode(RequestObject<string> requestObject)
        {
            Guid userID;
            try
            {
                userID = Guid.Parse(requestObject.UserID);
            }
            catch (Exception)
            {
                userID = default;
            }
            var rez = await ValidateLogIn.Validate(unitOfWork, requestObject.AppID, userID);
            if (rez.StatusCode != 200)
            {
                return Ok(rez);
            }
            var param = await paramsBusiness.GetParamsByCode(requestObject.Data);
            return Ok(param);
        }
        [HttpPost]
        public async Task<ActionResult> Post(RequestObject<Params> requestObject)
        {
            var rez = await ValidateLogIn.Validate(unitOfWork, requestObject.AppID, requestObject.Data.CreatedBy);
            if (rez.StatusCode != 200)
            {
                return Ok(rez);
            }

            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            var res = await paramsBusiness.Create(requestObject.Data);
            return Ok(res);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> Put([FromBody] RequestObject<Params> requestObject, Guid id)
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

            int res = await paramsBusiness.Update(requestObject.Data);
            ResponseMessage<Params> responseMessage = new ResponseMessage<Params>();
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
    }
}
