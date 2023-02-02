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
    public class SettingsController : ControllerBase
    {
        private readonly JWTService _jwtService;
        private readonly SettingsBusiness settingsBusiness;
        private readonly IUnitOfWork unitOfWork;

        public SettingsController(JWTService jWTService, SettingsBusiness settingsBusiness, IUnitOfWork unitOfWork)
        {
            _jwtService = jWTService;
            this.settingsBusiness = settingsBusiness;
            this.unitOfWork = unitOfWork;
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
        public async Task<ActionResult> GetSettingsByUserID(Guid userID, Guid appID)
        {
            var rez = await ValidateLogIn.Validate(unitOfWork, appID, userID);
            if (rez.StatusCode == 209)
            {
                return Ok(rez);
            }

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
        public async Task<ActionResult> Put([FromBody] RequestObject<Settings> requestObject, Guid id)
        {
            Guid userID;
            try
            {
                userID = Guid.Parse(requestObject.UserID);
            }
            catch (Exception)
            {
                return BadRequest("Invalid entries!");
            }
            var rez = await ValidateLogIn.Validate(unitOfWork, requestObject.AppID, userID);
            if (rez.StatusCode != 200)
            {
                return Ok(rez);
            }

            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            if (requestObject.Data.ID != id)
                return BadRequest("Invalid record!");

            var res = await settingsBusiness.Update(requestObject.Data);
            return Ok(res);
        }
    }
}
