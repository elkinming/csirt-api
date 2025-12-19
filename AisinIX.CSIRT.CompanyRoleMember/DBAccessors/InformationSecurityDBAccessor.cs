using System.Collections.Generic;
using System.Threading.Tasks;
using AisinIX.CSIRT.CompanyRoleMember.Models;
using AisinIX.CSIRT.CompanyRoleMember.Db;
using Dapper;
using System;
using System.Text;
namespace AisinIX.CSIRT.CompanyRoleMember.DBAccessors
{
    public class InformationSecurityDBAccesor: IInformationSecurityDBAccessor
    {
        private readonly DapperDbContext dbContext;
        public InformationSecurityDBAccesor(DapperDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>
        /// すべての情報セキュリティのレコードを取得する
        /// </summary>
        public async Task <IEnumerable <InformationSecurity>> GetAllInformationSecurityRecords()
        {
            return await dbContext.DbConnection.QueryAsync<InformationSecurity>(GetAllInformationSecurityRecordsSql());
        }


        /// <summary>
        /// 情報セキュリティ検索
        /// </summary>
        public async Task <IEnumerable <InformationSecurity>> GetInformationSecurityRecordsBySearch( InformationSecuritySearchDto dto)
        {
            var parameters = new Dictionary<string, object>();
            var sql = GetInformationSecuritySearchSql(dto, out parameters);
            return await dbContext.DbConnection.QueryAsync<InformationSecurity>(sql, parameters);
        }

        private static string GetAllInformationSecurityRecordsSql()
        {
            string sql = @"SELECT
            
                company_code1           companyCode1, 
                company_code2           companyCode2, 
                company_type            companyType, 
                company_name            companyName, 
                company_name_en         companyNameEn, 
                company_short_name      companyShortName, 
                group_code              groupCode, 
                region                  region, 
                country                 country, 
                role_code               roleCode,
                ops_email               opsEmail,
                ops_url                 opsUrl,
                ops_email_url           opsEmailUrl,
                ops_vulnerability       opsVulnerability,
                ops_info                opsInfo,
                dept_name               deptName,
                location                location,
                position                position,
                person_name             personName,
                person_code             personCode,
                email                   email,
                emergency_contact       emergencyContact,
                language                language

            FROM public.v_information_security;
            ";

            return sql;
        }

        private static string GetInformationSecuritySearchSql(InformationSecuritySearchDto dto, out Dictionary<string, object> parameters )
        {

            parameters = new Dictionary<string, object>();
            var whereClause = BuildWhereClause(dto, out parameters);

            string sql = @$"SELECT
            
                company_code1           companyCode1, 
                company_code2           companyCode2, 
                company_type            companyType, 
                company_name            companyName, 
                company_name_en         companyNameEn, 
                company_short_name      companyShortName, 
                group_code              groupCode, 
                region                  region, 
                country                 country, 
                role_code               roleCode,
                ops_email               opsEmail,
                ops_url                 opsUrl,
                ops_email_url           opsEmailUrl,
                ops_vulnerability       opsVulnerability,
                ops_info                opsInfo,
                dept_name               deptName,
                location                location,
                position                position,
                person_name             personName,
                person_code             personCode,
                email                   email,
                emergency_contact       emergencyContact,
                language                language

            FROM public.v_information_security
            {whereClause};
            ";

            return sql;
        }

        private static string BuildCondition(
            string column,
            FilterType type,
            string paramName)
        {
            return type switch
            {
                FilterType.Contains     => $"{column} LIKE @{paramName}",
                FilterType.ExactMatch   => $"{column} = @{paramName}",
                FilterType.NotContains  => $"{column} NOT LIKE @{paramName}",
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private static string BuildFilter(
            string column,
            FilterContent f,
            Dictionary<string, object> parameters,
            ref int index)
        {
            var parts = new List<string>();

            if (!string.IsNullOrEmpty(f.filterData1))
            {
                var p = $"p{index++}";
                parts.Add(BuildCondition(column, f.filterType1, p));
                parameters[p] = f.filterType1 == FilterType.ExactMatch
                    ? f.filterData1
                    : $"%{f.filterData1}%";
            }

            if (!string.IsNullOrEmpty(f.filterData2))
            {
                var p = $"p{index++}";
                var logic = f.logicCondition1 == LogicCondition.And ? "AND" : "OR";
                parts.Add($"{logic} {BuildCondition(column, f.filterType2, p)}");
                parameters[p] = f.filterType2 == FilterType.ExactMatch
                    ? f.filterData2
                    : $"%{f.filterData2}%";
            }

            if (!string.IsNullOrEmpty(f.filterData3))
            {
                var p = $"p{index++}";
                var logic = f.logicCondition2 == LogicCondition.And ? "AND" : "OR";
                parts.Add($"{logic} {BuildCondition(column, f.filterType3, p)}");
                parameters[p] = f.filterType3 == FilterType.ExactMatch
                    ? f.filterData3
                    : $"%{f.filterData3}%";
            }

            return parts.Count > 0 ? $"({string.Join(" ", parts)})" : "";
        }


        private static string ToSnakeCase(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            var sb = new StringBuilder();
            for (int i = 0; i < input.Length; i++)
            {
                var c = input[i];

                if (char.IsUpper(c))
                {
                    if (i > 0)
                        sb.Append('_');

                    sb.Append(char.ToLowerInvariant(c));
                }
                else
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        private static string BuildWhereClause(
            InformationSecuritySearchDto dto,
            out Dictionary<string, object> parameters)
        {
            parameters = new();
            var whereParts = new List<string>();
            int index = 0;

            foreach (var prop in dto.GetType().GetProperties())
            {
                if (prop.GetValue(dto) is FilterContent filter)
                {
                    var columnName = ToSnakeCase(prop.Name);
                    var clause = BuildFilter(columnName, filter, parameters, ref index);
                    if (!string.IsNullOrEmpty(clause))
                        whereParts.Add(clause);
                }
            }

            return whereParts.Count > 0
                ? "WHERE " + string.Join(" AND ", whereParts)
                : "";
        }


    }
}