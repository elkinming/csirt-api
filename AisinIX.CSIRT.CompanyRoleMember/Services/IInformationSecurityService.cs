using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AisinIX.CSIRT.CompanyRoleMember.Models;

namespace AisinIX.CSIRT.CompanyRoleMember.Services
{
    public interface IInformationSecurityService
    {
        /// <summary>
        /// 情報セキュリティ一覧取得
        /// </summary>
        Task<List<InformationSecurityDto>> QueryInformationSecurityListAsync(
            CancellationToken ct = default
        );
        Task<List<InformationSecurityDto>> QueryInformationSecuritySearchAsync(
            InformationSecuritySearchDto dto,
            CancellationToken ct = default
        );
    }
}
