using System.Collections.Generic;
using System.Threading.Tasks;
using AisinIX.CSIRT.CompanyRoleMember.Models;

namespace AisinIX.CSIRT.CompanyRoleMember.DBAccessors
{
    public interface ICompanyDBAccesor
    {
        Task<IEnumerable<Company>> GetAllCompanyRecords();
    }
}