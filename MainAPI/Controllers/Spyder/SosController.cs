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
    public class SosController : ControllerBase
    {
        private readonly SosBusiness _sosBusiness;
        private readonly JWTService _jwtService;
        private readonly IUnitOfWork unitOfWork;

        public SosController(SosBusiness sosBusiness, JWTService jWTService, IUnitOfWork unitOfWork)
        {
            _sosBusiness = sosBusiness;
            _jwtService = jWTService;
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var res = await _sosBusiness.GetSos();
            return Ok(res);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            var res = await _sosBusiness.GetSosByID(id);
            return Ok(res);
        }
        [HttpPost]
        public async Task<ActionResult> Post(RequestObject<Sos> requestObject)
        {
            var rez = await ValidateLogIn.Validate(unitOfWork, requestObject.AppID, requestObject.Data.UserID);
            if (rez.StatusCode != 200)
            {
                return Ok(rez);
            }

            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            var res = await _sosBusiness.Create(requestObject.Data);
            return Ok(res);
        }
    }
}
