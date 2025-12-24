using System;

namespace AisinIX.CSIRT.CompanyRoleMember.Models
{
    public class Company
    {
        public string companyCode1 { get; set; } = "";
        public string companyCode2 { get; set; } = "";
        public string companyType { get; set; } = "";
        public string companyName { get; set; } = "";
        public string companyNameEn { get; set; } = "";
        public string companyShortName { get; set; } = "";
        public string groupCode { get; set; } = "";
        public string region { get; set; } = "";
        public string country { get; set; } = "";
        public string registUser { get; set; } = "";
        public DateTime registDate { get; set; } = new DateTime();
        public string updateUser { get; set; } = "";
        public DateTime lastUpdate { get; set; } = new DateTime();
    }
}
