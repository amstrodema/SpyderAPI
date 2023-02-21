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
    public class MajorFunctionLogBusiness
    {
        private readonly IUnitOfWork _unitOfWork;

        public MajorFunctionLogBusiness(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<MajorFunctionLog>> GetMajorFunctionLogs() =>
         await _unitOfWork.MajorFunctionLogs.GetAll();

        public async Task<MajorFunctionLog> GetMajorFunctionLogByID(Guid id) =>
                  await _unitOfWork.MajorFunctionLogs.Find(id);
        public async Task<ResponseMessage<MajorFunctionLog>> Create(MajorFunctionLog MajorFunctionLog)
        {
            ResponseMessage<MajorFunctionLog> responseMessage = new ResponseMessage<MajorFunctionLog>();
            try
            {
                MajorFunctionLog.ID = Guid.NewGuid();
                MajorFunctionLog.DateCreated = DateTime.Now;
                await _unitOfWork.MajorFunctionLogs.Create(MajorFunctionLog);
                if (await _unitOfWork.Commit() >= 1)
                {
                    responseMessage.Data = MajorFunctionLog;
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
