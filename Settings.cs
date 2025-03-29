using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Windows.Forms;
using static BiliLiveRecorder.LiveRecMain;

namespace BiliLiveRecorder
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            //show settings
            LiveRecMain.Settings s = GetSettings();
            string settingsjson = JsonSerializer.Serialize(s);
            JsonObject data = JsonNode.Parse(settingsjson)!.AsObject();
            foreach (var x in data)
            {
                S.Items.Add(new ListViewItem(new string[] { x.Key, $"{x.Value}"}));
            }
        }

        private void S_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip1.Show(S, e.Location);//鼠标右键按下弹出菜单
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (S.SelectedItems != null)
            {
                string rawsettings = S.SelectedItems[0].SubItems[1].Text;
                string t = S.SelectedItems[0].Text;
                Form1.tempstr = rawsettings;
                if (Input.Show(out string strID) == DialogResult.OK)
                {
                    S.SelectedItems[0].SubItems[1].Text = strID;
                }
                Form1.tempstr = String.Empty;
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            LiveRecMain.Settings s = new LiveRecMain.Settings();
            foreach (ListViewItem x in S.Items)
            {
                if (x.SubItems[0].Text == "Monitoring_Interval") s.Monitoring_Interval = (double)x.SubItems[1].Text.TryParse("double");
                if (x.SubItems[0].Text == "BILI_API_MaxRetry") s.BILI_API_MaxRetry = (int)x.SubItems[1].Text.TryParse("int");
                if (x.SubItems[0].Text == "Download_Threads") s.Download_Threads = (int)x.SubItems[1].Text.TryParse("int");
                if (x.SubItems[0].Text == "Download_WriteFileTrigger") s.Download_WriteFileTrigger = (int)x.SubItems[1].Text.TryParse("int");
                if (x.SubItems[0].Text == "OutputList_AutoClear") s.OutputList_AutoClear = (int)x.SubItems[1].Text.TryParse("int");
                if (x.SubItems[0].Text == "TimeOut") s.TimeOut = (int)x.SubItems[1].Text.TryParse("int");

                if (x.SubItems[0].Text == "WorkingDirectory") s.WorkingDirectory = x.SubItems[1].Text;
                if (x.SubItems[0].Text == "WorkingDirectory_SubDirectory") s.WorkingDirectory_SubDirectory = x.SubItems[1].Text;
                if (x.SubItems[0].Text == "BILI_LiveStreamAPI") s.BILI_LiveStreamAPI = x.SubItems[1].Text;
                if (x.SubItems[0].Text == "BILI_LiveStreamInfoAPI") s.BILI_LiveStreamInfoAPI = x.SubItems[1].Text;
                if (x.SubItems[0].Text == "BILI_MessageStreamAPI") s.BILI_MessageStreamAPI = x.SubItems[1].Text;
                if (x.SubItems[0].Text == "BILI_APIAutoReplace_Keywords") s.BILI_APIAutoReplace_Keywords = x.SubItems[1].Text;
            }
            await WriteSettings(s);
            SetSettings(s);
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(String.Join("\r\n", LiveRecMain.Help) + "\r\n\r\n[T:02]", "Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
