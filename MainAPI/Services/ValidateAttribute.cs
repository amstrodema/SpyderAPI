using MainAPI.Data.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainAPI.Services
{
    public class ValidateAttribute : Attribute, IAsyncActionFilter
    {
        private readonly IUnitOfWork _unitOfWork;

        public ValidateAttribute(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var val = context.HttpContext.Request.Query["userID"];
            //if (await _unitOfWork.LogInMonitors.GetLogInMonitorByUserID() == null)
            //{

            //}

            await next();
        }
        private async Task CheckValidation()
        {
            var redirectTarget = new RouteValueDictionary
            {
                {"action", "Log" },
                {"controller", "Log" }
            };

            var result = new RedirectToRouteResult(redirectTarget);
        }
    }
}
