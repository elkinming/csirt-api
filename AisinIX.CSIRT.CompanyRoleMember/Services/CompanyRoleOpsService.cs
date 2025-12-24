using System;
using System.Collections.Generic;
using System.Linq;
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

        /// <summary>
        /// 複数の会社ロール運用情報を非同期で登録します。
        /// </summary>
        /// <param name="companyRoleOpsList">登録する会社ロール運用情報のコレクション</param>
        /// <returns>非同期操作を表すタスク。タスクの結果には登録の成否が含まれます。</returns>
        /// <exception cref="ArgumentException">companyRoleOpsListがnullまたは空の場合にスローされます</exception>
        public async Task<bool> InsertCompanyRoleOpsArrayAsync(IEnumerable<CompanyRoleOps> companyRoleOpsList)
        {
            if (companyRoleOpsList == null || !companyRoleOpsList.Any())
            {
                throw new ArgumentException("会社ロール運用情報のコレクションがnullまたは空です。", nameof(companyRoleOpsList));
            }

            return await _companyRoleOpsDBAccessor.InsertCompanyRoleOpsRecordsArray(companyRoleOpsList);
        }
    }
}
