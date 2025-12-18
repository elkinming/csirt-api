using System;
using System.Net;
using System.Net.Http;

namespace AisinIX.Amateras.Common.Utilities
{
    public interface IJsonServiceUtility
    {
        
        string Get(string uri);

        T Get<T>(string uri, Func<HttpStatusCode, T> _ResponseAction)
        where T : new();

        string Post(string uri, string body);

        void PostAsync(string uri, ICredentials credentials, string body, Action<HttpStatusCode, WebHeaderCollection, string> _SuccessAction, Action<Exception> _FailureAction);
        
        void GetAsync(string uri, ICredentials credentials, Action<HttpStatusCode, WebHeaderCollection, string> _SuccessAction, Action<Exception> _FailureAction);

        string Put(string uri, string body);
    }
}