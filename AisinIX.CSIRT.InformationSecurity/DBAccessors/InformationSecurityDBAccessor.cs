using System.Collections.Generic;
using System.Threading.Tasks;
using AisinIX.CSIRT.InformationSecurity.Models;
using AisinIX.CSIRT.Common.Db;
using Dapper;
using System;
using System.Text;
using Npgsql.Replication;
namespace AisinIX.CSIRT.InformationSecurity.DBAccessors
{
    public class InformationSecurityDBAccesor: IInformationSecurityDBAccessor
    {
        private readonly DapperDbContext dbContext;

        private const string baseSql = @"SELECT
            
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

            FROM public.v_information_security ";

        private const string orderBySql = @"ORDER BY company_code1, company_code2, role_code";
        public InformationSecurityDBAccesor(DapperDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>
        /// すべての情報セキュリティのレコードを取得する
        /// </summary>
        public async Task <IEnumerable <InformationSecurityModel>> GetAllInformationSecurityRecords()
        {
            return await dbContext.DbConnection.QueryAsync<InformationSecurityModel>(baseSql);
        }

        public async Task <IEnumerable <InformationSecurityModel>> GetAllInformationSecurityRecordsByKeyword( string searchKeyword)
        {
            var parameters = new Dictionary<string, object>();
            var sql = GetInformationSecuritySqlByKeyword(searchKeyword, out parameters);
            return await dbContext.DbConnection.QueryAsync<InformationSecurityModel>(sql, parameters);
        }

        private static string GetInformationSecuritySqlByKeyword(string searchKeyword, out Dictionary<string, object> parameters )
        {

            parameters = new Dictionary<string, object>();
            // parameters["p0"] = searchKeyword;
            parameters["p0"] = $"%{searchKeyword}%";
            parameters["p1"] = -1;

            if (int.TryParse(searchKeyword, out var roleCode))
            {
                parameters["p1"] = roleCode;
            }

            string sql = @$"
            {baseSql}
            WHERE
                (company_code1 ILIKE @p0) OR
                (company_code2 ILIKE @p0) OR
                (company_type ILIKE @p0) OR
                (company_name ILIKE @p0) OR
                (company_name_en ILIKE @p0) OR
                (company_short_name ILIKE @p0) OR
                (group_code ILIKE @p0) OR
                (region ILIKE @p0) OR
                (country ILIKE @p0) OR
                (role_code = @p1 ) OR
                (ops_email ILIKE @p0) OR
                (ops_url ILIKE @p0) OR
                (ops_email_url ILIKE @p0) OR
                (ops_vulnerability ILIKE @p0) OR
                (ops_info ILIKE @p0) OR
                (dept_name ILIKE @p0) OR
                (location ILIKE @p0) OR
                (position ILIKE @p0) OR
                (person_name ILIKE @p0) OR
                (person_code ILIKE @p0) OR
                (email ILIKE @p0) OR
                (emergency_contact ILIKE @p0) OR
                (language ILIKE @p0)
            {orderBySql}
            ;
            ";

            return sql;
        }


        /// <summary>
        /// 情報セキュリティ検索
        /// </summary>
        public async Task <IEnumerable <InformationSecurityModel>> GetInformationSecurityRecordsBySearch( InformationSecuritySearchDto dto)
        {
            var parameters = new Dictionary<string, object>();
            var sql = GetInformationSecuritySearchSql(dto, out parameters);
            return await dbContext.DbConnection.QueryAsync<InformationSecurityModel>(sql, parameters);
        }


        private static string GetInformationSecuritySearchSql(InformationSecuritySearchDto dto, out Dictionary<string, object> parameters )
        {

            parameters = new Dictionary<string, object>();
            var whereClause = BuildWhereClause(dto, out parameters);

            string sql = @$"
            {baseSql}
            {whereClause}
            {orderBySql}
            ;
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