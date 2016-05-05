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

    public class FDdns : Form
    {
        private Logger _logger;
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
        private StatusStrip statusStrip1;
        private ColumnHeader value;
        private ToolStripMenuItem 查看日志ToolStripMenuItem;
        private ToolStripMenuItem 禁用动态解析ToolStripMenuItem;
        private ToolStripMenuItem 启用动态解析ToolStripMenuItem;

        public FDdns()
        {
            this.components = null;
            this.components = null;
            this._logger = new Logger("ui");
            this.InitializeComponent();
        }

        public FDdns(FLogin fLogin, Api.Domain domain) : this()
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
                    ListViewGroup group = new ListViewGroup("动态记录");
                    ListViewGroup group2 = new ListViewGroup("非动态记录");
                    this.lvRecords.Groups.Add(group);
                    this.lvRecords.Groups.Add(group2);
                    using (List<Api.Record>.Enumerator enumerator = recordList.GetEnumerator())
                    {
                        Predicate<Config.DDNSConfig> predicate2 = null;
                        Api.Record record;
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
                                record.IsDdns = this.fLogin.Config.GetDdnses().Find(match) != null;
                                ListViewItem item = new ListViewItem(string.Format("{0}.{1}.", record.Name, this.domain.Name));
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

        private void Ddns_IPChanged(string ip)
        {
            this.SetStatus("当前IP为：" + ip);
            this.BindData();
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
            this.SetStatus("当前IP为：" + this.fLogin.Ddns.LastIp);
            this.fLogin.Ddns.IPChanged += new Action<string>(this.Ddns_IPChanged);
            this.fLogin.Ddns.Info += delegate (string msg) {
                this.SetStatus(msg);
            };
            ImageList list2 = new ImageList {
                ImageSize = new Size(1, 20)
            };
            ImageList list = list2;
            this.lvRecords.SmallImageList = list;
            this.RemoveInvalidDdns();
            this.BindData();
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            this.statusStrip1 = new StatusStrip();
            this.lblStatus = new ToolStripStatusLabel();
            this.lvRecords = new DoubleBufferListView();
            this.name = new ColumnHeader();
            this.value = new ColumnHeader();
            this.recordtype = new ColumnHeader();
            this.line = new ColumnHeader();
            this.panel1 = new Panel();
            this.label1 = new Label();
            this.contextMenuStrip1 = new ContextMenuStrip(this.components);
            this.启用动态解析ToolStripMenuItem = new ToolStripMenuItem();
            this.禁用动态解析ToolStripMenuItem = new ToolStripMenuItem();
            this.查看日志ToolStripMenuItem = new ToolStripMenuItem();
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
            this.lvRecords.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.lvRecords.Columns.AddRange(new ColumnHeader[] { this.name, this.value, this.recordtype, this.line });
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
            this.name.Width = 150;
            this.value.Text = "记录值";
            this.value.Width = 150;
            this.recordtype.Text = "记录类型";
            this.line.Text = "记录线路";
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
            this.label1.Text = "动态解析";
            this.contextMenuStrip1.Items.AddRange(new ToolStripItem[] { this.启用动态解析ToolStripMenuItem, this.禁用动态解析ToolStripMenuItem, this.查看日志ToolStripMenuItem });
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new Size(0x99, 0x5c);
            this.启用动态解析ToolStripMenuItem.Name = "启用动态解析ToolStripMenuItem";
            this.启用动态解析ToolStripMenuItem.Size = new Size(0x98, 0x16);
            this.启用动态解析ToolStripMenuItem.Text = "启用动态解析";
            this.启用动态解析ToolStripMenuItem.Click += new EventHandler(this.启用动态解析ToolStripMenuItem_Click);
            this.禁用动态解析ToolStripMenuItem.Name = "禁用动态解析ToolStripMenuItem";
            this.禁用动态解析ToolStripMenuItem.Size = new Size(0x98, 0x16);
            this.禁用动态解析ToolStripMenuItem.Text = "禁用动态解析";
            this.禁用动态解析ToolStripMenuItem.Click += new EventHandler(this.禁用动态解析ToolStripMenuItem_Click);
            this.查看日志ToolStripMenuItem.Name = "查看日志ToolStripMenuItem";
            this.查看日志ToolStripMenuItem.Size = new Size(0x98, 0x16);
            this.查看日志ToolStripMenuItem.Text = "查看日志";
            this.查看日志ToolStripMenuItem.Click += new EventHandler(this.查看日志ToolStripMenuItem_Click);
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.ClientSize = new Size(0x29f, 0x16f);
            base.Controls.Add(this.panel1);
            base.Controls.Add(this.lvRecords);
            base.Controls.Add(this.statusStrip1);
            base.Name = "FDdns";
            this.Text = "动态解析-DNSPodClientLite";
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
                    if (tag.IsDdns)
                    {
                        this.contextMenuStrip1.Items[0].Visible = false;
                        this.contextMenuStrip1.Items[1].Visible = true;
                        this.contextMenuStrip1.Items[2].Visible = true;
                    }
                    else
                    {
                        this.contextMenuStrip1.Items[0].Visible = true;
                        this.contextMenuStrip1.Items[1].Visible = false;
                        this.contextMenuStrip1.Items[2].Visible = false;
                    }
                    this.contextMenuStrip1.Show(this.lvRecords, e.Location);
                }
            }
        }

        private void RemoveInvalidDdns()
        {
            Predicate<Config.DDNSConfig> match = null;
            Predicate<Config.DDNSConfig> predicate4 = null;
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
                using (List<Config.DDNSConfig>.Enumerator enumerator = this.fLogin.Config.GetDdnses().FindAll(match).GetEnumerator())
                {
                    Predicate<Api.Record> predicate3 = null;
                    Config.DDNSConfig cfg;
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
                            this.fLogin.Config.RemoveDdns(cfg.RecordId);
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
                string str2 = string.Format("{0}-{1}.log", "ddns", DateTime.Now.ToString("yyyy-MM-dd"));
                Process.Start(Path.Combine(str, str2));
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                this._logger.Error("fddns.showlog has an error:{0}", new object[] { exception });
            }
        }

        private void 禁用动态解析ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.lvRecords.SelectedItems.Count >= 1)
                {
                    Api.Record tag = (Api.Record) this.lvRecords.SelectedItems[0].Tag;
                    this.fLogin.Config.RemoveDdns(tag.RecordId);
                    this.fLogin.Config.Save();
                    this.BindData();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                this._logger.Error("fddns.disable_ddns has an error:{0}", new object[] { exception });
            }
        }

        private void 启用动态解析ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.lvRecords.SelectedItems.Count >= 1)
                {
                    Api.Record tag = (Api.Record) this.lvRecords.SelectedItems[0].Tag;
                    Config.DDNSConfig config2 = new Config.DDNSConfig {
                        Domain = this.domain.Name,
                        DomainId = this.domain.DomainId,
                        RecordId = tag.RecordId,
                        Subdomain = tag.Name
                    };
                    Config.DDNSConfig item = config2;
                    this.fLogin.Config.AddDdns(item);
                    this.fLogin.Api.Ddns(this.domain.DomainId, tag.RecordId, this.fLogin.Ddns.LastIp);
                    new Logger("ddns").Info("change ip:{0}.{1}({2})-{3}", new object[] { tag.Name, this.domain.Name, tag.RecordId, this.fLogin.Ddns.LastIp });
                    this.fLogin.Config.Save();
                    this.BindData();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                this._logger.Error("fddns.enable_ddns has an error:{0}", new object[] { exception });
            }
        }
    }
}

