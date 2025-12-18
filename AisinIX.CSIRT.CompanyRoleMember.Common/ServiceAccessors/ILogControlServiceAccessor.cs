using AisinIX.CSIRT.CompanyRoleMember.Common.Models;
using System.Collections.Generic;

namespace AisinIX.CSIRT.CompanyRoleMember.Common.ServiceAccessors
{
    public interface ICompanyRoleMemberServiceAccessor
    {
        /// <summary>
        /// ログ情報一覧取得
        /// </summary>
        List<LogInfo> QuerylogInfo(string logDate, IAsyncProcessHelper helper = null);
        /// <summary>
        /// ログ情報新規登録
        /// </summary>
        void LogInfoInsert(LogInfo logInfo,IAsyncProcessHelper helper = null);
        /// <summary>
        /// 対象年一覧取得
        /// </summary>
        List<LogInfo> QueryYearList(IAsyncProcessHelper helper = null);
    }
}