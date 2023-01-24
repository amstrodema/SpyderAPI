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
    public class WalletController : ControllerBase
    {

        private readonly WalletBusiness _walletBusiness;
        private readonly JWTService _jwtService;

        public WalletController(WalletBusiness walletBusiness, JWTService jWTService)
        {
            _walletBusiness = walletBusiness;
            _jwtService = jWTService;
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
        public async Task<ActionResult> GetWalletDetailsByUserID(Guid userId)
        {
            return Ok(await _walletBusiness.GetWalletDetailsByUserID(userId));
        }

        [HttpGet("GetReferalWallets")]
        public async Task<ActionResult> GetReferalWallets(Guid userId)
        {
            return Ok(await _walletBusiness.GetReferalWallets(userId));
        }

        [HttpPost("Recharge")]
        public async Task<ActionResult> Recharge(Payment payment)
        {
            return Ok(await _walletBusiness.Recharge(payment));
        }

        [HttpPost("Swap")]
        public async Task<ActionResult> Swap(Transaction transaction)
        {
            return Ok(await _walletBusiness.Swap(transaction));
        }

        [HttpPost("Transfer")]
        public async Task<ActionResult> Transfer(Transaction transaction)
        {
            var res = await _walletBusiness.Transfer(transaction);
            return Ok(res);
        }

        [HttpGet("GetWalletDetails")]
        public async Task<ActionResult> GetWalletDetails(Guid userID)
        {
            var User = await _walletBusiness.GetWalletDetails(userID);
            return Ok(User);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _walletBusiness.Delete(id);
            return Ok("Record deleted successfully");
        }
    }
}
