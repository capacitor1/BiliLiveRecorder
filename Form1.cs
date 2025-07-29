using BiliLiveRecorder.Core;
using BiliLiveRecorder.Core.API;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Timers;
using Monitor = BiliLiveRecorder.Core.Monitor;

namespace BiliLiveRecorder
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            System.Timers.Timer timer1 = new System.Timers.Timer(1000);
            timer1.Elapsed += new ElapsedEventHandler(UpdateInfo);
            timer1.AutoReset = true;
            timer1.Enabled = true;
            System.Timers.Timer timer11 = new System.Timers.Timer(1800000);
            timer11.Elapsed += new ElapsedEventHandler(CheckNetwork);
            timer11.AutoReset = true;
            timer11.Enabled = true;
            CheckNetwork(timer11, null);
            Text += $" {Ver}";
        }
        public static Dictionary<string, Recorder> Recorders = new Dictionary<string, Recorder>();
        public static string Ver = "Ver 1.0.0+20250729";
        public async void CheckNetwork(object source, ElapsedEventArgs? e)
        {
            ((System.Timers.Timer)source).Enabled = false;
            try
            {
                HttpClient client = new HttpClient(Settings.httpClientHandler);
                client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/136.0.0.0 Safari/537.36");
                HttpResponseMessage response = await client.GetAsync($"https://api.live.bilibili.com/room/v1/Room/get_info?{await WBI.GetWBIByID("1")}", HttpCompletionOption.ResponseHeadersRead);
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    if ((int)response.StatusCode >= 400)
                    {
                        throw new Exception($"Failed : {response.StatusCode}");
                    }
                }
                Stream contentStream = await response.Content.ReadAsStreamAsync();
                int i = contentStream.ReadByte();
                throw new Exception($"Success : {(int)response.StatusCode} & FirstByte {i}");
            }
            catch (Exception ex)
            {
                string log = $"[NetworkCheck <{DateTime.Now}> 0]\r\n{ex.Message}\r\n\r\n";
                fs.Write(Encoding.Default.GetBytes(log));
                fs.Flush();
                if(!ex.Message.StartsWith("Success")) Log.AppendText(log);
            }
            finally
            {
                ((System.Timers.Timer)source).Enabled = true;
            }

        }

        public void OnLogUpdate(object? o, Log.LogUpdateEventArgs e)
        {
            string log = $"[{e.Level} <{e.Time}> {e.ID}]\r\n{e.Message}\r\n\r\n";
            fs.Write(Encoding.Default.GetBytes(log));
            fs.Flush();
            if (e.Level == Core.Log.LogLevel.Debug && showErrorOnlyToolStripMenuItem.Checked) return;
            Log.AppendText(log);


            if (e.Level == Core.Log.LogLevel.Messsage)
            {
                notifyIcon1.Visible = true; // 设置控件可见
                notifyIcon1.Icon = Icon;
                notifyIcon1.ShowBalloonTip(500, e.ID, e.Message, ToolTipIcon.Info);
                notifyIcon1.Visible = false; //
            }
        }
        public void OnStatusChanged1(object? o, Log.StatusChangedEventArgs e)
        {
            try
            {
                //Debug.WriteLine(e.ID);
                foreach (ListViewItem item in StreamIDList.Items)
                {
                    if (item.Text == e.ID) item.SubItems[2].Text = e.Status.ToString();
                }
            }
            catch
            {
                return;
            }
        }
        public void UpdateInfo(object source, ElapsedEventArgs e)
        {
            //
            UptimeL.Text = Monitor.GetUpTime();
            List<Recorder> rec = Recorders.Values.ToList().FindAll(x => x.IsRunning);
            ActiveMonitoringL.Text = $"{rec.Count} / {Recorders.Count}";
            OverrallBandwidthL.Text = Monitor.GetDownloadSpeed();
            HTTPREQ.Text = Monitor.GetHttpRequests();
            OverrallTrafficL.Text = $"{Downloader.CountSize(Downloader.TotalDownload)}";
        }
        private async void addStreamToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Input.Show(out string strID) == DialogResult.OK)
            {
                if (!ulong.TryParse(strID, out ulong _))
                {
                    MessageBox.Show("ID必须是数字。", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                Recorder r = new(strID, "BiliLiveRecorder_Files");
                r.LogUpdate += OnLogUpdate;
                r.StatusChanged += OnStatusChanged1;
                bool canadd = Recorders.TryAdd(strID, r);
                if (!canadd)
                {
                    MessageBox.Show("房间已存在。", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                //
                string info = await Core.API.GetData.GetRID(strID);
                if (info.Contains("$Err"))
                {
                    MessageBox.Show("获取房间信息失败。", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (strID != info) MessageBox.Show($"{strID} 的真实ID是 {info}", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                strID = info;
                StreamIDList.Items.Add(new ListViewItem([strID, await Core.API.GetData.GetTitle(strID), "Stop"]));
            }
            else
            {
                return;
            }

            //test
            //BiliLiveAPI.ParseJSONAndGetURL(strID);
            saveIDListToolStripMenuItem_Click(sender, e);
        }

        private void OnStatusChanged(object? sender, Log.StatusChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (StreamIDList.SelectedItems != null)
            {
                string id = StreamIDList.SelectedItems[0].Text;
                Recorder recorder = Recorders[id];
                if (recorder.IsRunning)
                {
                    MessageBox.Show($"{id} 正在录制。", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                recorder.Start();
                StreamIDList.SelectedItems[0].SubItems[2].Text = recorder.IsRunning ? "Running" : "Stop";
            }
        }

        private void IDListMenu_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void StreamIDList_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                IDListMenu.Show(StreamIDList, e.Location);
            }
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (StreamIDList.SelectedItems != null)
            {
                string id = StreamIDList.SelectedItems[0].Text;
                Recorder recorder = Recorders[id];
                recorder.Stop();
                StreamIDList.SelectedItems[0].SubItems[2].Text = recorder.IsRunning ? "Running" : "Stop";
            }
        }


        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (StreamIDList.SelectedItems != null)
            {
                DialogResult dr = MessageBox.Show("是否移除房间？", "Remove", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

                if (dr == DialogResult.OK)
                {
                    if (StreamIDList.SelectedItems[0].SubItems[2].Text == "Running" || StreamIDList.SelectedItems[0].SubItems[2].Text == "Recording")
                    {
                        MessageBox.Show("必须先停止录制。");
                        return;
                    }
                    string s = StreamIDList.SelectedItems[0].Text;
                    Recorder recorder = Recorders[s];
                    recorder.Stop();
                    Recorders.Remove(s);
                    StreamIDList.Items.Remove(StreamIDList.SelectedItems[0]);
                }
                saveIDListToolStripMenuItem_Click(sender, e);
            }

        }
        private async void saveIDListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<string> list = new List<string>();
            foreach (ListViewItem item in StreamIDList.Items)
            {
                list.Add(item.Text);
            }
            await WriteIDList(list.ToArray());
        }
        public static async Task WriteIDList(string[] l)
        {
            Directory.CreateDirectory("BiliLiveRecorder_Files/Internal");
            string f = "BiliLiveRecorder_Files/Internal/IDs.sidl";
            if (File.Exists(f)) File.Delete(f);
            await File.WriteAllLinesAsync(f, l);
        }
        public static async Task<string[]> ReadIDList()
        {
            string p = "BiliLiveRecorder_Files/Internal";
            if (!Directory.Exists(p)) Directory.CreateDirectory(p);
            string f = Path.Combine(p, "IDs.sidl");
            if (File.Exists(f))
            {
                string[] l = await File.ReadAllLinesAsync(f);
                return l;
            }
            return [];
        }
        public static FileStream fs = new(Path.GetTempPath() + $"{System.DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")}.log", FileMode.Append, FileAccess.Write,FileShare.Read);
        private async void Form1_Load(object sender, EventArgs e)
        {

            if (fs.Position == 0)
            {
                await fs.WriteAsync(Encoding.Default.GetBytes("BiliLiveRecorder Log File\r\n" + Form1.Ver + "\r\n"));
            }
            //

            string[] s = await ReadIDList();
            foreach (string s2 in s)
            {
                Recorder r = new(s2, "BiliLiveRecorder_Files");
                r.LogUpdate += OnLogUpdate;
                r.StatusChanged += OnStatusChanged1;
                if (Recorders.TryAdd(s2, r))
                {
                    StreamIDList.Items.Add(new ListViewItem([s2, string.Empty, "Stop"]));
                }
            }
            await Task.Delay(100);
            for (int i = 0; i < StreamIDList.Items.Count; i++)
            {
                string id = StreamIDList.Items[i].Text;
                string info = await Core.API.GetData.GetTitle(id);
                if (!info.Contains("$Err"))
                {
                    StreamIDList.Items[i].SubItems[1].Text = info;
                }
                else
                {
                    StreamIDList.Items[i].SubItems[1].Text = "Stop";
                }
            }
            //toolStripMenuItem1_Click(sender, e);
        }
        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("是否退出？", "Quit", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

            if (dr == DialogResult.OK)
            {
                e.Cancel = false;
            }
            else if (dr == DialogResult.Cancel)
            {
                e.Cancel = true;
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < StreamIDList.Items.Count; i++)
            {
                string id = StreamIDList.Items[i].Text;
                Recorder recorder = Recorders[id];
                if (recorder.IsRunning)
                {
                    continue;
                }
                recorder.Start();
                StreamIDList.Items[i].SubItems[2].Text = recorder.IsRunning ? "Running" : "Stop";
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < StreamIDList.Items.Count; i++)
            {
                string id = StreamIDList.Items[i].Text;
                Recorder recorder = Recorders[id];
                if (!recorder.IsRunning)
                {
                    continue;
                }
                recorder.Stop();
                StreamIDList.Items[i].SubItems[2].Text = recorder.IsRunning ? "Running" : "Stop";
            }
        }

        private void URLOpen_Click(object sender, EventArgs e)
        {
            if (StreamIDList.SelectedItems != null)
            {
                string id = StreamIDList.SelectedItems[0].Text;
                Process.Start(new ProcessStartInfo()
                {
                    FileName = $"https://live.bilibili.com/{id}",
                    UseShellExecute = true
                });
            }

        }

        private void openLocalFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (StreamIDList.SelectedItems != null)
            {
                string id = StreamIDList.SelectedItems[0].Text;
                string path = Path.Combine(Environment.CurrentDirectory, "BiliLiveRecorder_Files/Record", id);
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                Process.Start(new ProcessStartInfo()
                {
                    FileName = path,
                    UseShellExecute = true
                });
            }
        }

        private void Log_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string clip = Log.Text;
            try
            {
                Clipboard.SetDataObject(clip, true, 10, 200);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"剪贴板错误：\r\n{ex.Message}\r\n[E:01]", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void clearToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Log.Text = string.Empty;
        }

        private void 查看日志ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo()
            {
                FileName = fs.Name,
                UseShellExecute = true
            });
        }
    }
}
