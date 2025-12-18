using System;

namespace AisinIX.Amateras.Common.Models
{
    public class UserIdentity
    {
        public UserIdentity(string id, string groupCompanyCode)
        {
            Id = id;
            GroupCompanyCode = groupCompanyCode;
        }

        public string Id { get; private set; }
        public string GroupCompanyCode { get; private set; }

        public string ToSimpleValue()
        {
            return Id + "-" + GroupCompanyCode;
        }
    }
}