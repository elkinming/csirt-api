using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;

namespace AisinIX.CSIRT.Common.Db
{
    public class DapperDbContext
    {
        private readonly IDbConnection _dbConnection;

        public DapperDbContext(IConfiguration configuration)
        {
            string? connectionString = configuration.GetConnectionString("Postgres");
            _dbConnection = new NpgsqlConnection(connectionString);
        }

        public IDbConnection DbConnection => _dbConnection;
    }
}