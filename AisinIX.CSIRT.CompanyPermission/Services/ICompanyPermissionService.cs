using System.Collections.Generic;
using System.Threading.Tasks;
using AisinIX.CSIRT.CompanyPermission.Models;

namespace AisinIX.CSIRT.CompanyPermission.Services
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
        Task<IEnumerable<CompanyPermissionModel>> GetAllCompanyPermissionAsync();


        /// <summary>
        /// 会社権限情報の配列を非同期で登録します。
        /// </summary>
        /// <param name="permissions">登録する会社権限情報の配列</param>
        /// <returns>非同期操作を表すタスク。タスクの結果には登録の成否が含まれます。</returns>
        Task<bool> InsertCompanyPermissionArrayAsync(IEnumerable<CompanyPermissionModel> permissions);
    }
}