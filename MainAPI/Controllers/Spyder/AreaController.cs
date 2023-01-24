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
    public class AreaController : ControllerBase
    {
        private readonly AreaBusiness _areaBusiness;
        private readonly JWTService _jwtService;

        public AreaController(AreaBusiness areaBusiness, JWTService jWTService)
        {
            _areaBusiness = areaBusiness;
            _jwtService = jWTService;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var res = await _areaBusiness.GetAreas();
            return Ok(res);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            var res = await _areaBusiness.GetAreaByID(id);
            return Ok(res);
        }
        [HttpPost]
        public async Task<ActionResult> Post(Area area)
        {

            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            var res = await _areaBusiness.Create(area);
            return Ok(res);
        }
    }
}
