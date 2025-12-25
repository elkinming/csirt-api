using System;
using System.Collections.Generic;
using System.Linq;
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

        /// <summary>
        /// 複数の会社情報を非同期で登録します。
        /// </summary>
        /// <param name="companies">登録する会社情報のコレクション</param>
        /// <returns>非同期操作を表すタスク。タスクの結果には登録の成否が含まれます。</returns>
        public async Task<bool> InsertCompanyArrayAsync(IEnumerable<Company> companies)
        {
            if (companies == null || !companies.Any())
            {
                throw new ArgumentException("会社情報のコレクションがnullまたは空です。", nameof(companies));
            }

            // Validate each company in the list
            foreach (var company in companies)
            {
                if (string.IsNullOrWhiteSpace(company.companyCode1))
                    throw new ArgumentException("会社コード1は必須です。", nameof(Company.companyCode1));
                    
                if (string.IsNullOrWhiteSpace(company.companyCode2))
                    throw new ArgumentException("会社コード2は必須です。", nameof(Company.companyCode2));
                    
                if (string.IsNullOrWhiteSpace(company.companyType))
                    throw new ArgumentException("会社種別は必須です。", nameof(Company.companyType));
                    
                if (string.IsNullOrWhiteSpace(company.companyName))
                    throw new ArgumentException("会社名は必須です。", nameof(Company.companyName));
                    
                if (string.IsNullOrWhiteSpace(company.companyNameEn))
                    throw new ArgumentException("会社名（英語）は必須です。", nameof(Company.companyNameEn));
                    
                if (string.IsNullOrWhiteSpace(company.companyShortName))
                    throw new ArgumentException("会社略称は必須です。", nameof(Company.companyShortName));
                    
                if (string.IsNullOrWhiteSpace(company.groupCode))
                    throw new ArgumentException("グループコードは必須です。", nameof(Company.groupCode));
                    
                if (string.IsNullOrWhiteSpace(company.region))
                    throw new ArgumentException("地域は必須です。", nameof(Company.region));
                    
                if (string.IsNullOrWhiteSpace(company.country))
                    throw new ArgumentException("国は必須です。", nameof(Company.country));
                    
                if (string.IsNullOrWhiteSpace(company.registUser))
                    throw new ArgumentException("登録ユーザーは必須です。", nameof(Company.registUser));
                    
                if (company.registDate == default)
                    company.registDate = DateTime.UtcNow;
                    
                if (string.IsNullOrWhiteSpace(company.updateUser))
                    company.updateUser = company.registUser;
                    
                if (company.lastUpdate == default)
                    company.lastUpdate = DateTime.UtcNow;
            }

            return await _companyDBAccessor.InsertCompanyRecordsArray(companies);
        }
    }
}
