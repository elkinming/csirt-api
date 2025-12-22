using System.Collections.Generic;
using System.Threading.Tasks;
using AisinIX.CSIRT.CompanyRoleMember.Models;
using Npgsql.Internal.TypeHandlers;

namespace AisinIX.CSIRT.CompanyRoleMember.DBAccessors
{
    public interface IInformationSecurityDBAccessor
    {
        Task<IEnumerable<InformationSecurity>> GetAllInformationSecurityRecords();
        Task<IEnumerable<InformationSecurity>> GetAllInformationSecurityRecordsByKeyword( string searchKeyword );
        Task<IEnumerable<InformationSecurity>> GetInformationSecurityRecordsBySearch( InformationSecuritySearchDto dto);
    }
}