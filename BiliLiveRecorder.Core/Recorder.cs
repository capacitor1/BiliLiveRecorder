using BiliLiveRecorder.Core.API;
using BiliLiveRecorder.Core.FMP4Processor;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using static BiliLiveRecorder.Core.Log;

namespace BiliLiveRecorder.Core
{
    public class Recorder
    {
        private readonly string _ID;
        private readonly string _Dir;
        private System.Timers.Timer timer;
        private bool _isRunning = false;
        public Recorder(string iD, string baseDir)//basedir "BiliLiveRecorder_Files/"
        {
            _ID = iD;
            timer = new System.Timers.Timer(Settings.Interval);
            timer.Enabled = false;
            timer.Elapsed += new ElapsedEventHandler(_1);
            timer.AutoReset = true;
            _Dir = Path.Combine(baseDir,"Record",_ID);
            Directory.CreateDirectory(_Dir);
        }
        private async void _1(object source, ElapsedEventArgs e)
        {
            await _2();
        }
        public event EventHandler<LogUpdateEventArgs>? LogUpdate;
        public event EventHandler<TitleUpdateEventArgs>? TitleUpdate;
        public event EventHandler<StatusChangedEventArgs>? StatusChanged;
        private async Task _2()
        {
            if (!_isRunning) return;
            var tokenSource = new CancellationTokenSource(Settings.TimeOut);
            timer.Stop();
        //

        RetryGm3u:
            bool canstart = await GetData.IsLiving(_ID);
            if (!canstart)
            {
                timer.Start();
                return;
            }

            //start
            string timenow = System.DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_ffff");
            LogUpdate?.Invoke(this, new(){
                Level = LogLevel.Messsage,
                Time = DateTime.Now,
                ID = _ID,
                Message = $"开始录制 {timenow}"
            });
            StatusChanged?.Invoke(this, new()
            {
                ID = _ID,
                Status = Status.Recording
            });
            //get info
            string info = await GetData.GetM3U8URL(_ID);
            if (info.Contains("$Err"))
            {
                LogUpdate?.Invoke(this, new()
                {
                    Level = LogLevel.Error,
                    Time = DateTime.Now,
                    ID = _ID,
                    Message = $"不能获取直播元数据。（{info}）5秒后重试。"
                });
                if (!_isRunning)
                {
                    return;
                }
                await Task.Delay(5000);
                goto RetryGm3u;
            }
            //get m3u8
            string[] m3u = await GetData.ReadM3U8URL(info);
            if (m3u.Length <= 0)
            {
                LogUpdate?.Invoke(this, new()
                {
                    Level = LogLevel.Error,
                    Time = DateTime.Now,
                    ID = _ID,
                    Message = $"不能获取m3u8内容（{m3u[0]}）。5秒后重试。"
                });
                if (!_isRunning)
                {
                    return;
                }
                await Task.Delay(5000);
                goto RetryGm3u;
            }
            //prepare
            string DIR = Path.Combine(_Dir,timenow);
            Directory.CreateDirectory(DIR);

            await File.WriteAllLinesAsync(Path.Combine(DIR, "_Info.md"), await GetData.GetInfoText(_ID));
            await File.WriteAllTextAsync(Path.Combine(DIR, "_Index.m3u8.txt"), info);
            string _tmp_title = await GetData.GetTitle(_ID);
            TitleUpdate?.Invoke(this, new()
            {
                ID = _ID,
                Title = _tmp_title
            });
            string C = await GetData.GetCover(_ID);
            if (C.StartsWith("http"))
            {
                await Downloader.DownloadFile(C, Path.Combine(DIR, "_Cover.jpg"), tokenSource.Token);
            }

            //create file f = m3u8file
            FileStream m3u8r = new(Path.Combine(DIR, "_Index.m3u8"), FileMode.Append, FileAccess.Write);
            StreamWriter m3u8 = new(m3u8r);
            //

            string serverhost = info.Replace("index.m3u8", "");
            //write m3u8 header
            m3u8.WriteLine("""
                    #EXTM3U
                    #EXT-X-VERSION:7
                    #EXT-X-START:TIME-OFFSET=0
                    #EXT-X-TARGETDURATION:0
                    """);
            //

            //rec
            LogUpdate?.Invoke(this, new()
            {
                Level = LogLevel.Debug,
                Time = DateTime.Now,
                ID = _ID,
                Message = $"元数据下载完毕！"
            });
            string[] thism3u = [];
            long[] todownload = [];
            long total = 0, count = 0, totalthis = 0, countthis = 0, range = 0;
            DateTime s = DateTime.Now;
            try
            {
                long last = 0;
                while (await GetData.IsLiving(_ID) && _isRunning)
                {
                    //
                    totalthis = countthis = 0;
                    s = DateTime.Now;
                    //
                    thism3u = await GetData.ReadM3U8URL(info);
                    if (thism3u.Length <= 1)
                    {
                        throw new Exception("Invalid M3U8.");
                    }
                    todownload = M3U8.GetPieces(thism3u);
                    if (todownload.First() - last > 1 && last != 0)
                    {
                        List<long> rep = [];
                        for (long i = last + 1; i < todownload.First(); i++)
                        {
                            await Task.Run(async () =>//1
                            {
                                string fn = Path.Combine(DIR, $"{i}.m4s");
                                if (!File.Exists(fn))
                                {
                                    int r = 0;
                                Retry:
                                    int ret = await Downloader.DownloadFile($"{serverhost}{i}.m4s", fn);
                                    if (ret == 0)
                                    {
                                        LogUpdate?.Invoke(this, new()
                                        {
                                            Level = LogLevel.Debug,
                                            Time = DateTime.Now,
                                            ID = _ID,
                                            Message = $"Received {i}.m4s[{new FileInfo(fn).Length}]"
                                        });
                                        totalthis += new FileInfo(fn).Length;
                                        countthis++;
                                    }
                                    else if (ret == 404)
                                    {
                                        throw new Exception("Record expired.");
                                    }
                                    else if (r < Endpoint.Retry)
                                    {
                                        
                                        LogUpdate?.Invoke(this, new()
                                        {
                                            Level = LogLevel.Warn,
                                            Time = DateTime.Now,
                                            ID = _ID,
                                            Message = $"Received unexpected {todownload[i]}.m4s[{new FileInfo(fn).Length}] , retrying {r}/{Endpoint.Retry}"
                                        });
                                        r++;
                                        goto Retry;
                                    }
                                }

                            });
                            rep.Add(i);
                        }
                        await M3U8.WriteRepairM3U([.. rep], m3u8);
                    }
                    //
                    for (int i = 0; i < todownload.Length; i++)
                    {
                        await Task.Run(async () =>//1
                        {
                            int r = 0;
                            string fn = Path.Combine(DIR, $"{todownload[i]}.m4s.unverified");
                            if (!File.Exists(fn) && !File.Exists(Path.Combine(DIR, $"{todownload[i]}.m4s")))
                            {
                            Retry:
                                int ret = await Downloader.DownloadFile($"{serverhost}{todownload[i]}.m4s", fn);
                                if (ret == 0)
                                {
                                    LogUpdate?.Invoke(this, new()
                                    {
                                        Level = LogLevel.Debug,
                                        Time = DateTime.Now,
                                        ID = _ID,
                                        Message = $"Received {todownload[i]}.m4s[{new FileInfo(fn).Length}]"
                                    });
                                    totalthis += new FileInfo(fn).Length;
                                    countthis++;
                                }
                                else if (ret == 404)
                                {
                                    throw new Exception("Record expired.");
                                }
                                else if (r < Endpoint.Retry)
                                {
                                    LogUpdate?.Invoke(this, new()
                                    {
                                        Level = LogLevel.Warn,
                                        Time = DateTime.Now,
                                        ID = _ID,
                                        Message = $"Received unexpected {todownload[i]}.m4s[{new FileInfo(fn).Length}] , retrying {r}/{Endpoint.Retry}"
                                    });
                                    r++;
                                    goto Retry;
                                }
                            }

                        });
                    }
                    last = todownload.Last();
                    count += countthis;
                    total += totalthis;
                    range++;
                    //
                    await M3U8.WriteM3U(thism3u, m3u8, DIR, _ID, serverhost,LogUpdate);
                    //
                    LogUpdate?.Invoke(this, new()
                    {
                        Level = LogLevel.Debug,
                        Time = DateTime.Now,
                        ID = _ID,
                        Message = $"片段 {range} 录制完毕，用时 {DateTime.Now - s}\r\n分片：{last}.m4s\r\n字节数：{totalthis}（{countthis} 个分片，{Downloader.CountSize((ulong)totalthis)}）\r\n总录制：{total}（{count} 个分片，{Downloader.CountSize((ulong)total)}）"
                    });
                    if (range % 100 == 0)
                    {
                        LogUpdate?.Invoke(this, new()
                        {
                            Level = LogLevel.Info,
                            Time = DateTime.Now,
                            ID = _ID,
                            Message = $"总录制：{total}（{count} 个分片，{Downloader.CountSize((ulong)total)}）"
                        });
                    }
                    if (totalthis < 1)
                    {
                        await Task.Delay(1000);
                    }
                }
                //finish
                m3u8.WriteLine("#EXT-X-ENDLIST");
                m3u8.Dispose();
                await Task.Delay(100);
                
                LogUpdate?.Invoke(this, new()
                {
                    Level = LogLevel.Messsage,
                    Time = DateTime.Now,
                    ID = _ID,
                    Message = $"{timenow} 录制结束。\r\n总录制：{count} 个分片，{Downloader.CountSize((ulong)total)}"
                });
                StatusChanged?.Invoke(this, new()
                {
                    ID = _ID,
                    Status = _isRunning ? Status.Running : Status.Stop
                });
                if (_isRunning)
                {
                    timer.Start();
                }
                return;
            }
            catch (Exception ex)
            {
                m3u8.WriteLine("#EXT-X-ENDLIST");
                m3u8.Dispose();
                LogUpdate?.Invoke(this, new()
                {
                    Level = LogLevel.Messsage,
                    Time = DateTime.Now,
                    ID = _ID,
                    Message = $"{timenow} 录制结束，但有错误。\r\n总录制：{count} 个分片，{Downloader.CountSize((ulong)total)}"
                });
                LogUpdate?.Invoke(this, new()
                {
                    Level = LogLevel.Warn,
                    Time = DateTime.Now,
                    ID = _ID,
                    Message = $"{timenow} 录制结束，但有错误：\r\n{ex.Message}"
                });
                StatusChanged?.Invoke(this, new()
                {
                    ID = _ID,
                    Status = _isRunning ? Status.Running : Status.Stop
                });
                //Debug.WriteLine(ex.StackTrace);
                if (_isRunning)
                {
                    timer.Start();
                }
                LogUpdate?.Invoke(this, new()
                {
                    Level = LogLevel.Debug,
                    Time = DateTime.Now,
                    ID = _ID,
                    Message = $"检测到录制错误，正在重试..."
                });
                await _2();
                return;
            }
        
        }

        public void Start()
        {
            _isRunning = timer.Enabled = true;
            StatusChanged?.Invoke(this, new()
            {
                ID = _ID,
                Status = Status.Running
            });
            Task.Run(_2);
        }
        public void Stop()
        {
            _isRunning = false;
            StatusChanged?.Invoke(this, new()
            {
                ID = _ID,
                Status = Status.Stop
            });
        }
        public bool IsRunning { get { return _isRunning; } }
    }
}
