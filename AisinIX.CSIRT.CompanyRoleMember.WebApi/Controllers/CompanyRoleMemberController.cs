using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AisinIX.Amateras.Common.Utilities;
using AisinIX.CSIRT.CompanyRoleMember.Models;
using AisinIX.CSIRT.CompanyRoleMember.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AisinIX.CSIRT.CompanyRoleMember.WebApi.Controllers
{
    [Route("api/CompanyRoleMember")]
    [ApiController]
    public class CompanyRoleMemberController : ControllerBase
    {
        private readonly ILogger<CompanyRoleMemberController> _logger;
        private readonly IConfigUtility _configUtility;
        private readonly ICompanyRoleMemberService _companyRoleMemberService;

        public CompanyRoleMemberController(
            ILogger<CompanyRoleMemberController> logger,
            ICompanyRoleMemberService companyRoleMemberService,
            IConfigUtility configUtility)
        {
            _logger = logger;
            _configUtility = configUtility;
            _companyRoleMemberService = companyRoleMemberService;
        }

        /// <summary>
        /// 会社体制情報 一覧取得
        /// </summary>
        /// GET api/CompanyRoleMember/list?companyCode1=C00001&companyCode2=C00001&roleCode=3&personCode=P000003
        [HttpGet]
        [Route("list")]
        public async Task<ActionResult<List<CompanyRoleMemberInfo>>> QueryCompanyRoleMemberList(CancellationToken ct)
        {
            try
            {
                string url = HttpContext.Request.GetDisplayUrl();
                _logger.LogInformation("┏■ QueryCompanyRoleMemberList 開始 ■");
                _logger.LogInformation($"URL:{url}");

                IQueryCollection query = HttpContext.Request.Query;

                string companyCode1 = query["companyCode1"];
                string companyCode2 = query["companyCode2"];
                string roleCodeStr = query["roleCode"];
                string personCode = query["personCode"];

                _logger.LogInformation($"会社コード1(companyCode1=[{companyCode1}])");
                _logger.LogInformation($"会社コード2(companyCode2=[{companyCode2}])");
                _logger.LogInformation($"役割コード(roleCode=[{roleCodeStr}])");
                _logger.LogInformation($"氏名コード(personCode=[{personCode}])");

                // 必須チェック
                if (string.IsNullOrWhiteSpace(companyCode1) || string.IsNullOrWhiteSpace(companyCode2))
                {
                    _logger.LogInformation($"ステータスコード:{(int)HttpStatusCode.BadRequest}");
                    return StatusCode(400);
                }

                // ROLE_CODE 任意（指定あれば数値チェック）
                int? roleCode = null;
                if (!string.IsNullOrWhiteSpace(roleCodeStr))
                {
                    if (int.TryParse(roleCodeStr, out var rc))
                    {
                        roleCode = rc;
                    }
                    else
                    {
                        _logger.LogInformation($"ステータスコード:{(int)HttpStatusCode.BadRequest}");
                        return StatusCode(400);
                    }
                }

                var result = await _companyRoleMemberService.QueryCompanyRoleMemberListAsync(
                    companyCode1,
                    companyCode2,
                    roleCode,
                    personCode,
                    ct
                );

                _logger.LogInformation("正常終了");
                _logger.LogInformation($"取得件数:{result.Count}件");
                _logger.LogInformation($"ステータスコード:{(int)HttpStatusCode.OK}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                int eventlogID = int.Parse(_configUtility.GetStringSetting("EventLogID"));
                _logger.LogError(eventlogID, $"エラーが発生({ex.Message})");
                _logger.LogInformation($"ステータスコード:{(int)HttpStatusCode.InternalServerError}");
                return StatusCode(500);
            }
            finally
            {
                _logger.LogInformation("┗■ QueryCompanyRoleMemberList 終了 ■");
            }
        }
    }
}
