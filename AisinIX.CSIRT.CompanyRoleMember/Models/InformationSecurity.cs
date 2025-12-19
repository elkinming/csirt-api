namespace AisinIX.CSIRT.CompanyRoleMember.Models
{
    public class InformationSecurity
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
        public string roleCode { get; set; } = "";
        public string opsEmail { get; set; } = "";
        public string opsUrl { get; set; } = "";
        public string opsEmailUrl { get; set; } = "";
        public string opsVulnerability { get; set; } = "";
        public string opsInfo { get; set; } = "";
        public string deptName { get; set; } = "";
        public string location { get; set; } = "";
        public string position { get; set; } = "";
        public string personName { get; set; } = "";
        public string personCode { get; set; } = "";
        public string email { get; set; } = "";
        public string emergencyContact { get; set; } = "";
        public string language { get; set; } = "";

    }

    public class InformationSecurityDto: InformationSecurity
    {
        public bool isMainRecord { get; set; } = false;
        public int childrenNumber { get; set; } = 0;

        public InformationSecurityDto(InformationSecurity src)
        {
            companyCode1 = src.companyCode1;
            companyCode2 = src.companyCode2;
            companyType = src.companyType;
            companyName = src.companyName;
            companyNameEn = src.companyNameEn;
            companyShortName = src.companyShortName;
            groupCode = src.groupCode;
            region = src.region;
            country = src.country;
            roleCode = src.roleCode;
            opsEmail = src.opsEmail;
            opsUrl = src.opsUrl;
            opsEmailUrl = src.opsEmailUrl;
            opsVulnerability = src.opsVulnerability;
            opsInfo = src.opsInfo;
            deptName = src.deptName;
            location = src.location;
            position = src.position;
            personName = src.personName;
            personCode = src.personCode;
            email = src.email;
            emergencyContact = src.emergencyContact;
            language = src.language;
        }
        
    }

    public enum FilterType
    {
        Contains = 1,
        ExactMatch = 2,
        NotContains = 3
    }

    public enum LogicCondition
    {
        And = 1,
        Or = 2
    }

    public class FilterContent
    {
        public FilterType filterType1 { get; set; } = FilterType.Contains;    
        public FilterType filterType2 { get; set; } = FilterType.Contains;    
        public FilterType filterType3 { get; set; } = FilterType.Contains;    
        public string filterData1 { get; set; } = "";    
        public string filterData2 { get; set; } = "";    
        public string filterData3 { get; set; } = "";    
        public LogicCondition logicCondition1 { get; set; } = LogicCondition.Or;    
        public LogicCondition logicCondition2 { get; set; } = LogicCondition.Or;    
    }

    public class InformationSecuritySearchDto
    {
        public FilterContent companyCode1 { get; set;}
        public FilterContent companyCode2 { get; set;}
        public FilterContent companyType { get; set;}
        public FilterContent companyName { get; set;}
        public FilterContent companyNameEn { get; set;}
        public FilterContent companyShortName { get; set;}
        public FilterContent groupCode { get; set;}
        public FilterContent region { get; set;}
        public FilterContent country { get; set;}
    }

  
}