using System;
using System.Net;
using System.Text.Json;
using AisinIX.Amateras.Common.Utilities;
using AisinIX.CSIRT.CompanyRoleMember.Common.Configs;
using AisinIX.CSIRT.CompanyRoleMember.Common.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace AisinIX.CSIRT.CompanyRoleMember.Common.ServiceAccessors
{
    public class CompanyRoleMemberServiceAccessor : ICompanyRoleMemberServiceAccessor
    {

        private readonly ILogger<CompanyRoleMemberServiceAccessor> _logger;
        private readonly IConfigUtility _configUtility;
        private readonly ICompanyRoleMemberConfig _CompanyRoleMemberConfig;
        private readonly IJsonServiceUtility _jsonServiceUtility;
        private readonly string _CompanyRoleMemberBaseUri;

        public CompanyRoleMemberServiceAccessor(ILogger<CompanyRoleMemberServiceAccessor> logger, IConfigUtility configUtility,
            ICompanyRoleMemberConfig CompanyRoleMemberConfig, IJsonServiceUtility jsonServiceUtility)
        {
            _logger = logger;
            _configUtility = configUtility;
            _CompanyRoleMemberConfig = CompanyRoleMemberConfig;
            _jsonServiceUtility = jsonServiceUtility;

            try
            {
                _CompanyRoleMemberBaseUri = new Uri(_CompanyRoleMemberConfig.CompanyRoleMemberServiceUrl).ToString().TrimEnd('/');
            }
            catch(UriFormatException ex)
            {
                throw new UriFormatException("設定値[CompanyRoleMember.Service.URL]が見つかりませんでした。", ex);
            }
                
        }

        /// <summary>
        /// SP1 ログ情報一覧取得
        /// </summary>
        public List<LogInfo> QuerylogInfo(string logDate, IAsyncProcessHelper helper = null)
        {
            try
            {
                helper?.PreInvoke();
                string url = $"{_CompanyRoleMemberBaseUri}/logList?logDate="+logDate;

                _logger.LogInformation($"┏サービス連携開始 連携先URL:{url}");

                var response = _jsonServiceUtility.Get(url);

                helper?.PostInvoke();
                if (String.IsNullOrEmpty(response)) {
                    _logger.LogInformation($"ステータスコード:{(int)HttpStatusCode.BadRequest}");
                    _logger.LogInformation($"┗サービス連携終了 ログ情報一覧取得に失敗しました。");
                    return null;
                }

                var resultList = JsonSerializer.Deserialize<List<LogInfo>>(response);
                _logger.LogInformation($"ステータスコード:{(int)HttpStatusCode.OK}");
                _logger.LogInformation($"┗サービス連携終了 正常終了しました。");  
                return resultList;
            }
            catch (Exception)
            {
                _logger.LogInformation($"ステータスコード:{(int)HttpStatusCode.InternalServerError}");
                _logger.LogInformation($"┗サービス連携終了 ログ情報一覧取得に失敗しました。");
                throw;
            }
           


        }

        /// <summary>
        /// ログ情報新規登録
        /// </summary>
        public void LogInfoInsert(LogInfo logInfo, IAsyncProcessHelper helper = null)
        {
            try
            {
                helper?.PreInvoke();
                string url = $"{_CompanyRoleMemberBaseUri}/logInsert";

                _logger.LogInformation($"┏サービス連携開始 連携先URL:{url}");

                var stringContent = JsonSerializer.Serialize(logInfo);
                var response = _jsonServiceUtility.Post(url, stringContent);

                helper?.PostInvoke();
                _logger.LogInformation($"ステータスコード:{(int)HttpStatusCode.OK}");
                _logger.LogInformation($"┗サービス連携終了 正常終了しました。");  
            }
            catch (Exception)
            {
                _logger.LogInformation($"ステータスコード:{(int)HttpStatusCode.InternalServerError}");
                _logger.LogInformation($"┗サービス連携終了 ログ登録に失敗しました。");
                throw;
            }
        }

        /// <summary>
        /// SP3 対象年一覧取得
        /// </summary>
        public List<LogInfo> QueryYearList(IAsyncProcessHelper helper = null)
        {
            try
            {
                helper?.PreInvoke();
                string url = $"{_CompanyRoleMemberBaseUri}/getYearList";

                _logger.LogInformation($"┏サービス連携開始 連携先URL:{url}");

                var  response = _jsonServiceUtility.Get(url);

                helper?.PostInvoke();

                if (String.IsNullOrEmpty(response)) {
                    _logger.LogInformation($"ステータスコード:{(int)HttpStatusCode.BadRequest}");
                    _logger.LogInformation($"┗サービス連携終了 ログ情報一覧取得に失敗しました。");
                    return null;
                }

                var resultList = JsonSerializer.Deserialize<List<LogInfo>>(response);
                _logger.LogInformation($"ステータスコード:{(int)HttpStatusCode.OK}");
                _logger.LogInformation($"┗サービス連携終了 正常終了しました。");  
                return resultList;
            }
            catch (Exception)
            {
                _logger.LogInformation($"ステータスコード:{(int)HttpStatusCode.InternalServerError}");
                _logger.LogInformation($"┗サービス連携終了 対象年一覧取得に失敗しました。");
                throw;
            }
        }

    }
}