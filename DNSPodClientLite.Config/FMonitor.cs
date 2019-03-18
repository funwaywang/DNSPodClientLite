namespace DNSPodClientLite
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Threading;
    using System.Windows.Forms;

    public class FMonitor : Form
    {
        private Logger _logger;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private ColumnHeader columnHeader3;
        private IContainer components;
        private ContextMenuStrip contextMenuStrip1;
        private DnsPodApi.Domain domain;
        private Label label1;
        private ToolStripStatusLabel lblStatus;
        private ColumnHeader line;
        private DoubleBufferListView lvRecords;
        private ColumnHeader name;
        private Panel panel1;
        private ColumnHeader recordtype;
        private ColumnHeader status;
        private StatusStrip statusStrip1;
        private ColumnHeader value;
        private ToolStripMenuItem 查看日志ToolStripMenuItem;
        private ToolStripMenuItem 查看图表ToolStripMenuItem;
        private ToolStripMenuItem 启用监控ToolStripMenuItem;

        public FMonitor()
        {
            components = null;
            _logger = new Logger("ui");
            InitializeComponent();
        }

        public FMonitor(DnsPodApi.Domain domain) : this()
        {
            this.domain = domain;
        }

        private void BindData()
        {
            if (base.InvokeRequired)
            {
                ThreadStart method = new ThreadStart(BindData);
                base.Invoke(method);
            }
            else
            {
                try
                {
                    List<DnsPodApi.Record> recordList = AppStatus.Default.Api.GetRecordList(domain.DomainId);
                    lvRecords.Items.Clear();
                    lvRecords.Groups.Clear();
                    ListViewGroup group = new ListViewGroup("状态：宕机");
                    ListViewGroup group2 = new ListViewGroup("状态：正常");
                    ListViewGroup group3 = new ListViewGroup("状态：未知");
                    ListViewGroup group4 = new ListViewGroup("未监控");
                    lvRecords.Groups.Add(group);
                    lvRecords.Groups.Add(group2);
                    lvRecords.Groups.Add(group3);
                    lvRecords.Groups.Add(group4);
                    using (List<DnsPodApi.Record>.Enumerator enumerator = recordList.GetEnumerator())
                    {
                        Predicate<Config.MonitorConfig> predicate2 = null;
                        DnsPodApi.Record record;
                        Predicate<Config.MonitorConfig> match = null;
                        while (enumerator.MoveNext())
                        {
                            record = enumerator.Current;
                            if (record.RecordType == "A")
                            {
                                string text = "未知";
                                if (match == null)
                                {
                                    if (predicate2 == null)
                                    {
                                        predicate2 = i => i.RecordId == record.RecordId;
                                    }
                                    match = predicate2;
                                }
                                Config.MonitorConfig config = AppStatus.Default.Config.GetMonitors().Find(match);
                                record.IsMonitor = config != null;
                                ListViewItem item2 = new ListViewItem(string.Format("{0}.{1}.", record.Name, domain.Name))
                                {
                                    Group = group4
                                };
                                ListViewItem item = item2;
                                if (config != null)
                                {
                                    switch (config.Status)
                                    {
                                        case "unknow":
                                            text = "未知";
                                            item.Group = group3;
                                            break;

                                        case "down":
                                            text = "宕机";
                                            item.UseItemStyleForSubItems = true;
                                            item.BackColor = Color.Red;
                                            item.Group = group;
                                            break;

                                        case "ok":
                                            text = "正常";
                                            item.Group = group2;
                                            break;
                                    }
                                }
                                item.Tag = record;
                                item.SubItems.Add(record.Value);
                                item.SubItems.Add(record.RecordType);
                                item.SubItems.Add(record.Line);
                                item.SubItems.Add(text);
                                if (config != null)
                                {
                                    item.SubItems.Add(config.Qiehuan ? "是" : "否");
                                    item.SubItems.Add(config.SmartQiehuan ? "智能切换" : config.BakValue);
                                    item.SubItems.Add(config.Ip);
                                }
                                lvRecords.Items.Add(item);
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error("fddns.binddata has an error:{0}", new object[] { exception });
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void FRecordList_Load(object sender, EventArgs e)
        {
            AppStatus.Default.Monitor.StatusChanged += Monitor_StatusChanged;
            AppStatus.Default.Monitor.InformationReceived += Monitor_InformationReceived;

            ImageList list2 = new ImageList
            {
                ImageSize = new Size(1, 20)
            };
            ImageList list = list2;
            lvRecords.SmallImageList = list;
            RemoveInvalidMonitors();
            BindData();
        }

        private void Monitor_InformationReceived(object sender, MessageEventArgs e)
        {
            SetStatus(e.Message);
        }

        private void InitializeComponent()
        {
            components = new Container();
            statusStrip1 = new StatusStrip();
            lblStatus = new ToolStripStatusLabel();
            panel1 = new Panel();
            label1 = new Label();
            contextMenuStrip1 = new ContextMenuStrip(components);
            启用监控ToolStripMenuItem = new ToolStripMenuItem();
            查看日志ToolStripMenuItem = new ToolStripMenuItem();
            查看图表ToolStripMenuItem = new ToolStripMenuItem();
            lvRecords = new DoubleBufferListView();
            name = new ColumnHeader();
            value = new ColumnHeader();
            recordtype = new ColumnHeader();
            line = new ColumnHeader();
            status = new ColumnHeader();
            columnHeader1 = new ColumnHeader();
            columnHeader2 = new ColumnHeader();
            columnHeader3 = new ColumnHeader();
            statusStrip1.SuspendLayout();
            panel1.SuspendLayout();
            contextMenuStrip1.SuspendLayout();
            base.SuspendLayout();
            statusStrip1.Items.AddRange(new ToolStripItem[] { lblStatus });
            statusStrip1.Location = new Point(0, 0x159);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(0x29f, 0x16);
            statusStrip1.TabIndex = 0;
            statusStrip1.Text = "statusStrip1";
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(0x20, 0x11);
            lblStatus.Text = "就绪";
            panel1.BackColor = Color.DeepSkyBlue;
            panel1.Controls.Add(label1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(0x29f, 40);
            panel1.TabIndex = 6;
            label1.AutoSize = true;
            label1.Font = new Font("宋体", 15.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.White;
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(0x5e, 0x15);
            label1.TabIndex = 1;
            label1.Text = "宕机监控";
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { 启用监控ToolStripMenuItem, 查看日志ToolStripMenuItem, 查看图表ToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(0x7d, 70);
            启用监控ToolStripMenuItem.Name = "启用监控ToolStripMenuItem";
            启用监控ToolStripMenuItem.Size = new Size(0x7c, 0x16);
            启用监控ToolStripMenuItem.Text = "启用监控";
            启用监控ToolStripMenuItem.Click += new EventHandler(启用监控ToolStripMenuItem_Click);
            查看日志ToolStripMenuItem.Name = "查看日志ToolStripMenuItem";
            查看日志ToolStripMenuItem.Size = new Size(0x7c, 0x16);
            查看日志ToolStripMenuItem.Text = "查看日志";
            查看日志ToolStripMenuItem.Click += new EventHandler(查看日志ToolStripMenuItem_Click);
            查看图表ToolStripMenuItem.Name = "查看图表ToolStripMenuItem";
            查看图表ToolStripMenuItem.Size = new Size(0x7c, 0x16);
            查看图表ToolStripMenuItem.Text = "查看图表";
            查看图表ToolStripMenuItem.Click += new EventHandler(查看图表ToolStripMenuItem_Click);
            lvRecords.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            lvRecords.Columns.AddRange(new ColumnHeader[] { name, value, recordtype, line, status, columnHeader1, columnHeader2, columnHeader3 });
            lvRecords.FullRowSelect = true;
            lvRecords.Location = new Point(0, 0x2a);
            lvRecords.MultiSelect = false;
            lvRecords.Name = "lvRecords";
            lvRecords.Size = new Size(0x29f, 300);
            lvRecords.TabIndex = 2;
            lvRecords.UseCompatibleStateImageBehavior = false;
            lvRecords.View = View.Details;
            lvRecords.MouseClick += new MouseEventHandler(lvRecords_MouseClick);
            name.Text = "记录名称";
            name.Width = 100;
            value.Text = "当前记录值";
            value.Width = 100;
            recordtype.Text = "记录类型";
            line.Text = "线路";
            status.Text = "状态";
            columnHeader1.Text = "是否切换";
            columnHeader2.Text = "备用IP";
            columnHeader2.Width = 100;
            columnHeader3.Text = "原始记录值";
            columnHeader3.Width = 100;
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.ClientSize = new Size(0x29f, 0x16f);
            base.Controls.Add(panel1);
            base.Controls.Add(lvRecords);
            base.Controls.Add(statusStrip1);
            base.Name = "FMonitor";
            Text = "宕机监控-DNSPodClientLite";
            base.Load += new EventHandler(FRecordList_Load);
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            contextMenuStrip1.ResumeLayout(false);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void lvRecords_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ListViewHitTestInfo info = lvRecords.HitTest(e.Location);
                if (info.Item != null)
                {
                    DnsPodApi.Record tag = (DnsPodApi.Record)info.Item.Tag;
                    if (tag.IsMonitor)
                    {
                        contextMenuStrip1.Items[1].Visible = true;
                        contextMenuStrip1.Items[2].Visible = true;
                        contextMenuStrip1.Items[0].Text = "禁用监控";
                    }
                    else
                    {
                        contextMenuStrip1.Items[1].Visible = false;
                        contextMenuStrip1.Items[2].Visible = false;
                        contextMenuStrip1.Items[0].Text = "启用监控";
                    }
                    contextMenuStrip1.Show(lvRecords, e.Location);
                }
            }
        }

        private void Monitor_StatusChanged(object sender, RecordStatusChangedEventArgs e)
        {
            BindData();
        }

        private void RemoveInvalidMonitors()
        {
            Predicate<Config.MonitorConfig> match = null;
            Predicate<Config.MonitorConfig> predicate4 = null;
            try
            {
                List<DnsPodApi.Record> recordList = AppStatus.Default.Api.GetRecordList(domain.DomainId);
                if (match == null)
                {
                    if (predicate4 == null)
                    {
                        predicate4 = t => t.DomainId == domain.DomainId;
                    }
                    match = predicate4;
                }
                using (List<Config.MonitorConfig>.Enumerator enumerator = AppStatus.Default.Config.GetMonitors().FindAll(match).GetEnumerator())
                {
                    Predicate<DnsPodApi.Record> predicate3 = null;
                    Config.MonitorConfig cfg;
                    Predicate<DnsPodApi.Record> predicate2 = null;
                    while (enumerator.MoveNext())
                    {
                        cfg = enumerator.Current;
                        if (predicate2 == null)
                        {
                            if (predicate3 == null)
                            {
                                predicate3 = x => x.RecordId == cfg.RecordId;
                            }
                            predicate2 = predicate3;
                        }
                        if (recordList.Find(predicate2) == null)
                        {
                            AppStatus.Default.Config.RemoveMonitor(cfg.RecordId);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                _logger.Error("RemoveInvalidMonitors has an error:{0}", new object[] { exception });
            }
        }

        private void SetStatus(string msg)
        {
            if (base.InvokeRequired)
            {
                Action<string> method = new Action<string>(SetStatus);
                base.Invoke(method, new object[] { msg });
            }
            else
            {
                lblStatus.Text = msg;
            }
        }

        private void 查看日志ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string str = Path.Combine(Environment.CurrentDirectory, "log");
                string str2 = string.Format("{0}-{1}.log", "monitor", DateTime.Now.ToString("yyyy-MM-dd"));
                Process.Start(Path.Combine(str, str2));
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _logger.Error("fmonitor.showlog has an error:{0}", new object[] { exception });
            }
        }

        private void 查看图表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Predicate<Config.MonitorConfig> match = null;
                DnsPodApi.Record record;
                if (lvRecords.SelectedItems.Count >= 1)
                {
                    record = (DnsPodApi.Record)lvRecords.SelectedItems[0].Tag;
                    if (match == null)
                    {
                        match = i => i.RecordId == record.RecordId;
                    }
                    Config.MonitorConfig monitorCfg = AppStatus.Default.Config.GetMonitors().Find(match);
                    if (monitorCfg != null)
                    {
                        new FMonitorChart(monitorCfg).Show();
                    }
                }
            }
            catch (Win32Exception)
            {
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _logger.Error("fmonitor.showchart has an error:{0}", new object[] { exception });
            }
        }

        private void 启用监控ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (lvRecords.SelectedItems.Count >= 1)
                {
                    DnsPodApi.Record tag = (DnsPodApi.Record)lvRecords.SelectedItems[0].Tag;
                    ToolStripMenuItem item = (ToolStripMenuItem)sender;
                    if (item.Text == "启用监控")
                    {
                        new FMonitorSetting(domain, tag).ShowDialog();
                    }
                    else
                    {
                        AppStatus.Default.Config.RemoveMonitor(tag.RecordId);
                    }
                    AppStatus.Default.Config.Save();
                    BindData();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _logger.Error("fmonitor.enableddns has an error:{0}", new object[] { exception });
            }
        }
    }
}

