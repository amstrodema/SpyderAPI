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
    public class LogInMonitorBusiness
    {
        private readonly IUnitOfWork _unitOfWork;

        public LogInMonitorBusiness(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<LogInMonitor>> GetLogInMonitors() =>
         await _unitOfWork.LogInMonitors.GetAll();

        public async Task<LogInMonitor> GetLogInMonitorByID(Guid id) =>
                  await _unitOfWork.LogInMonitors.Find(id);

        public async Task<LogInMonitor> GetLogInMonitorByUserID(Guid userID) =>
                  await _unitOfWork.LogInMonitors.GetLogInMonitorByUserID(userID);

        public async Task<ResponseMessage<LogInMonitor>> Create(LogInMonitor LogInMonitor)
        {
            ResponseMessage<LogInMonitor> responseMessage = new ResponseMessage<LogInMonitor>();
            try
            {
                LogInMonitor.ID = Guid.NewGuid();
                LogInMonitor.DateCreated = DateTime.Now;
                LogInMonitor.IsActive = true;
                await _unitOfWork.LogInMonitors.Create(LogInMonitor);
                if (await _unitOfWork.Commit() >= 1)
                {
                    responseMessage.Data = LogInMonitor;
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
