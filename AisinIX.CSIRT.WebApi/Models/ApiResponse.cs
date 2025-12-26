using System.Collections.Generic;
using AisinIX.CSIRT.CompanyRoleMember.Models;
using NPOI.SS.Formula.Functions;

namespace AisinIX.CSIRT.WebApi.Models
{
    public class ApiResponseGeneric
    {
        public int statusCode { get; set; }
        public string message { get; set; }
    }
    public class ApiResponse<T> : ApiResponseGeneric
    {
        public T data { get; set; }
    }

}