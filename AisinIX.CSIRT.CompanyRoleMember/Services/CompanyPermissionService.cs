using System;
using System.Collections.Generic;
using System.Linq;
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


        /// <summary>
        /// 会社権限情報の配列を非同期で登録します。
        /// </summary>
        /// <param name="permissions">登録する会社権限情報の配列</param>
        /// <returns>非同期操作を表すタスク。タスクの結果には登録の成否が含まれます。</returns>
        public async Task<bool> InsertCompanyPermissionArrayAsync(IEnumerable<CompanyPermission> permissions)
        {
            if (permissions == null || !permissions.Any())
            {
                throw new ArgumentException("登録する権限情報を指定してください。", nameof(permissions));
            }

            // Validate each permission in the list
            foreach (var permission in permissions)
            {
                if (string.IsNullOrWhiteSpace(permission.ownCompanyCode1))
                    throw new ArgumentException("自社コード1は必須です。", nameof(CompanyPermission.ownCompanyCode1));
                    
                if (string.IsNullOrWhiteSpace(permission.ownCompanyCode2))
                    throw new ArgumentException("自社コード2は必須です。", nameof(CompanyPermission.ownCompanyCode2));
                    
                if (string.IsNullOrWhiteSpace(permission.viewCompanyCode1))
                    throw new ArgumentException("閲覧先会社コード1は必須です。", nameof(CompanyPermission.viewCompanyCode1));
                    
                if (string.IsNullOrWhiteSpace(permission.viewCompanyCode2))
                    throw new ArgumentException("閲覧先会社コード2は必須です。", nameof(CompanyPermission.viewCompanyCode2));
                    
                if (string.IsNullOrWhiteSpace(permission.applicantCompanyCode1))
                    throw new ArgumentException("申請元会社コード1は必須です。", nameof(CompanyPermission.applicantCompanyCode1));
                    
                if (string.IsNullOrWhiteSpace(permission.applicantCompanyCode2))
                    throw new ArgumentException("申請元会社コード2は必須です。", nameof(CompanyPermission.applicantCompanyCode2));
                    
                if (string.IsNullOrWhiteSpace(permission.registUser))
                    throw new ArgumentException("登録ユーザーは必須です。", nameof(CompanyPermission.registUser));
                    
                if (permission.registDate == default)
                    permission.registDate = DateTime.UtcNow;
                    
                if (string.IsNullOrWhiteSpace(permission.updateUser))
                    permission.updateUser = permission.registUser;
                    
                if (permission.lastUpdate == default)
                    permission.lastUpdate = DateTime.UtcNow;
            }

            return await _companyPermissionDBAccessor.InsertCompanyPermissionRecordsArray(permissions);
        }
    }
}