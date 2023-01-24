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
    [ApiKeyAuth]
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly NotificationBusiness notificationBusiness;
        private readonly JWTService _jwtService;

        public NotificationController(NotificationBusiness notificationBusiness, JWTService jWTService)
        {
            this.notificationBusiness = notificationBusiness;
            _jwtService = jWTService;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var res = await notificationBusiness.GetNotifications();
            return Ok(res);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            var res = await notificationBusiness.GetNotificationByID(id);
            return Ok(res);
        }

        [HttpGet("GetAllNotificationByRecieverID")]
        public async Task<ActionResult> GetAllNotificationByRecieverID(Guid id)
        {
            var res = await notificationBusiness.GetAllNotificationByRecieverID(id);
            return Ok(res);
        }

        [HttpGet("GetAllNotificationAlert")]
        public async Task<ActionResult> GetAllNotificationAlert(Guid id)
        {
            var res = await notificationBusiness.GetAllNotificationAlert(id);
            return Ok(res);
        }
        [HttpPost]
        public async Task<ActionResult> Post(Notification notification)
        {

            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            var res = await notificationBusiness.Create(notification);
            return Ok(res);
        }
    }
}
