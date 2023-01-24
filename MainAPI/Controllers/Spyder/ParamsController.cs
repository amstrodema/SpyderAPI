using MainAPI.Business.Spyder;
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

        public ParamsController(ParamsBusiness paramsBusiness, JWTService jWTService)
        {
            this.paramsBusiness = paramsBusiness;
            _jwtService = jWTService;
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
        [HttpGet("GetParamsByCode")]
        public async Task<ActionResult> Get(string code)
        {
            var param = await paramsBusiness.GetParamsByCode(code);
            return Ok(param);
        }
        [HttpPost]
        public async Task<ActionResult> Post(Params param)
        {

            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            var res = await paramsBusiness.Create(param);
            return Ok(res);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> Put([FromBody] Params param, Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            if (param.ID != id)
                return BadRequest("Invalid record!");

            int res = await paramsBusiness.Update(param);
            ResponseMessage<Params> responseMessage = new ResponseMessage<Params>();
            if (res >= 1)
            {
                responseMessage.Message = "Record updated!";
                responseMessage.StatusCode = 200;
                responseMessage.Data = param;
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
