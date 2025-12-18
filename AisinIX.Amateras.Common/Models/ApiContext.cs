using System;
using Microsoft.AspNetCore.Http;
using AisinIX.Amateras.Common.Configs;

namespace AisinIX.Amateras.Common.Models
{
    /// <summary>
    /// APIコンテキスト
    /// </summary>
    public class ApiContext : IApiContext
    {
        /// <summary>
        /// 要求IDのヘッダー名
        /// </summary>
        public string RequestIDHeaderName { get; } = "X-AMATERAS-GUID";

        /// <summary>
        /// 要求日時のヘッダー名
        /// </summary>
        public string RequestDateTimeHeaderName { get; } = "X-AMATERAS-DATETIME";

        /// <summary>
        /// 要求URIのヘッダー名
        /// </summary>
        public string RequestUriHeaderName { get; } = "X-AMATERAS-URL";

        /// <summary>
        /// 要求ホストのヘッダー名
        /// </summary>
        public string RequestHostHeaderName { get; } = "X-AMATERAS-HOST";

        /// <summary>
        /// ユーザIDのヘッダー名
        /// </summary>
        public string UserHeaderName { get; } = "X-AMATERAS-USER";

        /// <summary>
        /// AI-Cerf互換用ユーザIDのヘッダー名
        /// </summary>
        public string IVUserHeaderName { get; } = "IV-User";

        /// <summary>
        /// アイシングループ会社コードのヘッダー名(GAIA用)
        /// </summary>
        public string GAIAGroupCompanyCodeHeaderName { get; } = "X-GAIA-AisinGroupCompanyCd";

        /// <summary>
        /// 会社コードのヘッダー名(GAIA用)
        /// </summary>
        public string GAIACompanyCodeHeaderName { get; } = "X-GAIA-CompanyCd";

        /// <summary>
        /// ユーザIDのヘッダー名(GAIA用)
        /// </summary>
        public string GAIAUserHeaderName { get; } = "X-GAIA-NameCd";

        /// <summary>
        /// クライアントIPアドレスのヘッダー名
        /// </summary>
        public string ClientIPAddressHeaderName { get; } = "X-Forwarded-For";

        /// <summary>
        /// 要求ID
        /// </summary>
        public Guid RequestID { get; private set; }

        /// <summary>
        /// 要求日時
        /// </summary>
        public DateTime RequestDateTime { get; private set; }

        /// <summary>
        /// 要求URI情報
        /// </summary>
        public Uri RequestUri { get; private set; }

        /// <summary>
        /// 要求ホスト
        /// </summary>
        public string RequestHost { get; private set; }

        /// <summary>
        /// 実行ユーザのID
        /// </summary>
        public string UserID { get; private set; }

        /// <summary>
        /// 実行ユーザのアイシングループ会社コード
        /// </summary>
        public string UserGroupCompanyCode { get; private set; }

        /// <summary>
        /// 実行ユーザのアイシン会社コード
        /// </summary>
        public string UserCompanyCode { get; private set; }

        /// <summary>
        /// クライアントIPアドレス
        /// </summary>
        public string ClientIPAddress { get; private set; }

        private UserIdentity _uo;
        /// <summary>
        /// 実行ユーザのIDとアイシングループ会社コードを含むオブジェクト
        /// </summary>
        public UserIdentity UserObject {
            get {
                if (_uo == null)
                {
                    _uo = new UserIdentity(this.UserID, this.UserGroupCompanyCode);
                }
                return _uo;
            }
        }

        /// <summary>
        /// 現在のHttpContext
        /// </summary>
        public HttpContext CurrentHttpContext
        {
            get {
                return _httpContextAccessor.HttpContext;
            }
        }

        private readonly IApiContextConfig _apiContextConfig;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private ApiContext()
        {
            // 引数無しコンストラクタは無効
        }

        /// <summary>
        /// 新しいAPIコンテキストのインスタンスを作成します。
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        public ApiContext(IApiContextConfig apiContextConfig, IHttpContextAccessor httpContextAccessor)
        {
            if ((apiContextConfig == null) || (httpContextAccessor == null))
            {
                throw new ArgumentNullException();
            }

            _apiContextConfig = apiContextConfig;
            _httpContextAccessor = httpContextAccessor;

            var ctx = _httpContextAccessor.HttpContext;
            var headers = ctx.Request.Headers;

            // 要求IDの設定 //
            Guid requestGUID;
            if (Guid.TryParse(GetHeaderValue(headers, RequestIDHeaderName), out requestGUID))
            {
                this.RequestID = requestGUID;
            }
            else
            {
                this.RequestID = Guid.NewGuid();
            }
            //log4net.MDC.Set("REQUEST_ID", this.RequestID.ToString("D"));

            // 要求日時の設定 //
            DateTime requestDateTime;
            if (DateTime.TryParse(GetHeaderValue(headers, RequestDateTimeHeaderName), out requestDateTime))
            {
                this.RequestDateTime = requestDateTime;
            }
            else
            {
                this.RequestDateTime = DateTime.Now;
            }

            // 要求Uriの設定 //
            try
            {
                this.RequestUri = new Uri(GetHeaderValue(headers, RequestUriHeaderName));
            }
            catch (Exception)
            {
                this.RequestUri = GetUri(ctx.Request);
            }

            // 要求ホストの設定 //
            this.RequestHost = GetHeaderValue(headers, RequestHostHeaderName, ctx.Request.Host.Value);

            // ユーザIDの設定 //
            this.UserID = GetHeaderValue(headers, UserHeaderName);
            if (String.IsNullOrWhiteSpace(this.UserID)) this.UserID = GetHeaderValue(headers, GAIAUserHeaderName);
            if (String.IsNullOrWhiteSpace(this.UserID)) this.UserID = ctx.User.Identity.Name;
            if (String.IsNullOrWhiteSpace(this.UserID)) this.UserID = _apiContextConfig.UserID;
            if (String.IsNullOrWhiteSpace(this.UserID)) this.UserID = String.Empty;
            //log4net.MDC.Set("USER_ID", this.UserID);

            // クライアントIPアドレスの設定 //
            this.ClientIPAddress = GetHeaderValue(headers, ClientIPAddressHeaderName);

            // 実行ユーザのアイシングループ会社コードの設定 //
            this.UserGroupCompanyCode = GetHeaderValue(headers, GAIAGroupCompanyCodeHeaderName);
            if (String.IsNullOrWhiteSpace(this.UserGroupCompanyCode)) this.UserGroupCompanyCode = _apiContextConfig.UserGroupCompanyCode;

            // 実行ユーザのアイシン会社コードの設定 //
            this.UserCompanyCode = GetHeaderValue(headers, GAIACompanyCodeHeaderName);
        
        }

        /// <summary>
        /// ヘッダーから指定キーの値を取得します。
        /// </summary>
        /// <param name="headers"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        private string GetHeaderValue(IHeaderDictionary headers, string key, string defaultValue = null)
        {
            if (headers.ContainsKey(key))
            {
                return headers[key][0];
            }
            if (defaultValue != null)
            {
                return defaultValue;
            }
            return string.Empty;
        }

        /// <summary>
        /// URIを取得します。
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private Uri GetUri(HttpRequest request)
        {
            var builder = new UriBuilder();
            builder.Scheme = request.Scheme;
            builder.Path = request.Path;
            builder.Query = request.QueryString.ToUriComponent();
            builder.Host = request.Host.Host;
            if (request.Host.Port.HasValue)
            {
                builder.Port = request.Host.Port.Value;
            }
            return builder.Uri;
        }
    }
}
