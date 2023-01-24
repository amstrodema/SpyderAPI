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
    public class ClientController : ControllerBase
    {
        private readonly ClientBusiness _clientBusiness;
        private readonly JWTService _jwtService;

        public ClientController(ClientBusiness clientBusiness, JWTService jWTService)
        {
            _clientBusiness = clientBusiness;
            _jwtService = jWTService;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var clients = await _clientBusiness.GetClients();
            return Ok(clients);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            var client = await _clientBusiness.GetClientByID(id);
            return Ok(client);
        }

        [HttpGet("{code}")]
        public async Task<ActionResult> GetClientByCode(string code)
        {
            var client = await _clientBusiness.GetClientByCode(code);
            return Ok(client);
        }

        [HttpGet("{license}")]
        public async Task<ActionResult> GetClientByLicense(string license)
        {
            var client = await _clientBusiness.GetClientByLicense(license);
            return Ok(client);
        }

        [HttpPost]
        public async Task<ActionResult> Post(Client client)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            await _clientBusiness.Create(client);
            return Ok(client);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put([FromBody] Client client, Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            if (client.ID != id)
                return BadRequest("Invalid record!");

            await _clientBusiness.Update(client);
            return Ok(client);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _clientBusiness.Delete(id);
            return Ok("Record deleted successfully");
        }
    }
}
