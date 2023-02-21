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
    public class LocationCenterBusiness
    {
        private readonly IUnitOfWork _unitOfWork;

        public LocationCenterBusiness(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<LocationCenter>> GetLocationCenters() =>
         await _unitOfWork.LocationCenters.GetAll();

        public async Task<LocationCenter> GetLocationCenterByID(Guid id) =>
                  await _unitOfWork.LocationCenters.Find(id);
        public async Task<ResponseMessage<LocationCenter>> Create(LocationCenter LocationCenter)
        {
            ResponseMessage<LocationCenter> responseMessage = new ResponseMessage<LocationCenter>();
            try
            {
                LocationCenter.ID = Guid.NewGuid();
                LocationCenter.DateCreated = DateTime.Now;
                await _unitOfWork.LocationCenters.Create(LocationCenter);
                if (await _unitOfWork.Commit() >= 1)
                {
                    responseMessage.Data = LocationCenter;
                    responseMessage.StatusCode = 200;
                    responseMessage.Message = "Successful!";
                }
                else
                {
                    responseMessage.StatusCode = 201;
                    responseMessage.Message = "Not successful!";
                }
            }
            catch (Exception)
            {
                responseMessage.StatusCode = 1018;
                responseMessage.Message = "Failed. Try Again!";
            }

            return responseMessage;
        }
    }
}
