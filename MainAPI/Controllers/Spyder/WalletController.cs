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
    public class WalletController : ControllerBase
    {

        private readonly WalletBusiness _walletBusiness;
        private readonly JWTService _jwtService;
        private readonly IUnitOfWork unitOfWork;

        public WalletController(WalletBusiness walletBusiness, JWTService jWTService, IUnitOfWork unitOfWork)
        {
            _walletBusiness = walletBusiness;
            _jwtService = jWTService;
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var Users = await _walletBusiness.GetWallets();
            return Ok(Users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            var User = await _walletBusiness.GetWalletByID(id);
            return Ok(User);
        }

        [HttpGet("GetWalletDetailsByUserID")]
        public async Task<ActionResult> GetWalletDetailsByUserID(Guid userId, Guid appID)
        {
            var rez = await ValidateLogIn.Validate(unitOfWork, appID, userId);
            if (rez.StatusCode == 209)
            {
                return Ok(rez);
            }
            return Ok(await _walletBusiness.GetWalletDetailsByUserID(userId));
        }

        [HttpGet("GetReferalWallets")]
        public async Task<ActionResult> GetReferalWallets(Guid userId, Guid appID)
        {
            var rez = await ValidateLogIn.Validate(unitOfWork, appID, userId);
            if (rez.StatusCode != 200)
            {
                return Ok(rez);
            }
            return Ok(await _walletBusiness.GetReferalWallets(userId));
        }

        [HttpPost("Recharge")]
        public async Task<ActionResult> Recharge(RequestObject<Payment> requestObject)
        {
            var rez = await ValidateLogIn.Validate(unitOfWork, requestObject.AppID, requestObject.Data.UserID);
            if (rez.StatusCode != 200)
            {
                return Ok(rez);
            }

            return Ok(await _walletBusiness.Recharge(requestObject.Data));
        }

        [HttpPost("Swap")]
        public async Task<ActionResult> Swap(RequestObject<Transaction> requestObject)
        {
            var rez = await ValidateLogIn.Validate(unitOfWork, requestObject.AppID, requestObject.Data.SenderID);
            if (rez.StatusCode != 200)
            {
                return Ok(rez);
            }

            return Ok(await _walletBusiness.Swap(requestObject.Data));
        }

        [HttpPost("Transfer")]
        public async Task<ActionResult> Transfer(RequestObject<Transaction> requestObject)
        {
            var rez = await ValidateLogIn.Validate(unitOfWork, requestObject.AppID, requestObject.Data.SenderID);
            if (rez.StatusCode != 200)
            {
                return Ok(rez);
            }

            var res = await _walletBusiness.Transfer(requestObject.Data);
            return Ok(res);
        }

        [HttpGet("GetWalletDetails")]
        public async Task<ActionResult> GetWalletDetails(Guid userID, Guid appID)
        {
            var rez = await ValidateLogIn.Validate(unitOfWork, appID, userID);
            if (rez.StatusCode != 200)
            {
                return Ok(rez);
            }
            var User = await _walletBusiness.GetWalletDetails(userID);
            return Ok(User);
        }

        //[HttpDelete("{id}")]
        //public async Task<ActionResult> Delete(Guid id)
        //{
        //    await _walletBusiness.Delete(id);
        //    return Ok("Record deleted successfully");
        //}
    }
}
