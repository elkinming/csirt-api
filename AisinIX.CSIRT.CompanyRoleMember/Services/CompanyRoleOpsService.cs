using System.Collections.Generic;
using System.Threading.Tasks;
using AisinIX.CSIRT.CompanyRoleMember.DBAccessors;
using AisinIX.CSIRT.CompanyRoleMember.Models;

namespace AisinIX.CSIRT.CompanyRoleMember.Services
{
    public class CompanyRoleOpsService : ICompanyRoleOpsService
    {
        private readonly ICompanyRoleOpsDBAccessor _companyRoleOpsDBAccessor;

        /// <summary>
        /// CompanyServiceの新しいインスタンスを初期化します。
        /// </summary>
        public CompanyRoleOpsService(ICompanyRoleOpsDBAccessor companyRoleOpsDBAccessor)
        {
            _companyRoleOpsDBAccessor = companyRoleOpsDBAccessor;
        }

        /// <summary>
        /// 全社情報一覧を非同期で取得します。
        /// </summary>
        /// <returns>非同期操作を表すタスク。タスクの結果には会社の一覧が含まれます。</returns>
        public async Task<IEnumerable<CompanyRoleOps>> GetAllCompanyRoleOpsAsync()
        {
            return await _companyRoleOpsDBAccessor.GetAllCompanyRoleOpsRecords();
        }
    }
}
