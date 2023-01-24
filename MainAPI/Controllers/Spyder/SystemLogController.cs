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
    public class SystemLogController : ControllerBase
    {
        private readonly SystemLogsBusiness systemLogsBusiness;
        private readonly JWTService _jwtService;

        public SystemLogController(SystemLogsBusiness systemLogsBusiness, JWTService jWTService)
        {
            this.systemLogsBusiness = systemLogsBusiness;
            _jwtService = jWTService;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var res = await systemLogsBusiness.GetSystemLogs();
            return Ok(res);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            var res = await systemLogsBusiness.GetSystemLogByID(id);
            return Ok(res);
        }
        [HttpPost]
        public async Task<ActionResult> Post(SystemLog majorFunctionLog)
        {

            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            var res = await systemLogsBusiness.Create(majorFunctionLog);
            return Ok(res);
        }
    }
}
