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
    public class QuestionImageController : ControllerBase
    {
        private readonly QuestionImageBusiness _QuestionImageBusiness;
        private readonly JWTService _jwtService;

        public QuestionImageController(QuestionImageBusiness QuestionImageBusiness, JWTService jWTService)
        {
            _QuestionImageBusiness = QuestionImageBusiness;
            _jwtService = jWTService;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var QuestionImages = await _QuestionImageBusiness.GetQuestionImages();
            return Ok(QuestionImages);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            var QuestionImage = await _QuestionImageBusiness.GetQuestionImageByID(id);
            return Ok(QuestionImage);
        }

        [HttpPost]
        public async Task<ActionResult> Post(QuestionImage QuestionImage)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            await _QuestionImageBusiness.Create(QuestionImage);
            return Ok(QuestionImage);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put([FromBody] QuestionImage QuestionImage, Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            if (QuestionImage.ID != id)
                return BadRequest("Invalid record!");

            await _QuestionImageBusiness.Update(QuestionImage);
            return Ok(QuestionImage);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _QuestionImageBusiness.Delete(id);
            return Ok("Record deleted successfully");
        }
    }
}
