using System.Collections.Generic;
using System.Threading.Tasks;
using AisinIX.CSIRT.CompanyRoleMember.Models;

namespace AisinIX.CSIRT.CompanyRoleMember.DBAccessors
{
    public interface ICompanyPermissionDBAccessor
    {
        Task<IEnumerable<CompanyPermission>> GetAllCompanyPermissionRecords();
    }
}
