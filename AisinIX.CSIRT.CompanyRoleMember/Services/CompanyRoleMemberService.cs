using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AisinIX.Amateras.Common.Utilities;
using AisinIX.CSIRT.CompanyRoleMember.DBAccessors;
using AisinIX.CSIRT.CompanyRoleMember.Models;
using Microsoft.Extensions.Logging;

namespace AisinIX.CSIRT.CompanyRoleMember.Services
{
    public class CompanyRoleMemberService : ICompanyRoleMemberService
    {
        private readonly IDbConnector _dbConnector;
        private readonly ILogger<CompanyRoleMemberService> _logger;
        private readonly ICompanyRoleMemberDBAccessor _companyRoleMemberDBAccessor;

        public CompanyRoleMemberService(
            IDbConnector dbConnector,
            ILogger<CompanyRoleMemberService> logger,
            ICompanyRoleMemberDBAccessor companyRoleMemberDBAccessor)
        {
            _dbConnector = dbConnector;
            _logger = logger;
            _companyRoleMemberDBAccessor = companyRoleMemberDBAccessor;
        }

        /// <summary>
        /// 会社体制情報一覧取得（Async）
        /// </summary>
        public async Task<List<CompanyRoleMemberInfo>> QueryCompanyRoleMemberListAsync(
            string companyCode1,
            string companyCode2,
            int? roleCode,
            string? personCode,
            CancellationToken ct = default)
        {
            _logger.LogDebug("┏DB接続Open");
            using (var conn = await _dbConnector.ConnectAsync(ct))
            {
                _logger.LogDebug("┗DB接続Open");

                var result = await _companyRoleMemberDBAccessor.QueryCompanyRoleMemberListAsync(
                    companyCode1,
                    companyCode2,
                    roleCode,
                    personCode,
                    conn,
                    ct
                );

                _logger.LogDebug($"会社体制情報一覧取得 {result.Count}件取得");
                return result;
            }
        }

    }
}
