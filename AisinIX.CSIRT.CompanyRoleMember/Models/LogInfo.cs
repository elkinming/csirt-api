using System;

namespace AisinIX.CSIRT.CompanyRoleMember.Models
{
    public class LogInfo
    {
        public string userCode { get; set; } = "";
        public DateTime logDate { get; set; } = new DateTime();
        public string viewName { get; set; } = "";
        public string operation { get; set; } = "";
    }
}
