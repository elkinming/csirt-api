using System.Collections.Generic;
using System.Threading.Tasks;
using AisinIX.CSIRT.CompanyRoleMember.Models;
using AisinIX.CSIRT.CompanyRoleMember.Db;
using Dapper;
using System;

namespace AisinIX.CSIRT.CompanyRoleMember.DBAccessors
{
    public class CompanyRoleOpsDBAccessor: ICompanyRoleOpsDBAccessor
    {
        private readonly DapperDbContext dbContext;

        public CompanyRoleOpsDBAccessor(DapperDbContext _dbContext)
        {
            this.dbContext = _dbContext;
        }

        public async Task<IEnumerable<CompanyRoleOps>> GetAllCompanyRoleOpsRecords()
        {
            return await dbContext.DbConnection.QueryAsync<CompanyRoleOps>(GetAllCompanyRoleOpsRecordsSql());
        }

        public async Task<bool> InsertCompanyRoleOpsRecordsArray(IEnumerable<CompanyRoleOps> companyRoleOpsList)
        {
            dbContext.DbConnection.Open();
            using var transaction = dbContext.DbConnection.BeginTransaction();
            try
            {
                const string sql = @"
                    INSERT INTO t_company_role_ops (
                        company_code1,
                        company_code2,
                        role_code,
                        ops_email,
                        ops_url,
                        ops_email_url,
                        ops_vulnerability,
                        ops_info,
                        regist_user,
                        regist_date,
                        update_user,
                        last_update
                    ) VALUES (
                        @companyCode1,
                        @companyCode2,
                        @roleCode,
                        @opsEmail,
                        @opsUrl,
                        @opsEmailUrl,
                        @opsVulnerability,
                        @opsInfo,
                        @registUser,
                        @registDate,
                        @updateUser,
                        @lastUpdate
                    )";

                foreach (var companyRoleOps in companyRoleOpsList)
                {
                    await dbContext.DbConnection.ExecuteAsync(sql, companyRoleOps, transaction);
                }
                
                transaction.Commit();
                dbContext.DbConnection.Close();
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                dbContext.DbConnection.Close();
                throw;
            }
        }

        private static string GetAllCompanyRoleOpsRecordsSql()
        {
            string sql = @"
                SELECT 
                    company_code1 as companyCode1,
                    company_code2 as companyCode2,
                    role_code as roleCode,
                    ops_email as opsEmail,
                    ops_url as opsUrl,
                    ops_email_url as opsEmailUrl,
                    ops_vulnerability as opsVulnerability,
                    ops_info as opsInfo,
                    regist_user as registUser,
                    regist_date as registDate,
                    update_user as updateUser,
                    last_update as lastUpdate
                FROM 
                    t_company_role_ops";

            return sql;
        }
    }
}
