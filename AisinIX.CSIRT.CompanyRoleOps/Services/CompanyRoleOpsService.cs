using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AisinIX.CSIRT.CompanyRoleOps.DBAccessors;
using AisinIX.CSIRT.CompanyRoleOps.Models;

namespace AisinIX.CSIRT.CompanyRoleOps.Services
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
        public async Task<IEnumerable<CompanyRoleOpsModel>> GetAllCompanyRoleOpsAsync()
        {
            return await _companyRoleOpsDBAccessor.GetAllCompanyRoleOpsRecords();
        }

        /// <summary>
        /// 複数の会社ロール運用情報を非同期で登録します。
        /// </summary>
        /// <param name="companyRoleOpsList">登録する会社ロール運用情報のコレクション</param>
        /// <returns>非同期操作を表すタスク。タスクの結果には登録の成否が含まれます。</returns>
        /// <exception cref="ArgumentException">companyRoleOpsListがnullまたは空の場合にスローされます</exception>
        public async Task<bool> InsertCompanyRoleOpsArrayAsync(IEnumerable<CompanyRoleOpsModel> companyRoleOpsList)
        {
            if (companyRoleOpsList == null || !companyRoleOpsList.Any())
            {
                throw new ArgumentException("会社ロール運用情報のコレクションがnullまたは空です。", nameof(companyRoleOpsList));
            }

            // Validate each company role ops in the list
            foreach (var ops in companyRoleOpsList)
            {
                if (string.IsNullOrWhiteSpace(ops.companyCode1))
                    throw new ArgumentException("会社コード1は必須です。", nameof(CompanyRoleOpsModel.companyCode1));
                    
                if (string.IsNullOrWhiteSpace(ops.companyCode2))
                    throw new ArgumentException("会社コード2は必須です。", nameof(CompanyRoleOpsModel.companyCode2));
                    
                if (ops.roleCode <= 0)
                    throw new ArgumentException("有効なロールコードを指定してください。", nameof(CompanyRoleOpsModel.roleCode));
                    
                if (string.IsNullOrWhiteSpace(ops.opsEmail))
                    throw new ArgumentException("運用メールは必須です。", nameof(CompanyRoleOpsModel.opsEmail));
                    
                if (string.IsNullOrWhiteSpace(ops.opsUrl))
                    throw new ArgumentException("運用URLは必須です。", nameof(CompanyRoleOpsModel.opsUrl));
                    
                if (string.IsNullOrWhiteSpace(ops.opsEmailUrl))
                    throw new ArgumentException("運用メールURLは必須です。", nameof(CompanyRoleOpsModel.opsEmailUrl));
                    
                if (string.IsNullOrWhiteSpace(ops.opsVulnerability))
                    throw new ArgumentException("脆弱性は必須です。", nameof(CompanyRoleOpsModel.opsVulnerability));

                if (string.IsNullOrWhiteSpace(ops.opsInfo))
                    throw new ArgumentException("情報は必須です。", nameof(CompanyRoleOpsModel.opsInfo));
                
                if (string.IsNullOrWhiteSpace(ops.registUser))
                    throw new ArgumentException("登録ユーザーは必須です。", nameof(CompanyRoleOpsModel.registUser));
                    
                if (ops.registDate == default)
                    ops.registDate = DateTime.UtcNow;
                    
                if (string.IsNullOrWhiteSpace(ops.updateUser))
                    ops.updateUser = ops.registUser;
                    
                if (ops.lastUpdate == default)
                    ops.lastUpdate = DateTime.UtcNow;
            }

            return await _companyRoleOpsDBAccessor.InsertCompanyRoleOpsRecordsArray(companyRoleOpsList);
        }
    }
}
