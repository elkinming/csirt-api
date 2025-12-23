using System.Collections.Generic;
using System.Threading.Tasks;
using AisinIX.CSIRT.CompanyRoleMember.Models;

namespace AisinIX.CSIRT.CompanyRoleMember.Services
{
    public interface ICompanyService
    {
        /// <summary>
        /// 全社情報一覧取得
        /// </summary>
        /// <returns>非同期操作を表すタスク。タスクの結果には会社の一覧が含まれます。</returns>
        Task<IEnumerable<Company>> GetAllCompaniesAsync();
    }
}
