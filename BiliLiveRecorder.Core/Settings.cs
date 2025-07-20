using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiliLiveRecorder.Core
{
    internal static class Settings
    {
        public static int TimeOut = 10000;
        public static int Interval = 50000;
        public static HttpClientHandler httpClientHandler = new HttpClientHandler()
        {
            Proxy = null,
            UseProxy = false
        };
        public static string[] ReplaceOptions = ["_bluray", "_prohevc", "_2500", "_1500", "_4000"];
    }
}
