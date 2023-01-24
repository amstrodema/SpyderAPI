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
    public class SettingsController : ControllerBase
    {
        private readonly JWTService _jwtService;
        private readonly SettingsBusiness settingsBusiness;

        public SettingsController(JWTService jWTService, SettingsBusiness settingsBusiness)
        {
            _jwtService = jWTService;
            this.settingsBusiness = settingsBusiness;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var res = await settingsBusiness.GetSettings();
            return Ok(res);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            var res = await settingsBusiness.GetSettingsByID(id);
            return Ok(res);
        }

        [HttpGet("GetSettingsByUserID")]
        public async Task<ActionResult> GetSettingsByUserID(Guid userID)
        {
            var res = await settingsBusiness.GetSettingsByUserID(userID);
            return Ok(res);
        }
        //[HttpPost]
        //public async Task<ActionResult> Post(Area area)
        //{

        //    if (!ModelState.IsValid)
        //        return BadRequest("Invalid entries!");

        //    var res = await _areaBusiness.Create(area);
        //    return Ok(res);
        //}
        [HttpPut("{id}")]
        public async Task<ActionResult> Put([FromBody] Settings settings, Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            if (settings.ID != id)
                return BadRequest("Invalid record!");

            var res = await settingsBusiness.Update(settings);
            return Ok(res);
        }
    }
}
