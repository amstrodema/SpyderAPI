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
    [Route("api/[controller]")]
    [ApiController]
    public class MajorFunctionLogController : ControllerBase
    {
        private readonly MajorFunctionLogBusiness majorFunctionLogBusiness;
        private readonly JWTService _jwtService;

        public MajorFunctionLogController(MajorFunctionLogBusiness majorFunctionLogBusiness, JWTService jWTService)
        {
            this.majorFunctionLogBusiness = majorFunctionLogBusiness;
            _jwtService = jWTService;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var res = await majorFunctionLogBusiness.GetMajorFunctionLogs();
            return Ok(res);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            var res = await majorFunctionLogBusiness.GetMajorFunctionLogByID(id);
            return Ok(res);
        }
        [HttpPost]
        public async Task<ActionResult> Post(MajorFunctionLog majorFunctionLog)
        {

            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            var res = await majorFunctionLogBusiness.Create(majorFunctionLog);
            return Ok(res);
        }
    }
}
