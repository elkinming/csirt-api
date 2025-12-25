using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AisinIX.CSIRT.CompanyRoleMember.Db;
using AisinIX.CSIRT.CompanyRoleMember.Models;
using Dapper;

namespace AisinIX.CSIRT.CompanyRoleMember.DBAccessors
{
    /// <summary>
    /// ログ情報データベースアクセサ
    /// </summary>
    public class LogInfoDBAccessor : ILogInfoDBAccessor
    {
        private readonly DapperDbContext _dbContext;

        /// <summary>
        /// LogInfoDBAccessorの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="dbContext">データベースコンテキスト</param>
        public LogInfoDBAccessor(DapperDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        /// <summary>
        /// 指定された年月のログ情報を非同期で取得します。
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <returns>非同期操作を表すタスク。タスクの結果にはログ情報の一覧が含まれます。</returns>
        public async Task<IEnumerable<LogInfo>> GetLogDataByMonthAndYear(int year, int month)
        {
            // Validate input parameters
            if (year <= 0 || year > 9999)
                throw new ArgumentOutOfRangeException(nameof(year), "年は0000から9999の間で指定してください。");
                
            if (month < 1 || month > 12)
                throw new ArgumentOutOfRangeException(nameof(month), "月は1から12の間で指定してください。");

            // Calculate date range
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1);

            var sql = @"
                SELECT 
                    user_code AS userCode,
                    log_date AS logDate,
                    view_name AS viewName,
                    operation
                FROM t_log_info
                WHERE log_date >= :startDate 
                AND log_date < :endDate
                ORDER BY log_date DESC";

            var parameters = new { startDate, endDate };

            return await _dbContext.DbConnection.QueryAsync<LogInfo>(sql, parameters);
        }
    }
}
