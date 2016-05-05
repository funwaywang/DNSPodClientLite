namespace DNSPodClientLite
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class FDomainList : Form
    {
        private Logger _logger;
        private Button btnImport;
        private Button btnNetCard;
        private ColumnHeader columnHeader1;
        private IContainer components;
        private ColumnHeader ddns;
        private FLogin fLogin;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private Label label1;
        private DoubleBufferListView lvDomains;
        private ListView lvMsg;
        private ColumnHeader monitor;
        private ColumnHeader name;
        private Panel panel1;

        public FDomainList()
        {
            this.components = null;
            this._logger = new Logger("ui");
            this.components = null;
            this.InitializeComponent();
        }

        public FDomainList(FLogin fLogin) : this()
        {
            this.fLogin = fLogin;
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            new FImportRecords().Show();
        }

        private void btnNetCard_Click(object sender, EventArgs e)
        {
            new FNetCard(this.fLogin).Show();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void FDomainList_Load(object sender, EventArgs e)
        {
            try
            {
                ImageList list3 = new ImageList {
                    ImageSize = new Size(1, 20)
                };
                ImageList list = list3;
                this.lvDomains.SmallImageList = list;
                ListViewExtender extender = new ListViewExtender(this.lvDomains);
                ListViewButtonColumn column = new ListViewButtonColumn(1);
                column.Click += new EventHandler<ListViewColumnMouseEventArgs>(this.OnButtonActionClick);
                extender.AddColumn(column);
                column = new ListViewButtonColumn(2);
                column.Click += new EventHandler<ListViewColumnMouseEventArgs>(this.OnButtonActionClick);
                extender.AddColumn(column);
                List<Api.Domain> domainList = this.fLogin.Api.GetDomainList();
                foreach (Api.Domain domain in domainList)
                {
                    ListViewItem item2 = new ListViewItem(domain.Name) {
                        Tag = domain
                    };
                    ListViewItem item = item2;
                    item.SubItems.Add("动态解析");
                    item.SubItems.Add("监控");
                    this.lvDomains.Items.Add(item);
                }
            }
            catch (Exception exception)
            {
                this._logger.Error("FDomainList_Load has an error:{0}", new object[] { exception });
                MessageBox.Show(exception.Message);
            }
        }

        private void InitializeComponent()
        {
            this.panel1 = new Panel();
            this.label1 = new Label();
            this.groupBox1 = new GroupBox();
            this.lvDomains = new DoubleBufferListView();
            this.name = new ColumnHeader();
            this.ddns = new ColumnHeader();
            this.monitor = new ColumnHeader();
            this.groupBox2 = new GroupBox();
            this.lvMsg = new ListView();
            this.columnHeader1 = new ColumnHeader();
            this.btnNetCard = new Button();
            this.btnImport = new Button();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            base.SuspendLayout();
            this.panel1.BackColor = Color.DeepSkyBlue;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = DockStyle.Top;
            this.panel1.Location = new Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0x3b2, 0x2a);
            this.panel1.TabIndex = 1;
            this.label1.AutoSize = true;
            this.label1.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.label1.ForeColor = Color.White;
            this.label1.Location = new Point(0x20, 10);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x60, 0x19);
            this.label1.TabIndex = 0;
            this.label1.Text = "控制面板";
            this.groupBox1.Anchor = AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.groupBox1.Controls.Add(this.lvDomains);
            this.groupBox1.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.groupBox1.Location = new Point(0, 0x30);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(360, 0x1d4);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "域名列表";
            this.lvDomains.Columns.AddRange(new ColumnHeader[] { this.name, this.ddns, this.monitor });
            this.lvDomains.Dock = DockStyle.Fill;
            this.lvDomains.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.lvDomains.FullRowSelect = true;
            this.lvDomains.Location = new Point(3, 0x11);
            this.lvDomains.Margin = new Padding(4);
            this.lvDomains.Name = "lvDomains";
            this.lvDomains.Size = new Size(0x162, 0x1c0);
            this.lvDomains.TabIndex = 0;
            this.lvDomains.UseCompatibleStateImageBehavior = false;
            this.lvDomains.View = View.Details;
            this.name.Text = "域名";
            this.name.Width = 150;
            this.ddns.Text = "动态解析";
            this.ddns.Width = 80;
            this.monitor.Text = "宕机监控";
            this.monitor.Width = 80;
            this.groupBox2.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.groupBox2.Controls.Add(this.lvMsg);
            this.groupBox2.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.groupBox2.Location = new Point(0x16f, 0x30);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(0x243, 0x1d4);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "信息列表";
            this.lvMsg.Columns.AddRange(new ColumnHeader[] { this.columnHeader1 });
            this.lvMsg.Dock = DockStyle.Fill;
            this.lvMsg.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lvMsg.Location = new Point(3, 0x11);
            this.lvMsg.Name = "lvMsg";
            this.lvMsg.Size = new Size(0x23d, 0x1c0);
            this.lvMsg.TabIndex = 0;
            this.lvMsg.UseCompatibleStateImageBehavior = false;
            this.lvMsg.View = View.Details;
            this.columnHeader1.Text = "信息";
            this.columnHeader1.Width = 500;
            this.btnNetCard.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.btnNetCard.Location = new Point(3, 0x30);
            this.btnNetCard.Name = "btnNetCard";
            this.btnNetCard.Size = new Size(0x49, 0x18);
            this.btnNetCard.TabIndex = 5;
            this.btnNetCard.Text = "网卡设置";
            this.btnNetCard.UseVisualStyleBackColor = true;
            this.btnNetCard.Visible = false;
            this.btnNetCard.Click += new EventHandler(this.btnNetCard_Click);
            this.btnImport.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.btnImport.Location = new Point(0x52, 0x30);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new Size(0x49, 0x18);
            this.btnImport.TabIndex = 6;
            this.btnImport.Text = "导入记录";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Visible = false;
            this.btnImport.Click += new EventHandler(this.btnImport_Click);
            base.AutoScaleDimensions = new SizeF(9f, 18f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x3b2, 0x204);
            base.Controls.Add(this.btnImport);
            base.Controls.Add(this.btnNetCard);
            base.Controls.Add(this.groupBox2);
            base.Controls.Add(this.groupBox1);
            base.Controls.Add(this.panel1);
            this.Font = new Font("Microsoft Sans Serif", 11.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            base.Margin = new Padding(4);
            base.Name = "FDomainList";
            this.Text = "控制面板-DNSPodClientLite";
            base.Load += new EventHandler(this.FDomainList_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        public void NotifyInfo(bool warn, string msg)
        {
            if (base.InvokeRequired)
            {
                DNotifyInfo method = new DNotifyInfo(this.NotifyInfo);
                base.Invoke(method, new object[] { warn, msg });
            }
            else
            {
                ListViewItem item2 = new ListViewItem(msg) {
                    ToolTipText = msg
                };
                ListViewItem item = item2;
                if (warn)
                {
                    item.BackColor = Color.Red;
                }
                lock (this.lvMsg)
                {
                    this.lvMsg.Items.Insert(0, item);
                    if (this.lvMsg.Items.Count > 0x3e8)
                    {
                        this.lvMsg.Items.Clear();
                    }
                }
            }
        }

        private void OnButtonActionClick(object sender, ListViewColumnMouseEventArgs e)
        {
            Api.Domain tag = (Api.Domain) e.Item.Tag;
            if (e.SubItem.Text == "动态解析")
            {
                new FDdns(this.fLogin, tag).Show();
            }
            else
            {
                new FMonitor(this.fLogin, tag).Show();
            }
        }

        private delegate void DNotifyInfo(bool warn, string msg);
    }
}

