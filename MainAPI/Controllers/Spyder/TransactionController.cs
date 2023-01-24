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
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly TransactionBusiness transactionBusiness;
        private readonly JWTService _jwtService;

        public TransactionController(TransactionBusiness transactionBusiness, JWTService jWTService)
        {
            this.transactionBusiness = transactionBusiness;
            _jwtService = jWTService;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var res = await transactionBusiness.GetTransactions();
            return Ok(res);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            var res = await transactionBusiness.GetTransactionByID(id);
            return Ok(res);
        }
    }
}
