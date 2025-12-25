using System.Collections.Generic;
using System.Threading.Tasks;
using AisinIX.CSIRT.CompanyRoleMember.Models;

namespace AisinIX.CSIRT.CompanyRoleMember.DBAccessors
{
    /// <summary>
    /// ログ情報データベースアクセサインターフェース
    /// </summary>
    public interface ILogInfoDBAccessor
    {
        /// <summary>
        /// 指定された年月のログ情報を非同期で取得します。
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <returns>非同期操作を表すタスク。タスクの結果にはログ情報の一覧が含まれます。</returns>
        Task<IEnumerable<LogInfo>> GetLogDataByMonthAndYear(int year, int month);
    }
}
