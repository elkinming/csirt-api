using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AisinIX.CSIRT.LogInfo.DBAccessors;
using AisinIX.CSIRT.LogInfo.Models;

namespace AisinIX.CSIRT.LogInfo.Services
{
    /// <summary>
    /// ログ情報関連のサービス
    /// </summary>
    public class LogInfoService : ILogInfoService
    {
        private readonly ILogInfoDBAccessor _logInfoDBAccessor;

        /// <summary>
        /// LogInfoServiceの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="logInfoDBAccessor">ログ情報データベースアクセサ</param>
        public LogInfoService(ILogInfoDBAccessor logInfoDBAccessor)
        {
            _logInfoDBAccessor = logInfoDBAccessor ?? 
                throw new ArgumentNullException(nameof(logInfoDBAccessor));
        }

        /// <summary>
        /// 指定された年月のログ情報を非同期で取得します。
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <returns>非同期操作を表すタスク。タスクの結果にはログ情報の一覧が含まれます。</returns>
        public async Task<IEnumerable<LogInfoModel>> GetLogDataByDate(int year, int month)
        {
            // Input validation is handled by the DBAccessor
            return await _logInfoDBAccessor.GetLogDataByMonthAndYear(year, month);
        }
    }
}
