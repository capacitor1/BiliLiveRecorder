using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiliLiveRecorder.Core.API
{
    internal class Endpoint
    {
        public const string Stream = "https://api.live.bilibili.com/xlive/web-room/v2/index/getRoomPlayInfo?protocol=0,1&format=0,1,2&codec=0,1,2&room_id=";
        public const string Info = "https://api.live.bilibili.com/room/v1/Room/get_info?";
        public const int Retry = 5;
    }
}
