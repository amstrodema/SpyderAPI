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
    public class ScheduleGroupController : ControllerBase
    {
        private readonly ScheduleGroupBusiness _scheduleGroupBusiness;
        private readonly JWTService _jwtService;

        public ScheduleGroupController(ScheduleGroupBusiness scheduleGroupBusiness, JWTService jWTService)
        {
            _scheduleGroupBusiness = scheduleGroupBusiness;
            _jwtService = jWTService;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var scheduleGroups = await _scheduleGroupBusiness.GetScheduleGrps();
            return Ok(scheduleGroups);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            var scheduleGroup = await _scheduleGroupBusiness.GetScheduleGroupByID(id);
            return Ok(scheduleGroup);
        }

        [HttpGet("GetSchedulesGroupByScheduleID")]
        public async Task<ActionResult> GetSchedulesGroupByScheduleID(Guid ScheduleID)
        {
            var scheduleGroup = await _scheduleGroupBusiness._GetSchedulesGroupByScheduleID(ScheduleID);
            return Ok(scheduleGroup);
        }

        [HttpPost]
        public async Task<ActionResult> Post(ScheduleGroup scheduleGroup)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            await _scheduleGroupBusiness.Create(scheduleGroup);
            return Ok(scheduleGroup);
        }

        [HttpPost("AttachGroupsToSchedule")]
        public async Task<ActionResult> AttachGroupsToSchedule(ScheduleGroupsVM scheduleGroupsVM)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            var res = await _scheduleGroupBusiness.AttachGroupsToSchedule(scheduleGroupsVM);
            return Ok(res);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put([FromBody] ScheduleGroup scheduleGroup, Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            if (scheduleGroup.ID != id)
                return BadRequest("Invalid record!");

            await _scheduleGroupBusiness.Update(scheduleGroup);
            return Ok(scheduleGroup);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _scheduleGroupBusiness.Delete(id);
            return Ok("Record deleted successfully");
        }
    }
}
