using System.Collections.Generic;
using System.Threading.Tasks;
using AisinIX.CSIRT.Company.Models;

namespace AisinIX.CSIRT.Company.DBAccessors
{
    public interface ICompanyDBAccesor
    {
        Task<IEnumerable<CompanyModel>> GetAllCompanyRecords();
        Task<bool> InsertCompanyRecordsArray(IEnumerable<CompanyModel> companies);
    }
}