using MainAPI.Data.Interface;
using MainAPI.Models;
using MainAPI.Models.Spyder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Business.Spyder
{
    public class CountryBusiness
    {
        private readonly IUnitOfWork _unitOfWork;

        public CountryBusiness(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Country>> GetCountries() =>
         await _unitOfWork.Countries.GetAll();

        public async Task<int> Update(Country country)
        {
            country.DateModified = DateTime.Now;
            _unitOfWork.Countries.Update(country);
            return await _unitOfWork.Commit();
        }

        public async Task<Country> GetCountryByID(Guid id) =>
                  await _unitOfWork.Countries.Find(id);
        public async Task<ResponseMessage<Country>> Create(Country country)
        {
            ResponseMessage<Country> responseMessage = new ResponseMessage<Country>();
            try
            {
                country.ID = Guid.NewGuid();
                country.DateCreated = DateTime.Now;
                country.IsActive = true;
                await _unitOfWork.Countries.Create(country);
                if (await _unitOfWork.Commit() >= 1)
                {
                    responseMessage.Data = country;
                    responseMessage.StatusCode = 200;
                    responseMessage.Message = "Operation successful!";
                }
                else
                {
                    responseMessage.StatusCode = 201;
                    responseMessage.Message = "Operation not successful!";
                }
            }
            catch (Exception)
            {
                responseMessage.StatusCode = 1018;
                responseMessage.Message = "Something went wrong. Try Again!";
            }

            return responseMessage;
        }

    }
}
