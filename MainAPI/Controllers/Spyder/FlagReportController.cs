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
    public class FlagReportController : ControllerBase
    {
        private readonly FlagReportBusiness flagReportBusiness;
        private readonly JWTService _jwtService;

        public FlagReportController(FlagReportBusiness flagReportBusiness, JWTService jWTService)
        {
            this.flagReportBusiness = flagReportBusiness;
            _jwtService = jWTService;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var countries = await flagReportBusiness.GetFlagReports();
            return Ok(countries);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            var country = await flagReportBusiness.GetFlagReportByID(id);
            return Ok(country);
        }
        [HttpPost]
        public async Task<ActionResult> Post(FlagReport flagReport)
        {

            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            var res = await flagReportBusiness.Create(flagReport);
            return Ok(res);
        }
    }
}
