using System;
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


    /// <summary>
    /// 複数の会社情報を登録します。
    /// </summary>
    /// <param name="companies">登録する会社情報のリスト</param>
    /// <returns>登録結果を含むレスポンス</returns>
    [HttpPost]
    [Route("company/list")]
    public async Task<IActionResult> InsertCompanyList([FromBody] List<Company> companies)
    {
        if (companies == null || !companies.Any())
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
            var result = await _companyService.InsertCompanyArrayAsync(companies);
            return Ok(new ApiResponse<object>
            {
                statusCode = 200,
                message = "会社情報の登録が完了しました。",
                data = new { success = result }
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<object>
            {
                statusCode = 500,
                message = $"会社情報の登録中にエラーが発生しました: {ex.Message}",
                data = null
            });
        }
    }
}