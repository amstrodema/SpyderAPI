using MainAPI.Business.Spyder;
using MainAPI.Data.Interface;
using MainAPI.Data.Repository;
using MainAPI.Generics;
using MainAPI.Models;
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
    public class CountryController : ControllerBase
    {
        private readonly CountryBusiness _countryBusiness;
        private readonly JWTService _jwtService;
        private readonly IUnitOfWork unitOfWork;

        public CountryController(CountryBusiness countryBusiness, JWTService jWTService, IUnitOfWork unitOfWork)
        {
            _countryBusiness = countryBusiness;
            _jwtService = jWTService;
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var countries = await _countryBusiness.GetCountries();
            return Ok(countries);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            var country = await _countryBusiness.GetCountryByID(id);
            return Ok(country);
        }
        [HttpPost]
        public async Task<ActionResult> Post(RequestObject<Country> requestObject)
        {
            var rez = await ValidateLogIn.Validate(unitOfWork, requestObject.AppID, requestObject.Data.CreatedBy);
            if (rez.StatusCode != 200)
            {
                return Ok(rez);
            }

            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            var res = await _countryBusiness.Create(requestObject.Data);
            return Ok(res);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> Put([FromBody] RequestObject<Country> requestObject, Guid id)
        {
            var rez = await ValidateLogIn.Validate(unitOfWork, requestObject.AppID, requestObject.Data.ModifiedBy);
            if (rez.StatusCode != 200)
            {
                return Ok(rez);
            }

            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            if (requestObject.Data.ID != id)
                return BadRequest("Invalid record!");

            int res = await _countryBusiness.Update(requestObject.Data);
            ResponseMessage<Country> responseMessage = new ResponseMessage<Country>();
            if (res >= 1)
            {
                responseMessage.Message = "Record updated!";
                responseMessage.StatusCode = 200;
            }
            else
            {
                responseMessage.Message = "No record updated!";
                responseMessage.StatusCode = 201;
            }
            return Ok(responseMessage);
        }
    }
}
