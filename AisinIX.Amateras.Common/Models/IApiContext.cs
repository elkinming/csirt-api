using System;
using Microsoft.AspNetCore.Http;

namespace AisinIX.Amateras.Common.Models
{
    public interface IApiContext
    {
        /// <summary>
        /// 要求IDのヘッダー名
        /// </summary>
        string RequestIDHeaderName { get; }

        /// <summary>
        /// 要求日時のヘッダー名
        /// </summary>
        string RequestDateTimeHeaderName { get; }

        /// <summary>
        /// 要求URIのヘッダー名
        /// </summary>
        string RequestUriHeaderName { get; }

        /// <summary>
        /// 要求ホストのヘッダー名
        /// </summary>
        string RequestHostHeaderName { get; }

        /// <summary>
        /// ユーザIDのヘッダー名
        /// </summary>
        string UserHeaderName { get; }

        /// <summary>
        /// AI-Cerf互換用ユーザIDのヘッダー名
        /// </summary>
        string IVUserHeaderName { get; }

        /// <summary>
        /// アイシングループ会社コードのヘッダー名(GAIA用)
        /// </summary>
        string GAIAGroupCompanyCodeHeaderName { get; }

        /// <summary>
        /// 会社コードのヘッダー名(GAIA用)
        /// </summary>
        string GAIACompanyCodeHeaderName { get; }

        /// <summary>
        /// ユーザIDのヘッダー名(GAIA用)
        /// </summary>
        string GAIAUserHeaderName { get; }

        /// <summary>
        /// クライアントIPアドレスのヘッダー名
        /// </summary>
        string ClientIPAddressHeaderName { get; }
        /// <summary>
        /// 要求ID
        /// </summary>
        public Guid RequestID { get; }

        /// <summary>
        /// 要求日時
        /// </summary>
        public DateTime RequestDateTime { get; }

        /// <summary>
        /// 要求URI情報
        /// </summary>
        public Uri RequestUri { get; }

        /// <summary>
        /// 要求ホスト
        /// </summary>
        public string RequestHost { get; }

        /// <summary>
        /// 実行ユーザのID
        /// </summary>
        public string UserID { get; }

        /// <summary>
        /// 実行ユーザのアイシングループ会社コード
        /// </summary>
        public string UserGroupCompanyCode { get; }

        /// <summary>
        /// 実行ユーザのアイシン会社コード
        /// </summary>
        public string UserCompanyCode { get; }

        /// <summary>
        /// クライアントIPアドレス
        /// </summary>
        public string ClientIPAddress { get; }

        /// <summary>
        /// 実行ユーザのIDとアイシングループ会社コードを含むオブジェクト
        /// </summary>
        public UserIdentity UserObject { get; }

        /// <summary>
        /// 現在のHttpContext
        /// </summary>
        public HttpContext CurrentHttpContext { get; }
    }
}
