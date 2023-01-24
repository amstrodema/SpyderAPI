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
    public class InstructorExamController : ControllerBase
    {
        private readonly InstructorExamBusiness _instructorExamBusiness;
        private readonly JWTService _jwtService;

        public InstructorExamController(InstructorExamBusiness instructorExamBusiness, JWTService jWTService)
        {
            _instructorExamBusiness = instructorExamBusiness;
            _jwtService = jWTService;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var exams = await _instructorExamBusiness.GetInstructorExams();
            return Ok(exams);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            var exam = await _instructorExamBusiness.GetInstructorExamByID(id);
            return Ok(exam);
        }

        [HttpGet("GetExamInstructorsByNodeID")]
        public async Task<ActionResult> GetExamInstructorsByNodeID(Guid nodeID)
        {
            var res = await _instructorExamBusiness.GetExamInstructorsByNodeID(nodeID);
            return Ok(res);
        }

        [HttpGet("GetExamInstructorsByExamID")]
        public async Task<ActionResult> GetExamInstructorsByExamID(Guid examID)
        {
            var res = await _instructorExamBusiness.GetAllInstructorsAssignedToExam(examID);
            return Ok(res);
        }

        [HttpPost]
        public async Task<ActionResult> Post(InstructorExam instructorExam)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            await _instructorExamBusiness.Create(instructorExam);
            return Ok(instructorExam);
        }

        [HttpPost("AttachInstructorsToExam")]
        public async Task<ActionResult> AttachInstructorsToExam(InstructorExamVM instructorExamVM)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            var res = await _instructorExamBusiness.AttachInstructorsToExam(instructorExamVM);
            return Ok(res);
        }
    }
}
