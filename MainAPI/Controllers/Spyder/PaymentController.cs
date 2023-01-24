using MainAPI.Business.Spyder;
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
    public class PaymentController : ControllerBase
    {
        private readonly PaymentBusiness paymentBusiness;
        private readonly JWTService _jwtService;

        public PaymentController(PaymentBusiness paymentBusiness, JWTService jWTService)
        {
            this.paymentBusiness = paymentBusiness;
            _jwtService = jWTService;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var res = await paymentBusiness.GetPayments();
            return Ok(res);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            var res = await paymentBusiness.GetPaymentByID(id);
            return Ok(res);
        }
    }
}
