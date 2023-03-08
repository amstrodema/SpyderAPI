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
    public class WithdrawalController : ControllerBase
    {
        private readonly WithdrawalBusiness _withdrawalBusiness;
        private readonly JWTService _jwtService;
        private readonly IUnitOfWork unitOfWork;

        public WithdrawalController(WithdrawalBusiness withdrawalBusiness, JWTService jWTService, IUnitOfWork unitOfWork)
        {
            _withdrawalBusiness = withdrawalBusiness;
            _jwtService = jWTService;
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var res = await _withdrawalBusiness.GetWithdrawals();
            return Ok(res);
        }

        [HttpPost("GetWithdrawals")]
        public async Task<ActionResult> GetWithdrawals(RequestObject<int> requestObject)
        {
            try
            {
                var rez = await ValidateLogIn.ValidateAdmin(unitOfWork, requestObject.AppID, Guid.Parse(requestObject.UserID), 5);
                if (rez.StatusCode != 200)
                {
                    return Ok(rez);
                }
            }
            catch (Exception)
            {
                return Ok("An error occured!");
            }

            var res = await _withdrawalBusiness.GetWithdrawals(requestObject.Data);
            return Ok(res);
        }

        [HttpPost("PrintPayOut")]
        public async Task<ActionResult> PrintPayOut(RequestObject<int> requestObject)
        {
            try
            {
                var rez = await ValidateLogIn.ValidateAdmin(unitOfWork, requestObject.AppID, Guid.Parse(requestObject.UserID), 5);
                if (rez.StatusCode != 200)
                {
                    return Ok(rez);
                }
            }
            catch (Exception)
            {
                return Ok("An error occured!");
            }

            var res = await _withdrawalBusiness.PrintPayOut(requestObject.Data);
            return Ok(res);
        }

        [HttpPost("PayOut")]
        public async Task<ActionResult> PayOut(RequestObject<string> requestObject)
        {
            try
            {
                var rez = await ValidateLogIn.ValidateAdmin(unitOfWork, requestObject.AppID, Guid.Parse(requestObject.UserID), 5);
                if (rez.StatusCode != 200)
                {
                    return Ok(rez);
                }
            }
            catch (Exception)
            {
                return Ok("An error occured!");
            }

            var res = await _withdrawalBusiness.PayOut();
            return Ok(res);
        }
        [HttpGet("GetWithdrawalsByUserID")]
        public async Task<ActionResult> GetWithdrawalsByUserID(Guid userID, Guid appID)
        {
            var rez = await ValidateLogIn.Validate(unitOfWork, appID, userID);
            if (rez.StatusCode != 200)
            {
                return Ok(rez);
            }
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
        public async Task<ActionResult> Withdraw(Guid userID, Guid appID)
        {
            var rez = await ValidateLogIn.Validate(unitOfWork, appID, userID);
            if (rez.StatusCode != 200)
            {
                return Ok(rez);
            }

            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            var res = await _withdrawalBusiness.Create(userID);
            return Ok(res);
        }
    }
}
