using System;
using System.Reflection;
using Loop54.Public;

namespace Loop54
{
    public class Request : Response
    {
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

        public string Name { get; private set; }

        public Request(string requestName)
        {
            Name = requestName.ToLower().Trim();
        }

        public void SetValue<T>(string key, T value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            lock (Data)
            {
                Data[key] = value;
            }
        }

        public void SetCollection(string key, ItemCollection value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            SetValue(key, value);
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
        private static string _publicVersion = null;
        private static string PublicVersion
        {
            get
            {
                if (_publicVersion == null)
                    _publicVersion = Assembly.GetAssembly(typeof(Entity)).GetName().Version.ToString();

                return _publicVersion;
            }
        }
        public string Serialized
        {
            get
            {
                var ret = "\"" + Name + "\":{";

                ret += "\"UserId\":\"" + Escape(UserId) + "\",";
                ret += "\"IP\":\"" + IP + "\",";

                ret += "\"Referer\":\"" + Escape(Referer) + "\",";
                ret += "\"Url\":\"" + Escape(Url) + "\",";
                ret += "\"UserAgent\":\"" + Escape(UserAgent) + "\",";

                ret += "\"LibraryVersion\":\"" + LibraryVersion + "\",";
                ret += "\"PublicVersion\":\"" + PublicVersion + "\",";
                

                lock (Data)
                {
                    foreach (var key in Data.Keys)
                    {
                        ret += "\"" + key + "\":" + Public.Serialization.SerializeObject(Data[key], null) + ",";
                    }
                }

                ret = ret.Trim(',');

                ret += "}";

                return ret;
            }
        }

        private static readonly string _backSlash = "\\";
        private static readonly string _escapedBackSlash = "\\\\";
        private static readonly string _quote = "\"";
        private static readonly string _escapedQuote = "\\\"";
        private static readonly string _newline = "\n";
        private static readonly string _return = "\r";
        private static readonly string _tab = "\t";
        private readonly static string _verticalTab = "\v";
        private readonly static string _dataLink = ((char)16).ToString();
        private static string Escape(string str)
        {
            if (str == null)
                return null;

            return str
                .Replace(_backSlash, _escapedBackSlash) //double backslash
                .Replace(_quote, _escapedQuote) //quote

                .Replace(_newline, string.Empty) //line break
                .Replace(_return, string.Empty) //carriage return
                .Replace(_tab, string.Empty) //tab
                .Replace(_verticalTab, string.Empty) //vertical tab (i know, right?)

                .Replace(_dataLink, string.Empty); //data link char escape
        }

    }
}
