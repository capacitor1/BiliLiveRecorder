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
            saveIDListToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator4 = new ToolStripSeparator();
            toolStripMenuItem1 = new ToolStripMenuItem();
            toolStripMenuItem2 = new ToolStripMenuItem();
            settingsToolStripMenuItem = new ToolStripMenuItem();
            tableLayoutPanel1 = new TableLayoutPanel();
            StreamIDList = new ListView();
            columnHeader9 = new ColumnHeader();
            columnHeader10 = new ColumnHeader();
            columnHeader11 = new ColumnHeader();
            MainOutput = new ListViewBuff();
            columnHeader1 = new ColumnHeader();
            columnHeader2 = new ColumnHeader();
            columnHeader3 = new ColumnHeader();
            columnHeader4 = new ColumnHeader();
            columnHeader5 = new ColumnHeader();
            columnHeader6 = new ColumnHeader();
            columnHeader7 = new ColumnHeader();
            columnHeader8 = new ColumnHeader();
            groupBox1 = new GroupBox();
            tableLayoutPanel2 = new TableLayoutPanel();
            groupBox4 = new GroupBox();
            OverrallTrafficL = new Label();
            label10 = new Label();
            FragmentsInMemoryL = new Label();
            label7 = new Label();
            OverrallBandwidthL = new Label();
            label5 = new Label();
            ActiveMonitoringL = new Label();
            label3 = new Label();
            groupBox2 = new GroupBox();
            SOutputLinesL = new Label();
            label6 = new Label();
            GIntervalL = new Label();
            label4 = new Label();
            UptimeL = new Label();
            label1 = new Label();
            groupBox3 = new GroupBox();
            PktTrafficL = new Label();
            label9 = new Label();
            PktBWL = new Label();
            label12 = new Label();
            TPktL = new Label();
            label8 = new Label();
            IDListMenu = new ContextMenuStrip(components);
            startToolStripMenuItem = new ToolStripMenuItem();
            stopToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator3 = new ToolStripSeparator();
            removeToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            URLOpen = new ToolStripMenuItem();
            MainMenu = new ContextMenuStrip(components);
            copyMessageToolStripMenuItem = new ToolStripMenuItem();
            copyToolStripMenuItem = new ToolStripMenuItem();
            clearToolStripMenuItem = new ToolStripMenuItem();
            menuStrip1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            groupBox1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            groupBox4.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            IDListMenu.SuspendLayout();
            MainMenu.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { streamToolStripMenuItem, settingsToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1546, 25);
            menuStrip1.TabIndex = 1;
            menuStrip1.Text = "menuStrip1";
            // 
            // streamToolStripMenuItem
            // 
            streamToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { addStreamToolStripMenuItem, toolStripSeparator1, saveIDListToolStripMenuItem, toolStripSeparator4, toolStripMenuItem1, toolStripMenuItem2 });
            streamToolStripMenuItem.Name = "streamToolStripMenuItem";
            streamToolStripMenuItem.Size = new Size(61, 21);
            streamToolStripMenuItem.Text = "Stream";
            // 
            // addStreamToolStripMenuItem
            // 
            addStreamToolStripMenuItem.Name = "addStreamToolStripMenuItem";
            addStreamToolStripMenuItem.Size = new Size(145, 22);
            addStreamToolStripMenuItem.Text = "Add Stream";
            addStreamToolStripMenuItem.Click += addStreamToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(142, 6);
            // 
            // saveIDListToolStripMenuItem
            // 
            saveIDListToolStripMenuItem.Name = "saveIDListToolStripMenuItem";
            saveIDListToolStripMenuItem.Size = new Size(145, 22);
            saveIDListToolStripMenuItem.Text = "Save ID List";
            saveIDListToolStripMenuItem.Click += saveIDListToolStripMenuItem_Click;
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new Size(142, 6);
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(145, 22);
            toolStripMenuItem1.Text = "Start All";
            toolStripMenuItem1.Click += toolStripMenuItem1_Click;
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new Size(145, 22);
            toolStripMenuItem2.Text = "Stop All";
            toolStripMenuItem2.Click += toolStripMenuItem2_Click;
            // 
            // settingsToolStripMenuItem
            // 
            settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            settingsToolStripMenuItem.Size = new Size(66, 21);
            settingsToolStripMenuItem.Text = "Settings";
            settingsToolStripMenuItem.Click += settingsToolStripMenuItem_Click;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 230F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(StreamIDList, 0, 0);
            tableLayoutPanel1.Controls.Add(MainOutput, 1, 0);
            tableLayoutPanel1.Controls.Add(groupBox1, 1, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 25);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 150F));
            tableLayoutPanel1.Size = new Size(1546, 656);
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
            StreamIDList.Size = new Size(224, 650);
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
            columnHeader10.Text = "Title";
            columnHeader10.Width = 90;
            // 
            // columnHeader11
            // 
            columnHeader11.Text = "Status";
            // 
            // MainOutput
            // 
            MainOutput.Columns.AddRange(new ColumnHeader[] { columnHeader1, columnHeader2, columnHeader3, columnHeader4, columnHeader5, columnHeader6, columnHeader7, columnHeader8 });
            MainOutput.Dock = DockStyle.Fill;
            MainOutput.FullRowSelect = true;
            MainOutput.GridLines = true;
            MainOutput.Location = new Point(233, 3);
            MainOutput.MultiSelect = false;
            MainOutput.Name = "MainOutput";
            MainOutput.Size = new Size(1310, 500);
            MainOutput.TabIndex = 1;
            MainOutput.UseCompatibleStateImageBehavior = false;
            MainOutput.View = View.Details;
            MainOutput.MouseClick += MainOutput_MouseClick;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "Time";
            columnHeader1.Width = 120;
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "Stream ID";
            columnHeader2.Width = 100;
            // 
            // columnHeader3
            // 
            columnHeader3.Text = "Type";
            // 
            // columnHeader4
            // 
            columnHeader4.Text = "Message";
            columnHeader4.Width = 500;
            // 
            // columnHeader5
            // 
            columnHeader5.Text = "Operation";
            columnHeader5.Width = 80;
            // 
            // columnHeader6
            // 
            columnHeader6.Text = "OperatedFile";
            columnHeader6.Width = 230;
            // 
            // columnHeader7
            // 
            columnHeader7.Text = "StreamPosition";
            columnHeader7.Width = 100;
            // 
            // columnHeader8
            // 
            columnHeader8.Text = "OperatedLength";
            columnHeader8.Width = 100;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(tableLayoutPanel2);
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.Location = new Point(233, 509);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(1310, 144);
            groupBox1.TabIndex = 2;
            groupBox1.TabStop = false;
            groupBox1.Text = "Info";
            groupBox1.Enter += groupBox1_Enter;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 3;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel2.Controls.Add(groupBox4, 0, 0);
            tableLayoutPanel2.Controls.Add(groupBox2, 0, 0);
            tableLayoutPanel2.Controls.Add(groupBox3, 1, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(3, 19);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Size = new Size(1304, 122);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(OverrallTrafficL);
            groupBox4.Controls.Add(label10);
            groupBox4.Controls.Add(FragmentsInMemoryL);
            groupBox4.Controls.Add(label7);
            groupBox4.Controls.Add(OverrallBandwidthL);
            groupBox4.Controls.Add(label5);
            groupBox4.Controls.Add(ActiveMonitoringL);
            groupBox4.Controls.Add(label3);
            groupBox4.Dock = DockStyle.Fill;
            groupBox4.Location = new Point(437, 3);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(428, 116);
            groupBox4.TabIndex = 2;
            groupBox4.TabStop = false;
            groupBox4.Text = "RecordTask";
            // 
            // OverrallTrafficL
            // 
            OverrallTrafficL.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            OverrallTrafficL.Location = new Point(113, 53);
            OverrallTrafficL.Name = "OverrallTrafficL";
            OverrallTrafficL.Size = new Size(309, 17);
            OverrallTrafficL.TabIndex = 13;
            OverrallTrafficL.Text = "↑ 0 B / ↓ 0 B";
            OverrallTrafficL.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label10
            // 
            label10.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            label10.AutoSize = true;
            label10.Location = new Point(6, 53);
            label10.Name = "label10";
            label10.Size = new Size(101, 17);
            label10.TabIndex = 12;
            label10.Text = "OverrallTraffic : ";
            // 
            // FragmentsInMemoryL
            // 
            FragmentsInMemoryL.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            FragmentsInMemoryL.Location = new Point(152, 70);
            FragmentsInMemoryL.Name = "FragmentsInMemoryL";
            FragmentsInMemoryL.Size = new Size(270, 17);
            FragmentsInMemoryL.TabIndex = 11;
            FragmentsInMemoryL.Text = "0 / 0";
            FragmentsInMemoryL.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            label7.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            label7.AutoSize = true;
            label7.Location = new Point(6, 70);
            label7.Name = "label7";
            label7.Size = new Size(140, 17);
            label7.TabIndex = 10;
            label7.Text = "FragmentsInMemory : ";
            // 
            // OverrallBandwidthL
            // 
            OverrallBandwidthL.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            OverrallBandwidthL.Location = new Point(138, 36);
            OverrallBandwidthL.Name = "OverrallBandwidthL";
            OverrallBandwidthL.Size = new Size(284, 17);
            OverrallBandwidthL.TabIndex = 9;
            OverrallBandwidthL.Text = "↑ 0 B/s / ↓ 0 B/s";
            OverrallBandwidthL.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            label5.AutoSize = true;
            label5.Location = new Point(6, 36);
            label5.Name = "label5";
            label5.Size = new Size(126, 17);
            label5.TabIndex = 8;
            label5.Text = "OverrallBandwidth : ";
            // 
            // ActiveMonitoringL
            // 
            ActiveMonitoringL.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            ActiveMonitoringL.Location = new Point(130, 19);
            ActiveMonitoringL.Name = "ActiveMonitoringL";
            ActiveMonitoringL.Size = new Size(292, 17);
            ActiveMonitoringL.TabIndex = 7;
            ActiveMonitoringL.Text = "0 / 0";
            ActiveMonitoringL.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            label3.AutoSize = true;
            label3.Location = new Point(6, 19);
            label3.Name = "label3";
            label3.Size = new Size(118, 17);
            label3.TabIndex = 6;
            label3.Text = "ActiveMonitoring : ";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(SOutputLinesL);
            groupBox2.Controls.Add(label6);
            groupBox2.Controls.Add(GIntervalL);
            groupBox2.Controls.Add(label4);
            groupBox2.Controls.Add(UptimeL);
            groupBox2.Controls.Add(label1);
            groupBox2.Dock = DockStyle.Fill;
            groupBox2.Location = new Point(3, 3);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(428, 116);
            groupBox2.TabIndex = 0;
            groupBox2.TabStop = false;
            groupBox2.Text = "Application";
            // 
            // SOutputLinesL
            // 
            SOutputLinesL.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            SOutputLinesL.Location = new Point(100, 53);
            SOutputLinesL.Name = "SOutputLinesL";
            SOutputLinesL.Size = new Size(322, 17);
            SOutputLinesL.TabIndex = 9;
            SOutputLinesL.Text = "Cached 0 / Shown 0 / Max 0,0";
            SOutputLinesL.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            label6.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            label6.AutoSize = true;
            label6.Location = new Point(6, 53);
            label6.Name = "label6";
            label6.Size = new Size(88, 17);
            label6.TabIndex = 8;
            label6.Text = "OutputLines : ";
            // 
            // GIntervalL
            // 
            GIntervalL.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            GIntervalL.Location = new Point(112, 36);
            GIntervalL.Name = "GIntervalL";
            GIntervalL.Size = new Size(310, 17);
            GIntervalL.TabIndex = 7;
            GIntervalL.Text = "U0 / I0 / M0";
            GIntervalL.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            label4.AutoSize = true;
            label4.Location = new Point(6, 36);
            label4.Name = "label4";
            label4.Size = new Size(100, 17);
            label4.TabIndex = 6;
            label4.Text = "GlobalInterval : ";
            // 
            // UptimeL
            // 
            UptimeL.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            UptimeL.Location = new Point(73, 19);
            UptimeL.Name = "UptimeL";
            UptimeL.Size = new Size(349, 17);
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
            label1.Size = new Size(61, 17);
            label1.TabIndex = 0;
            label1.Text = "Uptime : ";
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(PktTrafficL);
            groupBox3.Controls.Add(label9);
            groupBox3.Controls.Add(PktBWL);
            groupBox3.Controls.Add(label12);
            groupBox3.Controls.Add(TPktL);
            groupBox3.Controls.Add(label8);
            groupBox3.Dock = DockStyle.Fill;
            groupBox3.Location = new Point(871, 3);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(430, 116);
            groupBox3.TabIndex = 1;
            groupBox3.TabStop = false;
            groupBox3.Text = "MessageStream";
            // 
            // PktTrafficL
            // 
            PktTrafficL.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            PktTrafficL.Location = new Point(105, 53);
            PktTrafficL.Name = "PktTrafficL";
            PktTrafficL.Size = new Size(317, 17);
            PktTrafficL.TabIndex = 17;
            PktTrafficL.Text = "↑ 0 B / ↓ 0 B";
            PktTrafficL.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            label9.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            label9.AutoSize = true;
            label9.Location = new Point(6, 53);
            label9.Name = "label9";
            label9.Size = new Size(93, 17);
            label9.TabIndex = 16;
            label9.Text = "PacketTraffic : ";
            // 
            // PktBWL
            // 
            PktBWL.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            PktBWL.Location = new Point(138, 36);
            PktBWL.Name = "PktBWL";
            PktBWL.Size = new Size(284, 17);
            PktBWL.TabIndex = 15;
            PktBWL.Text = "↑ 0 B/s / ↓ 0 B/s";
            PktBWL.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label12
            // 
            label12.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            label12.AutoSize = true;
            label12.Location = new Point(6, 36);
            label12.Name = "label12";
            label12.Size = new Size(118, 17);
            label12.TabIndex = 14;
            label12.Text = "PacketBandwidth : ";
            // 
            // TPktL
            // 
            TPktL.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            TPktL.Location = new Point(127, 19);
            TPktL.Name = "TPktL";
            TPktL.Size = new Size(295, 17);
            TPktL.TabIndex = 13;
            TPktL.Text = "Recv 0 / Send 0";
            TPktL.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            label8.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            label8.AutoSize = true;
            label8.Location = new Point(6, 19);
            label8.Name = "label8";
            label8.Size = new Size(122, 17);
            label8.TabIndex = 12;
            label8.Text = "TotalPacketTraffic : ";
            // 
            // IDListMenu
            // 
            IDListMenu.Items.AddRange(new ToolStripItem[] { startToolStripMenuItem, stopToolStripMenuItem, toolStripSeparator3, removeToolStripMenuItem, toolStripSeparator2, URLOpen });
            IDListMenu.Name = "IDListMenu";
            IDListMenu.Size = new Size(136, 104);
            IDListMenu.MouseClick += IDListMenu_MouseClick;
            // 
            // startToolStripMenuItem
            // 
            startToolStripMenuItem.Name = "startToolStripMenuItem";
            startToolStripMenuItem.Size = new Size(135, 22);
            startToolStripMenuItem.Text = "Start";
            startToolStripMenuItem.Click += startToolStripMenuItem_Click;
            // 
            // stopToolStripMenuItem
            // 
            stopToolStripMenuItem.Name = "stopToolStripMenuItem";
            stopToolStripMenuItem.Size = new Size(135, 22);
            stopToolStripMenuItem.Text = "Stop";
            stopToolStripMenuItem.Click += stopToolStripMenuItem_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Size(132, 6);
            // 
            // removeToolStripMenuItem
            // 
            removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            removeToolStripMenuItem.Size = new Size(135, 22);
            removeToolStripMenuItem.Text = "Remove";
            removeToolStripMenuItem.Click += removeToolStripMenuItem_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(132, 6);
            // 
            // URLOpen
            // 
            URLOpen.Name = "URLOpen";
            URLOpen.Size = new Size(135, 22);
            URLOpen.Text = "Open URL";
            URLOpen.Click += URLOpen_Click;
            // 
            // MainMenu
            // 
            MainMenu.Items.AddRange(new ToolStripItem[] { copyMessageToolStripMenuItem, copyToolStripMenuItem, clearToolStripMenuItem });
            MainMenu.Name = "MainMenu";
            MainMenu.Size = new Size(164, 70);
            // 
            // copyMessageToolStripMenuItem
            // 
            copyMessageToolStripMenuItem.Name = "copyMessageToolStripMenuItem";
            copyMessageToolStripMenuItem.Size = new Size(163, 22);
            copyMessageToolStripMenuItem.Text = "Copy Message";
            copyMessageToolStripMenuItem.Click += copyMessageToolStripMenuItem_Click;
            // 
            // copyToolStripMenuItem
            // 
            copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            copyToolStripMenuItem.Size = new Size(163, 22);
            copyToolStripMenuItem.Text = "Copy All";
            copyToolStripMenuItem.Click += copyToolStripMenuItem_Click;
            // 
            // clearToolStripMenuItem
            // 
            clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            clearToolStripMenuItem.Size = new Size(163, 22);
            clearToolStripMenuItem.Text = "Clear";
            clearToolStripMenuItem.Click += clearToolStripMenuItem_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1546, 681);
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
            groupBox1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            IDListMenu.ResumeLayout(false);
            MainMenu.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private MenuStrip menuStrip1;
        private ToolStripMenuItem streamToolStripMenuItem;
        private ToolStripMenuItem settingsToolStripMenuItem;
        private ToolStripMenuItem addStreamToolStripMenuItem;
        private TableLayoutPanel tableLayoutPanel1;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private ColumnHeader columnHeader3;
        private ColumnHeader columnHeader4;
        private ColumnHeader columnHeader5;
        private ColumnHeader columnHeader6;
        private ColumnHeader columnHeader7;
        private ColumnHeader columnHeader8;
        private GroupBox groupBox1;
        public ListViewBuff MainOutput;
        private ContextMenuStrip IDListMenu;
        private ToolStripMenuItem startToolStripMenuItem;
        private ToolStripMenuItem stopToolStripMenuItem;
        private ToolStripMenuItem removeToolStripMenuItem;
        private ContextMenuStrip MainMenu;
        private ToolStripMenuItem copyToolStripMenuItem;
        private ToolStripMenuItem copyMessageToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem saveIDListToolStripMenuItem;
        private ToolStripMenuItem clearToolStripMenuItem;
        private TableLayoutPanel tableLayoutPanel2;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private Label UptimeL;
        private Label label1;
        private Label GIntervalL;
        private Label label4;
        private Label SOutputLinesL;
        private Label label6;
        private GroupBox groupBox4;
        private Label OverrallBandwidthL;
        private Label label5;
        private Label ActiveMonitoringL;
        private Label label3;
        private Label FragmentsInMemoryL;
        private Label label7;
        private Label TPktL;
        private Label label8;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripMenuItem toolStripMenuItem2;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem URLOpen;
        private Label OverrallTrafficL;
        private Label label10;
        private Label PktTrafficL;
        private Label label9;
        private Label PktBWL;
        private Label label12;
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
    }
}
