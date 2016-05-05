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
        private Api.Domain domain;
        private FLogin fLogin;
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
            this.components = null;
            this._logger = new Logger("ui");
            this.InitializeComponent();
        }

        public FMonitor(FLogin fLogin, Api.Domain domain) : this()
        {
            this.fLogin = fLogin;
            this.domain = domain;
        }

        private void BindData()
        {
            if (base.InvokeRequired)
            {
                ThreadStart method = new ThreadStart(this.BindData);
                base.Invoke(method);
            }
            else
            {
                try
                {
                    List<Api.Record> recordList = this.fLogin.Api.GetRecordList(this.domain.DomainId);
                    this.lvRecords.Items.Clear();
                    this.lvRecords.Groups.Clear();
                    ListViewGroup group = new ListViewGroup("状态：宕机");
                    ListViewGroup group2 = new ListViewGroup("状态：正常");
                    ListViewGroup group3 = new ListViewGroup("状态：未知");
                    ListViewGroup group4 = new ListViewGroup("未监控");
                    this.lvRecords.Groups.Add(group);
                    this.lvRecords.Groups.Add(group2);
                    this.lvRecords.Groups.Add(group3);
                    this.lvRecords.Groups.Add(group4);
                    using (List<Api.Record>.Enumerator enumerator = recordList.GetEnumerator())
                    {
                        Predicate<Config.MonitorConfig> predicate2 = null;
                        Api.Record record;
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
                                Config.MonitorConfig config = this.fLogin.Config.GetMonitors().Find(match);
                                record.IsMonitor = config != null;
                                ListViewItem item2 = new ListViewItem(string.Format("{0}.{1}.", record.Name, this.domain.Name)) {
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
                                this.lvRecords.Items.Add(item);
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    this._logger.Error("fddns.binddata has an error:{0}", new object[] { exception });
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void FRecordList_Load(object sender, EventArgs e)
        {
            this.fLogin.Monitor.StatusChanged += new HttpMonitor.DStatusChanged(this.Monitor_StatusChanged);
            this.fLogin.Monitor.Info += delegate (string msg) {
                this.SetStatus(msg);
            };
            ImageList list2 = new ImageList {
                ImageSize = new Size(1, 20)
            };
            ImageList list = list2;
            this.lvRecords.SmallImageList = list;
            this.RemoveInvalidMonitors();
            this.BindData();
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            this.statusStrip1 = new StatusStrip();
            this.lblStatus = new ToolStripStatusLabel();
            this.panel1 = new Panel();
            this.label1 = new Label();
            this.contextMenuStrip1 = new ContextMenuStrip(this.components);
            this.启用监控ToolStripMenuItem = new ToolStripMenuItem();
            this.查看日志ToolStripMenuItem = new ToolStripMenuItem();
            this.查看图表ToolStripMenuItem = new ToolStripMenuItem();
            this.lvRecords = new DoubleBufferListView();
            this.name = new ColumnHeader();
            this.value = new ColumnHeader();
            this.recordtype = new ColumnHeader();
            this.line = new ColumnHeader();
            this.status = new ColumnHeader();
            this.columnHeader1 = new ColumnHeader();
            this.columnHeader2 = new ColumnHeader();
            this.columnHeader3 = new ColumnHeader();
            this.statusStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            base.SuspendLayout();
            this.statusStrip1.Items.AddRange(new ToolStripItem[] { this.lblStatus });
            this.statusStrip1.Location = new Point(0, 0x159);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new Size(0x29f, 0x16);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new Size(0x20, 0x11);
            this.lblStatus.Text = "就绪";
            this.panel1.BackColor = Color.DeepSkyBlue;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = DockStyle.Top;
            this.panel1.Location = new Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0x29f, 40);
            this.panel1.TabIndex = 6;
            this.label1.AutoSize = true;
            this.label1.Font = new Font("宋体", 15.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.label1.ForeColor = Color.White;
            this.label1.Location = new Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x5e, 0x15);
            this.label1.TabIndex = 1;
            this.label1.Text = "宕机监控";
            this.contextMenuStrip1.Items.AddRange(new ToolStripItem[] { this.启用监控ToolStripMenuItem, this.查看日志ToolStripMenuItem, this.查看图表ToolStripMenuItem });
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new Size(0x7d, 70);
            this.启用监控ToolStripMenuItem.Name = "启用监控ToolStripMenuItem";
            this.启用监控ToolStripMenuItem.Size = new Size(0x7c, 0x16);
            this.启用监控ToolStripMenuItem.Text = "启用监控";
            this.启用监控ToolStripMenuItem.Click += new EventHandler(this.启用监控ToolStripMenuItem_Click);
            this.查看日志ToolStripMenuItem.Name = "查看日志ToolStripMenuItem";
            this.查看日志ToolStripMenuItem.Size = new Size(0x7c, 0x16);
            this.查看日志ToolStripMenuItem.Text = "查看日志";
            this.查看日志ToolStripMenuItem.Click += new EventHandler(this.查看日志ToolStripMenuItem_Click);
            this.查看图表ToolStripMenuItem.Name = "查看图表ToolStripMenuItem";
            this.查看图表ToolStripMenuItem.Size = new Size(0x7c, 0x16);
            this.查看图表ToolStripMenuItem.Text = "查看图表";
            this.查看图表ToolStripMenuItem.Click += new EventHandler(this.查看图表ToolStripMenuItem_Click);
            this.lvRecords.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.lvRecords.Columns.AddRange(new ColumnHeader[] { this.name, this.value, this.recordtype, this.line, this.status, this.columnHeader1, this.columnHeader2, this.columnHeader3 });
            this.lvRecords.FullRowSelect = true;
            this.lvRecords.Location = new Point(0, 0x2a);
            this.lvRecords.MultiSelect = false;
            this.lvRecords.Name = "lvRecords";
            this.lvRecords.Size = new Size(0x29f, 300);
            this.lvRecords.TabIndex = 2;
            this.lvRecords.UseCompatibleStateImageBehavior = false;
            this.lvRecords.View = View.Details;
            this.lvRecords.MouseClick += new MouseEventHandler(this.lvRecords_MouseClick);
            this.name.Text = "记录名称";
            this.name.Width = 100;
            this.value.Text = "当前记录值";
            this.value.Width = 100;
            this.recordtype.Text = "记录类型";
            this.line.Text = "线路";
            this.status.Text = "状态";
            this.columnHeader1.Text = "是否切换";
            this.columnHeader2.Text = "备用IP";
            this.columnHeader2.Width = 100;
            this.columnHeader3.Text = "原始记录值";
            this.columnHeader3.Width = 100;
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.ClientSize = new Size(0x29f, 0x16f);
            base.Controls.Add(this.panel1);
            base.Controls.Add(this.lvRecords);
            base.Controls.Add(this.statusStrip1);
            base.Name = "FMonitor";
            this.Text = "宕机监控-DNSPodClientLite";
            base.Load += new EventHandler(this.FRecordList_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void lvRecords_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ListViewHitTestInfo info = this.lvRecords.HitTest(e.Location);
                if (info.Item != null)
                {
                    Api.Record tag = (Api.Record) info.Item.Tag;
                    if (tag.IsMonitor)
                    {
                        this.contextMenuStrip1.Items[1].Visible = true;
                        this.contextMenuStrip1.Items[2].Visible = true;
                        this.contextMenuStrip1.Items[0].Text = "禁用监控";
                    }
                    else
                    {
                        this.contextMenuStrip1.Items[1].Visible = false;
                        this.contextMenuStrip1.Items[2].Visible = false;
                        this.contextMenuStrip1.Items[0].Text = "启用监控";
                    }
                    this.contextMenuStrip1.Show(this.lvRecords, e.Location);
                }
            }
        }

        private void Monitor_StatusChanged(int recordid, string status)
        {
            this.BindData();
        }

        private void RemoveInvalidMonitors()
        {
            Predicate<Config.MonitorConfig> match = null;
            Predicate<Config.MonitorConfig> predicate4 = null;
            try
            {
                List<Api.Record> recordList = this.fLogin.Api.GetRecordList(this.domain.DomainId);
                if (match == null)
                {
                    if (predicate4 == null)
                    {
                        predicate4 = t => t.DomainId == this.domain.DomainId;
                    }
                    match = predicate4;
                }
                using (List<Config.MonitorConfig>.Enumerator enumerator = this.fLogin.Config.GetMonitors().FindAll(match).GetEnumerator())
                {
                    Predicate<Api.Record> predicate3 = null;
                    Config.MonitorConfig cfg;
                    Predicate<Api.Record> predicate2 = null;
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
                            this.fLogin.Config.RemoveMonitor(cfg.RecordId);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                this._logger.Error("RemoveInvalidMonitors has an error:{0}", new object[] { exception });
            }
        }

        private void SetStatus(string msg)
        {
            if (base.InvokeRequired)
            {
                Action<string> method = new Action<string>(this.SetStatus);
                base.Invoke(method, new object[] { msg });
            }
            else
            {
                this.lblStatus.Text = msg;
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
                MessageBox.Show(exception.Message);
                this._logger.Error("fmonitor.showlog has an error:{0}", new object[] { exception });
            }
        }

        private void 查看图表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Predicate<Config.MonitorConfig> match = null;
                Api.Record record;
                if (this.lvRecords.SelectedItems.Count >= 1)
                {
                    record = (Api.Record) this.lvRecords.SelectedItems[0].Tag;
                    if (match == null)
                    {
                        match = i => i.RecordId == record.RecordId;
                    }
                    Config.MonitorConfig monitorCfg = this.fLogin.Config.GetMonitors().Find(match);
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
                MessageBox.Show(exception.Message);
                this._logger.Error("fmonitor.showchart has an error:{0}", new object[] { exception });
            }
        }

        private void 启用监控ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.lvRecords.SelectedItems.Count >= 1)
                {
                    Api.Record tag = (Api.Record) this.lvRecords.SelectedItems[0].Tag;
                    ToolStripMenuItem item = (ToolStripMenuItem) sender;
                    if (item.Text == "启用监控")
                    {
                        new FMonitorSetting(this.domain, this.fLogin, tag).ShowDialog();
                    }
                    else
                    {
                        this.fLogin.Config.RemoveMonitor(tag.RecordId);
                    }
                    this.fLogin.Config.Save();
                    this.BindData();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                this._logger.Error("fmonitor.enableddns has an error:{0}", new object[] { exception });
            }
        }
    }
}

