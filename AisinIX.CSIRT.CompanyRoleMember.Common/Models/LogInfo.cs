using System;

namespace AisinIX.CSIRT.CompanyRoleMember.Common.Models
{
    public class LogInfo
    {
        /// <summary>ユーザ名</summary>
        public string userName { get; set; } = "";
        /// <summary>法人コード</summary>
        public string compCode  { get; set; } = "";
        /// <summary>日付</summary>
        public DateTime logDate  { get; set; } = new DateTime();
        /// <summary>画面名</summary>
        public string viewName  { get; set; } = "";
        /// <summary>対象テーブル</summary>
        public string tableName  { get; set; } = "";
        /// <summary>管理番号</summary>
        public string no  { get; set; } = "";
        /// <summary>CRUD</summary>
        public string crud  { get; set; } = "";
        public string logYear { get; set; } = "";
    }
}