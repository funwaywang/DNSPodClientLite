using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace DNSPodClientLite
{
    public class FDdns : Form
    {
        private Logger _logger;
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
        private StatusStrip statusStrip1;
        private ColumnHeader value;
        private ToolStripMenuItem 查看日志ToolStripMenuItem;
        private ToolStripMenuItem 禁用动态解析ToolStripMenuItem;
        private ToolStripMenuItem 启用动态解析ToolStripMenuItem;

        public FDdns()
        {
            components = null;
            components = null;
            _logger = new Logger("ui");
            InitializeComponent();
        }

        public FDdns(DnsPodApi.Domain domain) : this()
        {
            this.domain = domain;
        }

        private void BindData()
        {
            if (InvokeRequired)
            {
                ThreadStart method = new ThreadStart(BindData);
                Invoke(method);
            }
            else
            {
                try
                {
                    List<DnsPodApi.Record> recordList = AppStatus.Default.Api.GetRecordList(domain.DomainId);
                    lvRecords.Items.Clear();
                    lvRecords.Groups.Clear();
                    ListViewGroup group = new ListViewGroup("动态记录");
                    ListViewGroup group2 = new ListViewGroup("非动态记录");
                    lvRecords.Groups.Add(group);
                    lvRecords.Groups.Add(group2);
                    using (List<DnsPodApi.Record>.Enumerator enumerator = recordList.GetEnumerator())
                    {
                        Predicate<Config.DDNSConfig> predicate2 = null;
                        DnsPodApi.Record record;
                        Predicate<Config.DDNSConfig> match = null;
                        while (enumerator.MoveNext())
                        {
                            record = enumerator.Current;
                            if ((record.RecordType == "A") || (record.RecordType == "MX"))
                            {
                                if (match == null)
                                {
                                    if (predicate2 == null)
                                    {
                                        predicate2 = i => i.RecordId == record.RecordId;
                                    }
                                    match = predicate2;
                                }
                                record.IsDdns = AppStatus.Default.Config.GetDdnses().Find(match) != null;
                                ListViewItem item = new ListViewItem(string.Format("{0}.{1}.", record.Name, domain.Name));
                                if (record.IsDdns)
                                {
                                    item.Group = group;
                                }
                                else
                                {
                                    item.Group = group2;
                                }
                                item.Tag = record;
                                item.SubItems.Add(record.Value);
                                item.SubItems.Add(record.RecordType);
                                item.SubItems.Add(record.Line);
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
            SetStatus("当前IP为：" + AppStatus.Default.Ddns.LastIp);
            AppStatus.Default.Ddns.IPChanged += Ddns_IPChangedNotified1;
            AppStatus.Default.Ddns.InformationReceived += Ddns_InformationNotified;

            ImageList list2 = new ImageList
            {
                ImageSize = new Size(1, 20)
            };
            ImageList list = list2;
            lvRecords.SmallImageList = list;
            RemoveInvalidDdns();
            BindData();
        }

        private void Ddns_IPChangedNotified1(object sender, IpChangedEventArgs e)
        {
            SetStatus("当前IP为：" + e.IP);
            BindData();
        }

        private void Ddns_InformationNotified(object sender, MessageEventArgs e)
        {
            SetStatus(e.Message);
        }

        private void InitializeComponent()
        {
            components = new Container();
            statusStrip1 = new StatusStrip();
            lblStatus = new ToolStripStatusLabel();
            lvRecords = new DoubleBufferListView();
            name = new ColumnHeader();
            value = new ColumnHeader();
            recordtype = new ColumnHeader();
            line = new ColumnHeader();
            panel1 = new Panel();
            label1 = new Label();
            contextMenuStrip1 = new ContextMenuStrip(components);
            启用动态解析ToolStripMenuItem = new ToolStripMenuItem();
            禁用动态解析ToolStripMenuItem = new ToolStripMenuItem();
            查看日志ToolStripMenuItem = new ToolStripMenuItem();
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
            lvRecords.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            lvRecords.Columns.AddRange(new ColumnHeader[] { name, value, recordtype, line });
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
            name.Width = 150;
            value.Text = "记录值";
            value.Width = 150;
            recordtype.Text = "记录类型";
            line.Text = "记录线路";
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
            label1.Text = "动态解析";
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { 启用动态解析ToolStripMenuItem, 禁用动态解析ToolStripMenuItem, 查看日志ToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(0x99, 0x5c);
            启用动态解析ToolStripMenuItem.Name = "启用动态解析ToolStripMenuItem";
            启用动态解析ToolStripMenuItem.Size = new Size(0x98, 0x16);
            启用动态解析ToolStripMenuItem.Text = "启用动态解析";
            启用动态解析ToolStripMenuItem.Click += new EventHandler(启用动态解析ToolStripMenuItem_Click);
            禁用动态解析ToolStripMenuItem.Name = "禁用动态解析ToolStripMenuItem";
            禁用动态解析ToolStripMenuItem.Size = new Size(0x98, 0x16);
            禁用动态解析ToolStripMenuItem.Text = "禁用动态解析";
            禁用动态解析ToolStripMenuItem.Click += new EventHandler(禁用动态解析ToolStripMenuItem_Click);
            查看日志ToolStripMenuItem.Name = "查看日志ToolStripMenuItem";
            查看日志ToolStripMenuItem.Size = new Size(0x98, 0x16);
            查看日志ToolStripMenuItem.Text = "查看日志";
            查看日志ToolStripMenuItem.Click += new EventHandler(查看日志ToolStripMenuItem_Click);
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.ClientSize = new Size(0x29f, 0x16f);
            base.Controls.Add(panel1);
            base.Controls.Add(lvRecords);
            base.Controls.Add(statusStrip1);
            base.Name = "FDdns";
            Text = "动态解析-DNSPodClientLite";
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
                    if (tag.IsDdns)
                    {
                        contextMenuStrip1.Items[0].Visible = false;
                        contextMenuStrip1.Items[1].Visible = true;
                        contextMenuStrip1.Items[2].Visible = true;
                    }
                    else
                    {
                        contextMenuStrip1.Items[0].Visible = true;
                        contextMenuStrip1.Items[1].Visible = false;
                        contextMenuStrip1.Items[2].Visible = false;
                    }
                    contextMenuStrip1.Show(lvRecords, e.Location);
                }
            }
        }

        private void RemoveInvalidDdns()
        {
            Predicate<Config.DDNSConfig> match = null;
            Predicate<Config.DDNSConfig> predicate4 = null;
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
                using (List<Config.DDNSConfig>.Enumerator enumerator = AppStatus.Default.Config.GetDdnses().FindAll(match).GetEnumerator())
                {
                    Predicate<DnsPodApi.Record> predicate3 = null;
                    Config.DDNSConfig cfg;
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
                            AppStatus.Default.Config.RemoveDdns(cfg.RecordId);
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
                string str2 = string.Format("{0}-{1}.log", "ddns", DateTime.Now.ToString("yyyy-MM-dd"));
                Process.Start(Path.Combine(str, str2));
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _logger.Error("fddns.showlog has an error:{0}", new object[] { exception });
            }
        }

        private void 禁用动态解析ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (lvRecords.SelectedItems.Count >= 1)
                {
                    DnsPodApi.Record tag = (DnsPodApi.Record)lvRecords.SelectedItems[0].Tag;
                    AppStatus.Default.Config.RemoveDdns(tag.RecordId);
                    AppStatus.Default.Config.Save();
                    BindData();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _logger.Error("fddns.disable_ddns has an error:{0}", new object[] { exception });
            }
        }

        private void 启用动态解析ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (lvRecords.SelectedItems.Count >= 1)
                {
                    DnsPodApi.Record tag = (DnsPodApi.Record)lvRecords.SelectedItems[0].Tag;
                    Config.DDNSConfig config2 = new Config.DDNSConfig
                    {
                        Domain = domain.Name,
                        DomainId = domain.DomainId,
                        RecordId = tag.RecordId,
                        Subdomain = tag.Name
                    };
                    Config.DDNSConfig item = config2;
                    AppStatus.Default.Config.AddDdns(item);
                    AppStatus.Default.Api.Ddns(domain.DomainId, tag.RecordId, AppStatus.Default.Ddns.LastIp);
                    new Logger("ddns").Info("change ip:{0}.{1}({2})-{3}", new object[] { tag.Name, domain.Name, tag.RecordId, AppStatus.Default.Ddns.LastIp });
                    AppStatus.Default.Config.Save();
                    BindData();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _logger.Error("fddns.enable_ddns has an error:{0}", new object[] { exception });
            }
        }
    }
}

