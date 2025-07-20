
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiliLiveRecorder.Core
{
    public class Monitor
    {
        public static readonly DateTime Uptime = DateTime.Now;
        public static string GetDownloadSpeed() => Downloader.CountBandWidth(true).Value;
        public static string GetTotalDownload() => Downloader.CountSize(Downloader.TotalDownload);
        public static string GetHttpRequests() => $"{Downloader.TotalRequestOK} OK / {Downloader.TotalRequest} Send";
        public static string GetUpTime() => (DateTime.Now - Uptime).ToString();
    }
}
