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
    public class ScheduleController : ControllerBase
    {
        private readonly ScheduleBusiness _scheduleBusiness;
        private readonly JWTService _jwtService;

        public ScheduleController(ScheduleBusiness scheduleBusiness, JWTService jWTService)
        {
            _scheduleBusiness = scheduleBusiness;
            _jwtService = jWTService;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var Schedules = await _scheduleBusiness.GetSchedules();
            return Ok(Schedules);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            var Schedule = await _scheduleBusiness.GetScheduleByID(id);
            return Ok(Schedule);
        }

        [HttpGet("GetSchedulesByNodeID")]
        public async Task<ActionResult> GetSchedulesByNodeID(Guid nodeID)
        {
            var res = await _scheduleBusiness.GetAllSchedulesByNodeID(nodeID);
            return Ok(res);
        }

        [HttpGet("ToggleSchedule")]
        
        public async Task<ActionResult> ToggleSchedule(Guid scheduleID)
        {
            var res = await _scheduleBusiness.ToggleSchedule(scheduleID);
            return Ok(res);
        }



        [HttpPost]
        public async Task<ActionResult> Post(ScheduleVM scheduleVM)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            await _scheduleBusiness.Create(scheduleVM);
            return Ok(scheduleVM);
        }

        [HttpPost("PostMultiple")]
        public async Task<ActionResult> PostMultiple(ScheduleVM[] scheduleVMs)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            var res = await _scheduleBusiness.CreateMultiple(scheduleVMs);
            return Ok(res);
        }

        [HttpPut("EditSchedule")]
        public async Task<ActionResult> EditSchedule(ScheduleVM scheduleVM)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            var res = await _scheduleBusiness.Update(scheduleVM);
            return Ok(res);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _scheduleBusiness.Delete(id);
            return Ok("Record deleted successfully");
        }
    }
}
