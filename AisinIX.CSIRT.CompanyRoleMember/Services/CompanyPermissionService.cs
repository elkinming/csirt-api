using System.Collections.Generic;
using System.Threading.Tasks;
using AisinIX.CSIRT.CompanyRoleMember.DBAccessors;
using AisinIX.CSIRT.CompanyRoleMember.Models;

namespace AisinIX.CSIRT.CompanyRoleMember.Services
{
    /// <summary>
    /// 会社権限関連のサービス
    /// </summary>
    public class CompanyPermissionService : ICompanyPermissionService
    {
        private readonly ICompanyPermissionDBAccessor _companyPermissionDBAccessor;

        /// <summary>
        /// CompanyPermissionServiceの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="companyPermissionDBAccessor">会社権限データベースアクセサ</param>
        public CompanyPermissionService(ICompanyPermissionDBAccessor companyPermissionDBAccessor)
        {
            _companyPermissionDBAccessor = companyPermissionDBAccessor;
        }

        /// <summary>
        /// 全社権限情報一覧を非同期で取得します。
        /// </summary>
        /// <returns>非同期操作を表すタスク。タスクの結果には会社権限の一覧が含まれます。</returns>
        public async Task<IEnumerable<CompanyPermission>> GetAllCompanyPermissionAsync()
        {
            return await _companyPermissionDBAccessor.GetAllCompanyPermissionRecords();
        }
    }
}