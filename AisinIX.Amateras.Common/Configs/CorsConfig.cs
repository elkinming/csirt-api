using System;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AisinIX.Amateras.Common.Models;

namespace AisinIX.Amateras.Common.Configs
{
    public static class CorsConfigExtensions 
    {
        private static readonly string AmaterasAllowSpecificOrigins = "_amaterasAllowSpecificOrigins";

        /// <summary>
        /// サービスコレクションにCORSを追加します。
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddAmaterasCors(this IServiceCollection services, IConfiguration configuration)
        {
            string[] urls;
            try
            {
                var appSettings = configuration.Get<AppSettings>();
                urls = appSettings.Configs["Amateras.CorsOrigins"].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                for (var i = 0; i < urls.Length; i++) {
                    urls[i] = urls[i].Trim();
                }
            }
            catch(Exception)
            {
                // 設定値の取得に失敗した場合は何もしない
                return services;
            }

            return services.AddCors(options => {
                options.AddPolicy(AmaterasAllowSpecificOrigins,
                builder =>
                {
                    builder.WithOrigins(urls)
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
        }

        /// <summary>
        /// WebアプリケーションパイプラインにCORSを追加します。
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseAmaterasCors(this IApplicationBuilder builder)
        {
            return builder.UseCors(AmaterasAllowSpecificOrigins);
        }

    }
}