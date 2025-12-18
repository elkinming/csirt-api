using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using AisinIX.Amateras.Common.Models;

namespace AisinIX.Amateras.Common.Utilities
{
    public class Log4NetMdcMiddleware
    {
        private readonly RequestDelegate _next;

        public Log4NetMdcMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IApiContext _apiContext)
        {
            var userID = "";
            if (string.IsNullOrEmpty(_apiContext.UserID))
            {
                userID = "Unknown";
            }
            else
            {
                userID = _apiContext.UserID;
                if (!string.IsNullOrEmpty(_apiContext.UserGroupCompanyCode))
                {
                    userID +=  "-" + _apiContext.UserGroupCompanyCode;
                }
            }

            log4net.MDC.Set("REQUEST_ID", _apiContext.RequestID.ToString("D"));
            log4net.MDC.Set("USER_ID", userID);

            await _next.Invoke(context);
        }
    }
}
