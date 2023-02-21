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
    public class AreaBusiness
    {
        private readonly IUnitOfWork _unitOfWork;

        public AreaBusiness(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Area>> GetAreas() =>
         await _unitOfWork.Areas.GetAll();

        public async Task<Area> GetAreaByID(Guid id) =>
                  await _unitOfWork.Areas.Find(id);
        public async Task<ResponseMessage<Area>> Create(Area Area)
        {
            ResponseMessage<Area> responseMessage = new ResponseMessage<Area>();
            try
            {
                Area.ID = Guid.NewGuid();
                await _unitOfWork.Areas.Create(Area);
                if (await _unitOfWork.Commit() >= 1)
                {
                    responseMessage.Data = Area;
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
