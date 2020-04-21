using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace SysWork.Net
{
    public class HttpManager
    {
        private string _url;

        public HttpManager(string url)
        {
            _url = url;
        }

        public string GETTextPlainData()
        {
            string result = null;
            HttpManagerResult httpManagerResult = GETStringData("text/plain");

            if (httpManagerResult.StatucCode == HttpStatusCode.OK)
            {
                result = httpManagerResult.Result;
            }
            else
            {
                result = "";
            }

            return result;
        }

        public string GETJsonData()
        {
            string result = null;
            HttpManagerResult httpManagerResult = GETStringData("application/json");

            if (httpManagerResult.StatucCode == HttpStatusCode.OK)
            {
                result = httpManagerResult.Result;
            }
            else
            {
                result = "";
            }

            return result;
        }

        public HttpManagerResult GETStringData(string contentType)
        {
            return AsyncHelper.RunSync<HttpManagerResult>(() => GETStringDataAsync(contentType));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<HttpManagerResult> GETStringDataAsync()
        {
            return await GETStringDataAsync("text/plain");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public async Task<HttpManagerResult> GETStringDataAsync(string contentType)
        {
            

            HttpClient httpClient = new HttpClient();
            
            HttpManagerResult httpManagerResult = new HttpManagerResult();

            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
            httpClient.DefaultRequestHeaders.Add("Accept-Encoding", "UTF-8");
            //httpClient.DefaultRequestHeaders.Add("Content-Type", contentType);

            try
                {
                    using (HttpResponseMessage response = await httpClient.GetAsync(new Uri(_url)))
                    {
                        using (HttpContent content = response.Content)
                        {
                            httpManagerResult.Result = await content.ReadAsStringAsync();
                            httpManagerResult.ReasonPhrase = response.ReasonPhrase;
                            httpManagerResult.Headers = response.Headers;
                            httpManagerResult.StatucCode = response.StatusCode;
                        }
                    }
                }
                catch (Exception ex)
                {
                    httpManagerResult.ErrorMessage = ex.Message;
                    httpManagerResult.OriginalException = ex;
                }

            return httpManagerResult;
        }



        public HttpManagerResult PUTStringData(string contentType,string stringContent)
        {
            return AsyncHelper.RunSync<HttpManagerResult>(() => PUTStringDataAsync(contentType, stringContent));
        }


        public async Task<HttpManagerResult> PUTStringDataAsync(string contentType, string stringData)
        {
            HttpClient httpClient = new HttpClient();

            HttpManagerResult httpManagerResult = new HttpManagerResult();

            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Add("Accept-Encoding", "UTF-8");
            
            try
            {

                HttpContent httpContent = new StringContent(stringData);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);

                using (HttpResponseMessage response = await httpClient.PutAsync(new Uri(_url), httpContent))
                {
                    using (HttpContent content = response.Content)
                    {
                        httpManagerResult.Result = await content.ReadAsStringAsync();
                        httpManagerResult.ReasonPhrase = response.ReasonPhrase;
                        httpManagerResult.Headers = response.Headers;
                        httpManagerResult.StatucCode = response.StatusCode;
                    }
                }
            }
            catch (Exception ex)
            {
                httpManagerResult.ErrorMessage = ex.Message;
                httpManagerResult.OriginalException = ex;
            }

            return httpManagerResult;
        }

        public async Task<HttpManagerResult> POSTStringDataAsync(string contentType, string stringData)
        {

            HttpClient httpClient = new HttpClient();

            HttpManagerResult httpManagerResult = new HttpManagerResult();

            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Add("Accept-Encoding", "UTF-8");
            
            try
            {

                HttpContent httpContent = new StringContent(stringData);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);

                using (HttpResponseMessage response = await httpClient.PostAsync(new Uri(_url), httpContent))
                {
                    using (HttpContent content = response.Content)
                    {
                        httpManagerResult.Result = await content.ReadAsStringAsync();
                        httpManagerResult.ReasonPhrase = response.ReasonPhrase;
                        httpManagerResult.Headers = response.Headers;
                        httpManagerResult.StatucCode = response.StatusCode;
                    }
                }
            }
            catch (Exception ex)
            {
                httpManagerResult.ErrorMessage = ex.Message;
                httpManagerResult.OriginalException = ex;
            }

            return httpManagerResult;
        }

        private static class AsyncHelper
        {
            private static readonly TaskFactory _taskFactory = new
                TaskFactory(CancellationToken.None,
                            TaskCreationOptions.None,
                            TaskContinuationOptions.None,
                            TaskScheduler.Default);

            public static TResult RunSync<TResult>(Func<Task<TResult>> func)
                => _taskFactory
                    .StartNew(func)
                    .Unwrap()
                    .GetAwaiter()
                    .GetResult();

            public static void RunSync(Func<Task> func)
                => _taskFactory
                    .StartNew(func)
                    .Unwrap()
                    .GetAwaiter()
                    .GetResult();
        }
    }


    public class HttpManagerResult
    {
        public string Result { get; set; }
        public string ReasonPhrase { get; set; }
        public HttpResponseHeaders Headers { get; set; }
        public HttpStatusCode StatucCode { get; set; }
        public string ErrorMessage { get; set; }
        public Exception OriginalException { get; set; }

    }
}
