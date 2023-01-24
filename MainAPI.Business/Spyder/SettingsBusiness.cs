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
    public class SettingsBusiness
    {
        private readonly IUnitOfWork _unitOfWork;

        public SettingsBusiness(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Settings>> GetSettings() =>
         await _unitOfWork.Settings.GetAll();

        public async Task<Settings> GetSettingsByID(Guid id) =>
                  await _unitOfWork.Settings.Find(id);
        public async Task<ResponseMessage<Settings>> GetSettingsByUserID(Guid userID)
        {
            ResponseMessage<Settings> responseMessage = new ResponseMessage<Settings>();

            var userLogin = await _unitOfWork.LogInMonitors.GetLogInMonitorByUserID(userID);

            if (userLogin == null)
            {
                responseMessage.StatusCode = 201;
                responseMessage.Message = "Log in required!";
                return responseMessage;
            }
          var setting =  await _unitOfWork.Settings.GetByUserID(userID);

            if (setting == null)
            {
                var user = await _unitOfWork.Users.Find(userID);

                if (user == null)
                {
                    responseMessage.StatusCode = 201;
                    responseMessage.Message = "User not found!";
                    return responseMessage;
                }

                setting = new Settings()
                {
                    ID = Guid.NewGuid(),
                    DateCreated = DateTime.Now,
                    IsActive = true,
                    IsAllowMessaging = true,
                    IsShowEmail = false,
                    UserID = userID,
                    ViewCountryID = user.CountryID,
                    IsAllowAccess = true,
                    IsAnoymousMessaging = true,
                    IsLocalRange = false,
                    IsReactionNotification = true,
                    IsRecieveAnoymousMessages = true,
                    IsSendNotificationToMail = false,
                    IsShowPhoneNo = false,

                };
                await _unitOfWork.Settings.Create(setting);

                if (await _unitOfWork.Commit() < 1)
                {
                    responseMessage.StatusCode = 201;
                    responseMessage.Message = "Error! Settings not found";
                    return responseMessage;
                }
            }

            try
            {
                setting.Country = (await _unitOfWork.Countries.Find(setting.ViewCountryID)).Name;
            }
            catch (Exception)
            {
            }

            responseMessage.StatusCode = 200;
            responseMessage.Data = setting;
            return responseMessage;
        }
        //public async Task<ResponseMessage<Settings>> Create(Settings settings)
        //{
        //    ResponseMessage<Settings> responseMessage = new ResponseMessage<Settings>();
        //    try
        //    {
        //        Area.ID = Guid.NewGuid();
        //        await _unitOfWork.Areas.Create(Area);
        //        if (await _unitOfWork.Commit() >= 1)
        //        {
        //            responseMessage.Data = Area;
        //            responseMessage.StatusCode = 200;
        //            responseMessage.Message = "Operation successful!";
        //        }
        //        else
        //        {
        //            responseMessage.StatusCode = 201;
        //            responseMessage.Message = "Operation not successful!";
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        responseMessage.StatusCode = 1018;
        //        responseMessage.Message = "Something went wrong. Try Again!";
        //    }

        //    return responseMessage;
        //}
        public async Task<ResponseMessage<Settings>> Update(Settings settings)
        {
            ResponseMessage<Settings> responseMessage = new ResponseMessage<Settings>();

            settings.DateModified = DateTime.Now;
            _unitOfWork.Settings.Update(settings);

            if (await _unitOfWork.Commit() < 1)
            {
                responseMessage.StatusCode = 201;
                responseMessage.Message = "Error! Not successful";
            }
            else
            {
                responseMessage.StatusCode = 200;
                responseMessage.Message = "Settings updated";
            }
                        
            return responseMessage;
        }
    }
}
