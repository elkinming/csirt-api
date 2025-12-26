using System.Collections.Generic;
using System.Threading.Tasks;
using AisinIX.CSIRT.CompanyPermission.Models;

namespace AisinIX.CSIRT.CompanyPermission.DBAccessors
{
    public interface ICompanyPermissionDBAccessor
    {
        Task<IEnumerable<CompanyPermissionModel>> GetAllCompanyPermissionRecords();

        Task<bool> InsertCompanyPermissionRecordsArray(IEnumerable<CompanyPermissionModel> permissions);
    }
}
