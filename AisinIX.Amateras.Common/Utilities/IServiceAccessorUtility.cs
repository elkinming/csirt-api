using System;
using System.Threading.Tasks;

namespace AisinIX.Amateras.Common.Utilities
{
    public interface IServiceAccessorUtility
    {
        Task<string> GetStringAsync(Uri uri);
    }
}