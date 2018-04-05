using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using Loop54.AspNet;
using Loop54.Utils;
using Newtonsoft.Json;

namespace Loop54
{
    /// <summary>
    /// This class is used to set custom options when sending a request to an engine.
    /// </summary>
    public class RequestOptions
    {
        /// <summary>
        /// The HTTP timeout of the request, measured in milliseconds. Defaults to 10000.
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>
        /// NOTE: Deprecated! If set to true, the response will be parsed assuming collections have V2.2-style [Entity and/or String, Value], instead of the newer [Key, Value]. Defaults to false.
        /// </summary>
        [Obsolete]
        public bool V22Collections { get; set; }

        /// <summary>
        /// NOTE: Deprecated! If set to true, the request will serialize with the quest name inside the POST body, instead of in the Url. Additionally, the response will be parsed assuming the data is wrapped in a property with the quest name as key. Defaults to false.
        /// </summary>
        [Obsolete]
        public bool V25Url { get; set; }

        /// <summary>
        /// HTTP proxy used to send the request. The default is null (no proxy).
        /// </summary>
        public IWebProxy Proxy { get; set; }

        /// <summary>
        /// If set to true, the request will time the time it takes to serialize request, send request, recieve response and deserialize response. The values will be returned on the Response object. Note, this may decrease performande. Defaults to false.
        /// </summary>
        public bool MeasureTime { get; set; }

        public RequestOptions()
        {
            Timeout = 10000;
        }
    }

    /// <summary>
    /// Used to create and launch a request to the engine.
    /// </summary>
    public partial class Request
    {
        public RequestOptions Options { get; private set; }

        private readonly Http _http; // Not null

        private string _userId;
        /// <summary>
        /// The id of the end user. This can be any persistent unique identifier. If not set, HttpContext.Current will be used to set a random value and sent it to the user in the next http response.
        /// </summary>
        public string UserId
        {
            get
            {
                if (string.IsNullOrEmpty(_userId))
                    _userId = _http.GetUser();

                return _userId;
            }
            set { _userId = value; }
        }

        private string _ip;
        /// <summary>
        /// The IP of the end user. If not set, HttpContext.Current will be used to retrieve the IP of the end user.
        /// </summary>
        public string IP
        {
            get
            {
                if (string.IsNullOrEmpty(_ip))
                    _ip = _http.GetIP();

                return _ip;
            }
            set { _ip = value; }
        }

        private string _referer;

        /// <summary>
        /// The HTTP referer of the end user request. If not set, HttpContext.Current will be used to retrieve the Referer.
        /// </summary>
        public string Referer
        {
            get
            {
                if (string.IsNullOrEmpty(_referer))
                    _referer = _http.GetReferer();

                return _referer;
            }
            set { _referer = value; }
        }

        private string _userAgent;

        /// <summary>
        /// The HTTP user-agent of the end user request. If not set, HttpContext.Current will be used to retrieve the UserAgent.
        /// </summary>
        public string UserAgent
        {
            get
            {
                if (string.IsNullOrEmpty(_userAgent))
                    _userAgent = _http.GetUserAgent();

                return _userAgent;
            }
            set { _userAgent = value; }
        }

        private string _url;

        /// <summary>
        /// The HTTP url of the end user request. If not set, HttpContext.Current will be used to retrieve the Url.
        /// </summary>
        public string Url
        {
            get
            {
                if (string.IsNullOrEmpty(_url))
                    _url = _http.GetUrl();

                return _url;
            }
            set { _url = value; }
        }

        /// <summary>
        /// The name of the current request.
        /// </summary>
        public string QuestName { get; private set; }

        private Dictionary<string, object> Data = new Dictionary<string, object>();

        /// <summary>
        /// Sets a parameter of the request.
        /// </summary>
        /// <typeparam name="T">The type of data.</typeparam>
        /// <param name="key">The name of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        public void SetValue<T>(string key, T value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            lock (Data)
            {
                Data[key] = value;
            }
        }


        
        /// <summary>
        /// The serialized data of this request object, including all user data and parameters.
        /// </summary>
        public string Serialized
        {
            get
            {
                var ret = "{";
                
                if(UserId==null)
                    throw new ArgumentNullException("UserId", "UserId property cannot be null.");

                ret += "\"UserId\":" + JsonConvert.SerializeObject(UserId) + ",";

                if (IP == null)
                    throw new ArgumentNullException("IP", "IP property cannot be null.");

                ret += "\"IP\":" + JsonConvert.SerializeObject(IP) + ",";

                if(Referer!=null)
                    ret += "\"Referer\":" + JsonConvert.SerializeObject(Referer) + ",";

                if (Url != null)
                    ret += "\"Url\":" + JsonConvert.SerializeObject(Url) + ",";

                if (UserAgent != null)
                    ret += "\"UserAgent\":" + JsonConvert.SerializeObject(UserAgent) + ",";

                lock (Data)
                {
                    foreach (var key in Data.Keys)
                    {
                        ret += "\"" + key + "\":" + JsonConvert.SerializeObject(Data[key]) + ",";
                    }
                }

                ret = ret.Trim(',');
                
                ret += "}";

                return ret;
            }
        }
    }
}
