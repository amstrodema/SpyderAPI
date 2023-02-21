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
    public class SosBusiness
    {
        private readonly IUnitOfWork _unitOfWork;

        public SosBusiness(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Sos>> GetSos() =>
         await _unitOfWork.Sos.GetAll();

        public async Task<Sos> GetSosByID(Guid id) =>
                  await _unitOfWork.Sos.Find(id);
        public async Task<ResponseMessage<Sos>> Create(Sos Sos)
        {
            ResponseMessage<Sos> responseMessage = new ResponseMessage<Sos>();
            try
            {
                Sos.ID = Guid.NewGuid();
                Sos.DateCreated = DateTime.Now;
                await _unitOfWork.Sos.Create(Sos);
                if (await _unitOfWork.Commit() >= 1)
                {
                    responseMessage.Data = Sos;
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
