using System.Collections.Generic;
using System.Threading.Tasks;
using AisinIX.CSIRT.InformationSecurity.Models;
using Npgsql.Internal.TypeHandlers;

namespace AisinIX.CSIRT.InformationSecurity.DBAccessors
{
    public interface IInformationSecurityDBAccessor
    {
        Task<IEnumerable<InformationSecurityModel>> GetAllInformationSecurityRecords();
        Task<IEnumerable<InformationSecurityModel>> GetAllInformationSecurityRecordsByKeyword( string searchKeyword );
        Task<IEnumerable<InformationSecurityModel>> GetInformationSecurityRecordsBySearch( InformationSecuritySearchDto dto);
    }
}