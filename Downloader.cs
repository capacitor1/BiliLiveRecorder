using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;


namespace BiliLiveRecorder
{
    internal class Downloader
    {
        public static ulong TotalDownload = 0;
        public static ulong TotalUpload = 0;
        public static ulong MsgTotalDownload = 0;
        public static ulong MsgTotalUpload = 0;
        //public static HttpClient? client;
        public static ArrayPool<byte> shared = ArrayPool<byte>.Shared;
        public static async Task<int> Download(string fileUrl, Stream destinationStream,CancellationToken cts)
        {
            Retry400:
            HttpClient client = new HttpClient(Form1.httpClientHandler);
            //if (client == null) client = new HttpClient(Form1.httpClientHandler);
            int ret = -1;
            try
            {
                client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/134.0.0.0 Safari/537.36 Edg/134.0.0.0");
                HttpResponseMessage response = await client.GetAsync(fileUrl, HttpCompletionOption.ResponseHeadersRead);
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    //Debug.WriteLine(response.StatusCode.ToString());
                    if ((int)response.StatusCode == 400)
                    {
                        //client.Dispose();
                        //client = null;
                        //client = new HttpClient(Form1.httpClientHandler);
                        goto Retry400;
                    }
                    else
                    {
                        int r =  (int)response.StatusCode;
                        //client.Dispose();
                        return r;
                    }
                }
                using (Stream contentStream = await response.Content.ReadAsStreamAsync())
                {
                    var buffer = shared.Rent(524288);
                    int bytesRead;
                    while ((bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length, cts)) > 0)
                    {
                        await destinationStream.WriteAsync(buffer, 0, bytesRead);
                        TotalDownload += (ulong)bytesRead;
                    }

                    shared.Return(buffer);
                }
                ret = 0;
            }
            catch (OperationCanceledException)
            {
                ret = -2;
            }
            catch(Exception ex) when (ex is not OperationCanceledException)
            {
                //Debug.WriteLine(ex.Message);
                ret = -1;
            }
            //client.Dispose();
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
        public static KeyValuePair<string,string> CountBandWidth(bool istotal)//up/down
        {
            ulong beforeu = istotal ? TotalUpload : MsgTotalUpload;
            ulong befored = istotal ? TotalDownload : MsgTotalDownload;
            KeyValuePair<DateTime, ulong> t1 = new(DateTime.Now, befored);
            KeyValuePair<DateTime, ulong> t1u = new(DateTime.Now, beforeu);
            Thread.Sleep(1000);
            ulong afteru = istotal ? TotalUpload : MsgTotalUpload;
            ulong afterd = istotal ? TotalDownload : MsgTotalDownload;
            KeyValuePair<DateTime, ulong> t2 = new(DateTime.Now, afterd);
            KeyValuePair<DateTime, ulong> t2u = new(DateTime.Now, afteru);
            ulong s = t2.Value - t1.Value;
            ulong su = t2u.Value - t1u.Value;
            TimeSpan t = t2.Key - t1.Key;
            TimeSpan tu = t2u.Key - t1u.Key;
            ulong v = s / (ulong)t.Seconds;
            ulong vu = su / (ulong)tu.Seconds;
            return new (CountSize(vu) + "/s", CountSize(v) + "/s");
        }
    }

}
