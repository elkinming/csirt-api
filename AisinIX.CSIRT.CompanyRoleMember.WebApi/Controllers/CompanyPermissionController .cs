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
            var response = new ApiResponse<List<CompanyPermission>>
            {
                statusCode = 200,
                message = "Success",
                data = permissions.ToList()
            };
            return Ok(response);
        }
    }
}