using MainAPI.Business.Examina;
using MainAPI.Models.Examina;
using MainAPI.Models.ViewModel.Examina;
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
    public class QuestionController : ControllerBase
    {
        private readonly QuestionBusiness _QuestionBusiness;
        private readonly JWTService _jwtService;

        public QuestionController(QuestionBusiness QuestionBusiness, JWTService jWTService)
        {
            _QuestionBusiness = QuestionBusiness;
            _jwtService = jWTService;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var Questions = await _QuestionBusiness.GetQuestions();
            return Ok(Questions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            var Question = await _QuestionBusiness.GetQuestionByID(id);
            return Ok(Question);
        }

        [HttpPost]
        public async Task<ActionResult> Post(Question Question)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            await _QuestionBusiness.Create(Question);
            return Ok(Question);
        }

        [HttpPost("PostMultiple")]
        public async Task<ActionResult> PostMultiple(QuestionVM[] questionVMs)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            var res = await _QuestionBusiness.CreateMultiple(questionVMs);
            return Ok(res);
        }

        [HttpPut("EditQuestion")]
        public async Task<ActionResult> EditQuestion(QuestionVM questionVM)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            var res = await _QuestionBusiness.Update(questionVM);
            return Ok(res);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _QuestionBusiness.Delete(id);
            return Ok("Record deleted successfully");
        }
    }
}
