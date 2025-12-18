using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace AisinIX.Amateras.Common.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDbConnector
    {
        /// <summary>
        /// 指定した設定名を利用してデータベースに接続しIDbConnectionを（openして）返す。
        /// </summary>
        IDbConnection Connect(string name);

        /// <summary>
        /// 既定の設定名(default)を利用してデータベースに接続しIDbConnectionを（openして）返す。
        /// </summary>
        IDbConnection Connect();

        /// <summary>
        /// 指定した設定名を利用してデータベースに接続しIDbConnectionを（openして）返す（Async）。
        /// </summary>
        Task<IDbConnection> ConnectAsync(string name, CancellationToken ct = default);

        /// <summary>
        /// 既定の設定名(default)を利用してデータベースに接続しIDbConnectionを（openして）返す（Async）。
        /// </summary>
        Task<IDbConnection> ConnectAsync(CancellationToken ct = default);
    }
}
