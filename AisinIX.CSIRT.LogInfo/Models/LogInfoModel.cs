using System;

namespace AisinIX.CSIRT.LogInfo.Models
{
    public class LogInfoModel
    {
        public string userCode { get; set; } = "";
        public DateTime logDate { get; set; } = new DateTime();
        public string viewName { get; set; } = "";
        public string operation { get; set; } = "";
    }
}
