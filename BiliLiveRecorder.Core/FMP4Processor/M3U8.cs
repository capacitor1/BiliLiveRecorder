using BiliLiveRecorder.Core;
using BiliLiveRecorder.Core.API;
using static BiliLiveRecorder.Core.Log;

namespace BiliLiveRecorder.Core.FMP4Processor
{
    internal class M3U8
    {
        public static async Task WriteRepairM3U(long[] longs, StreamWriter sw)
        {
            foreach (long long2 in longs)
            {
                await sw.WriteLineAsync($"#EXT-BILI-AUX:000000|R|00000|00000000");
                await sw.WriteLineAsync($"#EXTINF:1.00,00000|00000000");
                await sw.WriteLineAsync($"{long2}.m4s");
                await sw.FlushAsync();
            }
        }
        public static long[] GetPieces(string[] m3u8)
        {
            List<long> pieces = new List<long>();
            foreach (string item in m3u8)
            {
                if (!item.Contains("#") && item != string.Empty)
                {
                    pieces.Add(long.Parse(item.Replace(".m4s", "")));
                }
            }
            return [.. pieces];
        }
        public static string GetMapLine(string[] m3u8)
        {
            string s = string.Empty;
            foreach (string item in m3u8)
            {
                if (item.StartsWith("#EXT-X-MAP"))
                {
                    s = item;
                }
            }
            return s;
        }
        public static async Task WriteM3U(string[] m3u, StreamWriter sw, string dir, string ID, string serverhost, EventHandler<LogUpdateEventArgs>? eventHandler)
        {
            List<string> purem3u = [];
            foreach (string s in m3u)
            {
                if (s.StartsWith("#EXTM3U")
                    || s.StartsWith("#EXT-X-VERSION")
                    || s.StartsWith("#EXT-X-START")
                    || s.StartsWith("#EXT-X-MEDIA-SEQUENCE")
                    || s.StartsWith("#EXT-X-TARGETDURATION")
                    || s.StartsWith("#EXT-X-DISCONTINUITY"))
                {
                    continue;
                }
                else if (s.StartsWith("#EXT-X-MAP"))
                {
                    string map = s.Replace("\"", "").Replace("#EXT-X-MAP:URI=", "");
                    if (!File.Exists(Path.Combine(dir, $"{map}")))
                    {
                        await sw.WriteLineAsync("#EXT-X-DISCONTINUITY");
                        await sw.WriteLineAsync(s);
                        await sw.FlushAsync();
                        await Downloader.DownloadFile($"{serverhost}{map}", Path.Combine(dir, $"{map}"));
                        eventHandler?.Invoke(new(), new()
                        {
                            Level = LogLevel.Debug,
                            Time = DateTime.Now,
                            ID = ID,
                            Message = $"检测到分辨率切换：{map}"
                        });
                    }
                }
                //
                else if (s.StartsWith("#EXT-BILI-AUX") || s.StartsWith("#EXTINF"))
                {
                    purem3u.Add(s);
                }
                //
                else if (!s.Contains("#") && s.Contains(".m4s"))
                {
                    purem3u.Add(s);
                }
            }
            for (int i = 0; i < purem3u.Count && long.TryParse(purem3u[i + 2].Replace(".m4s", ""), out long s); i += 3)
            {
                string fraw = Path.Combine(dir, $"{s}.m4s.unverified");
                if (File.Exists(fraw))
                {
                    await sw.WriteLineAsync(purem3u[i]);
                    await sw.WriteLineAsync(purem3u[i + 1]);
                    await sw.WriteLineAsync(purem3u[i + 2]);
                    await sw.FlushAsync();
                    int r = 0;
                    string crc32 = purem3u[i].Split('|').Last();
                    string text = await Crc32.GetFileCRC32(fraw);
                    while (!text.PadLeft(8, '0').Equals(crc32.PadLeft(8, '0'), StringComparison.CurrentCultureIgnoreCase) && r < Endpoint.Retry)
                    {
                        eventHandler?.Invoke(new(), new()
                        {
                            Level = LogLevel.Warn,
                            Time = DateTime.Now,
                            ID = ID,
                            Message = $"Crc32 failed on {s}.m4s , expected {crc32} but get {text}"
                        });
                        await File.WriteAllTextAsync(Path.Combine(dir, $"{s}.m4s.crc32failed"), "Expected " + crc32 + " but get " + text);
                        File.Delete(fraw);
                        await Downloader.DownloadFile($"{serverhost}{s}.m4s", fraw);
                        text = await Crc32.GetFileCRC32(fraw);
                        r++;
                    }
                    File.Move(fraw, Path.Combine(dir, $"{s}.m4s"));
                }
            }
            //
            return;
        }
    }
}
