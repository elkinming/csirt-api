using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AisinIX.CSIRT.CompanyRoleMember.Models;
using AisinIX.CSIRT.CompanyRoleMember.Services;
using AisinIX.CSIRT.CompanyRoleMember.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v1")]
public class CompanyController : ControllerBase
{
    private readonly ICompanyService _companyService;

    /// <summary>
    /// CompanyControllerの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="companyService">会社情報サービス</param>
    public CompanyController(ICompanyService companyService)
    {
        _companyService = companyService;
    }

    [HttpGet]
    /// <summary>
    /// 全社情報一覧を取得します。
    /// </summary>
    /// <returns>会社情報の一覧</returns>
    [Route("company/all")]
    public async Task<IActionResult> GetAllCompanyData()
    {
        var companyList = await _companyService.GetAllCompaniesAsync();
        ApiResponse<List<Company>> response = new ApiResponse<List<Company>>();
        response.statusCode = 200;
        response.message = "Success";
        response.data = companyList.ToList();
        return Ok(response); 
    }
}