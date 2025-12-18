using System;

namespace AisinIX.CSIRT.CompanyRoleMember.Models
{
    public class CompanyRoleMemberInfo
    {
        /// <summary>会社コード1</summary>
        public string companyCode1 { get; set; } = "";

        /// <summary>会社コード2</summary>
        public string companyCode2 { get; set; } = "";

        /// <summary>役割コード</summary>
        public int? roleCode { get; set; } = null;

        /// <summary>部署名</summary>
        public string deptName { get; set; } = "";

        /// <summary>勤務地</summary>
        public string location { get; set; } = "";

        /// <summary>役職</summary>
        public string position { get; set; } = "";

        /// <summary>氏名</summary>
        public string personName { get; set; } = "";

        /// <summary>氏名コード</summary>
        public string personCode { get; set; } = "";

        /// <summary>メールアドレス</summary>
        public string email { get; set; } = "";

        /// <summary>緊急連絡先</summary>
        public string emergencyContact { get; set; } = "";

        /// <summary>言語</summary>
        public string language { get; set; } = "";

        /// <summary>登録者</summary>
        public string registUser { get; set; } = "";

        /// <summary>登録日時</summary>
        public DateTime? registDate { get; set; } = null;

        /// <summary>更新者</summary>
        public string updateUser { get; set; } = "";

        /// <summary>最終更新日時</summary>
        public DateTime? lastUpdate { get; set; } = null;
    }
}
