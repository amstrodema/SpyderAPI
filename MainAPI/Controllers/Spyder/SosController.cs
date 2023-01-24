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
    public class SosController : ControllerBase
    {
        private readonly SosBusiness _sosBusiness;
        private readonly JWTService _jwtService;

        public SosController(SosBusiness sosBusiness, JWTService jWTService)
        {
            _sosBusiness = sosBusiness;
            _jwtService = jWTService;
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
        public async Task<ActionResult> Post(Sos sos)
        {

            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            var res = await _sosBusiness.Create(sos);
            return Ok(res);
        }
    }
}
