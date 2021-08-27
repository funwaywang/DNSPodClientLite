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
        private ToolStripMenuItem menuShowLogs;
        private ToolStripMenuItem menuDisableDdns;
        private ToolStripMenuItem menuManualRefresh;
        private ToolStripMenuItem menuEnableDdns;

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
            this.components = new System.ComponentModel.Container();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lvRecords = new DNSPodClientLite.DoubleBufferListView();
            this.name = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.value = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.recordtype = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.line = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuEnableDdns = new System.Windows.Forms.ToolStripMenuItem();
            this.menuManualRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.menuDisableDdns = new System.Windows.Forms.ToolStripMenuItem();
            this.menuShowLogs = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 345);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(671, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(32, 17);
            this.lblStatus.Text = "就绪";
            // 
            // lvRecords
            // 
            this.lvRecords.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvRecords.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.name,
            this.value,
            this.recordtype,
            this.line});
            this.lvRecords.FullRowSelect = true;
            this.lvRecords.HideSelection = false;
            this.lvRecords.Location = new System.Drawing.Point(0, 42);
            this.lvRecords.MultiSelect = false;
            this.lvRecords.Name = "lvRecords";
            this.lvRecords.Size = new System.Drawing.Size(671, 300);
            this.lvRecords.TabIndex = 2;
            this.lvRecords.UseCompatibleStateImageBehavior = false;
            this.lvRecords.View = System.Windows.Forms.View.Details;
            this.lvRecords.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lvRecords_MouseClick);
            // 
            // name
            // 
            this.name.Text = "记录名称";
            this.name.Width = 150;
            // 
            // value
            // 
            this.value.Text = "记录值";
            this.value.Width = 150;
            // 
            // recordtype
            // 
            this.recordtype.Text = "记录类型";
            // 
            // line
            // 
            this.line.Text = "记录线路";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(671, 40);
            this.panel1.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("SimSun", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 21);
            this.label1.TabIndex = 1;
            this.label1.Text = "动态解析";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuEnableDdns,
            this.menuManualRefresh,
            this.menuDisableDdns,
            this.menuShowLogs});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(149, 92);
            // 
            // menuEnableDdns
            // 
            this.menuEnableDdns.Name = "menuEnableDdns";
            this.menuEnableDdns.Size = new System.Drawing.Size(148, 22);
            this.menuEnableDdns.Text = "启用动态解析";
            this.menuEnableDdns.Click += new System.EventHandler(this.menuEnableDdns_Click);
            // 
            // menuManualRefresh
            // 
            this.menuManualRefresh.Name = "menuManualRefresh";
            this.menuManualRefresh.Size = new System.Drawing.Size(148, 22);
            this.menuManualRefresh.Text = "手工刷新";
            this.menuManualRefresh.Click += new System.EventHandler(this.menuManualRefresh_Click);
            // 
            // menuDisableDdns
            // 
            this.menuDisableDdns.Name = "menuDisableDdns";
            this.menuDisableDdns.Size = new System.Drawing.Size(148, 22);
            this.menuDisableDdns.Text = "禁用动态解析";
            this.menuDisableDdns.Click += new System.EventHandler(this.menuDisableDdns_Click);
            // 
            // menuShowLogs
            // 
            this.menuShowLogs.Name = "menuShowLogs";
            this.menuShowLogs.Size = new System.Drawing.Size(148, 22);
            this.menuShowLogs.Text = "查看日志";
            this.menuShowLogs.Click += new System.EventHandler(this.menuShowLogs_Click);
            // 
            // FDdns
            // 
            this.ClientSize = new System.Drawing.Size(671, 367);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lvRecords);
            this.Controls.Add(this.statusStrip1);
            this.Name = "FDdns";
            this.Text = "动态解析-DNSPodClientLite";
            this.Load += new System.EventHandler(this.FRecordList_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

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
                        menuEnableDdns.Visible = false;
                        menuManualRefresh.Visible = true;
                        menuDisableDdns.Visible = true;
                    }
                    else
                    {
                        menuEnableDdns.Visible = true;
                        menuManualRefresh.Visible = false;
                        menuDisableDdns.Visible = false;
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

        private void menuShowLogs_Click(object sender, EventArgs e)
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

        private void menuDisableDdns_Click(object sender, EventArgs e)
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

        private void menuEnableDdns_Click(object sender, EventArgs e)
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

        private void menuManualRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                if (lvRecords.SelectedItems.Count >= 1)
                {
                    DnsPodApi.Record tag = (DnsPodApi.Record)lvRecords.SelectedItems[0].Tag;
                    AppStatus.Default.Api.Ddns(domain.DomainId, tag.RecordId, AppStatus.Default.Ddns.LastIp);
                    new Logger("ddns").Info("change ip:{0}.{1}({2})-{3}", new object[] { tag.Name, domain.Name, tag.RecordId, AppStatus.Default.Ddns.LastIp });
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

