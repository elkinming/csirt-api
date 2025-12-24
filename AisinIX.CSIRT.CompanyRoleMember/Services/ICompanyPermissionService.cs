using System.Collections.Generic;
using System.Threading.Tasks;
using AisinIX.CSIRT.CompanyRoleMember.Models;

namespace AisinIX.CSIRT.CompanyRoleMember.Services
{
    /// <summary>
    /// 会社権限関連のサービスインターフェース
    /// </summary>
    public interface ICompanyPermissionService
    {
        /// <summary>
        /// 全社権限情報一覧を非同期で取得します。
        /// </summary>
        /// <returns>非同期操作を表すタスク。タスクの結果には会社権限の一覧が含まれます。</returns>
        Task<IEnumerable<CompanyPermission>> GetAllCompanyPermissionAsync();
    }
}