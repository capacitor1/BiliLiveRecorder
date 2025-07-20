using BiliLiveRecorder.Core.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace BiliLiveRecorder.Core.API
{
    internal class GetData
    {
        public static async Task<bool> IsLiving(string ID)
        {
            MemoryStream ms = new MemoryStream();
            int code = -1, r = 0;
            bool isliving = false;
            while (code != 0 && r < Endpoint.Retry)
            {
                try
                {
                    string wbi = await WBI.GetWBIByID(ID);
                    int scode = await Downloader.Download(Endpoint.Info + wbi, ms, new CancellationTokenSource(Settings.TimeOut).Token);
                    if (scode != 0) continue;
                    JsonNode JObjectNode = JsonArray.Parse(ms.ToArray())!;
                    if ((int)JObjectNode["code"]! != 0) return false;
                    string livings = JObjectNode["data"]!["live_status"]!.ToString();
                    if (livings == "1") isliving = true;
                    code = scode;
                }
                catch
                {
                    code = -1;
                    r++;
                }
            }
            ms.Dispose();
            return isliving;
        }
        public static async Task<string> GetM3U8URL(string ID)
        {
            MemoryStream ms = new MemoryStream();
            int code = -1, r = 0;
            string url = string.Empty;
            while (code != 0 && r < Endpoint.Retry)
            {
                try
                {
                    int scode = await Downloader.Download(Endpoint.Stream + ID, ms, new CancellationTokenSource(Settings.TimeOut).Token);
                    if (scode != 0) continue;
                    JsonNode json = JsonArray.Parse(ms.ToArray())!;
                    //Debug.WriteLine(json.ToJsonString());
                    if ((int)json["code"]! != 0) return $"Error : {(string)json["message"]!} ({(int)json["code"]!})";
                    string host = String.Empty;
                    string turl = String.Empty;
                    if (json["data"]!["playurl_info"] == null) return $"Error : No stream returned.";
                    JsonArray jsonArray = json["data"]!["playurl_info"]!["playurl"]!["stream"]!.AsArray();
                    foreach (var v in jsonArray)
                    {
                        if ((string)v!["protocol_name"]! == "http_hls")
                        {
                            foreach (var v2 in v["format"]!.AsArray())
                            {
                                if ((string)v2!["format_name"]! == "fmp4")
                                {
                                    foreach (var hr in v2["codec"]![0]!["url_info"]!.AsArray())
                                    {
                                        string hhost = (string)hr!["host"]!;
                                        if (!hhost.Contains("gotcha") && hhost.Contains(".bilivideo.com"))
                                        {
                                            host = hhost;
                                            break;
                                        }
                                        else
                                        {
                                            host = "https://cn-sxxa-cm-01-02.bilivideo.com";//default
                                        }
                                    }
                                    turl = (string)v2!["codec"]![0]!["base_url"]!;
                                }
                                else if ((string)v2!["format_name"]! == "ts")
                                {
                                    if (host == String.Empty || turl == String.Empty)
                                    {
                                        foreach (var hr in v2["codec"]![0]!["url_info"]!.AsArray())
                                        {
                                            string hhost = (string)hr!["host"]!;
                                            if (!hhost.Contains("gotcha") && hhost.Contains(".bilivideo.com"))
                                            {
                                                host = hhost;
                                                break;
                                            }
                                            else
                                            {
                                                host = "https://cn-sxxa-cm-01-02.bilivideo.com";//default
                                            }

                                        }
                                        turl = (string)v2!["codec"]![0]!["base_url"]!;

                                        turl = turl.Replace(".m3u8", "/index.m3u8");
                                    }
                                }
                                //

                            }
                        }
                    }
                    string burl = turl;
                    foreach (string s in Settings.ReplaceOptions)
                    {
                        burl = burl.Replace(s, "");
                    }
                    url = $"{host}{burl.Replace("?", "")}";
                    code = scode;
                }
                catch
                {
                    code = -1;
                    r++;
                }
            }
            ms.Dispose();
            return url;
        }
        public static async Task<string[]> GetInfoText(string ID)
        {
            MemoryStream ms = new MemoryStream();
            int code = -1, r = 0;
            List<string> list = new List<string>();
            while (code != 0 && r < Endpoint.Retry)
            {
                try
                {

                    string wbi = await WBI.GetWBIByID(ID);
                    int scode = await Downloader.Download(Endpoint.Info + wbi, ms, new CancellationTokenSource(Settings.TimeOut).Token);
                    if (scode != 0) continue;
                    JsonNode json = JsonArray.Parse(ms.ToArray())!;
                    if ((int)json["code"]! != 0) return [$"Error : {(string)json["message"]!} ({(int)json["code"]!})"];
                    list.Add($"# Info of `{(string)json["data"]!["title"]!}`");
                    list.Add($"");
                    list.Add($"RoomID : `{json["data"]!["room_id"]}`");
                    list.Add($"");
                    list.Add($"Desc : \r\n\r\n```\r\n{(string)json["data"]!["description"]!}\r\n```");
                    list.Add($"");
                    list.Add($"***");
                    list.Add($"");
                    list.Add($"CoverURL : [{(string)json["data"]!["user_cover"]!}]({(string)json["data"]!["user_cover"]!})");
                    list.Add($"");
                    list.Add($"Area : {json["data"]!["parent_area_name"]} - {json["data"]!["area_name"]}");
                    list.Add($"");
                    code = scode;
                }
                catch
                {
                    code = -1;
                    r++;
                }
            }
            ms.Dispose();
            return list.ToArray();
        }
        public static async Task<string> GetRID(string ID)
        {
            MemoryStream ms = new MemoryStream();
            int code = -1, r = 0;
            string rid = string.Empty;
            while (code != 0 && r < Endpoint.Retry)
            {
                try
                {

                    string wbi = await WBI.GetWBIByID(ID);
                    int scode = await Downloader.Download(Endpoint.Info + wbi, ms, new CancellationTokenSource(Settings.TimeOut).Token);
                    if (scode != 0) continue;
                    JsonNode json = JsonArray.Parse(ms.ToArray())!;
                    if ((int)json["code"]! != 0) return $"Error : {(string)json["message"]!} ({(int)json["code"]!})";
                    rid = json["data"]!["room_id"]!.ToString();
                    code = scode;
                }
                catch { code = -1; r++; }
            }
            ms.Dispose();
            return rid;
        }
        public static async Task<string> GetTitle(string ID)
        {
            MemoryStream ms = new MemoryStream();
            int code = -1, r = 0;
            string rid = string.Empty;
            while (code != 0 && r < Endpoint.Retry)
            {
                try
                {

                    string wbi = await WBI.GetWBIByID(ID);
                    int scode = await Downloader.Download(Endpoint.Info + wbi, ms, new CancellationTokenSource(Settings.TimeOut).Token);
                    if (scode != 0) continue;
                    JsonNode json = JsonArray.Parse(ms.ToArray())!;
                    if ((int)json["code"]! != 0) return $"Error : {(string)json["message"]!} ({(int)json["code"]!})";
                    rid = (string)json["data"]!["title"]!;
                    code = scode;
                }
                catch { code = -1; r++; }
            }
            ms.Dispose();
            return rid;
        }
        public static async Task<string> GetCover(string ID)
        {
            MemoryStream ms = new MemoryStream();
            int code = -1;
            int retry = 0;
            string rid = string.Empty;
            while (code != 0 && retry < Endpoint.Retry)
            {
                string wbi = await WBI.GetWBIByID(ID);
                int scode = await Downloader.Download(Endpoint.Info + wbi, ms, new CancellationTokenSource(Settings.TimeOut).Token);
                if (scode >= 400) { retry++; continue; }
                JsonNode json = JsonArray.Parse(ms.ToArray())!;
                if ((int)json["code"]! != 0) return $"Error : {(string)json["message"]!} ({(int)json["code"]!})";
                rid = (string)json["data"]!["user_cover"]!;
                code = scode;
            }
            ms.Dispose();
            return rid;
        }
        public static async Task<string[]> ReadM3U8URL(string url)
        {
            MemoryStream ms = new MemoryStream();
            int code = -1;
            int retry = 0;
            List<string> list = ["Null"];
            while (code != 0 && retry < Endpoint.Retry)
            {
                if (retry != 0) await Task.Delay(1000);
                int scode = await Downloader.Download(url, ms, new CancellationTokenSource(Settings.TimeOut).Token);
                if (scode >= 400)
                {
                    list = [$"{scode}"];
                    retry++; continue;
                }
                string[] r = Encoding.Default.GetString(ms.ToArray()).Split("\n");
                list = [];
                list.AddRange(r);
                code = scode;
            }
            ms.Dispose();
            return list.ToArray();
        }
    }
}
