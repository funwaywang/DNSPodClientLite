namespace DNSPodClientLite
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Net.NetworkInformation;
    using System.Net.Sockets;
    using System.Windows.Forms;

    public class FNetCard : Form
    {
        private Button button1;
        private ComboBox comboBox1;
        private IContainer components;
        private FLogin fLogin;
        private Label label1;
        private Panel panel1;

        public FNetCard()
        {
            this.components = null;
            this.components = null;
            this.InitializeComponent();
        }

        public FNetCard(FLogin fLogin) : this()
        {
            this.fLogin = fLogin;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string text = this.comboBox1.Text;
            this.fLogin.Config.NetCardId = text.Substring(0, text.IndexOf("$"));
            this.fLogin.Config.Save();
            MessageBox.Show("绑定成功，重启服务后生效");
            base.Close();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void FNetCard_Load(object sender, EventArgs e)
        {
            foreach (NetworkInterface interface2 in NetworkInterface.GetAllNetworkInterfaces())
            {
                if ((interface2.NetworkInterfaceType == NetworkInterfaceType.Ethernet) && (interface2.OperationalStatus == OperationalStatus.Up))
                {
                    string item = string.Format("{0}${1}", interface2.Id, interface2.Name);
                    foreach (UnicastIPAddressInformation information in interface2.GetIPProperties().UnicastAddresses)
                    {
                        if (information.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            item = item + string.Format("[{0}]", information.Address.ToString());
                        }
                    }
                    this.comboBox1.Items.Add(item);
                }
            }
            this.comboBox1.SelectedIndex = 0;
        }

        private void InitializeComponent()
        {
            this.panel1 = new Panel();
            this.label1 = new Label();
            this.comboBox1 = new ComboBox();
            this.button1 = new Button();
            this.panel1.SuspendLayout();
            base.SuspendLayout();
            this.panel1.BackColor = Color.DeepSkyBlue;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = DockStyle.Top;
            this.panel1.Location = new Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0x239, 0x2b);
            this.panel1.TabIndex = 8;
            this.label1.AutoSize = true;
            this.label1.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.label1.ForeColor = Color.White;
            this.label1.Location = new Point(12, 10);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x60, 0x19);
            this.label1.TabIndex = 1;
            this.label1.Text = "网卡设置";
            this.comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new Point(12, 60);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new Size(0x221, 20);
            this.comboBox1.TabIndex = 9;
            this.button1.Location = new Point(480, 0x6c);
            this.button1.Name = "button1";
            this.button1.Size = new Size(0x4b, 0x17);
            this.button1.TabIndex = 10;
            this.button1.Text = "绑定";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new EventHandler(this.button1_Click);
            base.ClientSize = new Size(0x239, 0x93);
            base.Controls.Add(this.button1);
            base.Controls.Add(this.comboBox1);
            base.Controls.Add(this.panel1);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "FNetCard";
            this.Text = "网卡设置";
            base.Load += new EventHandler(this.FNetCard_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            base.ResumeLayout(false);
        }
    }
}

