using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Timers;
using System.Web;

namespace BiliLiveRecorder
{
    public static class Ext
    {
        public static object TryParse(this string txt, string type)
        {
            bool issuccess = false;
            switch (type.ToLower())
            {
                case "int":
                    issuccess = int.TryParse(txt, out int i);
                    if (!issuccess)
                    {
                        MessageBox.Show($"Text '{txt}' must be a number.\r\n\r\n[W:04]", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return -1;
                    }
                    else return i;
                case "double":
                    issuccess = double.TryParse(txt, out double d);
                    if (!issuccess)
                    {
                        MessageBox.Show($"Text '{txt}' must be a double.\r\n\r\n[W:05]", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return -1.0;
                    }
                    else return d;
                default:
                    return txt;
            }
        }

    }
    public class LiveRecMain
    {
        private static object locker = new object();
        public static ulong TotalSendPacket = 0, TotalRecvPacket = 0;

        public static string[] Help = [
            "{ROOM_ID} : Stream ID (type integer '123456')",
            "{TIME_NOW} : Real time (type string 'yyyy_MM_dd_HH_mm_ss_ffff')",
            "{FILE_EXT} : File extension (type string 'fmp4')"
            ];
        public static HttpClient client = new();
        public static FileStream fs = new(Path.Combine(Form1.workdir, "Internal/Log/", "Log_" + System.DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_ffff") + ".rlog"), FileMode.OpenOrCreate, FileAccess.Write);
        public class Output
        {
            //operation : Get API,Post API,Get HLSInfo,Download File,etc.
            public Output(string iD, string message, string operation, string type = "Info", string OperatedFile = "-", long StreamPosition = 0, long OperatedLength = 0)
            {
                m_ID = iD;
                m_Time = DateTimeOffset.Now.ToUnixTimeSeconds();
                m_Type = type;
                m_Msg = message;
                m_Operation = operation;
                m_OperatedFile = OperatedFile;
                m_StreamPosition = StreamPosition;
                m_OperatedLength = OperatedLength;
            }
            private string m_ID;
            private string m_Msg;
            private string m_Type;
            private string m_Operation;
            private long m_Time;
            private string m_OperatedFile;
            private long m_StreamPosition;
            private long m_OperatedLength;
            //
            public long Time { get { return m_Time; } }
            public long StreamPosition { get { return m_StreamPosition; } }
            public long OperatedLength { get { return m_OperatedLength; } }
            public string ID { get { return m_ID; } }
            public string Message { get { return m_Msg; } }
            public string Type { get { return m_Type; } }
            public string Operation { get { return m_Operation; } }
            public string OperatedFile { get { return m_OperatedFile; } }

        }

        public class Settings
        {
            public double Monitoring_Interval { get; set; }
            public string WorkingDirectory { get; set; }
            public string WorkingDirectory_SubDirectory { get; set; }
            public string BILI_LiveStreamAPI { get; set; }
            public string BILI_LiveStreamInfoAPI { get; set; }
            public string BILI_MessageStreamAPI { get; set; }
            public string BILI_APIAutoReplace_Keywords { get; set; }
            public int BILI_API_MaxRetry { get; set; }
            public int Download_Threads { get; set; }
            public int Download_WriteFileTrigger { get; set; }
            public int OutputList_AutoClear { get; set; }
            public int TimeOut { get; set; }

        }
        public static void ShowOutput(string iD, string message, string operation, string type = "Info", string OperatedFile = "-", long StreamPosition = 0, long OperatedLength = 0)
        {
            Output output = new(iD, message, operation, type, OperatedFile, StreamPosition, OperatedLength);
            Form1.OutputLog.Add(output);
        }

        public static readonly byte[] rlogh = {0x52,0x65,0x63,0x6F,0x72,0x64,0x65,0x64,0x4C,0x6F,0x67,0x4F,0x75,0x74,0x70,0x75,
0x74,0x07,0x21,0x07,0x21,0x00,0x00,0x00,0x19,0x43,0x72,0x65,0x61,0x74,0x65,0x64,
0x20,0x42,0x79,0x20 };
        private static readonly byte[] rpacketh = {0x52,0x65,0x63,0x6F,0x72,0x64,0x65,0x64,0x4D,0x65,0x73,0x73,0x61,0x67,0x65,0x53,
0x74,0x72,0x65,0x61,0x6D,0x00,0x00,0x50,0x61,0x63,0x6B,0x65,0x74,0x73,0x4F,0x72,
0x69,0x67,0x69,0x6E,0x00,0x00 };
        public static void WriteOutput(Output o)
        {
            lock (locker)//加锁
            {
                byte[] b = Encoding.Default.GetBytes(JsonSerializer.Serialize(o));
                fs.Write(BitConverter.GetBytes(b.Length));
                fs.Write(b);
                fs.Flush();
            }
        }
        public static async Task WriteIDList(string[] l)
        {
            string p = Path.Combine(Form1.workdir, "Internal/");
            Directory.CreateDirectory(p);
            string f = Path.Combine(p, "IDs.sidl");
            if (File.Exists(f)) File.Delete(f);
            await File.WriteAllLinesAsync(f, l);
        }
        public static async Task<string[]> ReadIDList()
        {
            string p = Path.Combine(Form1.workdir, "Internal/");
            if (!Directory.Exists(p)) Directory.CreateDirectory(p);
            string f = Path.Combine(p, "IDs.sidl");
            if (File.Exists(f))
            {
                string[] l = await File.ReadAllLinesAsync(f);
                return l;
            }
            return [];
        }
        public static async Task<Settings?> ReadSettings()
        {
            string p = Path.Combine(Form1.workdir, "Internal/");
            Directory.CreateDirectory(p);
            string f = Path.Combine(p, "Settings.conf");
            if (File.Exists(f))
            {
                var deserializedSettings = JsonSerializer.Deserialize<Settings>(await File.ReadAllTextAsync(f));
                if (deserializedSettings != null)
                {
                    return deserializedSettings;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        public static void SetSettings(Settings? settings)
        {
            if (settings != null)
            {
                Form1.Interval = settings.Monitoring_Interval;
                Form1.workdir = settings.WorkingDirectory;
                Form1.SubDirName = settings.WorkingDirectory_SubDirectory;
                BiliLiveAPI.LiveJsonAPI = settings.BILI_LiveStreamAPI;
                BiliLiveAPI.LiveInfoAPI = settings.BILI_LiveStreamInfoAPI;
                BiliLiveAPI.GetMsgStmJsonAPI = settings.BILI_MessageStreamAPI;
                BiliLiveAPI.Retry = settings.BILI_API_MaxRetry;

                Form1.MultiThread = settings.Download_Threads;
                Form1.MultiThread_WriteFileTrigger = settings.Download_WriteFileTrigger;
                Form1.AutoClear = settings.OutputList_AutoClear;
                Form1.replaceoptions = settings.BILI_APIAutoReplace_Keywords;
                Form1.TimeOut = settings.TimeOut;
            }

        }
        public static Settings GetSettings()
        {
            var settings = new Settings();
            settings.Monitoring_Interval = Form1.Interval;
            settings.WorkingDirectory = Form1.workdir;
            settings.WorkingDirectory_SubDirectory = Form1.SubDirName;
            settings.BILI_LiveStreamAPI = BiliLiveAPI.LiveJsonAPI;
            settings.BILI_LiveStreamInfoAPI = BiliLiveAPI.LiveInfoAPI;
            settings.BILI_MessageStreamAPI = BiliLiveAPI.GetMsgStmJsonAPI;
            settings.BILI_API_MaxRetry = BiliLiveAPI.Retry;

            settings.Download_Threads = Form1.MultiThread;
            settings.Download_WriteFileTrigger = Form1.MultiThread_WriteFileTrigger;
            settings.OutputList_AutoClear = Form1.AutoClear;
            settings.BILI_APIAutoReplace_Keywords = Form1.replaceoptions;
            settings.TimeOut = Form1.TimeOut;
            return settings;

        }
        public static async Task WriteSettings(Settings settings)
        {
            string p = Path.Combine(Form1.workdir, "Internal/");
            Directory.CreateDirectory(p);
            string f = Path.Combine(p, "Settings.conf");
            if (File.Exists(f)) File.Delete(f);
            string settingsjson = JsonSerializer.Serialize(settings);
            await File.WriteAllTextAsync(f, settingsjson);
        }

        public class FMP4Parser
        {
            public static async Task<KeyValuePair<bool, string[]>> GetM3U8Str(string hlsurl, string id)
            {
                var tokenSource = new CancellationTokenSource(Form1.TimeOut);
                MemoryStream ms = new MemoryStream();
                int code = 0, retry1 = 0;
            Label_Retry:
                code = await Downloader.Download(hlsurl, ms, client,tokenSource.Token);
                if (code == -1)
                {
                    ShowOutput(id, $"HLS/M3U : Request Error [Internal Error ({code})]", "GET", OperatedFile: hlsurl);
                    return new(false, []);
                }
                if (code == -2)
                {
                    ShowOutput(id, $"HLS/M3U : Request Error [Operation TimeOut ({code}), retrying {retry1} / {BiliLiveAPI.Retry}]", "GET", OperatedFile: hlsurl);
                    await Task.Delay(1000);
                    if (retry1 < BiliLiveAPI.Retry)
                    {
                        await Task.Delay(1000);
                        goto Label_Retry;
                    }
                    else
                    {
                        return new(false, []);
                    }
                }
                else if (code > 400)
                {
                    retry1++;
                    ShowOutput(id, $"HLS/M3U : Request Error [Response Error ({code})] , retrying {retry1} / {BiliLiveAPI.Retry}", "GET", OperatedFile: hlsurl);
                    await Task.Delay(1000);
                    if (retry1 < BiliLiveAPI.Retry)
                    {
                        await Task.Delay(1000);
                        goto Label_Retry;
                    }
                    else
                    {
                        return new(false, []);
                    }
                }
                else if (code == 400)
                {
                    client.Dispose();
                    client = new HttpClient();
                    goto Label_Retry;

                }
                ShowOutput(id, $"HLS/M3U : Received {hlsurl}", "GET", OperatedFile: hlsurl, OperatedLength: ms.Length);
                string[] r = Encoding.Default.GetString(ms.ToArray()).Split("\n");
                ms.Dispose();
                return new(true, r);
            }
            public static string GetHMAPFromM3U(string[] m3u8)
            {
                string map = String.Empty;
                foreach (string item in m3u8)
                {
                    if (item.Contains("#EXT-X-MAP:URI=\""))
                    {
                        map = item.Replace("\"", "").Replace("#EXT-X-MAP:URI=", "");
                    }
                }

                return map;//if empty : not contains mapfile.
            }
            public static string GetSequenceOffsetFromM3U(string[] m3u8)
            {
                string map = String.Empty;
                foreach (string item in m3u8)
                {
                    if (!item.Contains("#"))
                    {
                        map = item.Replace(".m4s", "");
                        return map;
                    }
                }

                return string.Empty;//if empty : error.

            }
            public static async Task<KeyValuePair<bool, KeyValuePair<ulong, byte[]>>> DownloadFMP4Sequence(string sequrl, string id, bool ismap)//bool : false=finished
            {
                var tokenSource = new CancellationTokenSource(Form1.TimeOut);
                MemoryStream ms = new MemoryStream();
                Uri uri = new Uri(sequrl);
                var filename = HttpUtility.UrlDecode(uri.Segments.Last());
                int code = 0, retry1 = 0;
            Label_Retry:
                code = await Downloader.Download(sequrl, ms, client,tokenSource.Token);
                if (code == -1)
                {
                    retry1++;
                    ShowOutput(id, $"HLS/FMP4 : Request Error [Internal Error ({code})] , retrying {retry1} / {BiliLiveAPI.Retry}", "GET", OperatedFile: sequrl);
                    if (retry1 < BiliLiveAPI.Retry)
                    {
                        await Task.Delay(1000);
                        goto Label_Retry;
                    }
                    else
                    {
                        return new(false, new());
                    }
                }
                else if(code == -2)
                {
                    retry1++;
                    ShowOutput(id, $"HLS/FMP4 : Request Error [Operation TimeOut ({code})] , retrying {retry1} / {BiliLiveAPI.Retry}", "GET", OperatedFile: sequrl);
                    if (retry1 < BiliLiveAPI.Retry)
                    {
                        await Task.Delay(1000);
                        goto Label_Retry;
                    }
                    else
                    {
                        return new(false, new());
                    }
                }
                else if (code > 400)
                {
                    if (retry1 < BiliLiveAPI.Retry && await BiliLiveAPI.ParseJSON(id))
                    {
                        if (code != 404)
                        {
                            retry1++;
                            ShowOutput(id, $"HLS/FMP4 : Request Error [Response Error ({code})] , retrying {retry1} / {BiliLiveAPI.Retry}", "GET", OperatedFile: sequrl);
                        }
                        await Task.Delay(500);
                        goto Label_Retry;
                    }
                    else
                    {
                        return new(false, new());
                    }
                }
                else if (code == 400)
                {
                    client.Dispose();
                    client = new HttpClient();
                    goto Label_Retry;
                }
                //
                //long pos = stream.Position;
                //await stream.WriteAsync(ms.ToArray());
                //
                ShowOutput(id, $"HLS/FMP4 : Received {filename}", "GET", OperatedFile: filename, OperatedLength: ms.Length);
                //ShowOutput(id, $"HLS/FMP4 : {filename} --> {Path.GetFileName(stream.Name)}", "GET;WRITE", OperatedFile: Path.GetFileName(stream.Name), OperatedLength: ms.Length, StreamPosition: pos);
                ms.Dispose();
                filename = filename.Replace(".m4s", "");
                if (!ulong.TryParse(filename, out var value))
                {
                    return new(true, new(ulong.Parse(filename.Substring(1)), ms.ToArray()));
                }
                else
                {
                    return new(true, new(ulong.Parse(filename), ms.ToArray()));

                }
            }

            public static async Task WriteFMP4Sequence(string id, KeyValuePair<ulong, byte[]> k, FileStream fs)
            {
                long pos = fs.Position;
                await fs.WriteAsync(k.Value);
                ShowOutput(id, $"HLS/FMP4 : {k.Key}.m4s --> {Path.GetFileName(fs.Name)}", "WRITE", OperatedFile: Path.GetFileName(fs.Name), OperatedLength: k.Value.Length, StreamPosition: pos);
            }
            public static void WriteFMP4Sequences(string id, ref Dictionary<ulong, byte[]> Fragments, FileStream fs)
            {
                Dictionary<ulong, byte[]> dic1Asc = Fragments.OrderBy(o => o.Key).ToDictionary(o => o.Key, p => p.Value);
                long pos = fs.Position;
                string range = $"{dic1Asc.First().Key}.m4s -- {dic1Asc.Last().Key}.m4s";
                foreach (KeyValuePair<ulong, byte[]> f in dic1Asc)
                {
                    fs.Write(f.Value);

                    if (Fragments.Contains(f))
                    {
                        Fragments.Remove(f.Key);
                        Form1.FragmentsInMemoryC--;
                    }
                }
                long posnow = fs.Position;
                ShowOutput(id, $"HLS/FMP4 : {range} --> {Path.GetFileName(fs.Name)}", "WRITE", OperatedFile: Path.GetFileName(fs.Name), OperatedLength: posnow - pos, StreamPosition: pos);
            }
        }
        public class Main
        {
            public static void CreateFile(string path)
            {
                string? f = Path.GetDirectoryName(path);
                if (!File.Exists(f) && f != null)
                {
                    Directory.CreateDirectory(f);
                }
                else
                {
                    return;
                }
                //File.Create(path).Dispose();
            }
            public async static void DownloadIt(object source, ElapsedEventArgs e, string ID)
            {
                await DownloadIt(ID);
            }
            public async static Task DownloadIt(string ID)
            {
                var tokenSource = new CancellationTokenSource(Form1.TimeOut);
                if (!Form1.activestreams.ContainsKey(ID))
                {
                    Form1.PublicForm1.StreamIDList.FindItemWithText(ID).SubItems[2].Text = "Paused";
                    return;
                }
                var timer = Form1.activestreams[ID];
                timer.Stop();
                //

                Dictionary<ulong, byte[]> seqs = new Dictionary<ulong, byte[]>();
            RetryGm3u:
                KeyValuePair<bool, string> canstart = await BiliLiveAPI.ParseJSONAndGetURL(ID);
                if (!canstart.Key)
                {
                    timer.Start();
                    return;
                }
                bool isfinished = true;
                Form1.PublicForm1.StreamIDList.FindItemWithText(ID).SubItems[2].Text = "Recording";
                string timenow = System.DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_ffff");
                //get info
                Dictionary<string, string> info = await BiliLiveAPI.ParseJSONAndGetInfo(ID, false);
                if (info.ContainsKey("$Err"))
                {
                    ShowOutput(ID, $"HLS/M3U : Failed get stream info. Retry after 5000ms.", "GET", type: "Error");
                    await Task.Delay(5000);
                    if (!Form1.activestreams.ContainsKey(ID))
                    {
                        Form1.PublicForm1.StreamIDList.FindItemWithText(ID).SubItems[2].Text = "Paused";
                        return;
                    }
                    goto RetryGm3u;
                }
                else if (info["IsLiving"] != "1")
                {
                    ShowOutput(ID, $"HLS/REC : Stream status error : stream is null.", "Error");
                    if (Form1.activestreams.ContainsKey(ID))
                    {
                        Form1.PublicForm1.StreamIDList.FindItemWithText(ID).SubItems[2].Text = "Running";
                        timer.Start();
                    }
                    else
                    {
                        Form1.PublicForm1.StreamIDList.FindItemWithText(ID).SubItems[2].Text = "Paused";
                    }
                    return;
                }
            //get m3u8
                KeyValuePair<bool, string[]> m3u = await FMP4Parser.GetM3U8Str(canstart.Value, ID);
                if (!m3u.Key && await BiliLiveAPI.ParseJSON(ID))
                {
                    ShowOutput(ID, $"HLS/M3U : Failed get m3u8 index. Retry after 5000ms.", "GET", type: "Error");
                    await Task.Delay(5000);
                    if (!Form1.activestreams.ContainsKey(ID))
                    {
                        Form1.PublicForm1.StreamIDList.FindItemWithText(ID).SubItems[2].Text = "Paused";
                        return;
                    }
                    goto RetryGm3u;
                }

                Form1.RecordingC++;
                string infofile = Path.Combine(Form1.workdir, Form1.SubDirName.Replace("{ROOM_ID}", ID).Replace("{FILE_EXT}", "minf").Replace("{TIME_NOW}", timenow));
                string coverfile = Path.Combine(Form1.workdir, Form1.SubDirName.Replace("{ROOM_ID}", ID).Replace("{FILE_EXT}", "cjpg").Replace("{TIME_NOW}", timenow));
                CreateFile(infofile);
                List<string> lines = new List<string>(
                    [
                    $"# {info["Title"]}",$"",
                            $"> {info["Desc"]}",$"",
                            $"***",$"",
                            $"Area : {info["Area"]}",$"",
                            $"WatchLink : [{info["Title"]}](https://live.bilibili.com/{ID})",$"",
                            ]);
                await File.WriteAllLinesAsync(infofile, lines);
                Form1.PublicForm1.StreamIDList.FindItemWithText(ID).SubItems[1].Text = info["Title"];
                ShowOutput(ID, $"HLS/INFO : Set stream title '{info["Title"]}'", "GET", OperatedLength: info["Title"].Length);
                FileStream fss = new(coverfile, FileMode.OpenOrCreate, FileAccess.Write);
                await Downloader.Download(info["CoverURL"], fss, new HttpClient(),tokenSource.Token);
                ShowOutput(ID, $"HLS/INFO : Received Cover Image {info["CoverURL"]}", "GET;WRITE", OperatedFile: Path.GetFileName(fss.Name), OperatedLength: fss.Length);
                fss.Dispose();

                //create file
                string f = Path.Combine(Form1.workdir, Form1.SubDirName.Replace("{ROOM_ID}", ID).Replace("{FILE_EXT}", "fmp4").Replace("{TIME_NOW}", timenow));
                CreateFile(f);
                FileStream fs = new(f, FileMode.Append, FileAccess.Write);
                fs.Position = fs.Length;
                ShowOutput(ID, $"HLS/REC : Created {fs.Name}", "WRITE", OperatedFile: fs.Name);
                //
                string serverhost = canstart.Value.Replace("index.m3u8", "");
                string hmap = $"{serverhost}{FMP4Parser.GetHMAPFromM3U(m3u.Value)}";
                KeyValuePair<bool, KeyValuePair<ulong, byte[]>> h = await FMP4Parser.DownloadFMP4Sequence(hmap, ID, true);
                await FMP4Parser.WriteFMP4Sequence(ID, h.Value, fs);
                await fs.FlushAsync();
                long.TryParse(FMP4Parser.GetSequenceOffsetFromM3U(m3u.Value), out long seq);

            Label_MsgCon:
                MessageStreamParser parser = new MessageStreamParser(ID, timenow);
                try
                {
                //msgstream
                    await parser.Connect();
                    bool isok = await parser.VerifyConnectionInit();
                    isok = await parser.ReceiveVerifyConnectionInit();
                    if (!isok)
                    {
                        ShowOutput(ID, $"HLS/MSG : Failed connect and verify MessageStream. Retrying ...", "GET", type: "Warn");
                        goto Label_MsgCon;
                    }
                    parser.StartHeartBeat();
                    parser.StartReceive();
                    while (isfinished)
                    {
                        //
                        bool isfin = true;
                        for (int i = 0; i < Form1.MultiThread; i++)
                        {
                            var tokenSource1 = new CancellationTokenSource(Form1.TimeOut * 10);
                            await Task.Run(async () =>
                            {
                                KeyValuePair<bool, KeyValuePair<ulong, byte[]>> h1 = await FMP4Parser.DownloadFMP4Sequence($"{serverhost}{seq + i}.m4s", ID, false);
                                if (h1.Key && !seqs.ContainsKey(h1.Value.Key))
                                {
                                    seqs.Add(h1.Value.Key, h1.Value.Value);
                                    Form1.FragmentsInMemoryC++;
                                }
                                isfin = h1.Key;
                            },tokenSource1.Token);
                        }
                        //
                        if (seqs.Count > Form1.MultiThread_WriteFileTrigger)
                        {
                            FMP4Parser.WriteFMP4Sequences(ID, ref seqs, fs);
                            await fs.FlushAsync();
                        }
                        //
                        if (isfin == false)
                        {
                            await Task.Delay(500);
                            isfin = await BiliLiveAPI.ParseJSON(ID);
                            if (seqs.Count > 0)
                            {
                                FMP4Parser.WriteFMP4Sequences(ID, ref seqs, fs);
                                await fs.FlushAsync();

                            }
                            if (!isfin) break;
                            else continue;
                        }
                        seq += Form1.MultiThread;
                        if (!Form1.activestreams.ContainsKey(ID))
                        {
                            if (seqs.Count > 0)
                            {
                                FMP4Parser.WriteFMP4Sequences(ID, ref seqs, fs);
                                await fs.FlushAsync();
                            }
                            break;
                        }
                    }
                    //finish
                    fs.Dispose();
                    parser.Dispose();
                    await Task.Delay(100);
                    ShowOutput(ID, $"HLS/REC : Finish record {Path.GetFileName(fs.Name)}", "-");
                    Form1.RecordingC--;
                    if (Form1.activestreams.ContainsKey(ID))
                    {
                        Form1.PublicForm1.StreamIDList.FindItemWithText(ID).SubItems[2].Text = "Running";
                        timer.Start();
                    }
                    else
                    {
                        Form1.PublicForm1.StreamIDList.FindItemWithText(ID).SubItems[2].Text = "Paused";
                    }
                    await Task.Delay(400);
                    await DownloadIt(ID);
                    return;
                }
                catch (Exception ex)
                {
                    if (seqs.Count > 0)
                    {
                        FMP4Parser.WriteFMP4Sequences(ID, ref seqs, fs);
                        await fs.FlushAsync();
                    }
                    fs.Dispose();
                    parser.Dispose();
                    //ShowOutput(ID, $"HLS/M : {ex.Message}", "-", type: "Error");
                    ShowOutput(ID, $"HLS/REC : Finish record {Path.GetFileName(fs.Name)} with Error : {ex.Message}", "Error");
                    Form1.RecordingC--;
                    if (Form1.activestreams.ContainsKey(ID))
                    {
                        Form1.PublicForm1.StreamIDList.FindItemWithText(ID).SubItems[2].Text = "Running";
                        timer.Start();
                    }
                    else
                    {
                        Form1.PublicForm1.StreamIDList.FindItemWithText(ID).SubItems[2].Text = "Paused";
                    }
                    return;
                }
            }
        }

        internal class MessageStreamParser
        {
            public MessageStreamParser(string ID, string timenow)
            {
                m_ID = ID;
                m_Timenow = timenow;
            }
            private string m_Timenow = string.Empty;
            public async Task Connect()
            {
                KeyValuePair<bool, KeyValuePair<string, string>> init = await BiliLiveAPI.ParseJSONAndGetMsgStmToken(m_ID);
                if (init.Key)
                {
                    m_Token = init.Value.Key;
                    m_URL = init.Value.Value;

                    string infofile = Path.Combine(Form1.workdir, Form1.SubDirName.Replace("{ROOM_ID}", m_ID).Replace("{FILE_EXT}", "mrec").Replace("{TIME_NOW}", m_Timenow));
                    Main.CreateFile(infofile);
                    m_LocalFile = new(infofile, FileMode.Append, FileAccess.Write);
                }
                else
                {
                    return;
                }
                m_clientWebSocket = new ClientWebSocket();
                Uri serverUri = new Uri(m_URL);
                m_clientWebSocket.Options.Proxy = null;
                await m_clientWebSocket.ConnectAsync(serverUri, CancellationToken.None);
                ShowOutput(m_ID, $"HLS/MSG : WebSocket Connected ! ", "-", OperatedFile: m_URL);
            }
            private ClientWebSocket? m_clientWebSocket;
            private string m_Token = string.Empty;
            private FileStream? m_LocalFile;
            private MemoryStream m_Stream = new();
            private string m_ID = string.Empty;
            private string m_URL = string.Empty;
            private byte[] m_bufferpool = new byte[524288];
            private UInt32 m_Sequence = 0;
            private System.Timers.Timer m_Timer = new System.Timers.Timer(Form1.MsgStreamInterval);//heartbeat
            public void Dispose()
            {
                m_Timer.Dispose();
                m_clientWebSocket?.Dispose();
                m_LocalFile?.Dispose();
                m_Stream?.Dispose();
            }
            public void WritePackets(ArraySegment<byte> b)
            {
                lock (locker)//加锁
                {
                    if (m_LocalFile == null) return;
                    if (m_LocalFile.Position == 0)
                    {
                        m_LocalFile.Write(rpacketh);
                        m_LocalFile.Write(Encoding.Default.GetBytes("BiliLiveRecorder " + Form1.Ver));
                        fs.SetLength(512);
                        fs.Position = 512;
                    }
                    m_LocalFile.Write(BitConverter.GetBytes(b.Count));
                    m_LocalFile.Write(b);
                    m_LocalFile.Flush();
                }
            }
            public void WritePackets(byte[] b)
            {
                lock (locker)//加锁
                {
                    if (m_LocalFile == null) return;
                    if (m_LocalFile.Position == 0)
                    {
                        m_LocalFile.Write(rpacketh);
                        m_LocalFile.Write(Encoding.Default.GetBytes("BiliLiveRecorder " + Form1.Ver));
                        fs.SetLength(512);
                        fs.Position = 512;
                    }
                    m_LocalFile.Write(BitConverter.GetBytes(b.Length));
                    m_LocalFile.Write(b);
                    m_LocalFile.Flush();
                }

            }
            public async Task<bool> VerifyConnectionInit()
            {
                string json = $"{{\"uid\":{0},\"roomid\":{m_ID},\"protover\":3,\"platform\":\"web\",\"type\":2,\"key\":\"{m_Token}\"}}";
                if (m_clientWebSocket == null)
                {
                    return false;
                }
                else
                {
                    if (m_clientWebSocket.State != WebSocketState.Open) return false;
                    else
                    {
                        MemoryStream stream = new MemoryStream();
                        stream.Position = 16;
                        await stream.WriteAsync(Encoding.UTF8.GetBytes(json));
                        stream.Position = 0;
                        await stream.WriteAsync(CreatePacketHeader((uint)stream.Length, PacketType.HeartBeat, CommandType.Verify));
                        ArraySegment<byte> bytesToSend = new ArraySegment<byte>(stream.ToArray());
                        await m_clientWebSocket.SendAsync(bytesToSend, WebSocketMessageType.Text, true, CancellationToken.None);
                        Downloader.MsgTotalUpload += (ulong)bytesToSend.Count;
                        WritePackets(bytesToSend);
                        ShowOutput(m_ID, $"HLS/MSG : WebSocket Connection Verifying...", "SEND", OperatedFile: m_URL, OperatedLength: bytesToSend.Count);
                        return true;
                    }
                }
            }
            public async Task<bool> ReceiveVerifyConnectionInit()
            {
                if (m_clientWebSocket == null) return false;
                WebSocketReceiveResult result = await m_clientWebSocket.ReceiveAsync(m_bufferpool, CancellationToken.None);
                MemoryStream stream = new MemoryStream();
                await stream.WriteAsync(m_bufferpool, 0, result.Count);
                stream.Position = 16;
                if (Encoding.Default.GetString(stream.ToArray()[16..]) != "{\"code\":0}")
                {
                    ShowOutput(m_ID, $"HLS/MSG : WebSocket Verify failed (Received {Encoding.Default.GetString(stream.ToArray()[16..])})", "RECV", OperatedFile: m_URL, OperatedLength: stream.Length);
                    return false;
                }
                WritePackets(stream.ToArray());
                Downloader.MsgTotalDownload += (ulong)result.Count;
                ShowOutput(m_ID, $"HLS/MSG : WebSocket Verified ! ", "RECV", OperatedFile: m_URL, OperatedLength: stream.Length);
                return true;
            }
            public void StartHeartBeat()
            {
                m_Timer.Elapsed += new ElapsedEventHandler(Heartbeat);
                m_Timer.AutoReset = true;
                m_Timer.Enabled = true;
            }
            public async Task RetryConnect()
            {
                if (m_clientWebSocket == null) return;
                Uri serverUri = new Uri(m_URL);
                await m_clientWebSocket.ConnectAsync(serverUri, CancellationToken.None);
                ShowOutput(m_ID, $"HLS/MSG : WebSocket Connected ! ", "-", OperatedFile: m_URL);
                await VerifyConnectionInit();
                await ReceiveVerifyConnectionInit();
            }

            private readonly byte[] heartbeat = new byte[] { 0x5B, 0x6F, 0x62, 0x6A, 0x65, 0x63, 0x74, 0x20, 0x4F, 0x62, 0x6A, 0x65, 0x63, 0x74, 0x5D };
            private async void Heartbeat(object source, ElapsedEventArgs e)
            {
            r:
                if (m_clientWebSocket == null) { return; }
                try
                {
                    if (m_clientWebSocket.State != WebSocketState.Open) await RetryConnect();
                }
                catch
                {
                    return;
                }
                try
                {
                    //
                    MemoryStream stream = new MemoryStream();
                    stream.Position = 16;
                    await stream.WriteAsync(heartbeat);
                    stream.Position = 0;
                    await stream.WriteAsync(CreatePacketHeader((uint)stream.Length, PacketType.HeartBeat, CommandType.HeartBeat));
                    ArraySegment<byte> bytesToSend = new ArraySegment<byte>(stream.ToArray());
                    await m_clientWebSocket.SendAsync(bytesToSend, WebSocketMessageType.Text, true, CancellationToken.None);
                    Downloader.MsgTotalUpload += (ulong)bytesToSend.Count;
                    WritePackets(bytesToSend);
                    ShowOutput(m_ID, $"HLS/MSG : Send HeartBeat Packet SEQ={m_Sequence}", "SEND", OperatedFile: m_URL, OperatedLength: bytesToSend.Count,StreamPosition:m_LocalFile.Position);
                    stream.Dispose();
                    //
                }
                catch (Exception ex)
                {
                    ShowOutput(m_ID, $"HLS/MSG : Lost HeartBeat Packet SEQ={m_Sequence}", "SEND", OperatedFile: m_URL, type: "Error");
                    await Task.Delay(1000);
                    goto r;
                }

            }
            public async void StartReceive()
            {
                while (true)
                {
                    try
                    {
                        if (m_clientWebSocket.State != WebSocketState.Open || m_clientWebSocket == null) break;
                        WebSocketReceiveResult result = await m_clientWebSocket.ReceiveAsync(m_bufferpool, CancellationToken.None);
                        //ShowOutput(m_ID, $"HLS/MSG : MessagePacketTest --> {Path.GetFileName(m_LocalFile?.Name)}", "RECV", OperatedFile: m_URL, StreamPosition: m_LocalFile.Position);
                        await m_Stream.WriteAsync(m_bufferpool, 0, result.Count);
                        m_Stream.Position = 0;
                        Downloader.MsgTotalDownload += (ulong)result.Count;
                        TotalRecvPacket++;
                        PacketParseAndOutput(m_Stream.ToArray());
                        m_Stream.SetLength(0);
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
            public byte[] CreatePacketHeader(UInt32 PackLen, PacketType Type, CommandType CmdType)
            {
                MemoryStream ms = new MemoryStream();
                ms.Write(BitConverter.GetBytes(PackLen).Reverse().ToArray());
                ms.Write(new byte[] { 0x00, 0x10 });
                ms.Write(BitConverter.GetBytes((UInt16)Type).Reverse().ToArray());
                ms.Write(BitConverter.GetBytes((UInt32)CmdType).Reverse().ToArray());
                ms.Write(BitConverter.GetBytes(m_Sequence).Reverse().ToArray());
                m_Sequence++;
                TotalSendPacket++;
                return ms.ToArray();
            }
            public void PacketParseAndOutput(byte[] packet)
            {
                WritePackets(packet);
                //
                try
                {
                    CommandType ctype = (CommandType)BitConverter.ToUInt32(packet[8..12].Reverse().ToArray());
                    switch (ctype)
                    {
                        case CommandType.HeartBeatReply:
                            ShowOutput(m_ID, $"HLS/MSG : HeartBeat --> Average Concurrent Users = {BitConverter.ToUInt32(packet[16..20].Reverse().ToArray())}", "RECV", OperatedFile: m_URL, OperatedLength: packet.Length, StreamPosition: m_LocalFile.Position);
                            break;
                        case CommandType.CommandDefault:
                            ShowOutput(m_ID, $"HLS/MSG : MessagePacket --> {Path.GetFileName(m_LocalFile?.Name)}", "RECV", OperatedFile: m_URL, OperatedLength: packet.Length, StreamPosition: m_LocalFile.Position);
                            break;
                        default:
                            throw new Exception("Unsupported CommandType");
                    }
                }
                catch (Exception e)
                {
                    ShowOutput(m_ID, $"HLS/MSG : Packet parse failed. ({e.Message})", "RECV", OperatedFile: m_URL, OperatedLength: packet.Length);
                }
            }
            public enum PacketType
            {
                Default = 0,
                HeartBeat = 1,
                Zlib = 2,
                MultiBrotli = 3
            }
            public enum CommandType
            {
                HeartBeat = 2,
                HeartBeatReply = 3,
                CommandDefault = 5,
                Verify = 7,
                VerifyReply = 8
            }
        }
    }

}
