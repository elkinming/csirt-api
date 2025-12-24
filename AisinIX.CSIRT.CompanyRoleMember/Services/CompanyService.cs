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

            return await _companyDBAccessor.InsertCompanyRecordsArray(companies);
        }
    }
}
