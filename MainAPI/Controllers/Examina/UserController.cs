using MainAPI.Business.Examina;
using MainAPI.Models.Examina;
using MainAPI.Models.ViewModel.Examina;
using MainAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainAPI.Controllers.Examina
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserBusiness _userBusiness;
        private readonly JWTService _jwtService;

        public UserController(UserBusiness UserBusiness, JWTService jWTService)
        {
            _userBusiness = UserBusiness;
            _jwtService = jWTService;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var Users = await _userBusiness.GetUsers();
            return Ok(Users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            var User = await _userBusiness.GetUserByID(id);
            return Ok(User);
        }

        [HttpGet("GetInstructorsByNodeID")]
        public async Task<ActionResult> GetInstructorsByNodeID(Guid nodeID)
        {
            var res = await _userBusiness.GetInstructorsByNodeID(nodeID);
            return Ok(res);
        }

        [HttpPost]
        public async Task<ActionResult> Post(User User)
        {
            
            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            await _userBusiness.Create(User);
            return Ok(User);
        }

        [HttpPost("login")]
        public async Task<ActionResult> ValidateUser(LoginVM login)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            var res = await _userBusiness.ValidateUser(login);
            return Ok(res);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put([FromBody] User User, Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            if (User.ID != id)
                return BadRequest("Invalid record!");

            await _userBusiness.Update(User);
            return Ok(User);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _userBusiness.Delete(id);
            return Ok("Record deleted successfully");
        }
    }
}
