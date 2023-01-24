using MainAPI.Business.Examina;
using MainAPI.Models.Examina;
using MainAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainAPI.Controllers.Examina
{
    [Route("api/[controller]")]
    [ApiController]
    public class OptionController : ControllerBase
    {
        private readonly OptionBusiness _OptionBusiness;
        private readonly JWTService _jwtService;

        public OptionController(OptionBusiness OptionBusiness, JWTService jWTService)
        {
            _OptionBusiness = OptionBusiness;
            _jwtService = jWTService;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var Options = await _OptionBusiness.GetOptions();
            return Ok(Options);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            var Option = await _OptionBusiness.GetOptionByID(id);
            return Ok(Option);
        }

        [HttpPost]
        public async Task<ActionResult> Post(Option Option)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            await _OptionBusiness.Create(Option);
            return Ok(Option);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put([FromBody] Option Option, Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            if (Option.ID != id)
                return BadRequest("Invalid record!");

            await _OptionBusiness.Update(Option);
            return Ok(Option);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _OptionBusiness.Delete(id);
            return Ok("Record deleted successfully");
        }
    }
}
