using System.Collections.Generic;
using System.Threading.Tasks;
using AisinIX.CSIRT.CompanyRoleMember.Models;
using AisinIX.CSIRT.CompanyRoleMember.Db;
using Dapper;
namespace AisinIX.CSIRT.CompanyRoleMember.DBAccessors
{
    public class InformationSecurityDBAccesor: IInformationSecurityDBAccessor
    {
        private readonly DapperDbContext dbContext;
        public InformationSecurityDBAccesor(DapperDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task <IEnumerable <InformationSecurity>> GetAllInformationSecurityRecords()
        {
            return await dbContext.DbConnection.QueryAsync<InformationSecurity>(GetAllInformationSecurityRecordsSql());
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
    }
}