using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using AisinIX.CSIRT.CompanyRoleMember.Models;

namespace AisinIX.CSIRT.CompanyRoleMember.DBAccessors
{
    public interface ICompanyRoleMemberDBAccessor
    {
        /// <summary>
        /// 会社体制情報一覧取得
        /// </summary>
        /// <param name="companyCode1">会社コード1（必須）</param>
        /// <param name="companyCode2">会社コード2（必須）</param>
        /// <param name="roleCode">役割コード（任意）</param>
        /// <param name="personCode">氏名コード（任意）</param>
        /// <param name="conn">DB Connection</param>
        Task<List<CompanyRoleMemberInfo>> QueryCompanyRoleMemberListAsync(
            string companyCode1,
            string companyCode2,
            int? roleCode,
            string? personCode,
            IDbConnection conn,
            CancellationToken ct = default
        );
    }
}
