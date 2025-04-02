using Microsoft.VisualBasic.Logging;
using System;
using System.Diagnostics;
using System.Text;
using System.Text.Json.Nodes;
using System.Timers;
using System.Windows.Forms;
using static BiliLiveRecorder.LiveRecMain;

namespace BiliLiveRecorder
{
    public partial class Form1 : Form
    {
        

        private static object UIlock = new object();
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            PublicForm1 = this;
            Uptime = DateTime.Now;
            Directory.CreateDirectory(Path.Combine(Form1.workdir, "Internal/Log/"));
            timer.Elapsed += new ElapsedEventHandler(UpdateLog);
            timer.AutoReset = true;
            timer.Enabled = true;

            System.Timers.Timer timer1 = new System.Timers.Timer(UpdateInfoInterval);//UpdateLog monitoring timer
            timer1.Elapsed += new ElapsedEventHandler(UpdateInfo);
            timer1.AutoReset = true;
            timer1.Enabled = true;
        }
        public static Form1 PublicForm1;
        public System.Timers.Timer timer = new System.Timers.Timer(UpdateInterval);//UpdateLog monitoring timer
        public static DateTime Uptime;
        public static double UpdateInterval = 100;
        public static double UpdateInfoInterval = 1000;
        public static double MsgStreamInterval = 30000;
        public static double Interval = 60000;//60000ms
        public static int MultiThread = 5;
        public static int TimeOut = 10000;
        public static int AutoClear = 2048;
        public static int MultiThread_WriteFileTrigger = 15;
        public static List<string> RID = new List<string>();
        public static List<Output> OutputLog = new List<Output>();
        public static Dictionary<string, System.Timers.Timer> activestreams = new Dictionary<string, System.Timers.Timer>();
        public static string Ver = "Ver1.1.7";
        public static string tempstr = string.Empty;
        public static ulong FragmentsInMemoryC = 0;
        public static int RecordingC = 0;
        public static HttpClientHandler httpClientHandler = new HttpClientHandler();

        public static string workdir = String.Empty + "BiliLiveRecorder_Files/";//work
        public static string SubDirName = "Record/{ROOM_ID}/{TIME_NOW}.{FILE_EXT}";//MUST use '/'
        public static string replaceoptions = "_bluray/_prohevc/_2500/_1500";//MUST use '/'
        public void UpdateLog(object source, ElapsedEventArgs e)
        {
            timer.Stop();
            try
            {
                lock (UIlock)
                {
                    for (int i = 0; i < OutputLog.Count; i++)
                    {
                        Output output = OutputLog[i];
                        WriteOutput(output);/*
                        if (output.Message.StartsWith("HLS/FMP4 : Received ") && MainOutput.Items[^1].SubItems[3].Text.StartsWith("HLS/FMP4 : Received "))
                        {
                            MainOutput.Items[^1].SubItems[0].Text = DateTimeOffset.FromUnixTimeSeconds(output.Time).LocalDateTime.ToString();
                            MainOutput.Items[^1].SubItems[3].Text += $",{output.Message.Replace("HLS/FMP4 : Received ", "")}";
                            MainOutput.Items[^1].SubItems[5].Text += $",{output.OperatedFile}";
                            MainOutput.Items[^1].SubItems[6].Text = output.StreamPosition.ToString();
                            MainOutput.Items[^1].SubItems[7].Text = (long.Parse(MainOutput.Items[^1].SubItems[7].Text) + output.OperatedLength).ToString();
                        }
                        else
                        {*/
                        MainOutput.Items.Add(new ListViewItem(new string[] { DateTimeOffset.FromUnixTimeSeconds(output.Time).LocalDateTime.ToString(),
            output.ID,output.Type,output.Message,output.Operation,output.OperatedFile,output.StreamPosition.ToString(),output.OperatedLength.ToString()}));
                        //}
                        OutputLog.Remove(output);
                        while (MainOutput.Items.Count >= AutoClear)
                        {
                            MainOutput.Items.RemoveAt(0);
                        }
                        if (MainOutput.SelectedItems.Count == 0 && MainOutput.Items.Count > 0) MainOutput.Items[^1].EnsureVisible();
                    }
                    Application.DoEvents();
                }
            }
            catch(Exception ex)
            {
                MainOutput.Items.Add(new ListViewItem(new string[] { DateTime.Now.ToString(),"UI","Error",ex.Message,"-","-","0","0"}));
            }
            finally
            {
                timer.Start();
            }
        }
        public void UpdateInfo(object source, ElapsedEventArgs e)
        {
            KeyValuePair<string, string> k = Downloader.CountBandWidth(true);
            KeyValuePair<string, string> kmsg = Downloader.CountBandWidth(false);
            TimeSpan ts = DateTime.Now - Uptime;
            //
            UptimeL.Text = ts.ToString();
            ActiveMonitoringL.Text = $"{RecordingC} / {activestreams.Count}";
            GIntervalL.Text = $"U{UpdateInterval} / I{UpdateInfoInterval} / M{Interval}";
            OverrallBandwidthL.Text = $"↑ {k.Key} / ↓ {k.Value}";
            PktBWL.Text = $"↑ {kmsg.Key} / ↓ {kmsg.Value}";
            SOutputLinesL.Text = $"Cached {OutputLog.Count} / Shown {MainOutput.Items.Count} / Max {OutputLog.Capacity},{AutoClear}";
            FragmentsInMemoryL.Text = $"{FragmentsInMemoryC} / {MultiThread_WriteFileTrigger * RecordingC}";
            TPktL.Text = $"Recv {TotalRecvPacket} / Send {TotalSendPacket}";

            OverrallTrafficL.Text = $"↑ {Downloader.CountSize(Downloader.TotalUpload)} / ↓ {Downloader.CountSize(Downloader.TotalDownload)}";
            PktTrafficL.Text = $"↑ {Downloader.CountSize(Downloader.MsgTotalUpload)} / ↓ {Downloader.CountSize(Downloader.MsgTotalDownload)}";
        }
        private async void addStreamToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Input.Show(out string strID) == DialogResult.OK)
            {
                if (!ulong.TryParse(strID, out ulong id) == true)
                {
                    MessageBox.Show("ID must be a number.\r\n\r\n[W:01]", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            else
            {
                return;
            }
            if (RID.Contains(strID))
            {
                MessageBox.Show("Stream ID already exists.\r\n\r\n[W:02]", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            Dictionary<string, string> info = await BiliLiveAPI.ParseJSONAndGetInfo(strID,false);
            if (info.ContainsKey("$Err"))
            {
                MessageBox.Show("Failed get stream info.\r\n\r\n[E:00]", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if(strID != info["RID"]) MessageBox.Show($"{strID} actually is {info["RID"]}\r\n\r\n[T:03]", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            strID = info["RID"];
            StreamIDList.Items.Add(new ListViewItem(new string[] { strID, info["Title"],"Paused"}));
            RID.Add(strID);

            //test
            //BiliLiveAPI.ParseJSONAndGetURL(strID);
            saveIDListToolStripMenuItem_Click(sender, e);
        }

        private async void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (StreamIDList.SelectedItems != null)
            {
                string id = StreamIDList.SelectedItems[0].Text;
                System.Timers.Timer timer = new System.Timers.Timer(Interval);//Stream downloader timer
                if (!activestreams.ContainsKey(id))
                {
                    activestreams.Add(id, timer);
                    StreamIDList.SelectedItems[0].SubItems[2].Text = "Running";
                    timer.Elapsed += new ElapsedEventHandler((s, e) => Main.DownloadIt(s, e, id));
                    timer.AutoReset = true;
                    timer.Enabled = true;
                    await Main.DownloadIt(id);
                }
                else
                {
                    MessageBox.Show($"Stream {id} has started.\r\n\r\n[T:01]", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    StreamIDList.SelectedItems[0].SubItems[2].Text = "Running";
                }
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
                if (activestreams.ContainsKey(id))
                {
                    var timer = activestreams[id];
                    timer.Stop();
                    timer.Dispose();
                    activestreams.Remove(id);
                    if (StreamIDList.SelectedItems[0].SubItems[2].Text == "Recording") StreamIDList.SelectedItems[0].SubItems[2].Text = "Pausing";
                    else StreamIDList.SelectedItems[0].SubItems[2].Text = "Paused";
                }
            }
        }


        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Do you want to remove?", "Remove", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

            if (dr == DialogResult.OK)
            {
                stopToolStripMenuItem_Click(sender, e);
                StreamIDList.Items.Remove(StreamIDList.SelectedItems[0]);
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MainOutput.SelectedItems != null)
            {
                ListViewItem a = MainOutput.SelectedItems[0];
                string clip = $"Time:{a.SubItems[0].Text} ID:{a.SubItems[1].Text} Type:{a.SubItems[2].Text} Msg:{a.SubItems[3].Text} Op:{a.SubItems[4].Text} OpFile:{a.SubItems[5].Text} StreamPos:{a.SubItems[6].Text} OpLen:{a.SubItems[7].Text}";

                try
                {
                    Clipboard.SetDataObject(clip, true, 10, 200);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"ClipBoardError:\r\n{ex.Message}\r\n[E:01]", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void MainOutput_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                MainMenu.Show(MainOutput, e.Location);//鼠标右键按下弹出菜单
            }
        }

        private void copyMessageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MainOutput.SelectedItems != null)
            {
                ListViewItem a = MainOutput.SelectedItems[0];
                try
                {
                    Clipboard.SetDataObject(a.SubItems[3].Text, true, 10, 200);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"ClipBoardError:\r\n{ex.Message}\r\n[E:01]", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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

        private async void Form1_Load(object sender, EventArgs e)
        {
            
            httpClientHandler.Proxy = null;
            httpClientHandler.UseProxy = false;
            if (fs.Position == 0)
            {
                //await fs.WriteAsync(rlogh);
                await fs.WriteAsync(Encoding.Default.GetBytes("BiliLiveRecorder Log File\r\n"));
                await fs.WriteAsync(Encoding.Default.GetBytes("Version " + Form1.Ver + "\r\n"));
            }
            //
            SetSettings(await ReadSettings());
            string[] s = await ReadIDList();
            foreach (string s2 in s)
            {
                if (!RID.Contains(s2))
                {
                    StreamIDList.Items.Add(new ListViewItem(new string[] {s2, string.Empty, "Paused" }));
                    RID.Add(s2);
                }
            }
            await Task.Delay(100);
            for (int i = 0; i < StreamIDList.Items.Count; i++)
            {
                string id = StreamIDList.Items[i].Text;
                Dictionary<string, string> info = await BiliLiveAPI.ParseJSONAndGetInfo(id, false);
                if (!info.ContainsKey("$Err"))
                {
                    StreamIDList.Items[i].SubItems[1].Text = info["Title"];
                }
            }
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainOutput.Items.Clear();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings settings = new Settings();
            settings.Show();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("Do you want to quit?", "Quit", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

            if (dr == DialogResult.OK)
            {
                e.Cancel = false;
            }
            else if (dr == DialogResult.Cancel)
            {
                e.Cancel = true;
            }
        }

        private async void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < StreamIDList.Items.Count; i++)
            {
                string id = StreamIDList.Items[i].Text;
                System.Timers.Timer timer = new System.Timers.Timer(100);//Stream downloader timer
                if (!activestreams.ContainsKey(id))
                {
                    activestreams.Add(id, timer);
                    StreamIDList.Items[i].SubItems[2].Text = "Running";
                    timer.Elapsed += new ElapsedEventHandler((s, e) => Main.DownloadIt(s, e, id));
                    timer.AutoReset = true;
                    timer.Enabled = true;
                    await Task.Delay(100);
                    timer.Interval = Interval;
                }
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < StreamIDList.Items.Count; i++)
            {
                string id = StreamIDList.Items[i].Text;
                if (activestreams.ContainsKey(id))
                {
                    var timer = activestreams[id];
                    timer.Stop();
                    timer.Dispose();
                    activestreams.Remove(id);
                    if (StreamIDList.Items[i].SubItems[2].Text == "Recording") StreamIDList.Items[i].SubItems[2].Text = "Pausing";
                    else StreamIDList.Items[i].SubItems[2].Text = "Paused";
                }
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
    }
}
