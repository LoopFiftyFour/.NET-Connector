using System;
using System.Collections.Generic;
using System.Reflection;
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
        /// If set to true, the response will be parsed assuming collections have V2.2-style [Entity and/or String, Value], instead of the newer [Key, Value]. Defaults to false.
        /// </summary>
        public bool V22Collections { get; set; }

        /// <summary>
        /// If set to true, the request will serialize with the quest name inside the POST body, instead of in the Url. Additionally, the response will be parsed assuming the data is wrapped in a property with the quest name as key. Defaults to false.
        /// </summary>
        public bool V25Url { get; set; }

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
    public class Request
    {
     
        internal RequestOptions Options;

        private string _userId;
        /// <summary>
        /// The id of the end user. This can be any persistent unique identifier. If not set, HttpContext.Current will be used to set a random value and sent it to the user in the next http response.
        /// </summary>
        public string UserId
        {
            get
            {
                if (string.IsNullOrEmpty(_userId))
                    _userId = Utils.Http.GetUser();

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
                    _ip = Utils.Http.GetIP();

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
                    _referer = Utils.Http.GetReferer();

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
                    _userAgent = Utils.Http.GetUserAgent();

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
                    _url = Utils.Http.GetUrl();

                return _url;
            }
            set { _url = value; }
        }


        private static string _libraryVersion = null;
        /// <summary>
        /// The version of this assembly.
        /// </summary>
        private static string LibraryVersion
        {
            get
            {
                if (_libraryVersion == null)
                    _libraryVersion = Assembly.GetAssembly(typeof(Request)).GetName().Version.ToString();

                return _libraryVersion;
            }
        }



        /// <summary>
        /// The name of the current request.
        /// </summary>
        public string QuestName { get; private set; }

        /// <summary>
        /// Created a new request.
        /// </summary>
        /// <param name="requestName">The name of the request. Examples: "Search", "AutoComplete", "CreateEvents".</param>
        /// <param name="options">Optional request options for compatibility with older engines and other settings. Set to null to ignore.</param>
        public Request(string requestName,RequestOptions options)
        {
            QuestName = requestName;
            Options = options;
        }

        /// <summary>
        /// Created a new request.
        /// </summary>
        /// <param name="requestName">The name of the request. Examples: "Search", "AutoComplete", "CreateEvents".</param>
        public Request(string requestName) : this(requestName, new RequestOptions())
        {
            
        }

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
                
                if(Options.V25Url)
                    ret += JsonConvert.SerializeObject(QuestName) + ":{";

                if(UserId==null)
                    throw new ArgumentNullException("UserId", "UserId cannot be null.");

                ret += "\"UserId\":" + JsonConvert.SerializeObject(UserId) + ",";

                if (IP == null)
                    throw new ArgumentNullException("IP", "IP cannot be null.");

                ret += "\"IP\":" + JsonConvert.SerializeObject(IP) + ",";

                if(Referer!=null)
                    ret += "\"Referer\":" + JsonConvert.SerializeObject(Referer) + ",";

                if (Url != null)
                    ret += "\"Url\":" + JsonConvert.SerializeObject(Url) + ",";

                if (UserAgent != null)
                    ret += "\"UserAgent\":" + JsonConvert.SerializeObject(UserAgent) + ",";

                ret += "\"LibraryVersion\":" + JsonConvert.SerializeObject(LibraryVersion) + ",";



                lock (Data)
                {
                    foreach (var key in Data.Keys)
                    {
                        ret += "\"" + key + "\":" + JsonConvert.SerializeObject(Data[key]) + ",";
                    }
                }

                ret = ret.Trim(',');

                if (Options.V25Url)
                    ret += "}";


                ret += "}";

                return ret;
            }
        }
    }
}
