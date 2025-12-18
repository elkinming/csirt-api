using System;
using AisinIX.Amateras.Common.Utilities;

namespace AisinIX.Amateras.Common.Configs
{
    /// <summary>
    /// APIコンテキストの構成設定
    /// </summary>
    public class ApiContextConfig : IApiContextConfig
    {
        /// <summary>
        /// 認証されない場合の認証アイシングループ会社コード
        /// </summary>
        public string UserGroupCompanyCode { get; private set; }

        /// <summary>
        /// 認証されない場合の認証ユーザID
        /// </summary>
        public string UserID { get; private set; }


        /// <summary>
        /// ユーザIDのヘッダー名
        /// </summary>
        public string UserHeaderName { get; private set; }

        private ApiContextConfig()
        {
            // 引数無しコンストラクタは無効
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public ApiContextConfig(IConfigUtility configUtility)
        {
            if (configUtility == null)
            {
                throw new ArgumentNullException();
            }

            UserGroupCompanyCode = configUtility.GetStringSetting("ApiContext.UserGroupCompanyCode");
            UserID = configUtility.GetStringSetting("ApiContext.UserID");
            UserHeaderName = configUtility.GetStringSetting("ApiContext.UserHeaderName");
        }
    }
}