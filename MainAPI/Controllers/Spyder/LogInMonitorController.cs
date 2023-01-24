using MainAPI.Business.Spyder;
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
    public class LogInMonitorController : ControllerBase
    {
        private readonly LogInMonitorBusiness logInMonitorBusiness;
        private readonly JWTService _jwtService;

        public LogInMonitorController(LogInMonitorBusiness logInMonitorBusiness, JWTService jWTService)
        {
            this.logInMonitorBusiness = logInMonitorBusiness;
            _jwtService = jWTService;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var res = await logInMonitorBusiness.GetLogInMonitors();
            return Ok(res);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            var res = await logInMonitorBusiness.GetLogInMonitorByID(id);
            return Ok(res);
        }
        [HttpPost]
        public async Task<ActionResult> Post(LogInMonitor logInMonitor)
        {

            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            var res = await logInMonitorBusiness.Create(logInMonitor);
            return Ok(res);
        }
    }
}
