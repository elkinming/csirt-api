using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AisinIX.CSIRT.CompanyRoleMember.Models;

namespace AisinIX.CSIRT.CompanyRoleMember.Services
{
    public interface ICompanyRoleMemberService
    {
        /// <summary>
        /// 会社体制情報一覧取得
        /// </summary>
        Task<List<CompanyRoleMemberInfo>> QueryCompanyRoleMemberListAsync(
            string companyCode1,
            string companyCode2,
            int? roleCode,
            string? personCode,
            CancellationToken ct = default
        );
    }
}
