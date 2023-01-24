using MainAPI.Business.Examina;
using MainAPI.Models.Examina;
using MainAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainAPI.Controllers.Examina
{
    [Route("api/[controller]")]
    [ApiController]
    public class OptionImageController : ControllerBase
    {
        private readonly OptionImageBusiness _OptionImageBusiness;
        private readonly JWTService _jwtService;

        public OptionImageController(OptionImageBusiness OptionImageBusiness, JWTService jWTService)
        {
            _OptionImageBusiness = OptionImageBusiness;
            _jwtService = jWTService;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var OptionImages = await _OptionImageBusiness.GetOptionImages();
            return Ok(OptionImages);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            var OptionImage = await _OptionImageBusiness.GetOptionImageByID(id);
            return Ok(OptionImage);
        }

        [HttpPost]
        public async Task<ActionResult> Post(OptionImage OptionImage)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            await _OptionImageBusiness.Create(OptionImage);
            return Ok(OptionImage);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put([FromBody] OptionImage OptionImage, Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            if (OptionImage.ID != id)
                return BadRequest("Invalid record!");

            await _OptionImageBusiness.Update(OptionImage);
            return Ok(OptionImage);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _OptionImageBusiness.Delete(id);
            return Ok("Record deleted successfully");
        }
    }
}
