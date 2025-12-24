using System;

namespace AisinIX.CSIRT.CompanyRoleMember.Models
{
    public class CompanyPermission
    {
        public string ownCompanyCode1 { get; set; }
        public string ownCompanyCode2 { get; set; }
        public string viewCompanyCode1 { get; set; }
        public string viewCompanyCode2 { get; set; }
        public string applicantCompanyCode1 { get; set; }
        public string applicantCompanyCode2 { get; set; }
        public string registUser { get; set; }
        public string registDate { get; set; }
        public string updateUser { get; set; }
        public string lastUpdate { get; set; }
    }
}
