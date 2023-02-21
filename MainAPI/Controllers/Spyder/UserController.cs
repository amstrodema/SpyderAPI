using MainAPI.Business.Spyder;
using MainAPI.Data.Interface;
using MainAPI.Generics;
using MainAPI.Models;
using MainAPI.Models.Spyder;
using MainAPI.Models.ViewModel;
using MainAPI.Models.ViewModel.Spyder;
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
    public class UserController : ControllerBase
    {
        private readonly UserBusiness _userBusiness;
        private readonly JWTService _jwtService;
        private readonly IUnitOfWork unitOfWork;

        public UserController(UserBusiness UserBusiness, JWTService jWTService, IUnitOfWork unitOfWork)
        {
            _userBusiness = UserBusiness;
            _jwtService = jWTService;
            this.unitOfWork = unitOfWork;
        }
        [ApiKeyAuth]
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var Users = await _userBusiness.GetUsers();
            return Ok(Users);
        }
        [ApiKeyAuth]
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            var User = await _userBusiness.GetUserByID(id);
            return Ok(User);
        }
        [ApiKeyAuth]
        [HttpGet("GetUserByUserName")]
        public async Task<ActionResult> GetUserByUserName(string username)
        {
            var res = await _userBusiness.GetUserByUserName(username);
            return Ok(res);
        }
        [ApiKeyAuth]
        [HttpGet("LogOut")]
        public async Task<ActionResult> LogOut(string userID)
        {
            var res = await _userBusiness.LogOut(userID);
            return Ok(res);
        }
        [ApiKeyAuth]
        [HttpGet("CheckUsername")]
        public async Task<ActionResult> CheckUsername(string username)
        {
            var res = await _userBusiness.CheckUsername(username);
            return Ok(res);
        }

        [HttpGet("VerifyEmail")]
        public async Task<ActionResult> VerifyEmail(string verificationCode)
        {
            var res = await _userBusiness.VerifyEmail(verificationCode);
            return Ok(res);
        }
        [ApiKeyAuth]
        [HttpPost("ActivateAccount")]
        public async Task<ActionResult> ActivateAccount(RequestObject<Payment> requestObject)
        {
            var rez = await ValidateLogIn.Validate(unitOfWork, requestObject.AppID, requestObject.Data.UserID);
            if (rez.StatusCode == 209)
            {
                return Ok(rez);
            }

            var res = await _userBusiness.ActivateAccount(requestObject.Data);
            return Ok(res);
        }
        [ApiKeyAuth]
        [HttpGet("ResendEmailVerification")]
        public async Task<ActionResult> ResendEmailVerification(string email)
        {
            var res = await _userBusiness.ResendEmailVerification(email);
            return Ok(res);
        }
        [ApiKeyAuth]
        [HttpGet("GetUserProfile")]
        public async Task<ActionResult> GetUserProfile(Guid userID, Guid appID, Guid profileUserID)
        {
            var rez = await ValidateLogIn.Validate(unitOfWork, appID, userID);
            bool isUser = true;
            if (userID != profileUserID || rez.StatusCode != 200)
            {
                isUser = false;
            }

            var res = await _userBusiness.GetUserProfile(profileUserID, isUser);
            return Ok(res);
        }
        [ApiKeyAuth]
        [HttpGet("ForgotPassword")]
        public async Task<ActionResult> ForgotPassword(string email)
        {
            var res = await _userBusiness.ForgotPassword(email);
            return Ok(res);
        }

        [HttpGet("ResetPassword")]
        public async Task<ActionResult> ResetPassword(string resetPasswordCode)
        {
            var res = await _userBusiness.ResetPassword(resetPasswordCode);
            return Ok(res);
        }
        [ApiKeyAuth]
        [HttpPost]
        public async Task<ActionResult> Post(User User)
        {

            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            var res = await _userBusiness.Create(User);
            return Ok(res);
        }
        [ApiKeyAuth]
        [HttpPost("SetUserImage")]
        public async Task<ActionResult> SetUserImage(RequestObject<Image> requestObject)
        {
            var rez = await ValidateLogIn.Validate(unitOfWork, requestObject.AppID, requestObject.Data.CreatedBy);
            if (rez.StatusCode != 200)
            {
                return Ok(rez);
            }

            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            var res = await _userBusiness.SetUserImage(requestObject.Data, requestObject.Data.CreatedBy);
            return Ok(res);
        }
        [ApiKeyAuth]
        [HttpPost("login")]
        public async Task<ActionResult> ValidateUser(LoginVM login)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            var res = await _userBusiness.ValidateUser(login);

            try
            {
                if (res.StatusCode == 200)
                {
                    res.Data.ClientSystem.JwtToken = _jwtService.GenerateSecurityToken(new UserSession()
                    {
                        EmailAddress = res.Data.User.Email,
                        UserID = res.Data.User.ID,
                        Username = res.Data.User.Username
                    });
                }
            }
            catch (Exception)
            {
            }

            return Ok(res);
        }

        //action used to test email
        [ApiKeyAuth]
        [HttpPost("SendEmail")]
        public async Task<ActionResult> ValidateUser(Models.Email email)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            await _userBusiness.SendEmail(email);
            return Ok();
        }

        [ApiKeyAuth]
        [HttpPut("{id}")]
        public async Task<ActionResult> Put([FromBody] RequestObject<User> requestObject, Guid id)
        {
            var rez = await ValidateLogIn.Validate(unitOfWork, requestObject.AppID, requestObject.Data.ID);
            if (rez.StatusCode != 200)
            {
                return Ok(rez);
            }
            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            if (requestObject.Data.ID != id)
                return BadRequest("Invalid record!");

            await _userBusiness.Update(requestObject.Data);
            return Ok(User);
        }
        [ApiKeyAuth]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _userBusiness.Delete(id);
            return Ok("Record deleted successfully");
        }
    }
}
