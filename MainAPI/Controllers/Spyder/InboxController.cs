using MainAPI.Business.Spyder;
using MainAPI.Data.Interface;
using MainAPI.Generics;
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
    [ApiKeyAuth]
    [Route("api/[controller]")]
    [ApiController]
    public class InboxController : ControllerBase
    {
        private readonly InboxBusiness inboxBusiness;
        private readonly JWTService _jwtService;
        private readonly IUnitOfWork unitOfWork;

        public InboxController(InboxBusiness inboxBusiness, JWTService jWTService, IUnitOfWork unitOfWork)
        {
            this.inboxBusiness = inboxBusiness;
            _jwtService = jWTService;
            this.unitOfWork = unitOfWork;
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
        public async Task<ActionResult> Post(RequestObject<Inbox> requestObject)
        {
            var rez = await ValidateLogIn.Validate(unitOfWork, requestObject.AppID, requestObject.Data.SenderID);
            if (rez.StatusCode != 200)
            {
                return Ok(rez);
            }
            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            var res = await inboxBusiness.Create(requestObject.Data);
            return Ok(res);
        }
    }
}
