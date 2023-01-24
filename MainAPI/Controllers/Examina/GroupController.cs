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
    public class GroupController : ControllerBase
    {
        private readonly GroupBusiness _GroupBusiness;
        private readonly JWTService _jwtService;

        public GroupController(GroupBusiness GroupBusiness, JWTService jWTService)
        {
            _GroupBusiness = GroupBusiness;
            _jwtService = jWTService;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var Groups = await _GroupBusiness.GetGroups();
            return Ok(Groups);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            var Group = await _GroupBusiness.GetGroupByID(id);
            return Ok(Group);
        }

        [HttpGet("GetGroupsByNodeID")]
        public async Task<ActionResult> GetGroupsByNodeID(Guid nodeID)
        {
            var res = await _GroupBusiness.GetGroupsByNodeID(nodeID);
            return Ok(res);
        }

        [HttpPost]
        public async Task<ActionResult> Post(Group Group)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            await _GroupBusiness.Create(Group);
            return Ok(Group);
        }
        [HttpPost("PostMultiple")]
        public async Task<ActionResult> PostMultiple(Group[] Groups)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            var res = await _GroupBusiness.CreateMultiple(Groups);
            return Ok(res);
        }

        [HttpPut("GroupEdit")]
        public async Task<ActionResult> GroupEdit(Group Group)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            //if (Group.ID != id)
            //    return BadRequest("Invalid record!");

            return Ok(await _GroupBusiness.Update(Group));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _GroupBusiness.Delete(id);
            return Ok("Record deleted successfully");
        }
    }
}
