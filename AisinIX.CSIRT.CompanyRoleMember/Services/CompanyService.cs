using System.Collections.Generic;
using System.Threading.Tasks;
using AisinIX.CSIRT.CompanyRoleMember.DBAccessors;
using AisinIX.CSIRT.CompanyRoleMember.Models;

namespace AisinIX.CSIRT.CompanyRoleMember.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyDBAccesor _companyDBAccessor;

        /// <summary>
        /// CompanyServiceの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="companyDBAccessor">会社情報データベースアクセサ</param>
        public CompanyService(ICompanyDBAccesor companyDBAccessor)
        {
            _companyDBAccessor = companyDBAccessor;
        }

        /// <summary>
        /// 全社情報一覧を非同期で取得します。
        /// </summary>
        /// <returns>非同期操作を表すタスク。タスクの結果には会社の一覧が含まれます。</returns>
        public async Task<IEnumerable<Company>> GetAllCompaniesAsync()
        {
            return await _companyDBAccessor.GetAllCompanyRecords();
        }
    }
}
