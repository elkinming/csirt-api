using System;

namespace AisinIX.CSIRT.CompanyPermission.Models
{
    public class CompanyPermissionModel
    {
        public string ownCompanyCode1 { get; set; }
        public string ownCompanyCode2 { get; set; }
        public string viewCompanyCode1 { get; set; }
        public string viewCompanyCode2 { get; set; }
        public string applicantCompanyCode1 { get; set; }
        public string applicantCompanyCode2 { get; set; }
        public string registUser { get; set; }
        public DateTime registDate { get; set; }
        public string updateUser { get; set; }
        public DateTime lastUpdate { get; set; }
    }
}
