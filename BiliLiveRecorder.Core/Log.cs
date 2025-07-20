using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiliLiveRecorder.Core
{
    public class Log
    {
        public class LogUpdateEventArgs : EventArgs
        {
            public LogLevel Level { get; set; }
            public DateTime Time { get; set; }
            public string ID { get; set; }
            public string Message { get; set; }
        }
        public class TitleUpdateEventArgs : EventArgs
        {
            public string ID { get; set; }
            public string Title { get; set; }
        }
        public class StatusChangedEventArgs : EventArgs
        {
            public string ID { get; set; }
            public Status Status { get; set; }
        }
        public enum LogLevel
        {
            Messsage,Debug,Info, Warn, Error, Fatal
        }
        public enum Status
        {
            Stop,Running,Recording
        }
    }
}
