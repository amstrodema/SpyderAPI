using MainAPI.Business.Spyder;
using MainAPI.Models.Spyder;
using MainAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainAPI.Controllers.Spyder
{
    [Route("api/[controller]")]
    [ApiController]
    public class InboxController : ControllerBase
    {
        private readonly InboxBusiness inboxBusiness;
        private readonly JWTService _jwtService;

        public InboxController(InboxBusiness inboxBusiness, JWTService jWTService)
        {
            this.inboxBusiness = inboxBusiness;
            _jwtService = jWTService;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var res = await inboxBusiness.GetInboxes();
            return Ok(res);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            var res = await inboxBusiness.GetInboxByID(id);
            return Ok(res);
        }
        [HttpPost]
        public async Task<ActionResult> Post(Inbox inbox)
        {

            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            var res = await inboxBusiness.Create(inbox);
            return Ok(res);
        }
    }
}
