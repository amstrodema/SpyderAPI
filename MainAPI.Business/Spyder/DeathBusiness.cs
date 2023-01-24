using MainAPI.Data.Interface;
using MainAPI.Models;
using MainAPI.Models.Spyder;
using MainAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Business.Spyder
{
    public class DeathBusiness
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly WalletBusiness walletBusiness;

        public DeathBusiness(IUnitOfWork unitOfWork, WalletBusiness walletBusiness)
        {
            _unitOfWork = unitOfWork;
            this.walletBusiness = walletBusiness;
        }

        public async Task<List<Death>> GetDeaths() =>
         await _unitOfWork.Deaths.GetAll();

        public async Task<List<Death>> GetDeaths(string countryID)
        {
            var deaths = await _unitOfWork.Deaths.GetAll();
            try
            {
                Guid id = Guid.Parse(countryID);
                deaths = deaths.Where(p => p.CountryID == id).ToList();
            }
            catch (Exception)
            {

            }

            return deaths;
        }

        public async Task<Death> GetDeathByID(Guid id)
        {
            var death = await _unitOfWork.Deaths.Find(id);
            death.Image = ImageService.GetImageFromFolder(death.Image, "Death");
            return death;
        }

        public async Task<ResponseMessage<Death>> Create(Death Death)
        {
            ResponseMessage<Death> responseMessage = new ResponseMessage<Death>();
            try
            {
                Death.ID = Guid.NewGuid();
                Death.DateCreated = DateTime.Now;
                Death.IsActive = true;


                Params param = await _unitOfWork.Params.GetParamByCode("death_cost");
                decimal death_cost = 0;

                try
                {
                    death_cost = decimal.Parse(param.Value);
                }
                catch (Exception)
                {
                    responseMessage.StatusCode = 201;
                    responseMessage.Message = "Try again...";
                    return responseMessage;
                }

                var res = await walletBusiness.Payment(Death.CreatedBy, death_cost, Death.CountryID, "Death Register", Death.ID.ToString());

                if (res.StatusCode != 200)
                {
                    responseMessage.StatusCode = res.StatusCode;
                    responseMessage.Message = res.Message;
                    return responseMessage;
                }

                Death.Image = ImageService.SaveImageInFolder(Death.Image, Guid.NewGuid().ToString(), "Death");
                await _unitOfWork.Deaths.Create(Death);
                if (await _unitOfWork.Commit() >= 1)
                {
                    responseMessage.Data = Death;
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
