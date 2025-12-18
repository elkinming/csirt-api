using System.Threading.Tasks;
using AisinIX.CSIRT.CompanyRoleMember.DBAccessors;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

[ApiController]
[Route("api/test")]
public class TestController : ControllerBase
{
    private readonly PostgresConnection _db;
    private readonly ICompanyDBAccesor companyDBAccesor;

    public TestController(PostgresConnection db, ICompanyDBAccesor companyDBAccesor)
    {
        _db = db;
        this.companyDBAccesor = companyDBAccesor;
    }

    [HttpGet]
    public IActionResult Get()
    {
        using var conn = _db.GetConnection();
        conn.Open();

        using var cmd = new NpgsqlCommand("SELECT * FROM m_company", conn);
        var result = cmd.ExecuteReader();

        return Ok(result);
    }

    [HttpGet]
    [Route("v2")]
    public async Task<IActionResult> TestDapper()
    {
        var companyList = await companyDBAccesor.GetAllCompanyRecords();

        return Ok(companyList);
    }
}