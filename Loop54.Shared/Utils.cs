using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using Loop54.AspNet;
using Loop54.Exceptions;

namespace Loop54.Utils
{
    internal static class Strings
    {
        internal static string FixEngineUrl(string url)
        {
            var ret = url.ToLower().Trim().Replace("\\","/");

            if (!url.StartsWith("http://") && !url.StartsWith("https://"))
                throw new Exception("Invalid url: Url must use protocol http or https.");

            if (!ret.EndsWith("/"))
                ret += "/";

            return ret;
        }

        internal static string Escape(string val)
        {
            return val.Replace("\"", "\\\"").Replace("\t", "").Replace("\n", "").Replace("\r", "");
        }

        internal static string Random(int length, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < length; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }
    }

    internal class Http
    {
        internal struct HttpResponse
        {
            public string Content;
            public long ContentLength;

            public long RoundtripTime;
            public long RequestTime;
            public long EngineTime;
            public long ResponseTime;
            public long ReadDataTime;

            public long CreateRequestTime;
            public long AddHeadersTime;
        }

        public const string CookieName = "Loop54User";

        private readonly IHttpContextInfo _contextInfo;

        public Http(IHttpContextInfo contextInfo)
        {
            if (contextInfo == null)
                throw new ArgumentNullException("contextInfo");

            _contextInfo = contextInfo;
        }

        internal string GetUser()
        {
            //get existing cooke
            var cookie = _contextInfo.GetCookie(CookieName);
            if (cookie != null)
                return cookie;

            //create new cookie
            var ip = GetIP();
            if (ip == null)
                return null;

            var userId = ip.Replace(":",".") + "_" + Strings.Random(10, false);
            _contextInfo.SetCookie(CookieName, userId, DateTime.Now.AddYears(1));

            return userId;
        }

        internal string GetReferer()
        {
            return _contextInfo.GetReferrer();
        }

        internal string GetUserAgent()
        {
            return _contextInfo.GetUserAgent();
        }

        internal string GetUrl()
        {
            return _contextInfo.GetRequestUrl();
        }

        internal string GetIP()
        {
            //behind proxy?
            var forwarded = _contextInfo.GetRequestHeader("X-Forwarded-For");

            if (!string.IsNullOrEmpty(forwarded))
                return forwarded;

            return _contextInfo.GetRemoteIp();
        }

        internal static HttpResponse GetEngineResponse(string url, string verb, string data, int timeout, IWebProxy proxy, bool measureTime)
        {
            Stopwatch watch=null;
            if (measureTime)
            {
                watch=new Stopwatch();
                watch.Start();
            }


            HttpResponse httpResponse;
            try
            {
                httpResponse = GetResponseData2(url, verb, data, timeout, proxy, measureTime: measureTime);
            }
            catch (WebException ex)
            {
                string msg = url;
                if (ex.Response is HttpWebResponse)
                    msg += "; Engine response: " + new StreamReader(((HttpWebResponse)ex.Response).GetResponseStream()).ReadToEnd();

                if (ex.Status == WebExceptionStatus.ProtocolError)
                    throw new EngineErrorException(msg, ex);

                throw new EngineNotFoundException(msg, ex);
            }

            if (measureTime)
            {
                watch.Stop();

                httpResponse.RoundtripTime = watch.ElapsedMilliseconds;
            }

            return httpResponse;
        }


        private static HttpResponse GetResponseData2(string url, string verb, string stringData,
                                              int timeout, IWebProxy proxy, Encoding dataEncoding = null,
                                              IDictionary<string, string> headers = null, bool measureTime=false)
        {

            var ret = new HttpResponse();

            if (dataEncoding == null)
                dataEncoding = Encoding.UTF8;


            Stopwatch watch = null;
            if (measureTime)
            {
                watch = new Stopwatch();
                watch.Start();
            }

            const string apiVersionHeader = "Api-Version";
            const string apiVersion = "V26";

            const string libVersionHeader = "Lib-Version";
            const string libVersion = "NET:2016-10-28T141131";

            var request = (HttpWebRequest)WebRequest.Create(url);
            // .NET Core throws PlatformNotSupportedException when getting ServicePoint if proxy is not set to something (null is OK)
            request.Proxy = proxy; // https://github.com/dotnet/corefx/issues/26922

            request.Method = verb;
            request.Timeout = timeout;
            request.AutomaticDecompression = DecompressionMethods.GZip;
            request.SendChunked = false;
            request.KeepAlive = true;
            request.Headers.Add(apiVersionHeader, apiVersion);
            request.Headers.Add(libVersionHeader, libVersion);

            request.ServicePoint.UseNagleAlgorithm = false;
            request.ServicePoint.Expect100Continue = false;
            request.ServicePoint.ConnectionLimit = 5000;
            request.ServicePoint.MaxIdleTime = 120000;

            if (measureTime)
            {
                watch.Stop();
                ret.CreateRequestTime = watch.ElapsedMilliseconds;
                watch.Reset();
                watch.Start();
            }

            if(headers!=null)
                foreach (var headerPair in headers)
                    request.Headers[headerPair.Key] = headerPair.Value;

            if (measureTime)
            {
                watch.Stop();
                ret.AddHeadersTime = watch.ElapsedMilliseconds;
                watch.Reset();
                watch.Start();
            }

           
            if (stringData != null)
            {
                var buffer = dataEncoding.GetBytes(stringData);

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(buffer, 0, buffer.Length);
                }
            }

            
            if (measureTime)
            {
                watch.Stop();
                ret.RequestTime = watch.ElapsedMilliseconds;

                watch.Reset();
                watch.Start();
            }

            using (var response = (HttpWebResponse) request.GetResponse())
            {


                if (measureTime)
                {
                    watch.Stop();
                    ret.ResponseTime = watch.ElapsedMilliseconds;

                    long.TryParse(response.GetResponseHeader("Time"), out ret.EngineTime);

                    watch.Reset();
                    watch.Start();
                }

                using (var responseStream = response.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (var reader = new StreamReader(responseStream, dataEncoding))
                        {
                            ret.Content = reader.ReadToEnd();
                        }
                    }
                }

                if (measureTime)
                {
                    watch.Stop();
                    ret.ReadDataTime = watch.ElapsedMilliseconds;

                    watch.Reset();

                }

                long.TryParse(response.GetResponseHeader("Content-Length"), out ret.ContentLength);

            }

            return ret;
        }
    }
}
