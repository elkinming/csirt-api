using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using AisinIX.CSIRT.CompanyRoleMember.Models;

namespace AisinIX.CSIRT.CompanyRoleMember.DBAccessors
{
    public interface ICompanyRoleOpsDBAccessor
    {
        /// <summary>
        /// データベースから全ての会社ロール運用レコードを取得します
        /// </summary>
        /// <returns>CompanyRoleOpsオブジェクトのコレクション</returns>
        Task<IEnumerable<CompanyRoleOps>> GetAllCompanyRoleOpsRecords();
    }
}
