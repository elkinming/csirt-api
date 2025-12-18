using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using AisinIX.Amateras.Common.Utilities;
using AisinIX.CSIRT.CompanyRoleMember.Models;
using Dapper;
using Microsoft.Extensions.Logging;

namespace AisinIX.CSIRT.CompanyRoleMember.DBAccessors
{
    public class CompanyRoleMemberDBAccessor : ICompanyRoleMemberDBAccessor
    {
        private readonly ILogger<CompanyRoleMemberDBAccessor> _logger;
        private readonly bool isOutputSQLLog = false;

        public CompanyRoleMemberDBAccessor(ILogger<CompanyRoleMemberDBAccessor> logger, IConfigUtility configUtility)
        {
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            _logger = logger;
            isOutputSQLLog = configUtility.GetBooleanSetting("IsOutputSQLLog", false);
        }

        /// <summary>
        /// 会社体制情報一覧取得
        /// </summary>
        public async Task<List<CompanyRoleMemberInfo>> QueryCompanyRoleMemberListAsync(
            string companyCode1,
            string companyCode2,
            int? roleCode,
            string? personCode,
            IDbConnection conn,
            CancellationToken ct = default)
        {
            string sql = QueryCompanyRoleMemberListSql(roleCode, personCode);

            if (isOutputSQLLog)
            {
                _logger.LogDebug(sql);
            }

            var param = new
            {
                companyCode1 = companyCode1,
                companyCode2 = companyCode2,
                roleCode = roleCode,
                personCode = personCode
            };

            // cancellation token を確実に効かせるため CommandDefinition を使う
            var cmd = new CommandDefinition(sql, param, cancellationToken: ct);

            var rows = await conn.QueryAsync<CompanyRoleMemberInfo>(cmd);
            return rows.AsList();
        }

        /// <summary>
        /// 会社体制情報一覧取得用SQL
        /// 必須：COMPANY_CODE1, COMPANY_CODE2
        /// 任意：ROLE_CODE, PERSON_CODE
        /// </summary>
        private string QueryCompanyRoleMemberListSql(int? roleCode, string? personCode)
        {
            // Oracle（ODP.NET）想定なのでバインドは :param のまま
            var where = @"
                WHERE
                    COMPANY_CODE1 = :companyCode1
                AND COMPANY_CODE2 = :companyCode2
            ";

            if (roleCode.HasValue)
            {
                where += @"
                AND ROLE_CODE = :roleCode
                ";
            }

            if (!string.IsNullOrWhiteSpace(personCode))
            {
                where += @"
                AND PERSON_CODE = :personCode
                ";
            }

            return $@"
                SELECT
                    COMPANY_CODE1        companyCode1,
                    COMPANY_CODE2        companyCode2,
                    ROLE_CODE            roleCode,
                    DEPT_NAME            deptName,
                    LOCATION             location,
                    POSITION             position,
                    PERSON_NAME          personName,
                    PERSON_CODE          personCode,
                    EMAIL                email,
                    EMERGENCY_CONTACT    emergencyContact,
                    LANGUAGE             language,
                    REGIST_USER          registUser,
                    REGIST_DATE          registDate,
                    UPDATE_USER          updateUser,
                    LAST_UPDATE          lastUpdate
                FROM
                    T_COMPANY_ROLE_MEMBER
                {where}
                ORDER BY
                    COMPANY_CODE1 ASC,
                    COMPANY_CODE2 ASC,
                    ROLE_CODE ASC,
                    PERSON_CODE ASC
            ";
        }
    }
}
