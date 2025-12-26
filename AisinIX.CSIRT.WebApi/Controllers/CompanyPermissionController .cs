using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AisinIX.CSIRT.CompanyPermission.Models;
using AisinIX.CSIRT.CompanyPermission.Services;
using AisinIX.CSIRT.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace AisinIX.CSIRT.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1")]
    public class CompanyPermissionController : ControllerBase
    {
        private readonly ICompanyPermissionService _companyPermissionService;

        /// <summary>
        /// CompanyPermissionControllerの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="companyPermissionService">会社権限サービス</param>
        public CompanyPermissionController(ICompanyPermissionService companyPermissionService)
        {
            _companyPermissionService = companyPermissionService;
        }

        /// <summary>
        /// 全社権限情報一覧を取得します。
        /// </summary>
        /// <returns>会社権限情報の一覧</returns>
        [HttpGet]
        [Route("company-permissions/all")]
        public async Task<IActionResult> GetAllCompanyPermissions()
        {
            var permissions = await _companyPermissionService.GetAllCompanyPermissionAsync();
            var response = new ApiResponse<List<CompanyPermissionModel>>
            {
                statusCode = 200,
                message = "Success",
                data = permissions.ToList()
            };
            return Ok(response);
        }

        /// <summary>
        /// 会社権限情報のリストを登録します。
        /// </summary>
        /// <param name="permissions">登録する会社権限情報のリスト</param>
        /// <returns>登録結果を含むレスポンス</returns>
        [HttpPost("company-permissions/list")]
        public async Task<IActionResult> InsertCompanyPermissionList([FromBody] List<CompanyPermissionModel> permissions)
        {
            if (permissions == null || !permissions.Any())
            {
                return BadRequest(new ApiResponse<object>
                {
                    statusCode = 400,
                    message = "リクエストボディが無効です。",
                    data = null
                });
            }

            try
            {
                var result = await _companyPermissionService.InsertCompanyPermissionArrayAsync(permissions);
                return Ok(new ApiResponse<object>
                {
                    statusCode = 200,
                    message = "会社権限情報の登録が完了しました。",
                    data = new { success = result }
                });
            }
            catch (ArgumentException ex)
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
                    message = $"会社権限情報の登録中にエラーが発生しました: {ex.Message}",
                    data = null
                });
            }
        }
    }
}