using System.Collections.Generic;
using System.Threading.Tasks;
using AisinIX.CSIRT.CompanyRoleOps.Models;

namespace AisinIX.CSIRT.CompanyRoleOps.Services
{
    public interface ICompanyRoleOpsService
    {
        /// <summary>
        /// 全社情報一覧取得
        /// </summary>
        /// <returns>非同期操作を表すタスク。タスクの結果には会社の一覧が含まれます。</returns>
        Task<IEnumerable<CompanyRoleOpsModel>> GetAllCompanyRoleOpsAsync();

        /// <summary>
        /// 複数の会社ロール運用情報を非同期で登録します。
        /// </summary>
        /// <param name="companyRoleOpsList">登録する会社ロール運用情報のコレクション</param>
        /// <returns>非同期操作を表すタスク。タスクの結果には登録の成否が含まれます。</returns>
        /// <exception cref="ArgumentException">companyRoleOpsListがnullまたは空の場合にスローされます</exception>
        Task<bool> InsertCompanyRoleOpsArrayAsync(IEnumerable<CompanyRoleOpsModel> companyRoleOpsList);
    }
}
