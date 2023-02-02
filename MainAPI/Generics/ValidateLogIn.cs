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
                Params param = await unitOfWork.Params.GetParamByCode("systemIsUP");

                User user = await unitOfWork.Users.Find(userID);
                if (user == default)
                {
                    responseMessage.StatusCode = 209;
                    responseMessage.Message = "Invalid user account";
                }
                else if (await unitOfWork.LogInMonitors.GetLogInMonitorByAppIDAndUserID(appID, userID) == null)
                {
                    responseMessage.StatusCode = 209;
                    responseMessage.Message = "Login and try again";
                }
                else if (!user.IsActivated)
                {
                    responseMessage.StatusCode = 201;
                    responseMessage.Message = "Activate your account";
                    if (user.AccessLevel > 7)
                    {
                        responseMessage.StatusCode = 200;
                    }
                }
                else if (!user.IsActive)
                {
                    responseMessage.StatusCode = 209;
                    responseMessage.Message = "User account limited";
                    if (user.AccessLevel > 7)
                    {
                        responseMessage.StatusCode = 200;
                    }
                }
                else if (param.Value != "true")
                {
                    responseMessage.StatusCode = 209;
                    responseMessage.Message = "System Clean Up In Progress...Try later!";

                    if (user.AccessLevel > 5)
                    {
                        responseMessage.StatusCode = 200;
                    }
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
