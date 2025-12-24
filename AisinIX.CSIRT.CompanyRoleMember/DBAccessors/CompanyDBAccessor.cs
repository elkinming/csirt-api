using System.Collections.Generic;
using System.Threading.Tasks;
using AisinIX.CSIRT.CompanyRoleMember.Models;
using AisinIX.CSIRT.CompanyRoleMember.Db;
using Dapper;
using System;
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

        public async Task<bool> InsertCompanyRecordsArray(IEnumerable<Company> companies)
        {
            dbContext.DbConnection.Open();
            using var transaction = dbContext.DbConnection.BeginTransaction();
            try
            {
                const string sql = @"
                    INSERT INTO public.m_company (
                        company_code1,
                        company_code2,
                        company_type,
                        company_name,
                        company_name_en,
                        company_short_name,
                        group_code,
                        region,
                        country,
                        regist_user,
                        regist_date,
                        update_user,
                        last_update
                    ) VALUES (
                        @companyCode1,
                        @companyCode2,
                        @companyType,
                        @companyName,
                        @companyNameEn,
                        @companyShortName,
                        @groupCode,
                        @region,
                        @country,
                        @registUser,
                        @registDate,
                        @updateUser,
                        @lastUpdate
                    )";
                foreach (var company in companies)
                {
                    await dbContext.DbConnection.ExecuteAsync(sql, company, transaction);
                }
                transaction.Commit();
                dbContext.DbConnection.Close();
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                dbContext.DbConnection.Close();
                throw; // Re-throw the exception to be handled by the caller
            }
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