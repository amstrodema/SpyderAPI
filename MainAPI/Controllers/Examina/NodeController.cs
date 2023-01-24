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
    public class NodeController : ControllerBase
    {
        private readonly NodeBusiness _nodeBusiness;
        private readonly JWTService _jwtService;

        public NodeController(NodeBusiness nodeBusiness, JWTService jWTService)
        {
            _nodeBusiness = nodeBusiness;
            _jwtService = jWTService;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var nodes = await _nodeBusiness.GetNodes();
            return Ok(nodes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            var node = await _nodeBusiness.GetNodeByID(id);
            return Ok(node);
        }

        [HttpGet("{clientID}")]
        public async Task<ActionResult> GetNodeByClientID(Guid clientID)
        {
            var node = await _nodeBusiness.GetNodeByClientID(clientID);
            return Ok(node);
        }

        [HttpGet("{client_ID}")]
        public async Task<ActionResult> GetNodesByClientID(Guid client_ID)
        {
            var node = await _nodeBusiness.GetNodesByClientID(client_ID);
            return Ok(node);
        }

        [HttpGet("{accessCode}")]
        public async Task<ActionResult> GetNodeByAccessCode(string accessCode)
        {
            var node = await _nodeBusiness.GetNodeByAccessCode(accessCode);
            return Ok(node);
        }

        [HttpPost]
        public async Task<ActionResult> Post(Node node)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            await _nodeBusiness.Create(node);
            return Ok(node);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put([FromBody] Node node, Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            if (node.ID != id)
                return BadRequest("Invalid record!");

            await _nodeBusiness.Update(node);
            return Ok(node);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _nodeBusiness.Delete(id);
            return Ok("Record deleted successfully");
        }
    }
}
