using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Oracle.ManagedDataAccess.Client;
using AisinIX.Amateras.Common.Models;

namespace AisinIX.Amateras.Common.Utilities
{
    public class OracleDbConnector : IDbConnector
    {
        /// <summary>
        /// 既定設定名
        /// </summary>
        protected const string DefaultSettingName = "DBConnectionString";

        private readonly AppSettings _appSettings;

        public OracleDbConnector(IOptions<AppSettings> configAccessor)
        {
            _appSettings = configAccessor.Value;
        }

        public IDbConnection Connect(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("設定名が指定されていません。");

            var connectionString = _appSettings.Configs[name];

            if (string.IsNullOrEmpty(connectionString))
                throw new Exception("データベース接続文字列が取得できませんでした。");

            var connection = new OracleConnection(connectionString);
            connection.Open();
            return connection;
        }

        public IDbConnection Connect()
        {
            return Connect(DefaultSettingName);
        }

        //Async版
        public async Task<IDbConnection> ConnectAsync(string name, CancellationToken ct = default)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("設定名が指定されていません。");

            var connectionString = _appSettings.Configs[name];

            if (string.IsNullOrEmpty(connectionString))
                throw new Exception("データベース接続文字列が取得できませんでした。");

            var connection = new OracleConnection(connectionString);

            // OpenAsync が例外を投げた場合にリークしないようにする
            try
            {
                await connection.OpenAsync(ct);
                return connection;
            }
            catch
            {
                connection.Dispose();
                throw;
            }
        }

        // Async版（既定設定名）
        public Task<IDbConnection> ConnectAsync(CancellationToken ct = default)
        {
            return ConnectAsync(DefaultSettingName, ct);
        }
    }
}
