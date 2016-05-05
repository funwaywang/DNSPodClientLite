namespace DNSPodClientLite
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Net;
    using System.Windows.Forms;

    public class FMonitorSetting : Form
    {
        private Button btnCanel;
        private Button btnOk;
        private CheckBox chkQiehuan;
        private ComboBox cmbMonitorInterval;
        private IContainer components;
        private Api.Domain domain;
        private FLogin fLogin;
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
        private Api.Record record;
        private TextBox txtBakvalue;
        private TextBox txtPort;

        public FMonitorSetting()
        {
            this.components = null;
            this.components = null;
            this.InitializeComponent();
            this.cmbMonitorInterval.SelectedIndex = 0;
        }

        public FMonitorSetting(Api.Domain domain, FLogin fLogin, Api.Record record) : this()
        {
            this.domain = domain;
            this.fLogin = fLogin;
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
                if (!int.TryParse(this.txtPort.Text, out num))
                {
                    MessageBox.Show("端口填写不正确");
                    return;
                }
                if (this.chkQiehuan.Checked && (int.TryParse(this.txtBakvalue.Text, out num2) || !IPAddress.TryParse(this.txtBakvalue.Text, out address)))
                {
                    MessageBox.Show("备用IP填写不正确");
                    return;
                }
                Config.MonitorConfig config2 = new Config.MonitorConfig {
                    Domain = this.domain.Name,
                    Subdomain = this.record.Name,
                    Ip = this.record.Value,
                    Status = "unknow",
                    Port = num,
                    DomainId = this.domain.DomainId,
                    RecordId = this.record.RecordId,
                    BakValue = this.txtBakvalue.Text,
                    Line = this.record.Line,
                    Qiehuan = this.chkQiehuan.Checked,
                    SmartQiehuan = false,
                    MonitorInteval = int.Parse(this.cmbMonitorInterval.Text)
                };
                Config.MonitorConfig item = config2;
                this.fLogin.Config.AddMonitor(item);
                this.fLogin.Monitor.StartMonitor(this.record.RecordId);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                new Logger("ui").Error("fmonitor.enableddns has an error:{0}", new object[] { exception });
            }
            base.Close();
        }

        private void chkQiehuan_CheckedChanged(object sender, EventArgs e)
        {
            this.txtBakvalue.Enabled = this.chkQiehuan.Checked;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void FMonitorSetting_Load(object sender, EventArgs e)
        {
            this.lblIp.Text = this.record.Value;
            this.lblSubdomain.Text = this.record.Name + "." + this.domain.Name;
            this.lblLine.Text = this.record.Line;
        }

        private void InitializeComponent()
        {
            this.panel1 = new Panel();
            this.label1 = new Label();
            this.label2 = new Label();
            this.label3 = new Label();
            this.label4 = new Label();
            this.label5 = new Label();
            this.label6 = new Label();
            this.label7 = new Label();
            this.label8 = new Label();
            this.lblSubdomain = new Label();
            this.lblIp = new Label();
            this.lblLine = new Label();
            this.txtPort = new TextBox();
            this.cmbMonitorInterval = new ComboBox();
            this.label9 = new Label();
            this.txtBakvalue = new TextBox();
            this.chkQiehuan = new CheckBox();
            this.btnOk = new Button();
            this.btnCanel = new Button();
            this.panel1.SuspendLayout();
            base.SuspendLayout();
            this.panel1.BackColor = Color.DeepSkyBlue;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = DockStyle.Top;
            this.panel1.Location = new Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0x127, 40);
            this.panel1.TabIndex = 7;
            this.label1.AutoSize = true;
            this.label1.Font = new Font("宋体", 15.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.label1.ForeColor = Color.White;
            this.label1.Location = new Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x5e, 0x15);
            this.label1.TabIndex = 1;
            this.label1.Text = "监控设置";
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x10, 0x42);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x35, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "子域名：";
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0x10, 0x5f);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x35, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "记录值：";
            this.label4.AutoSize = true;
            this.label4.Location = new Point(0x1c, 0x7c);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x29, 12);
            this.label4.TabIndex = 10;
            this.label4.Text = "线路：";
            this.label5.AutoSize = true;
            this.label5.Location = new Point(0x1c, 0x99);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x29, 12);
            this.label5.TabIndex = 11;
            this.label5.Text = "端口：";
            this.label6.AutoSize = true;
            this.label6.Location = new Point(4, 0xb6);
            this.label6.Name = "label6";
            this.label6.Size = new Size(0x41, 12);
            this.label6.TabIndex = 12;
            this.label6.Text = "监控间隔：";
            this.label7.AutoSize = true;
            this.label7.Location = new Point(0x10, 0xed);
            this.label7.Name = "label7";
            this.label7.Size = new Size(0x35, 12);
            this.label7.TabIndex = 13;
            this.label7.Text = "备用IP：";
            this.label8.AutoSize = true;
            this.label8.Location = new Point(4, 0xd3);
            this.label8.Name = "label8";
            this.label8.Size = new Size(0x41, 12);
            this.label8.TabIndex = 14;
            this.label8.Text = "是否切换：";
            this.lblSubdomain.AutoSize = true;
            this.lblSubdomain.Location = new Point(0x4b, 0x42);
            this.lblSubdomain.Name = "lblSubdomain";
            this.lblSubdomain.Size = new Size(0x4d, 12);
            this.lblSubdomain.TabIndex = 15;
            this.lblSubdomain.Text = "lblSubdomain";
            this.lblIp.AutoSize = true;
            this.lblIp.Location = new Point(0x4b, 0x5f);
            this.lblIp.Name = "lblIp";
            this.lblIp.Size = new Size(0x23, 12);
            this.lblIp.TabIndex = 0x10;
            this.lblIp.Text = "lblIp";
            this.lblLine.AutoSize = true;
            this.lblLine.Location = new Point(0x4b, 0x7c);
            this.lblLine.Name = "lblLine";
            this.lblLine.Size = new Size(0x2f, 12);
            this.lblLine.TabIndex = 0x11;
            this.lblLine.Text = "lblLine";
            this.txtPort.Location = new Point(0x4f, 0x95);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new Size(0x8b, 0x15);
            this.txtPort.TabIndex = 0x12;
            this.txtPort.Text = "80";
            this.cmbMonitorInterval.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbMonitorInterval.FormattingEnabled = true;
            this.cmbMonitorInterval.Items.AddRange(new object[] { "1", "5", "10" });
            this.cmbMonitorInterval.Location = new Point(0x4f, 0xb2);
            this.cmbMonitorInterval.Name = "cmbMonitorInterval";
            this.cmbMonitorInterval.Size = new Size(0x79, 20);
            this.cmbMonitorInterval.TabIndex = 0x13;
            this.label9.AutoSize = true;
            this.label9.Location = new Point(0xcf, 0xba);
            this.label9.Name = "label9";
            this.label9.Size = new Size(0x1d, 12);
            this.label9.TabIndex = 20;
            this.label9.Text = "分钟";
            this.txtBakvalue.Enabled = false;
            this.txtBakvalue.Location = new Point(0x4f, 0xe9);
            this.txtBakvalue.Name = "txtBakvalue";
            this.txtBakvalue.Size = new Size(0x8b, 0x15);
            this.txtBakvalue.TabIndex = 0x15;
            this.chkQiehuan.AutoSize = true;
            this.chkQiehuan.Location = new Point(0x4f, 0xd1);
            this.chkQiehuan.Name = "chkQiehuan";
            this.chkQiehuan.Size = new Size(0x24, 0x10);
            this.chkQiehuan.TabIndex = 0x16;
            this.chkQiehuan.Text = "是";
            this.chkQiehuan.UseVisualStyleBackColor = true;
            this.chkQiehuan.CheckedChanged += new EventHandler(this.chkQiehuan_CheckedChanged);
            this.btnOk.Location = new Point(0x7f, 0x109);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new Size(0x4b, 0x17);
            this.btnOk.TabIndex = 0x17;
            this.btnOk.Text = "确定";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new EventHandler(this.btnOk_Click);
            this.btnCanel.Location = new Point(0xd0, 0x109);
            this.btnCanel.Name = "btnCanel";
            this.btnCanel.Size = new Size(0x4b, 0x17);
            this.btnCanel.TabIndex = 0x18;
            this.btnCanel.Text = "取消";
            this.btnCanel.UseVisualStyleBackColor = true;
            this.btnCanel.Click += new EventHandler(this.btnCanel_Click);
            base.ClientSize = new Size(0x127, 0x12a);
            base.Controls.Add(this.btnCanel);
            base.Controls.Add(this.btnOk);
            base.Controls.Add(this.chkQiehuan);
            base.Controls.Add(this.txtBakvalue);
            base.Controls.Add(this.label9);
            base.Controls.Add(this.cmbMonitorInterval);
            base.Controls.Add(this.txtPort);
            base.Controls.Add(this.lblLine);
            base.Controls.Add(this.lblIp);
            base.Controls.Add(this.lblSubdomain);
            base.Controls.Add(this.label8);
            base.Controls.Add(this.label7);
            base.Controls.Add(this.label6);
            base.Controls.Add(this.label5);
            base.Controls.Add(this.label4);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.panel1);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "FMonitorSetting";
            this.Text = "监控设置-DNSPodClientLite";
            base.Load += new EventHandler(this.FMonitorSetting_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }
    }
}

