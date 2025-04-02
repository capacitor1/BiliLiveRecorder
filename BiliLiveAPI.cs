using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BiliLiveRecorder
{
    public class BiliLiveAPI
    {
        public static string LiveJsonAPI = "https://api.live.bilibili.com/xlive/web-room/v2/index/getRoomPlayInfo?room_id={ROOM_ID}&protocol=0,1&format=0,1,2&codec=0,1,2";
        public static string GetMsgStmJsonAPI = "https://api.live.bilibili.com/xlive/web-room/v1/index/getDanmuInfo?id={ROOM_ID}";
        public static string LiveInfoAPI = "https://api.live.bilibili.com/room/v1/Room/get_info?room_id={ROOM_ID}";

        public static int Retry = 5;
        public static async Task<JsonNode> GetRawJsonAsync(string id,string url,bool isauto)
        {
            var tokenSource = new CancellationTokenSource(Form1.TimeOut);
            MemoryStream ms = new MemoryStream();
            int code = 0,retry1 = 0;
        Label_Retry:
            if (retry1 > Retry) return null;
            code = await Downloader.Download(url.Replace("{ROOM_ID}", id), ms,tokenSource.Token);
            if (code == -1)
            {
                LiveRecMain.ShowOutput(id, $"API/M : Request Error [Internal Error ({code})] , retrying {retry1} / {Retry}", "GET", OperatedFile: url.Replace("{ROOM_ID}", id));
                await Task.Delay(1000); 
                retry1++;
                goto Label_Retry;
            }
            if (code == -2)
            {
                LiveRecMain.ShowOutput(id, $"API/M : Request Error [Operation TimeOut ({code})] , retrying {retry1} / {Retry}", "GET", OperatedFile: url.Replace("{ROOM_ID}", id));
                await Task.Delay(1000);
                retry1++;
                goto Label_Retry;
            }
            else if (code >= 400)
            {
                LiveRecMain.ShowOutput(id, $"API/M : Request Error [Response Error ({code})] , retrying {retry1} / {Retry}", "GET", OperatedFile: url.Replace("{ROOM_ID}", id));
                await Task.Delay(1000);
                retry1++;
                goto Label_Retry;
            }
            try
            {
                JsonNode JObjectNode = JsonArray.Parse(ms.ToArray())!;
                //if(!isauto) LiveRecMain.ShowOutput(id, $"API/M : Received {url.Replace("{ROOM_ID}", id)}", "GET", OperatedFile: url.Replace("{ROOM_ID}", id), OperatedLength: ms.Length);
                return JObjectNode;
            }
            catch
            {
                LiveRecMain.ShowOutput(id, $"API/M : ParseJSON Error , retrying {retry1} / {Retry}", "GET", OperatedFile: url.Replace("{ROOM_ID}", id));
                retry1++;
                goto Label_Retry;
            }
        }

        public static string GetURL(JsonNode jsonNode)
        {
            var json = jsonNode.AsObject();
            int c = (int)json["code"];
            if(c == 0)
            {
                string host = String.Empty;
                string url = String.Empty;
                if(json["data"]["playurl_info"] == null) return $"Error : No stream returned.";
                JsonArray jsonArray = json["data"]["playurl_info"]["playurl"]["stream"].AsArray();
                foreach (var v in jsonArray)
                {
                    if ((string)v["protocol_name"] == "http_hls")
                    {
                        foreach (var v2 in v["format"].AsArray())
                        {
                            if ((string)v2["format_name"] == "fmp4")
                            {
                                foreach (var hr in v2["codec"][0]["url_info"].AsArray())
                                {
                                    string hhost = (string)hr["host"];
                                    if (!hhost.Contains("gotcha") && hhost.Contains(".bilivideo.com"))
                                    {
                                        host = hhost;
                                    }
                                    else
                                    {
                                        host = "https://cn-sxxa-cm-01-02.bilivideo.com";//default
                                    }
                                
                                }
                                url = (string)v2["codec"][0]["base_url"];
                            }
                            else if((string)v2["format_name"] == "ts")
                            {
                                if (host == String.Empty || url == String.Empty)
                                {
                                    foreach (var hr in v2["codec"][0]["url_info"].AsArray())
                                    {
                                        string hhost = (string)hr["host"];
                                        if (!hhost.Contains("gotcha") && hhost.Contains(".bilivideo.com"))
                                        {
                                            host = hhost;
                                        }
                                        else
                                        {
                                            host = "https://cn-sxxa-cm-01-02.bilivideo.com";//default
                                        }

                                    }
                                    url = (string)v2["codec"][0]["base_url"];

                                    url = url.Replace(".m3u8", "/index.m3u8");
                                }
                            }
                            //

                        }
                    }
                }
                string burl = url;
                foreach (string s in Form1.replaceoptions.Split('/'))
                {
                    burl = burl.Replace(s, "");
                }
                return $"{host}{burl.Replace("?", "")}";
            }
            else
            {
                return $"Error : {(string)json["message"]} ({(int)json["code"]})";
            }
        }
        public static string GetToken(JsonNode jsonNode)
        {
            var json = jsonNode.AsObject();
            int c = (int)json["code"];
            if (c == 0)
            {
                string token = (string)json["data"]["token"];
                return token;
            }
            else
            {
                return $"Error : {(string)json["message"]} ({(int)json["code"]})";
            }
        }
        public static string GetHost(JsonNode jsonNode)
        {
            var json = jsonNode.AsObject();
            int c = (int)json["code"];
            if (c == 0)
            {
                string t = (string)json["data"]["host_list"][0]["host"];
                string port = json["data"]["host_list"][0]["wss_port"].ToString();
                return $"wss://{t}:{port}/sub";
            }
            else
            {
                return $"Error : {(string)json["message"]} ({(int)json["code"]})";
            }
        }
        public static async Task<KeyValuePair<bool,string>> ParseJSONAndGetURL(string strID) //false=finished
        {
            try
            {
                JsonNode jsonNode = await GetRawJsonAsync(strID, LiveJsonAPI, false);
                //
                if (jsonNode == null) return new(false, String.Empty);
                string streamurl = GetURL(jsonNode);
                if (streamurl.StartsWith("Error : "))
                {
                    if (streamurl.Contains($"No stream returned"))
                    {
                        //LiveRecMain.ShowOutput(strID, "API/M : Stream is null.", "GET", type: "Warn");
                        return new(false, String.Empty);
                    }
                    else
                    {
                        LiveRecMain.ShowOutput(strID, "API/M : " + streamurl, "GET", type: "Error");
                        return new(false, String.Empty);
                    }
                }
                else
                {
                    LiveRecMain.ShowOutput(strID, $"Fetched URL : {streamurl}", "Get HLSInfo", OperatedLength: streamurl.Length);
                    return new(true, streamurl);
                }
            }
            catch(Exception ex)
            {
                LiveRecMain.ShowOutput(strID, "API/M : " + ex.Message, "GET", type: "Error");
                return new(false, String.Empty);

            }
        }
        public static async Task<KeyValuePair<bool, KeyValuePair<string, string>>> ParseJSONAndGetMsgStmToken(string strID) //false=err
        {
            JsonNode jsonNode = await GetRawJsonAsync(strID, GetMsgStmJsonAPI,false);
            //
            string token = GetToken(jsonNode);
            string host = GetHost(jsonNode);
            if (token.StartsWith("Error : ") || host.StartsWith("Error : "))
            {
                LiveRecMain.ShowOutput(strID, "API/M : MessageStream is null.", "GET", type: "Warn");
                return new(false, new(String.Empty, String.Empty));
            }
            else
            {
                return new(true, new(token,host));
            }
        }
        public static async Task<bool> ParseJSON(string strID) 
        {
            Dictionary<string, string> info = await BiliLiveAPI.ParseJSONAndGetInfo(strID,true);
            if (info.ContainsKey("$Err"))
            {
                return false;
            }
            string isliving = info["IsLiving"];
            if (isliving == "1") return true;
            else return false;
        }
        public static async Task<Dictionary<string, string>> ParseJSONAndGetInfo(string strID,bool isauto) 
        {
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            try
            {
                JsonNode jsonNode = await GetRawJsonAsync(strID, LiveInfoAPI,isauto);
                //
                var json = jsonNode.AsObject();
                int c = (int)json["code"];
                if (c == 0)
                {
                    keyValuePairs.Add("Title", (string)json["data"]["title"]);
                    keyValuePairs.Add("RID", json["data"]["room_id"].ToString());
                    keyValuePairs.Add("Desc", (string)json["data"]["description"]);
                    keyValuePairs.Add("CoverURL", (string)json["data"]["user_cover"]);
                    keyValuePairs.Add("IsLiving", json["data"]["live_status"].ToString());
                    keyValuePairs.Add("Area", $"{json["data"]["parent_area_name"]} - {json["data"]["area_name"]}");
                }
                else
                {
                    keyValuePairs.Add("$Err", $"Error : {(string)json["message"]} ({(int)json["code"]})");
                }
            }
            catch(Exception ex) 
            {
                keyValuePairs.Add("$Err", $"Error : {ex.Message}");
            }
            return keyValuePairs;
        }
    }
}
