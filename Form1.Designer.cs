namespace BiliLiveRecorder
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            menuStrip1 = new MenuStrip();
            streamToolStripMenuItem = new ToolStripMenuItem();
            addStreamToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            toolStripMenuItem1 = new ToolStripMenuItem();
            toolStripMenuItem2 = new ToolStripMenuItem();
            logToolStripMenuItem = new ToolStripMenuItem();
            copyToolStripMenuItem = new ToolStripMenuItem();
            clearToolStripMenuItem1 = new ToolStripMenuItem();
            showErrorOnlyToolStripMenuItem = new ToolStripMenuItem();
            tableLayoutPanel1 = new TableLayoutPanel();
            StreamIDList = new ListView();
            columnHeader9 = new ColumnHeader();
            columnHeader10 = new ColumnHeader();
            columnHeader11 = new ColumnHeader();
            groupBox1 = new GroupBox();
            tableLayoutPanel2 = new TableLayoutPanel();
            groupBox4 = new GroupBox();
            HTTPREQ = new Label();
            label11 = new Label();
            OverrallTrafficL = new Label();
            label10 = new Label();
            OverrallBandwidthL = new Label();
            label5 = new Label();
            groupBox2 = new GroupBox();
            UptimeL = new Label();
            label1 = new Label();
            label3 = new Label();
            ActiveMonitoringL = new Label();
            Log = new TextBox();
            IDListMenu = new ContextMenuStrip(components);
            startToolStripMenuItem = new ToolStripMenuItem();
            stopToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator3 = new ToolStripSeparator();
            removeToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            URLOpen = new ToolStripMenuItem();
            openLocalFileToolStripMenuItem = new ToolStripMenuItem();
            notifyIcon1 = new NotifyIcon(components);
            menuStrip1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            groupBox1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            groupBox4.SuspendLayout();
            groupBox2.SuspendLayout();
            IDListMenu.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { streamToolStripMenuItem, logToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(927, 25);
            menuStrip1.TabIndex = 1;
            menuStrip1.Text = "menuStrip1";
            // 
            // streamToolStripMenuItem
            // 
            streamToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { addStreamToolStripMenuItem, toolStripSeparator1, toolStripMenuItem1, toolStripMenuItem2 });
            streamToolStripMenuItem.Name = "streamToolStripMenuItem";
            streamToolStripMenuItem.Size = new Size(44, 21);
            streamToolStripMenuItem.Text = "房间";
            // 
            // addStreamToolStripMenuItem
            // 
            addStreamToolStripMenuItem.Name = "addStreamToolStripMenuItem";
            addStreamToolStripMenuItem.Size = new Size(124, 22);
            addStreamToolStripMenuItem.Text = "添加...";
            addStreamToolStripMenuItem.Click += addStreamToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(121, 6);
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(124, 22);
            toolStripMenuItem1.Text = "启动录制";
            toolStripMenuItem1.Click += toolStripMenuItem1_Click;
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new Size(124, 22);
            toolStripMenuItem2.Text = "停止录制";
            toolStripMenuItem2.Click += toolStripMenuItem2_Click;
            // 
            // logToolStripMenuItem
            // 
            logToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { copyToolStripMenuItem, clearToolStripMenuItem1, showErrorOnlyToolStripMenuItem });
            logToolStripMenuItem.Name = "logToolStripMenuItem";
            logToolStripMenuItem.Size = new Size(44, 21);
            logToolStripMenuItem.Text = "日志";
            // 
            // copyToolStripMenuItem
            // 
            copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            copyToolStripMenuItem.Size = new Size(180, 22);
            copyToolStripMenuItem.Text = "复制";
            copyToolStripMenuItem.Click += copyToolStripMenuItem_Click;
            // 
            // clearToolStripMenuItem1
            // 
            clearToolStripMenuItem1.Name = "clearToolStripMenuItem1";
            clearToolStripMenuItem1.Size = new Size(180, 22);
            clearToolStripMenuItem1.Text = "清除";
            clearToolStripMenuItem1.Click += clearToolStripMenuItem1_Click;
            // 
            // showErrorOnlyToolStripMenuItem
            // 
            showErrorOnlyToolStripMenuItem.Checked = true;
            showErrorOnlyToolStripMenuItem.CheckOnClick = true;
            showErrorOnlyToolStripMenuItem.CheckState = CheckState.Checked;
            showErrorOnlyToolStripMenuItem.Name = "showErrorOnlyToolStripMenuItem";
            showErrorOnlyToolStripMenuItem.Size = new Size(180, 22);
            showErrorOnlyToolStripMenuItem.Text = "只显示错误";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 230F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(StreamIDList, 0, 0);
            tableLayoutPanel1.Controls.Add(groupBox1, 1, 1);
            tableLayoutPanel1.Controls.Add(Log, 1, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 25);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 110F));
            tableLayoutPanel1.Size = new Size(927, 416);
            tableLayoutPanel1.TabIndex = 3;
            // 
            // StreamIDList
            // 
            StreamIDList.Columns.AddRange(new ColumnHeader[] { columnHeader9, columnHeader10, columnHeader11 });
            StreamIDList.Dock = DockStyle.Fill;
            StreamIDList.FullRowSelect = true;
            StreamIDList.GridLines = true;
            StreamIDList.Location = new Point(3, 3);
            StreamIDList.MultiSelect = false;
            StreamIDList.Name = "StreamIDList";
            tableLayoutPanel1.SetRowSpan(StreamIDList, 2);
            StreamIDList.Size = new Size(224, 410);
            StreamIDList.TabIndex = 0;
            StreamIDList.UseCompatibleStateImageBehavior = false;
            StreamIDList.View = View.Details;
            StreamIDList.MouseClick += StreamIDList_MouseClick;
            // 
            // columnHeader9
            // 
            columnHeader9.Text = "ID";
            // 
            // columnHeader10
            // 
            columnHeader10.Text = "标题";
            columnHeader10.Width = 90;
            // 
            // columnHeader11
            // 
            columnHeader11.Text = "状态";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(tableLayoutPanel2);
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.Location = new Point(233, 309);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(691, 104);
            groupBox1.TabIndex = 2;
            groupBox1.TabStop = false;
            groupBox1.Text = "信息";
            groupBox1.Enter += groupBox1_Enter;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel2.Controls.Add(groupBox4, 0, 0);
            tableLayoutPanel2.Controls.Add(groupBox2, 0, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(3, 19);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Size = new Size(685, 82);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(HTTPREQ);
            groupBox4.Controls.Add(label11);
            groupBox4.Controls.Add(OverrallTrafficL);
            groupBox4.Controls.Add(label10);
            groupBox4.Controls.Add(OverrallBandwidthL);
            groupBox4.Controls.Add(label5);
            groupBox4.Dock = DockStyle.Fill;
            groupBox4.Location = new Point(345, 3);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(337, 76);
            groupBox4.TabIndex = 2;
            groupBox4.TabStop = false;
            groupBox4.Text = "录制";
            // 
            // HTTPREQ
            // 
            HTTPREQ.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            HTTPREQ.Location = new Point(69, 53);
            HTTPREQ.Name = "HTTPREQ";
            HTTPREQ.Size = new Size(262, 17);
            HTTPREQ.TabIndex = 15;
            HTTPREQ.Text = "0 OK / 0 Send";
            HTTPREQ.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label11
            // 
            label11.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            label11.AutoSize = true;
            label11.Location = new Point(6, 53);
            label11.Name = "label11";
            label11.Size = new Size(57, 17);
            label11.TabIndex = 14;
            label11.Text = "Http请求";
            // 
            // OverrallTrafficL
            // 
            OverrallTrafficL.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            OverrallTrafficL.Location = new Point(68, 36);
            OverrallTrafficL.Name = "OverrallTrafficL";
            OverrallTrafficL.Size = new Size(263, 17);
            OverrallTrafficL.TabIndex = 13;
            OverrallTrafficL.Text = "0 B";
            OverrallTrafficL.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label10
            // 
            label10.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            label10.AutoSize = true;
            label10.Location = new Point(6, 36);
            label10.Name = "label10";
            label10.Size = new Size(44, 17);
            label10.TabIndex = 12;
            label10.Text = "总下载";
            // 
            // OverrallBandwidthL
            // 
            OverrallBandwidthL.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            OverrallBandwidthL.Location = new Point(68, 19);
            OverrallBandwidthL.Name = "OverrallBandwidthL";
            OverrallBandwidthL.Size = new Size(263, 17);
            OverrallBandwidthL.TabIndex = 9;
            OverrallBandwidthL.Text = "0 B/s";
            OverrallBandwidthL.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            label5.AutoSize = true;
            label5.Location = new Point(6, 19);
            label5.Name = "label5";
            label5.Size = new Size(56, 17);
            label5.TabIndex = 8;
            label5.Text = "全局网速";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(UptimeL);
            groupBox2.Controls.Add(label1);
            groupBox2.Controls.Add(label3);
            groupBox2.Controls.Add(ActiveMonitoringL);
            groupBox2.Dock = DockStyle.Fill;
            groupBox2.Location = new Point(3, 3);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(336, 76);
            groupBox2.TabIndex = 0;
            groupBox2.TabStop = false;
            groupBox2.Text = "进程";
            // 
            // UptimeL
            // 
            UptimeL.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            UptimeL.Location = new Point(68, 19);
            UptimeL.Name = "UptimeL";
            UptimeL.Size = new Size(262, 17);
            UptimeL.TabIndex = 1;
            UptimeL.Text = "00:00:00.0000000";
            UptimeL.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Location = new Point(6, 19);
            label1.Name = "label1";
            label1.Size = new Size(56, 17);
            label1.TabIndex = 0;
            label1.Text = "运行时长";
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            label3.AutoSize = true;
            label3.Location = new Point(6, 36);
            label3.Name = "label3";
            label3.Size = new Size(56, 17);
            label3.TabIndex = 6;
            label3.Text = "正在录制";
            // 
            // ActiveMonitoringL
            // 
            ActiveMonitoringL.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            ActiveMonitoringL.Location = new Point(68, 36);
            ActiveMonitoringL.Name = "ActiveMonitoringL";
            ActiveMonitoringL.Size = new Size(262, 17);
            ActiveMonitoringL.TabIndex = 7;
            ActiveMonitoringL.Text = "0 / 0";
            ActiveMonitoringL.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Log
            // 
            Log.Dock = DockStyle.Fill;
            Log.Font = new Font("Fira Code", 8.999999F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Log.Location = new Point(233, 3);
            Log.MaxLength = int.MaxValue;
            Log.Multiline = true;
            Log.Name = "Log";
            Log.ReadOnly = true;
            Log.ScrollBars = ScrollBars.Vertical;
            Log.Size = new Size(691, 300);
            Log.TabIndex = 3;
            Log.WordWrap = false;
            Log.MouseDown += Log_MouseDown;
            // 
            // IDListMenu
            // 
            IDListMenu.Items.AddRange(new ToolStripItem[] { startToolStripMenuItem, stopToolStripMenuItem, toolStripSeparator3, removeToolStripMenuItem, toolStripSeparator2, URLOpen, openLocalFileToolStripMenuItem });
            IDListMenu.Name = "IDListMenu";
            IDListMenu.Size = new Size(182, 126);
            IDListMenu.MouseClick += IDListMenu_MouseClick;
            // 
            // startToolStripMenuItem
            // 
            startToolStripMenuItem.Name = "startToolStripMenuItem";
            startToolStripMenuItem.Size = new Size(181, 22);
            startToolStripMenuItem.Text = "启动录制";
            startToolStripMenuItem.Click += startToolStripMenuItem_Click;
            // 
            // stopToolStripMenuItem
            // 
            stopToolStripMenuItem.Name = "stopToolStripMenuItem";
            stopToolStripMenuItem.Size = new Size(181, 22);
            stopToolStripMenuItem.Text = "停止录制";
            stopToolStripMenuItem.Click += stopToolStripMenuItem_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Size(178, 6);
            // 
            // removeToolStripMenuItem
            // 
            removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            removeToolStripMenuItem.Size = new Size(181, 22);
            removeToolStripMenuItem.Text = "移除";
            removeToolStripMenuItem.Click += removeToolStripMenuItem_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(178, 6);
            // 
            // URLOpen
            // 
            URLOpen.Name = "URLOpen";
            URLOpen.Size = new Size(181, 22);
            URLOpen.Text = "在浏览器中打开...";
            URLOpen.Click += URLOpen_Click;
            // 
            // openLocalFileToolStripMenuItem
            // 
            openLocalFileToolStripMenuItem.Name = "openLocalFileToolStripMenuItem";
            openLocalFileToolStripMenuItem.Size = new Size(181, 22);
            openLocalFileToolStripMenuItem.Text = "在本地文件夹打开...";
            openLocalFileToolStripMenuItem.Click += openLocalFileToolStripMenuItem_Click;
            // 
            // notifyIcon1
            // 
            notifyIcon1.Text = "notifyIcon1";
            notifyIcon1.Visible = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(927, 441);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(menuStrip1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            Name = "Form1";
            Text = "Bilibili Live Recorder";
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            groupBox1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            IDListMenu.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private MenuStrip menuStrip1;
        private ToolStripMenuItem streamToolStripMenuItem;
        private ToolStripMenuItem addStreamToolStripMenuItem;
        private TableLayoutPanel tableLayoutPanel1;
        private GroupBox groupBox1;
        private ContextMenuStrip IDListMenu;
        private ToolStripMenuItem startToolStripMenuItem;
        private ToolStripMenuItem stopToolStripMenuItem;
        private ToolStripMenuItem removeToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private TableLayoutPanel tableLayoutPanel2;
        private GroupBox groupBox2;
        private Label UptimeL;
        private Label label1;
        private GroupBox groupBox4;
        private Label OverrallBandwidthL;
        private Label label5;
        private Label ActiveMonitoringL;
        private Label label3;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripMenuItem toolStripMenuItem2;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem URLOpen;
        private Label OverrallTrafficL;
        private Label label10;
        public class ListViewBuff : System.Windows.Forms.ListView
        {
            public ListViewBuff()
            {
                this.SetStyle(
                ControlStyles.DoubleBuffer |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.AllPaintingInWmPaint, true);
                UpdateStyles();
            }
        }

        private ColumnHeader columnHeader9;
        private ColumnHeader columnHeader10;
        private ColumnHeader columnHeader11;
        public ListView StreamIDList;
        private Label HTTPREQ;
        private Label label11;
        private ToolStripMenuItem openLocalFileToolStripMenuItem;
        public TextBox Log;
        private ToolStripMenuItem logToolStripMenuItem;
        private ToolStripMenuItem copyToolStripMenuItem;
        private ToolStripMenuItem clearToolStripMenuItem1;
        public ToolStripMenuItem showErrorOnlyToolStripMenuItem;
        public NotifyIcon notifyIcon1;
    }
}
