using System.Collections.Generic;
using System.Threading.Tasks;
using AisinIX.CSIRT.CompanyRoleMember.Models;
using AisinIX.CSIRT.CompanyRoleMember.Db;
using Dapper;

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
