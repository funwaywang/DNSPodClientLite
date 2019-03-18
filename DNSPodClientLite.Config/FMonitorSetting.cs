using System;
using System.ComponentModel;
using System.Drawing;
using System.Net;
using System.Windows.Forms;

namespace DNSPodClientLite
{
    public class FMonitorSetting : Form
    {
        private Button btnCanel;
        private Button btnOk;
        private CheckBox chkQiehuan;
        private ComboBox cmbMonitorInterval;
        private IContainer components;
        private DnsPodApi.Domain domain;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private Label lblIp;
        private Label lblLine;
        private Label lblSubdomain;
        private Panel panel1;
        private DnsPodApi.Record record;
        private TextBox txtBakvalue;
        private TextBox txtPort;

        public FMonitorSetting()
        {
            components = null;
            components = null;
            InitializeComponent();
            cmbMonitorInterval.SelectedIndex = 0;
        }

        public FMonitorSetting(DnsPodApi.Domain domain, DnsPodApi.Record record) : this()
        {
            this.domain = domain;
            this.record = record;
        }

        private void btnCanel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                int num;
                IPAddress address;
                int num2;
                if (!int.TryParse(txtPort.Text, out num))
                {
                    MessageBox.Show("端口填写不正确");
                    return;
                }
                if (chkQiehuan.Checked && (int.TryParse(txtBakvalue.Text, out num2) || !IPAddress.TryParse(txtBakvalue.Text, out address)))
                {
                    MessageBox.Show("备用IP填写不正确");
                    return;
                }
                Config.MonitorConfig config2 = new Config.MonitorConfig
                {
                    Domain = domain.Name,
                    Subdomain = record.Name,
                    Ip = record.Value,
                    Status = "unknow",
                    Port = num,
                    DomainId = domain.DomainId,
                    RecordId = record.RecordId,
                    BakValue = txtBakvalue.Text,
                    Line = record.Line,
                    Qiehuan = chkQiehuan.Checked,
                    SmartQiehuan = false,
                    MonitorInteval = int.Parse(cmbMonitorInterval.Text)
                };
                Config.MonitorConfig item = config2;
                AppStatus.Default.Config.AddMonitor(item);
                AppStatus.Default.Monitor.StartMonitor(record.RecordId);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                new Logger("ui").Error("fmonitor.enableddns has an error:{0}", new object[] { exception });
            }
            base.Close();
        }

        private void chkQiehuan_CheckedChanged(object sender, EventArgs e)
        {
            txtBakvalue.Enabled = chkQiehuan.Checked;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void FMonitorSetting_Load(object sender, EventArgs e)
        {
            lblIp.Text = record.Value;
            lblSubdomain.Text = record.Name + "." + domain.Name;
            lblLine.Text = record.Line;
        }

        private void InitializeComponent()
        {
            panel1 = new Panel();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            label8 = new Label();
            lblSubdomain = new Label();
            lblIp = new Label();
            lblLine = new Label();
            txtPort = new TextBox();
            cmbMonitorInterval = new ComboBox();
            label9 = new Label();
            txtBakvalue = new TextBox();
            chkQiehuan = new CheckBox();
            btnOk = new Button();
            btnCanel = new Button();
            panel1.SuspendLayout();
            base.SuspendLayout();
            panel1.BackColor = Color.DeepSkyBlue;
            panel1.Controls.Add(label1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(0x127, 40);
            panel1.TabIndex = 7;
            label1.AutoSize = true;
            label1.Font = new Font("宋体", 15.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.White;
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(0x5e, 0x15);
            label1.TabIndex = 1;
            label1.Text = "监控设置";
            label2.AutoSize = true;
            label2.Location = new Point(0x10, 0x42);
            label2.Name = "label2";
            label2.Size = new Size(0x35, 12);
            label2.TabIndex = 8;
            label2.Text = "子域名：";
            label3.AutoSize = true;
            label3.Location = new Point(0x10, 0x5f);
            label3.Name = "label3";
            label3.Size = new Size(0x35, 12);
            label3.TabIndex = 9;
            label3.Text = "记录值：";
            label4.AutoSize = true;
            label4.Location = new Point(0x1c, 0x7c);
            label4.Name = "label4";
            label4.Size = new Size(0x29, 12);
            label4.TabIndex = 10;
            label4.Text = "线路：";
            label5.AutoSize = true;
            label5.Location = new Point(0x1c, 0x99);
            label5.Name = "label5";
            label5.Size = new Size(0x29, 12);
            label5.TabIndex = 11;
            label5.Text = "端口：";
            label6.AutoSize = true;
            label6.Location = new Point(4, 0xb6);
            label6.Name = "label6";
            label6.Size = new Size(0x41, 12);
            label6.TabIndex = 12;
            label6.Text = "监控间隔：";
            label7.AutoSize = true;
            label7.Location = new Point(0x10, 0xed);
            label7.Name = "label7";
            label7.Size = new Size(0x35, 12);
            label7.TabIndex = 13;
            label7.Text = "备用IP：";
            label8.AutoSize = true;
            label8.Location = new Point(4, 0xd3);
            label8.Name = "label8";
            label8.Size = new Size(0x41, 12);
            label8.TabIndex = 14;
            label8.Text = "是否切换：";
            lblSubdomain.AutoSize = true;
            lblSubdomain.Location = new Point(0x4b, 0x42);
            lblSubdomain.Name = "lblSubdomain";
            lblSubdomain.Size = new Size(0x4d, 12);
            lblSubdomain.TabIndex = 15;
            lblSubdomain.Text = "lblSubdomain";
            lblIp.AutoSize = true;
            lblIp.Location = new Point(0x4b, 0x5f);
            lblIp.Name = "lblIp";
            lblIp.Size = new Size(0x23, 12);
            lblIp.TabIndex = 0x10;
            lblIp.Text = "lblIp";
            lblLine.AutoSize = true;
            lblLine.Location = new Point(0x4b, 0x7c);
            lblLine.Name = "lblLine";
            lblLine.Size = new Size(0x2f, 12);
            lblLine.TabIndex = 0x11;
            lblLine.Text = "lblLine";
            txtPort.Location = new Point(0x4f, 0x95);
            txtPort.Name = "txtPort";
            txtPort.Size = new Size(0x8b, 0x15);
            txtPort.TabIndex = 0x12;
            txtPort.Text = "80";
            cmbMonitorInterval.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbMonitorInterval.FormattingEnabled = true;
            cmbMonitorInterval.Items.AddRange(new object[] { "1", "5", "10" });
            cmbMonitorInterval.Location = new Point(0x4f, 0xb2);
            cmbMonitorInterval.Name = "cmbMonitorInterval";
            cmbMonitorInterval.Size = new Size(0x79, 20);
            cmbMonitorInterval.TabIndex = 0x13;
            label9.AutoSize = true;
            label9.Location = new Point(0xcf, 0xba);
            label9.Name = "label9";
            label9.Size = new Size(0x1d, 12);
            label9.TabIndex = 20;
            label9.Text = "分钟";
            txtBakvalue.Enabled = false;
            txtBakvalue.Location = new Point(0x4f, 0xe9);
            txtBakvalue.Name = "txtBakvalue";
            txtBakvalue.Size = new Size(0x8b, 0x15);
            txtBakvalue.TabIndex = 0x15;
            chkQiehuan.AutoSize = true;
            chkQiehuan.Location = new Point(0x4f, 0xd1);
            chkQiehuan.Name = "chkQiehuan";
            chkQiehuan.Size = new Size(0x24, 0x10);
            chkQiehuan.TabIndex = 0x16;
            chkQiehuan.Text = "是";
            chkQiehuan.UseVisualStyleBackColor = true;
            chkQiehuan.CheckedChanged += new EventHandler(chkQiehuan_CheckedChanged);
            btnOk.Location = new Point(0x7f, 0x109);
            btnOk.Name = "btnOk";
            btnOk.Size = new Size(0x4b, 0x17);
            btnOk.TabIndex = 0x17;
            btnOk.Text = "确定";
            btnOk.UseVisualStyleBackColor = true;
            btnOk.Click += new EventHandler(btnOk_Click);
            btnCanel.Location = new Point(0xd0, 0x109);
            btnCanel.Name = "btnCanel";
            btnCanel.Size = new Size(0x4b, 0x17);
            btnCanel.TabIndex = 0x18;
            btnCanel.Text = "取消";
            btnCanel.UseVisualStyleBackColor = true;
            btnCanel.Click += new EventHandler(btnCanel_Click);
            base.ClientSize = new Size(0x127, 0x12a);
            base.Controls.Add(btnCanel);
            base.Controls.Add(btnOk);
            base.Controls.Add(chkQiehuan);
            base.Controls.Add(txtBakvalue);
            base.Controls.Add(label9);
            base.Controls.Add(cmbMonitorInterval);
            base.Controls.Add(txtPort);
            base.Controls.Add(lblLine);
            base.Controls.Add(lblIp);
            base.Controls.Add(lblSubdomain);
            base.Controls.Add(label8);
            base.Controls.Add(label7);
            base.Controls.Add(label6);
            base.Controls.Add(label5);
            base.Controls.Add(label4);
            base.Controls.Add(label3);
            base.Controls.Add(label2);
            base.Controls.Add(panel1);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "FMonitorSetting";
            Text = "监控设置-DNSPodClientLite";
            base.Load += new EventHandler(FMonitorSetting_Load);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }
    }
}

