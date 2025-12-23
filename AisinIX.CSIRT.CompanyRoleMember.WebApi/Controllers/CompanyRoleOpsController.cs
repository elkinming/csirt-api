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
    }
}
