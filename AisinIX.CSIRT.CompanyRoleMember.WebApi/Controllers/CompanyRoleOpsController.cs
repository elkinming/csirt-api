using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AisinIX.CSIRT.CompanyRoleMember.Models;
using AisinIX.CSIRT.CompanyRoleMember.Services;
using AisinIX.CSIRT.CompanyRoleMember.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace AisinIX.CSIRT.CompanyRoleMember.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1")]
    public class CompanyRoleOpsController : ControllerBase
    {
        private readonly ICompanyRoleOpsService _companyRoleOpsService;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="CompanyRoleOpsController"/>
        /// </summary>
        /// <param name="companyRoleOpsService">Servicio de operaciones de roles de compañía</param>
        public CompanyRoleOpsController(ICompanyRoleOpsService companyRoleOpsService)
        {
            _companyRoleOpsService = companyRoleOpsService;
        }

        /// <summary>
        /// Obtiene todos los registros de operaciones de roles de compañía
        /// </summary>
        /// <returns>Lista de operaciones de roles de compañía</returns>
        [HttpGet("company-role-ops/all")]
        public async Task<IActionResult> GetAllCompanyRoleOps()
        {
            var roleOpsList = await _companyRoleOpsService.GetAllCompanyRoleOpsAsync();
            
            var response = new ApiResponse<List<CompanyRoleOps>>
            {
                statusCode = 200,
                message = "Success",
                data = roleOpsList.ToList()
            };
            
            return Ok(response);
        }


        /// <summary>
        /// 複数の会社ロール運用情報を登録します。
        /// </summary>
        /// <param name="companyRoleOpsList">登録する会社ロール運用情報のリスト</param>
        /// <returns>登録結果を含むレスポンス</returns>
        [HttpPost]
        [Route("company-role-ops/list")]
        public async Task<IActionResult> InsertCompanyRoleOpsList([FromBody] List<CompanyRoleOps> companyRoleOpsList)
        {
            if (companyRoleOpsList == null || !companyRoleOpsList.Any())
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
                var result = await _companyRoleOpsService.InsertCompanyRoleOpsArrayAsync(companyRoleOpsList);
                return Ok(new ApiResponse<object>
                {
                    statusCode = 200,
                    message = "会社ロール運用情報の登録が完了しました。",
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
                    message = $"会社ロール運用情報の登録中にエラーが発生しました: {ex.Message}",
                    data = null
                });
            }
        }
    }
}
