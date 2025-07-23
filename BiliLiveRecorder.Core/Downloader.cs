using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BiliLiveRecorder.Core.API;

namespace BiliLiveRecorder.Core
{
    internal class Downloader
    {
        public static ulong TotalDownload = 0;
        public static ulong TotalRequest = 0;
        public static ulong TotalRequestOK = 0;

        public static ArrayPool<byte> shared = ArrayPool<byte>.Shared;
        public static async Task<int> DownloadFile(string fileUrl, string path, CancellationToken cts)
        {
            int i = -1, r = 0;
            while (i != 0 && i != 404 && r < Endpoint.Retry)
            {
                using (FileStream fs = new(path, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    i = await Download(fileUrl, fs, cts);
                }
            }
            return i;
        }
        public static async Task<int> DownloadFile(string fileUrl, string path)
        {
            int i = -1, r = 0;
            while (i != 0 && i != 404 && r < Endpoint.Retry)
            {
                using (FileStream fs = new(path, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    i = await Download(fileUrl, fs);
                }
            }
            return i;
        }
        public static async Task<int> Download(string fileUrl, Stream destinationStream, CancellationToken? cts = null)
        {
        Retry:
            HttpClient client = new HttpClient(Settings.httpClientHandler);
            //if (client == null) client = new HttpClient(Form1.httpClientHandler);
            int ret = -1;
            try
            {
                client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/136.0.0.0 Safari/537.36");
                HttpResponseMessage response = await client.GetAsync(fileUrl, HttpCompletionOption.ResponseHeadersRead);

                TotalRequest++;
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    //Debug.WriteLine(response.StatusCode.ToString());
                    if ((int)response.StatusCode != 404 && (int)response.StatusCode >= 400)
                    {
                        //client.Dispose();
                        //client = null;
                        //client = new HttpClient(Form1.httpClientHandler);
                        goto Retry;
                    }
                    else
                    {
                        //LiveRecMain.ShowOutput($"[Error:Downloader] '{fileUrl}' returned 404");
                        int r = (int)response.StatusCode;
                        //client.Dispose();
                        return r;
                    }
                }
                if (response.Content.Headers.ContentLength.HasValue)
                {
                    long fileSize = response.Content.Headers.ContentLength.Value;
                    destinationStream.SetLength(fileSize);
                }
                destinationStream.Seek(0, SeekOrigin.Begin);
                using (Stream contentStream = await response.Content.ReadAsStreamAsync())
                {
                    var buffer = shared.Rent(524288);
                    int bytesRead;
                    if (cts == null)
                    {

                        while ((bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                        {
                            await destinationStream.WriteAsync(buffer, 0, bytesRead);
                            TotalDownload += (ulong)bytesRead;
                        }
                    }
                    else
                    {

                        while ((bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length, (CancellationToken)cts)) > 0)
                        {
                            await destinationStream.WriteAsync(buffer, 0, bytesRead);
                            TotalDownload += (ulong)bytesRead;
                        }
                    }
                    shared.Return(buffer);
                }
                if (destinationStream.Position < destinationStream.Length)
                {
                    ret = -1;
                }
                else
                {
                    ret = 0;
                }
            }
            catch (OperationCanceledException)
            {
                ret = -2;
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                //Debug.WriteLine(ex.Message);
                ret = -1;
            }
            await destinationStream.FlushAsync();
            TotalRequestOK++;
            return ret;
        }
        public static string CountSize(ulong Size)
        {
            string m_strSize = "";
            ulong FactSize = 0;
            FactSize = Size;
            if (FactSize < 1024.00)
                m_strSize = FactSize.ToString("F0") + " B";
            else if (FactSize >= 1024.00 && FactSize < 1048576)
                m_strSize = (FactSize / 1024.00).ToString("F2") + " KB";
            else if (FactSize >= 1048576 && FactSize < 1073741824)
                m_strSize = (FactSize / 1024.00 / 1024.00).ToString("F3") + " MB";
            else if (FactSize >= 1073741824)
                m_strSize = (FactSize / 1024.00 / 1024.00 / 1024.00).ToString("F5") + " GB";
            return m_strSize;
        }
        public static string CountBandWidth()//up/down
        {
            ulong befored = TotalDownload;
            KeyValuePair<DateTime, ulong> t1 = new(DateTime.Now, befored);
            Thread.Sleep(1000);
            ulong afterd = TotalDownload;
            KeyValuePair<DateTime, ulong> t2 = new(DateTime.Now, afterd);
            ulong s = t2.Value - t1.Value;
            TimeSpan t = t2.Key - t1.Key;
            ulong v = s / (ulong)t.Seconds;
            return CountSize(v) + "/s";
        }
    }
}
