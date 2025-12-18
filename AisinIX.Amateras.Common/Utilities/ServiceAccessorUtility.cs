using System;
using System.Threading.Tasks;
using System.Net.Http;

namespace AisinIX.Amateras.Common.Utilities
{
    public class ServiceAccessorUtility: IServiceAccessorUtility
    {
        private readonly IHttpClientFactory _clientFactory;

        public ServiceAccessorUtility(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<string> GetStringAsync(Uri uri)
        {
            var client = _clientFactory.CreateClient();
            return await client.GetStringAsync(uri);
        }
    }
}