using MainAPI.Business.Spyder;
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
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly CountryBusiness _countryBusiness;
        private readonly JWTService _jwtService;

        public CountryController(CountryBusiness countryBusiness, JWTService jWTService)
        {
            _countryBusiness = countryBusiness;
            _jwtService = jWTService;
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
        public async Task<ActionResult> Post(Country country)
        {

            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            var res = await _countryBusiness.Create(country);
            return Ok(res);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> Put([FromBody] Country country, Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            if (country.ID != id)
                return BadRequest("Invalid record!");

            int res = await _countryBusiness.Update(country);
            ResponseMessage<Country> responseMessage = new ResponseMessage<Country>();
            if (res >= 1)
            {
                responseMessage.Message = "Record updated!";
                responseMessage.StatusCode = 200;
                responseMessage.Data = country;
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
