using AisinIX.CSIRT.CompanyRoleMember.Db;
using AisinIX.CSIRT.CompanyRoleMember.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AisinIX.CSIRT.CompanyRoleMember.DBAccessors
{
    public class CompanyPermissionDBAccessor : ICompanyPermissionDBAccessor
    {
        private readonly DapperDbContext dbContext;

        public CompanyPermissionDBAccessor(DapperDbContext _dbContext)
        {
            this.dbContext = _dbContext;
        }

        public async Task<IEnumerable<CompanyPermission>> GetAllCompanyPermissionRecords()
        {
            return await dbContext.DbConnection.QueryAsync<CompanyPermission>(GetAllCompanyPermissionRecordsSql());
        }

        public async Task<bool> InsertCompanyPermissionRecordsArray(IEnumerable<CompanyPermission> permissions)
        {
            dbContext.DbConnection.Open();
            using var transaction = dbContext.DbConnection.BeginTransaction();
            try
            {
                const string sql = @"
                    INSERT INTO t_company_permission (
                        own_company_code1,
                        own_company_code2,
                        view_company_code1,
                        view_company_code2,
                        applicant_company_code1,
                        applicant_company_code2,
                        regist_user,
                        regist_date,
                        update_user,
                        last_update
                    ) VALUES (
                        @ownCompanyCode1,
                        @ownCompanyCode2,
                        @viewCompanyCode1,
                        @viewCompanyCode2,
                        @applicantCompanyCode1,
                        @applicantCompanyCode2,
                        @registUser,
                        @registDate,
                        @updateUser,
                        @lastUpdate
                    )";
                foreach (var permission in permissions)
                {
                    await dbContext.DbConnection.ExecuteAsync(sql, permission, transaction);
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
        private static string GetAllCompanyPermissionRecordsSql()
        {
            string sql = @"SELECT
                own_company_code1 as ownCompanyCode1,
                own_company_code2 as ownCompanyCode2,
                view_company_code1 as viewCompanyCode1,
                view_company_code2 as viewCompanyCode2,
                applicant_company_code1 as applicantCompanyCode1,
                applicant_company_code2 as applicantCompanyCode2,
                regist_user as registUser,
                regist_date as registDate,
                update_user as updateUser,
                last_update as lastUpdate
            FROM t_company_permission";

            return sql;
        }
    }
}
