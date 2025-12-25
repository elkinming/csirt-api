using System;

namespace AisinIX.CSIRT.CompanyRoleMember.Models
{
    public class CompanyRoleOps
    {
        public string companyCode1 { get; set; } = "";
        public string companyCode2 { get; set; } = "";
        public int roleCode { get; set; }
        public string opsEmail { get; set; } = "";
        public string opsUrl { get; set; } = "";
        public string opsEmailUrl { get; set; } = "";
        public string opsVulnerability { get; set; } = "";
        public string opsInfo { get; set; } = "";
        public string registUser { get; set; } = "";
        public DateTime registDate { get; set; } = new DateTime();
        public string updateUser { get; set; } = "";
        public DateTime lastUpdate { get; set; } = new DateTime();
    }
}
