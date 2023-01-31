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
    public class FlagReportBusiness
    {
        private readonly IUnitOfWork _unitOfWork;

        public FlagReportBusiness(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<FlagReport>> GetFlagReports() =>
         await _unitOfWork.FlagReports.GetAll();

        public async Task<FlagReport> GetFlagReportByID(Guid id) =>
                  await _unitOfWork.FlagReports.Find(id);
        public async Task<FlagReport> GetReportByUserID_ItemID(Guid userID, Guid itemID) =>
                  await _unitOfWork.FlagReports.GetReportByUserID_ItemID(userID, itemID);
        public async Task<ResponseMessage<FlagReport>> Create(FlagReport flagReport)
        {
            ResponseMessage<FlagReport> responseMessage = new ResponseMessage<FlagReport>();
            try
            {
                FlagReport flagReport1 = await GetReportByUserID_ItemID(flagReport.PetitionerID, flagReport.ItemID);

                if (flagReport1 != null)
                {
                    responseMessage.StatusCode = 201;
                    responseMessage.Message = "Reported already";
                    return responseMessage;
                }

                flagReport.ID = Guid.NewGuid();
                flagReport.DateCreated = DateTime.Now;
                flagReport.IsActive = true;
                await _unitOfWork.FlagReports.Create(flagReport);
                if (await _unitOfWork.Commit() >= 1)
                {
                    responseMessage.Data = flagReport;
                    responseMessage.StatusCode = 200;
                    responseMessage.Message = "Report received!";
                }
                else
                {
                    responseMessage.StatusCode = 201;
                    responseMessage.Message = "Report not successful!";
                }
            }
            catch (Exception)
            {
                responseMessage.StatusCode = 1018;
                responseMessage.Message = "Try Again later.";
            }

            return responseMessage;
        }

    }
}
