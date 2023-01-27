using MainAPI.Business.Spyder;
using MainAPI.Data.Interface;
using MainAPI.Generics;
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
        private readonly IUnitOfWork unitOfWork;

        public FlagReportController(FlagReportBusiness flagReportBusiness, JWTService jWTService, IUnitOfWork unitOfWork)
        {
            this.flagReportBusiness = flagReportBusiness;
            _jwtService = jWTService;
            this.unitOfWork = unitOfWork;
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
        public async Task<ActionResult> Post(RequestObject<FlagReport> requestObject)
        {
            var rez = await ValidateLogIn.Validate(unitOfWork, requestObject.AppID, requestObject.Data.PetitionerID);
            if (rez.StatusCode != 200)
            {
                return Ok(rez);
            }

            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            var res = await flagReportBusiness.Create(requestObject.Data);
            return Ok(res);
        }
    }
}
