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
    public class SettingController : ControllerBase
    {
        private readonly SettingBusiness _settingBusiness;
        private readonly JWTService _jwtService;

        public SettingController(SettingBusiness settingBusiness, JWTService jWTService)
        {
            _settingBusiness = settingBusiness;
            _jwtService = jWTService;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var Settings = await _settingBusiness.GetSettings();
            return Ok(Settings);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            var Setting = await _settingBusiness.GetSettingByID(id);
            return Ok(Setting);
        }

        [HttpPost]
        public async Task<ActionResult> Post(Setting Setting)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            await _settingBusiness.Create(Setting);
            return Ok(Setting);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put([FromBody] Setting Setting, Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            if (Setting.ID != id)
                return BadRequest("Invalid record!");

            await _settingBusiness.Update(Setting);
            return Ok(Setting);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _settingBusiness.Delete(id);
            return Ok("Record deleted successfully");
        }
    }
}
