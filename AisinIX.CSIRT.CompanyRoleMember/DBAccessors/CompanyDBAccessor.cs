using System.Collections.Generic;
using System.Threading.Tasks;
using AisinIX.CSIRT.CompanyRoleMember.Models;
using AisinIX.CSIRT.CompanyRoleMember.Db;
using Dapper;
namespace AisinIX.CSIRT.CompanyRoleMember.DBAccessors
{
    public class CompanyDBAccesor: ICompanyDBAccesor
    {
        private readonly DapperDbContext dbContext;
        public CompanyDBAccesor(DapperDbContext _dbContext)
        {
            this.dbContext = _dbContext;
        }
        public async Task <IEnumerable <Company>> GetAllCompanyRecords()
        {
            return await dbContext.DbConnection.QueryAsync<Company>(GetAllCompanyRecordsSql());
        }

        private static string GetAllCompanyRecordsSql()
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
                regist_user             registUser, 
                regist_date             registDate, 
                update_user             updateUser, 
                last_update             lastUpdate

            FROM public.m_company;
            ";

            return sql;
        }
    }
}