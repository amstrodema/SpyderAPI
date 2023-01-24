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
    public class SystemLogsBusiness
    {
        private readonly IUnitOfWork _unitOfWork;

        public SystemLogsBusiness(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<SystemLog>> GetSystemLogs() =>
         await _unitOfWork.SystemLogs.GetAll();

        public async Task<SystemLog> GetSystemLogByID(Guid id) =>
                  await _unitOfWork.SystemLogs.Find(id);
        public async Task<ResponseMessage<SystemLog>> Create(SystemLog SystemLog)
        {
            ResponseMessage<SystemLog> responseMessage = new ResponseMessage<SystemLog>();
            try
            {
                SystemLog.ID = Guid.NewGuid();
                SystemLog.DateCreated = DateTime.Now;
                await _unitOfWork.SystemLogs.Create(SystemLog);
                if (await _unitOfWork.Commit() >= 1)
                {
                    responseMessage.Data = SystemLog;
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
