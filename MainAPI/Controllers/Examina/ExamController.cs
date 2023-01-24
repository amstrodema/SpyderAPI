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
    public class ExamController : ControllerBase
    {
        private readonly ExaminationBusiness _examBusiness;
        private readonly JWTService _jwtService;

        public ExamController(ExaminationBusiness examBusiness, JWTService jWTService)
        {
            _examBusiness = examBusiness;
            _jwtService = jWTService;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var exams = await _examBusiness.GetExams();
            return Ok(exams);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            var exam = await _examBusiness.GetExamByID(id);
            return Ok(exam);
        }

        [HttpGet("GetExamsByNodeID")]
        public async Task<ActionResult> GetExamsByNodeID(Guid nodeID, Guid userID)
        {
            var res = await _examBusiness.GetExamsByNodeID(nodeID, userID);
            return Ok(res);
        }

        [HttpGet("GetExamsDetailsByNodeID")]
        public async Task<ActionResult> GetExamsDetailsByNodeID(Guid nodeID)
        {
            var res = await _examBusiness.GetExamsDetailsByNodeID(nodeID);
            return Ok(res);
        }


        [HttpGet("GetNodeExamDetailsByExamID")]
        public async Task<ActionResult> GetNodeExamDetailsByExamID(Guid nodeID, Guid examID)
        {
            var res = await _examBusiness.GetNodeExamDetailsByExamID(nodeID, examID);
            return Ok(res);
        }

        [HttpPost]
        public async Task<ActionResult> Post(Exam exam)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            await _examBusiness.Create(exam);
            return Ok(exam);
        }

        [HttpPost("PostMultiple")]
        public async Task<ActionResult> PostMultiple(Exam[] exams)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            var res = await _examBusiness.CreateMultiple(exams);
            return Ok(res);
        }

        [HttpPut("ExamEdit")]
        public async Task<ActionResult> ExamEdit(Exam exam)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

           var res=  await _examBusiness.Update(exam);
            return Ok(res);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _examBusiness.Delete(id);
            return Ok("Record deleted successfully");
        }

    }
}
