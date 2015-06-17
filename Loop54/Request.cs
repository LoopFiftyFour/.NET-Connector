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

    public class Request
    {

        internal RequestOptions Options;

        private string _userId;
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

        public string QuestName { get; private set; }

        public Request(string requestName,RequestOptions options)
        {
            QuestName = requestName;
            Options = options;
        }

        public Request(string requestName) : this(requestName, new RequestOptions())
        {
            
        }

        private Dictionary<string, object> Data = new Dictionary<string, object>();

        public void SetValue<T>(string key, T value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            lock (Data)
            {
                Data[key] = value;
            }
        }


        private static string _libraryVersion = null;
        private static string LibraryVersion
        {
            get
            {
                if (_libraryVersion == null)
                    _libraryVersion = Assembly.GetAssembly(typeof(Request)).GetName().Version.ToString();

                return _libraryVersion;
            }
        }

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
