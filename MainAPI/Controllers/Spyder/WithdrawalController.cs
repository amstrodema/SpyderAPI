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
    public class WithdrawalController : ControllerBase
    {
        private readonly WithdrawalBusiness _withdrawalBusiness;
        private readonly JWTService _jwtService;

        public WithdrawalController(WithdrawalBusiness withdrawalBusiness, JWTService jWTService)
        {
            _withdrawalBusiness = withdrawalBusiness;
            _jwtService = jWTService;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var res = await _withdrawalBusiness.GetWithdrawals();
            return Ok(res);
        }
        [HttpGet("GetWithdrawalsByUserID")]
        public async Task<ActionResult> GetWithdrawalsByUserID(Guid userID)
        {
            var res = await _withdrawalBusiness.GetWithdrawalsByUserID(userID);
            return Ok(res);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            var res = await _withdrawalBusiness.GetWithdrawalByID(id);
            return Ok(res);
        }
        [HttpGet("Withdraw")]
        public async Task<ActionResult> Withdraw(Guid userID)
        {

            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            var res = await _withdrawalBusiness.Create(userID);
            return Ok(res);
        }
    }
}
