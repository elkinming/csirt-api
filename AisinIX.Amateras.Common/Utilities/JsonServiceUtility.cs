using System;
using System.IO;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AisinIX.Amateras.Common.Configs;
using AisinIX.Amateras.Common.Models;
using System.Linq;
using System.Web;

namespace AisinIX.Amateras.Common.Utilities
{
    /// <summary>
    /// Jsonサービス・ユーティリティ
    /// </summary>
    public class JsonServiceUtility : IJsonServiceUtility
    {
        private readonly IApiContext _apiContext;
        private readonly IApiContextConfig _apiContextConfig;

        public JsonServiceUtility(IApiContext apiContext, IApiContextConfig apiContextConfig)
        {
            _apiContext = apiContext;
            _apiContextConfig = apiContextConfig;
        }
        
        /// <summary>GetHttpClientメソッドで使用するディクショナリ</summary>
        private static Dictionary<string, HttpClient> _clients = new Dictionary<string, HttpClient>();

        /// <summary>GetHttpClientメソッドで使用するロックオブジェクト</summary>
        private static object lockObject = new Object();

        public static string UserID { get; set; }

        /// <summary>
        /// JSONサービスへGET要求を行い、応答内容を取得します。
        /// </summary>
        /// <param name="apiContext">APIコンテキスト</param>
        /// <param name="uri">URI</param>
        /// <returns>応答内容</returns>
        public string Get(string uri)
        {
            var client = GetHttpClient(uri);
            return GetExecuteResult(client, 
                () =>
                {
                    var msg = new HttpRequestMessage(HttpMethod.Get, uri);

                    SetupCommonHeaders(msg);

                    return msg;
                });
        }

        /// <summary>
        /// JSONサービスへPost要求を行い、応答内容を取得します。
        /// </summary>
        /// <param name="uri">URI</param>
        /// <param name="body">body</param>
        /// <returns>応答内容</returns>
        public string Post(string uri, string body)
        {
            return RequestWithBody(uri, body, HttpMethod.Post);
        }

        /// <summary>
        /// JSONサービスへPut要求を行い、応答内容を取得します。
        /// </summary>
        /// <param name="uri">URI</param>
        /// <param name="body">body</param>
        /// <returns>応答内容</returns>
        public string Put(string uri, string body)
        {
            return RequestWithBody(uri, body, HttpMethod.Put);
        }

        /// <summary>
        /// JSONサービスへGET要求を行い、応答内容を取得します。
        /// </summary>
        /// <typeparam name="T">戻り値の型</typeparam>
        /// <param name="apiContext">ApiContext</param>
        /// <param name="uri">サービスURL</param>
        /// <param name="_ResponseAction">サービスURL実行結果を判定する処理</param>
        /// <returns>判定結果</returns>
        public T Get<T>(string uri, Func<HttpStatusCode, T> _ResponseAction)
            where T : new()
        {
            try
            {
                T model = new T();

                HttpWebRequest httpWebRequest = WebRequest.Create(uri) as HttpWebRequest;
                httpWebRequest.Method = "GET";
                if (_apiContext != null)
                {
                    httpWebRequest.Headers[_apiContext.RequestIDHeaderName] = _apiContext.RequestID.ToString("D");
                    httpWebRequest.Headers[_apiContext.RequestDateTimeHeaderName] = _apiContext.RequestDateTime.ToString();
                    httpWebRequest.Headers[_apiContext.RequestUriHeaderName] = _apiContext.RequestUri.OriginalString;
                    httpWebRequest.Headers[_apiContext.RequestHostHeaderName] = _apiContext.RequestHost;
                    httpWebRequest.Headers[_apiContext.UserHeaderName] = _apiContext.UserID;
                    httpWebRequest.Headers[_apiContext.GAIAGroupCompanyCodeHeaderName] = _apiContext.UserGroupCompanyCode;
                    httpWebRequest.Headers[_apiContext.GAIACompanyCodeHeaderName] = _apiContext.UserCompanyCode;
                    httpWebRequest.Headers[_apiContext.GAIAUserHeaderName] = _apiContext.UserID;
                    httpWebRequest.Headers[_apiContext.ClientIPAddressHeaderName] = _apiContext.ClientIPAddress;
                }
                else if (!string.IsNullOrEmpty(_apiContextConfig.UserHeaderName))
                {
                    httpWebRequest.Headers[_apiContextConfig.UserHeaderName] = !string.IsNullOrEmpty(UserID) ? UserID : "Empty";
                }

                using (HttpWebResponse httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse)
                {
                    model = _ResponseAction(httpWebResponse.StatusCode);
                }

                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                UserID = null;
            }
        }

        /// <summary>
        /// JSonサービスへGET要求を行います。
        /// </summary>
        /// <param name="apiContext">APIコンテキスト</param>
        /// <param name="uri"></param>
        /// <param name="credentials"></param>
        /// <param name="_SuccessAction"></param>
        /// <param name="_FailureAction"></param>
        public void GetAsync(string uri, ICredentials credentials, Action<HttpStatusCode, WebHeaderCollection, string> _SuccessAction, Action<Exception> _FailureAction)
        {
            try
            {
                HttpWebRequest httpWebRequest = WebRequest.Create(uri) as HttpWebRequest;
                httpWebRequest.Credentials = credentials;
                httpWebRequest.Method = "GET";
                if (_apiContext != null)
                {
                    httpWebRequest.Headers[_apiContext.RequestIDHeaderName] = _apiContext.RequestID.ToString("D");
                    httpWebRequest.Headers[_apiContext.RequestDateTimeHeaderName] = _apiContext.RequestDateTime.ToString();
                    httpWebRequest.Headers[_apiContext.RequestUriHeaderName] = _apiContext.RequestUri.OriginalString;
                    httpWebRequest.Headers[_apiContext.RequestHostHeaderName] = _apiContext.RequestHost;
                    httpWebRequest.Headers[_apiContext.UserHeaderName] = _apiContext.UserID;
                    httpWebRequest.Headers[_apiContext.GAIAGroupCompanyCodeHeaderName] = _apiContext.UserGroupCompanyCode;
                    httpWebRequest.Headers[_apiContext.GAIACompanyCodeHeaderName] = _apiContext.UserCompanyCode;
                    httpWebRequest.Headers[_apiContext.GAIAUserHeaderName] = _apiContext.UserID;
                    httpWebRequest.Headers[_apiContext.ClientIPAddressHeaderName] = _apiContext.ClientIPAddress;
                }
                else if (!string.IsNullOrEmpty(_apiContextConfig.UserHeaderName))
                {
                    httpWebRequest.Headers[_apiContextConfig.UserHeaderName] = !string.IsNullOrEmpty(UserID) ? UserID : "Empty";
                }

                httpWebRequest.BeginGetResponse((asyncResult) =>
                {
                    HttpWebResponse httpWebResponse = null;
                    try
                    {
                        try
                        {
                            HttpWebRequest httpWebRequest2 = (HttpWebRequest)asyncResult.AsyncState;
                            using (httpWebResponse = (HttpWebResponse)(httpWebRequest2.EndGetResponse(asyncResult)))
                            {
                                _SuccessAction?.Invoke(httpWebResponse.StatusCode, httpWebResponse.Headers, GetBody(httpWebResponse));
                            }
                        }
                        catch (WebException webException)
                        {
                            if (webException.Response != null)
                            {
                                using (httpWebResponse = (HttpWebResponse)webException.Response)
                                {
                                    _SuccessAction?.Invoke(httpWebResponse.StatusCode, httpWebResponse.Headers, GetBody(httpWebResponse));
                                }
                            }
                            else
                            {
                                _FailureAction?.Invoke(webException);
                            }
                        }
                    }
                    catch (Exception ex1)
                    {
                        _FailureAction?.Invoke(ex1);
                    }
                },
                httpWebRequest);
            }
            catch (Exception ex)
            {
                _FailureAction?.Invoke(ex);
            }
            finally
            {
                UserID = null;
            }
        }

        /// <summary>
        /// JSonサービスへPost非同期要求を行います。
        /// </summary>
        /// <param name="apiContext">APIコンテキスト</param>
        /// <param name="uri"></param>
        /// <param name="credentials"></param>
        /// <param name="body"></param>
        /// <param name="_SuccessAction"></param>
        /// <param name="_FailureAction"></param>
        public void PostAsync(string uri, ICredentials credentials, string body, Action<HttpStatusCode, WebHeaderCollection, string> _SuccessAction, Action<Exception> _FailureAction)
        {
            try
            {
                HttpWebRequest httpWebRequest = WebRequest.Create(uri) as HttpWebRequest;
                httpWebRequest.Credentials = credentials;
                httpWebRequest.Method = "Post";
                httpWebRequest.ContentType = "application/json; charset=utf-8";
                if (_apiContext != null)
                {
                    httpWebRequest.Headers[_apiContext.RequestIDHeaderName] = _apiContext.RequestID.ToString("D");
                    httpWebRequest.Headers[_apiContext.RequestDateTimeHeaderName] = _apiContext.RequestDateTime.ToString();
                    httpWebRequest.Headers[_apiContext.RequestUriHeaderName] = _apiContext.RequestUri.OriginalString;
                    httpWebRequest.Headers[_apiContext.RequestHostHeaderName] = _apiContext.RequestHost;
                    httpWebRequest.Headers[_apiContext.UserHeaderName] = _apiContext.UserID;
                    httpWebRequest.Headers[_apiContext.GAIAGroupCompanyCodeHeaderName] = _apiContext.UserGroupCompanyCode;
                    httpWebRequest.Headers[_apiContext.GAIACompanyCodeHeaderName] = _apiContext.UserCompanyCode;
                    httpWebRequest.Headers[_apiContext.GAIAUserHeaderName] = _apiContext.UserID;
                    httpWebRequest.Headers[_apiContext.ClientIPAddressHeaderName] = _apiContext.ClientIPAddress;
                }
                else if (!string.IsNullOrEmpty(_apiContextConfig.UserHeaderName))
                {
                    httpWebRequest.Headers[_apiContextConfig.UserHeaderName] = !string.IsNullOrEmpty(UserID) ? UserID : "Empty";
                }

                httpWebRequest.BeginGetRequestStream((asyncResult1) =>
                {
                    try
                    {
                        HttpWebRequest httpWebRequest2 = (HttpWebRequest)asyncResult1.AsyncState;

                        using (Stream postStream = httpWebRequest2.EndGetRequestStream(asyncResult1))
                        {
                            byte[] byteArray = Encoding.UTF8.GetBytes(body);

                            postStream.Write(byteArray, 0, byteArray.Length);

                            postStream.Flush();
                        }

                        httpWebRequest2.BeginGetResponse((asyncResult) =>
                        {
                            HttpWebResponse httpWebResponse = null;
                            try
                            {
                                try
                                {
                                    HttpWebRequest httpWebRequest3 = (HttpWebRequest)asyncResult.AsyncState;
                                    using (httpWebResponse = (HttpWebResponse)(httpWebRequest3.EndGetResponse(asyncResult)))
                                    {
                                        _SuccessAction?.Invoke(httpWebResponse.StatusCode, httpWebResponse.Headers, GetBody(httpWebResponse));
                                    }
                                }
                                catch (WebException webException)
                                {
                                    if (webException.Response != null)
                                    {
                                        using (httpWebResponse = (HttpWebResponse)webException.Response)
                                        {
                                            _SuccessAction?.Invoke(httpWebResponse.StatusCode, httpWebResponse.Headers, GetBody(httpWebResponse));
                                        }
                                    }
                                    else
                                    {
                                        _FailureAction?.Invoke(new Exception());
                                    }
                                }
                            }
                            catch (Exception ex2)
                            {
                                _FailureAction?.Invoke(ex2);
                            }
                        },
                        httpWebRequest);
                    }
                    catch (Exception ex1)
                    {
                        _FailureAction?.Invoke(ex1);
                    }
                }, httpWebRequest);
            }
            catch (Exception ex)
            {
                _FailureAction?.Invoke(ex);
            }
            finally
            {
                UserID = null;
            }
        }

        /// <summary>
        /// 応答からBodyを取得します。
        /// </summary>
        /// <param name="httpWebResponse"></param>
        /// <returns></returns>
        private static string GetBody(HttpWebResponse httpWebResponse)
        {
            string body = null;

            using (Stream streamResponse = httpWebResponse.GetResponseStream())
            {
                using (StreamReader streamRead = new StreamReader(streamResponse))
                {
                    body = streamRead.ReadToEnd();
                }
            }

            return body;
        }

        private string RequestWithBody(string uri, string body, HttpMethod method)
        {
            HttpClient client = GetHttpClient(uri);
            return GetExecuteResult(client,
                () =>
                {
                    HttpRequestMessage msg = new HttpRequestMessage(method, uri);

                    SetupCommonHeaders(msg);

                    if(!string.IsNullOrEmpty(body))
                    {
                        ByteArrayContent content = new ByteArrayContent(Encoding.UTF8.GetBytes(body));
                        content.Headers.Add("Content-Type", "application/json; charset=UTF-8");
                        msg.Content = content;
                    }
                    return msg;
                });
        }

        /// <summary>
        /// APIコンテキストを基にHttpRequestMessageにヘッダーを追加します。
        /// </summary>
        /// <param name="apiContext">APIコンテキスト</param>
        /// <param name="msg">HttpRequestMessage</param>
        private void SetupCommonHeaders(HttpRequestMessage msg)
        {
            if (_apiContext != null)
            {
                msg.Headers.Add(_apiContext.RequestIDHeaderName, _apiContext.RequestID.ToString("D"));
                msg.Headers.Add(_apiContext.RequestDateTimeHeaderName, _apiContext.RequestDateTime.ToString());
                msg.Headers.Add(_apiContext.RequestUriHeaderName, _apiContext.RequestUri.OriginalString);
                msg.Headers.Add(_apiContext.RequestHostHeaderName, _apiContext.RequestHost);
                msg.Headers.Add(_apiContext.UserHeaderName, _apiContext.UserID);
                msg.Headers.Add(_apiContext.IVUserHeaderName, _apiContext.UserID);
                msg.Headers.Add(_apiContext.GAIAGroupCompanyCodeHeaderName, _apiContext.UserGroupCompanyCode);
                msg.Headers.Add(_apiContext.GAIACompanyCodeHeaderName, _apiContext.UserCompanyCode);
                msg.Headers.Add(_apiContext.GAIAUserHeaderName, _apiContext.UserID);
                msg.Headers.Add(_apiContext.ClientIPAddressHeaderName, _apiContext.ClientIPAddress);
            }
            else if (!string.IsNullOrEmpty(_apiContextConfig.UserHeaderName))
            {
                msg.Headers.Add(_apiContextConfig.UserHeaderName, !string.IsNullOrEmpty(UserID) ? UserID : "Empty");
            }
        }

        /// <summary>
        /// HttpClientの実行結果を取得します。
        /// 失敗した場合はリトライを3回まで実行します。
        /// </summary>
        /// <param name="client">HttpClient</param>
        /// <param name="_GetHttpRequestMessage">HttpRequestMessage生成</param>
        /// <returns>応答内容</returns>
        private static string GetExecuteResult(HttpClient client, Func<HttpRequestMessage> _GetHttpRequestMessage)
        {
            var counter = 3;
            var exceptions = new List<Exception>();
            do
            {
                --counter;
                try
                {
                    HttpRequestMessage msg = _GetHttpRequestMessage();
                    Task<HttpResponseMessage> sendTask = client.SendAsync(msg);
                    sendTask.Wait();
                    //Exception判定(BadRequest[400]からVersionNotSupported[505]までの間であればエラーとする)
                    if (((int)sendTask.Result.StatusCode >= (int)HttpStatusCode.BadRequest) &&
                        ((int)sendTask.Result.StatusCode <= (int)HttpStatusCode.HttpVersionNotSupported))
                    {
                        throw new Exception();
                    }

                    Task<string> readTask = sendTask.Result.Content.ReadAsStringAsync();
                    readTask.Wait();

                    return readTask.Result;
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }
            while (counter > 0);

            var uniqueExceptions = exceptions.Distinct();
            if (uniqueExceptions.Count() == 1)
            {
                throw uniqueExceptions.ToList().FirstOrDefault();
            }
            // ToDo:複数Exceptionをcatchする箇所のログの出力の仕方は要検討
            throw new AggregateException(uniqueExceptions);
        }

        /// <summary>
        /// 対象ホスト別のHttpClientを取得します。
        /// </summary>
        /// <param name="uriString">URI文字列</param>
        /// <returns>対象ホスト別のHttpClient</returns>
        private static HttpClient GetHttpClient(string uriString)
        {
            lock (lockObject)
            {
                // 基底のURI文字列を取得(HttpClient引き当てのキー)
                var baseUri = GetBaseURIString(uriString);

                // 既存であればそれを使う
                if (_clients.ContainsKey(baseUri))
                {
                    return _clients[baseUri];
                }

                // 既存でなければ新しく作成
                var ret = new HttpClient();

                // 共通の既定のヘッダーを設定
                ret.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                _clients.Add(baseUri, ret);

                // 接続が閉じれらるまでの時間を「1分」に設定
                var sp = ServicePointManager.FindServicePoint(new Uri(baseUri));
                sp.ConnectionLeaseTimeout = 60 * 1000;

                return ret;
            }
        }

        /// <summary>
        /// 引数から基底のURI文字列を取得します。
        /// URIとして解析できない場合はnullを返します。
        /// </summary>
        /// <param name="uriString">URIとして評価する文字列</param>
        /// <returns>ポート番号部分までのURI文字列</returns>
        private static string GetBaseURIString(string uriString)
        {
            Uri uri;
            if (Uri.TryCreate(uriString, UriKind.Absolute, out uri))
            {
                var portPart = ":" + uri.Port.ToString();
                if (((uri.Scheme.ToUpper() == "HTTP") && (uri.Port == 80)) ||
                     ((uri.Scheme.ToUpper() == "HTTPS") && (uri.Port == 443)))
                {
                    // 「http」または「https」で既定のポートの場合はポート番号は付与しない
                    portPart = "";
                }

                return uri.Scheme + "://" + uri.Host + portPart;
            }
            return null;
        }
    }
}