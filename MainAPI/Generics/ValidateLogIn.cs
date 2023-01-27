using MainAPI.Data.Interface;
using MainAPI.Models;
using MainAPI.Models.Spyder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainAPI.Generics
{
    public class ValidateLogIn
    {
        public static async Task<ResponseMessage<string>> Validate(IUnitOfWork unitOfWork, Guid appID, Guid userID)
        {
            ResponseMessage<string> responseMessage = new ResponseMessage<string>();
            try
            {
                User user = await unitOfWork.Users.Find(userID);
                if (user == default)
                {
                    responseMessage.StatusCode = 201;
                    responseMessage.Message = "Invalid user account";
                }
                else if (!user.IsActive)
                {
                    responseMessage.StatusCode = 201;
                    responseMessage.Message = "User account limited";
                }
                else if (!user.IsActivated)
                {
                    responseMessage.StatusCode = 201;
                    responseMessage.Message = "Activate your account";
                }
                else if (await unitOfWork.LogInMonitors.GetLogInMonitorByAppIDAndUserID(appID, userID) == null)
                {
                    responseMessage.StatusCode = 201;
                    responseMessage.Message = "Login and try again";
                }
                else
                {
                    responseMessage.StatusCode = 200;
                }
            }
            catch (Exception)
            {
                responseMessage.StatusCode = 201;
                responseMessage.Message = "Try again later";
            }

            return responseMessage;
        }
    }
}
