using AisinIX.Amateras.Common.Configs;
using AisinIX.Amateras.Common.Models;
using AisinIX.Amateras.Common.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Polly;
using Polly.Extensions.Http;
using System.Net.Http;
using System;
using AisinIX.CSIRT.CompanyRoleMember.DBAccessors;
using AisinIX.CSIRT.CompanyRoleMember.Services;

using System.Text.Encodings.Web;
using System.Text.Unicode;
using AisinIX.CSIRT.CompanyRoleMember.Db;

namespace AisinIX.CSIRT.CompanyRoleMember.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // CORS
            services.AddAmaterasCors(Configuration);

            var encoderSettings = new TextEncoderSettings();
            // 基本ラテン文字(U+0021 - U+007F)はエンコード対象外
            encoderSettings.AllowRange(UnicodeRanges.BasicLatin);
            // 基本ラテン文字範囲でもスラッシュはエンコード対象
            encoderSettings.ForbidCharacters('\u002F');
 
            // Controllers
            services.AddControllers().AddJsonOptions(options => 
            {
                options.JsonSerializerOptions.WriteIndented = true;
#if DEBUG
#else
                options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(encoderSettings);
#endif
            });
            // DBConnector
            services.AddScoped<IDbConnector, OracleDbConnector>();

            services.Configure<AppSettings>(Configuration);
            // HttpClient
            //サービス
            services.AddScoped<ICompanyRoleMemberService,CompanyRoleMemberService>();
            services.AddScoped<ICompanyRoleMemberDBAccessor,CompanyRoleMemberDBAccessor>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<ICompanyDBAccesor, CompanyDBAccesor>();
            services.AddScoped<ICompanyRoleOpsService, CompanyRoleOpsService>();
            services.AddScoped<ICompanyRoleOpsDBAccessor, CompanyRoleOpsDBAccessor>();
            services.AddScoped<IInformationSecurityService, InformationSecurityService>();
            services.AddScoped<IInformationSecurityDBAccessor, InformationSecurityDBAccesor>();
            services.AddScoped<ICompanyPermissionService, CompanyPermissionService>();
            services.AddScoped<ICompanyPermissionDBAccessor, CompanyPermissionDBAccessor>();
            services.AddScoped<ILogInfoService, LogInfoService>();
            services.AddScoped<ILogInfoDBAccessor, LogInfoDBAccessor>();

            // Amateras関係
            services.AddHttpContextAccessor();
            services.AddScoped<IApiContext, ApiContext>();
            services.AddSingleton<IConfigUtility, ConfigUtility>();
            services.AddScoped<IJsonServiceUtility, JsonServiceUtility>();
            services.AddSingleton<IApiContextConfig, ApiContextConfig>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AisinIX.CSIRT.CompanyRoleMember.WebApi", Version = "v1" });
            });

            services.AddScoped<PostgresConnection>();
            services.AddSingleton<DapperDbContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AisinIX.CSIRT.CompanyRoleMember.WebApi v1"));
            }

            // app.UseHttpsRedirection();
            // レスポンスヘッダにセキュリティ用ヘッダを追加
            app.UseWebUIResponseHeader();

            app.UseRouting();

            // UseCorsはUseRoutingとUseEndpointsの間に置く必要がある
            app.UseAmaterasCors();
            if (!env.IsDevelopment())
            {
                app.UseMiddleware<AmaterasRequireUserInfoMiddleware>();
            }

            // ログにユーザーIDと要求IDが入るように設定する
            app.UseMiddleware<Log4NetMdcMiddleware>();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        /// <summary>
        /// 指数バックオフとジッターを取り入れたリトライポリシーを取得します。
        /// </summary>
        /// <returns></returns>
        static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            Random jitterer = new Random();

            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(
                    6,  // exponential back-off plus some jitter
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                    + TimeSpan.FromMilliseconds(jitterer.Next(0, 100))
                );
        }
    }
}
