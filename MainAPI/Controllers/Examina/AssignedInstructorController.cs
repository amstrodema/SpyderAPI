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
    public class AssignedInstructorController : ControllerBase
    {
        private readonly AssignedInstructorBusiness _AssignedInstructorBusiness;
        private readonly JWTService _jwtService;

        public AssignedInstructorController(AssignedInstructorBusiness AssignedInstructorBusiness, JWTService jWTService)
        {
            _AssignedInstructorBusiness = AssignedInstructorBusiness;
            _jwtService = jWTService;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var AssignedInstructors = await _AssignedInstructorBusiness.GetAssignedInstructors();
            return Ok(AssignedInstructors);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            var AssignedInstructor = await _AssignedInstructorBusiness.GetAssignedInstructorByID(id);
            return Ok(AssignedInstructor);
        }

        [HttpPost]
        public async Task<ActionResult> Post(AssignedInstructor AssignedInstructor)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            await _AssignedInstructorBusiness.Create(AssignedInstructor);
            return Ok(AssignedInstructor);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put([FromBody] AssignedInstructor AssignedInstructor, Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            if (AssignedInstructor.ID != id)
                return BadRequest("Invalid record!");

            await _AssignedInstructorBusiness.Update(AssignedInstructor);
            return Ok(AssignedInstructor);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _AssignedInstructorBusiness.Delete(id);
            return Ok("Record deleted successfully");
        }
    }
}
