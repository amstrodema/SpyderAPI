using MainAPI.Business;
using MainAPI.Models;
using MainAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusEnumController : ControllerBase
    {
        private readonly StatusEnumBusiness _statusEnumBusiness;
        private readonly JWTService _jwtService;

        public StatusEnumController(StatusEnumBusiness statusEnumBusiness, JWTService jWTService)
        {
            _statusEnumBusiness = statusEnumBusiness;
            _jwtService = jWTService;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var statusEnums = await _statusEnumBusiness.GetStatusEnums();
            return Ok(statusEnums);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetStatusEnumByID(Guid id)
        {
            var statusEnum = await _statusEnumBusiness.GetStatusEnumByID(id);
            return Ok(statusEnum);
        }

        [HttpPost]
        public async Task<ActionResult> Post(StatusEnum statusEnum)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            await _statusEnumBusiness.Create(statusEnum);
            return Ok(statusEnum);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put([FromBody] StatusEnum statusEnum, Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            if (statusEnum.ID != id)
                return BadRequest("Invalid record!");

            await _statusEnumBusiness.Update(statusEnum);
            return Ok(statusEnum);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _statusEnumBusiness.Delete(id);
            return Ok("Record deleted successfully");
        }
    }
}
