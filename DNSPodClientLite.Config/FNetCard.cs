using System;
using System.ComponentModel;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Windows.Forms;

namespace DNSPodClientLite
{
    public class FNetCard : Form
    {
        private Button button1;
        private ComboBox comboBox1;
        private IContainer components;
        private Label label1;
        private Panel panel1;

        public FNetCard()
        {
            components = null;
            components = null;
            InitializeComponent();
        }


        private void Button1_Click(object sender, EventArgs e)
        {
            string text = comboBox1.Text;
            AppStatus.Default.Config.NetCardId = text.Substring(0, text.IndexOf("$"));
            AppStatus.Default.Config.Save();

            MessageBox.Show("绑定成功，重启服务后生效");
            base.Close();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
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
                    comboBox1.Items.Add(item);
                }
            }
            comboBox1.SelectedIndex = 0;
        }

        private void InitializeComponent()
        {
            panel1 = new Panel();
            label1 = new Label();
            comboBox1 = new ComboBox();
            button1 = new Button();
            panel1.SuspendLayout();
            base.SuspendLayout();
            panel1.BackColor = Color.DeepSkyBlue;
            panel1.Controls.Add(label1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(0x239, 0x2b);
            panel1.TabIndex = 8;
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.White;
            label1.Location = new Point(12, 10);
            label1.Name = "label1";
            label1.Size = new Size(0x60, 0x19);
            label1.TabIndex = 1;
            label1.Text = "网卡设置";
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(12, 60);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(0x221, 20);
            comboBox1.TabIndex = 9;
            button1.Location = new Point(480, 0x6c);
            button1.Name = "button1";
            button1.Size = new Size(0x4b, 0x17);
            button1.TabIndex = 10;
            button1.Text = "绑定";
            button1.UseVisualStyleBackColor = true;
            button1.Click += new EventHandler(Button1_Click);
            base.ClientSize = new Size(0x239, 0x93);
            base.Controls.Add(button1);
            base.Controls.Add(comboBox1);
            base.Controls.Add(panel1);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "FNetCard";
            Text = "网卡设置";
            base.Load += new EventHandler(FNetCard_Load);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            base.ResumeLayout(false);
        }
    }
}

