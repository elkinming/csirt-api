using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AisinIX.CSIRT.LogInfo.Models;
using AisinIX.CSIRT.LogInfo.Services;
using AisinIX.CSIRT.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace AisinIX.CSIRT.WebApi.Controllers
{
    /// <summary>
    /// ログ情報を管理するコントローラー
    /// </summary>
    [ApiController]
    [Route("api/v1")]
    public class LogInfoController : ControllerBase
    {
        private readonly ILogInfoService _logInfoService;

        /// <summary>
        /// LogInfoControllerの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="logInfoService">ログ情報サービス</param>
        public LogInfoController(ILogInfoService logInfoService)
        {
            _logInfoService = logInfoService ?? throw new ArgumentNullException(nameof(logInfoService));
        }

        /// <summary>
        /// 指定された年月のログ情報を取得します。
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <returns>ログ情報の一覧</returns>
        [HttpGet]
        [Route("log-info")]
        public async Task<IActionResult> GetLogs([FromQuery]int year, [FromQuery]int month)
        {
            try
            {
                var logs = await _logInfoService.GetLogDataByDate(year, month);
                
                var response = new ApiResponse<List<LogInfoModel>>
                {
                    statusCode = 200,
                    message = "ログ情報の取得が完了しました。",
                    data = logs.ToList()
                };
                
                return Ok(response);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(new ApiResponse<object>
                {
                    statusCode = 400,
                    message = ex.Message,
                    data = null
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    statusCode = 500,
                    message = $"ログ情報の取得中にエラーが発生しました: {ex.Message}",
                    data = null
                });
            }
        }
    }
}
