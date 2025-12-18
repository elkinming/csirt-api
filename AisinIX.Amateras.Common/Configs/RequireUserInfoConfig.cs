using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using AisinIX.Amateras.Common.Models;

namespace AisinIX.Amateras.Common.Configs
{
    public static class RequireUserInfoConfigExtensions 
    {

        /// <summary>
        /// Webアプリケーションパイプラインに認証情報チェックを追加します。
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseAmaterasRequireUserInfo(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AmaterasRequireUserInfoMiddleware>();
        }
    }

    public class AmaterasRequireUserInfoMiddleware
    {
        private readonly RequestDelegate _next;

        public AmaterasRequireUserInfoMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IApiContext _apiContext)
        {
            if (string.IsNullOrEmpty(_apiContext.UserID) || string.IsNullOrEmpty(_apiContext.UserGroupCompanyCode))
            {
                context.Response.Clear();
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return;
            }

            await _next.Invoke(context);
        }
    }    


}