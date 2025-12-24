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

        /// <summary>
        /// 複数の会社ロール運用レコードをトランザクション内で一括登録します。
        /// 1件でも登録に失敗した場合はすべての変更がロールバックされます。
        /// </summary>
        /// <param name="companyRoleOpsList">登録する会社ロール運用レコードのコレクション</param>
        /// <returns>非同期操作を表すタスク。タスクの結果には登録の成否が含まれます。</returns>
        Task<bool> InsertCompanyRoleOpsRecordsArray(IEnumerable<CompanyRoleOps> companyRoleOpsList);
    }
}
