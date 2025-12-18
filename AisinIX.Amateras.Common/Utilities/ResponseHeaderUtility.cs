using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AisinIX.Amateras.Common.Utilities
{
    /// <summary>
    /// レスポンスヘッダ付与ユーティリティ
    /// </summary>
    /// <remarks>
    /// Startup.csのConfigure()にapp.UseWebUIResponseHeader()を追加
    /// ※全てのAPIに対して付与
    /// </remarks>
    public static class ResponseHeaderExtensions
    {
        /// <summary>
        /// レスポンスヘッダに下記4種類を付与します。
        /// 「X-FRAME-OPTIONS: Deny」「X-XSS-Protection：1; mode=block」「X-Content-Type-Options：nosniff」「Strict-Transport-Security:max-age=10886400; includeSubDomains」
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseWebUIResponseHeader(this IApplicationBuilder builder)
        {
            return builder.Use(async (context, next) =>
            {
                // ブラウザがページをframe等に表示することを許可するか：しない
                context.Response.Headers.Add("X-FRAME-OPTIONS", "Deny");
                // クロスサイトスクリプト攻撃を検出するか、検出時の動作：フィルタリング有、検出時ページ描画停止
                context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
                // Content-Typeに示されたMIMEタイプに従わせるか：従わせる
                context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                // HTTPの代わりにHTTPSを使用させる：有効期間18週間、サブドメインを含む
                context.Response.Headers.Add("Strict-Transport-Security", "max-age=10886400; includeSubDomains");
                await next();
            });
        }
    }

    /// <summary>
    /// ファイルダウンロードAPI時のレスポンスヘッダ付与ユーティリティ
    /// </summary>
    /// <remakrs>
    /// ControlerのApi単位で[ServiceFilter(typeof(FileDownloadResponseHeaderAttribute))]を付与する
    /// StartUp.csのConfigureServices()にservices.AddScoped<FileDownloadResponseHeaderAttribute>()を追加
    /// ※特定のAPIにのみ付与
    /// </remakrs>
    public class FileDownloadResponseHeaderAttribute : ResultFilterAttribute
    {
        public FileDownloadResponseHeaderAttribute()
        {
        }

        /// <summary>
        /// アクション結果の実行前に呼び出されます。
        /// </summary>
        /// <param name="context"></param>
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if(context.Result is FileContentResult FileResult ||
               context.Result is OkObjectResult OkResult)
            {
                // Content-Typeに示されたMIMEタイプに従わせるか：従わせる
                context.HttpContext.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                if(!context.HttpContext.Response.Headers.ContainsKey("X-FRAME-OPTIONS"))
                {
                    // ブラウザがページをframe等に表示することを許可するか：しない
                    context.HttpContext.Response.Headers.Add("X-FRAME-OPTIONS", "Deny");
                }
                if(!context.HttpContext.Response.Headers.ContainsKey("Content-Disposition"))
                {
                    // ブラウザにインライン表示するか：インライン表示する（ダウンロードしてローカルに保存する添付ファイルとする）
                    context.HttpContext.Response.Headers.Add("Content-Disposition", "attachment");
                }
            }
            base.OnResultExecuting(context);
        }
    }
}
