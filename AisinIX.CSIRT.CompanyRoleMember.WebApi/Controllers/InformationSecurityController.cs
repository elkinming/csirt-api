using System.Collections.Generic;
using System.Threading.Tasks;
using AisinIX.CSIRT.CompanyRoleMember.DBAccessors;
using AisinIX.CSIRT.CompanyRoleMember.Models;
using AisinIX.CSIRT.CompanyRoleMember.Services;
using AisinIX.CSIRT.CompanyRoleMember.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

[Route("api/v1")]
[ApiController]
public class InformationSecurityController : ControllerBase
{
    private readonly PostgresConnection _db;
    private readonly IInformationSecurityService informationSecurityService;

    public InformationSecurityController(PostgresConnection db, IInformationSecurityService informationSecurityService)
    {
        _db = db;
        this.informationSecurityService = informationSecurityService;
    }

    /// <summary>
    /// 情報セキュリティ一覧取得
    /// </summary>
    /// <returns></returns>

    [HttpGet]
    [Route("information_security/all")]
    public async Task<IActionResult> GetAllInformationSecurity()
    {
        var recordList = await informationSecurityService.QueryInformationSecurityListAsync();
        ApiResponse<List<InformationSecurityDto>> response = new ApiResponse<List<InformationSecurityDto>>();
        response.statusCode = 200;
        response.message = "Success";
        response.data = recordList;
        return Ok(response);    
    }


    [HttpPost]
    [Route("information_security/search")]
    public async Task<IActionResult> GetInformationSecuritySearch([FromBody] InformationSecuritySearchDto requestDto)
    {
        var recordList = await informationSecurityService.QueryInformationSecuritySearchAsync(requestDto);
        ApiResponse<List<InformationSecurityDto>> response = new ApiResponse<List<InformationSecurityDto>>();
        response.statusCode = 200;
        response.message = "Success";
        response.data = recordList;
        return Ok(response);    
    }
}