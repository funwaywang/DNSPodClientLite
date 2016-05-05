namespace DNSPodClientLite
{
    using System;
    using System.ComponentModel;
    using System.Configuration.Install;
    using System.Drawing;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.ServiceProcess;
    using System.Windows.Forms;

    public class FLogin : Form
    {
        private FDomainList _fdomainlist;
        private Logger _logger = new Logger("ui");
        private Button btnLogin;
        private IContainer components = null;
        public DDns Ddns;
        private Label label1;
        private Label lblLoginEmail;
        private Label lblLoginPassword;
        private Label lblMessage;
        public HttpMonitor Monitor;
        private Panel panel1;
        private TextBox txtLoginEmail;
        private TextBox txtLoginPassword;

        public FLogin()
        {
            this.InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            Action<string> action = null;
            Action<string> action2 = null;
            Action<string> action3 = null;
            Action<string> action4 = null;
            try
            {
                this.btnLogin.Text = "登录中，请等待...";
                this.btnLogin.Enabled = false;
                this.TruncateLog();
                if (this.Config == null)
                {
                    this.Config = new DNSPodClientLite.Config();
                }
                this.Config.LoginEmail = this.txtLoginEmail.Text.Trim();
                this.Config.LoginPassword = this.txtLoginPassword.Text;
                this.Config.Save();
                this.Api = new DNSPodClientLite.Api(this.Config.LoginEmail, this.Config.LoginPassword, this.Config.GetLocal());
                this.Ddns = new DDns(this.Config.LastIp, this.Config.GetLocal());
                if (action == null)
                {
                    if (action3 == null)
                    {
                        action3 = delegate (string msg) {
                            this.NotifyInfo(false, msg);
                        };
                    }
                    action = action3;
                }
                this.Ddns.Info += action;
                this.Ddns.Start();
                this.Monitor = new HttpMonitor(this.Config);
                if (action2 == null)
                {
                    if (action4 == null)
                    {
                        action4 = delegate (string msg) {
                            this.NotifyInfo(false, msg);
                        };
                    }
                    action2 = action4;
                }
                this.Monitor.Info += action2;
                this.Monitor.Start();
                this.ShowDomainList();
            }
            catch (Exception exception)
            {
                this._logger.Error("flogin.btnLogin_Click has an error:{0}", new object[] { exception });
                MessageBox.Show(exception.Message);
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

        private void FLogin_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.RestartService("DNSPodLite", 0xbb8);
        }

        private void FLogin_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.ServiceIsExisted("DNSPodLite"))
                {
                    this.lblMessage.Text = "服务未安装";
                    this.InstallService("DNSPodLite");
                }
                else
                {
                    this.lblMessage.Text = "服务已安装";
                }
                this.Config = DNSPodClientLite.Config.Load();
                if (this.Config != null)
                {
                    this.Api = new DNSPodClientLite.Api(this.Config.LoginEmail, this.Config.LoginPassword, this.Config.GetLocal());
                    this.txtLoginEmail.Text = this.Config.LoginEmail;
                    this.txtLoginPassword.Text = this.Config.LoginPassword;
                }
            }
            catch (Exception exception)
            {
                this._logger.Error("flogin.load has an error:{0}", new object[] { exception });
                MessageBox.Show(exception.Message);
            }
        }

        private void form_FormClosed(object sender, FormClosedEventArgs e)
        {
            base.Close();
        }

        private void InitializeComponent()
        {
            this.lblLoginEmail = new Label();
            this.txtLoginEmail = new TextBox();
            this.btnLogin = new Button();
            this.txtLoginPassword = new TextBox();
            this.lblLoginPassword = new Label();
            this.panel1 = new Panel();
            this.label1 = new Label();
            this.lblMessage = new Label();
            this.panel1.SuspendLayout();
            base.SuspendLayout();
            this.lblLoginEmail.AutoSize = true;
            this.lblLoginEmail.Location = new Point(0x18, 0x41);
            this.lblLoginEmail.Name = "lblLoginEmail";
            this.lblLoginEmail.Size = new Size(0x35, 12);
            this.lblLoginEmail.TabIndex = 0;
            this.lblLoginEmail.Text = "用户名：";
            this.txtLoginEmail.Location = new Point(0x5f, 0x3d);
            this.txtLoginEmail.Name = "txtLoginEmail";
            this.txtLoginEmail.Size = new Size(0xe8, 0x15);
            this.txtLoginEmail.TabIndex = 2;
            this.btnLogin.FlatStyle = FlatStyle.Flat;
            this.btnLogin.Location = new Point(0xc2, 140);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new Size(0x85, 0x19);
            this.btnLogin.TabIndex = 4;
            this.btnLogin.Text = "登录";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new EventHandler(this.btnLogin_Click);
            this.txtLoginPassword.Location = new Point(0x5f, 0x69);
            this.txtLoginPassword.Name = "txtLoginPassword";
            this.txtLoginPassword.PasswordChar = '*';
            this.txtLoginPassword.Size = new Size(0xe8, 0x15);
            this.txtLoginPassword.TabIndex = 3;
            this.lblLoginPassword.AutoSize = true;
            this.lblLoginPassword.Location = new Point(0x18, 0x6d);
            this.lblLoginPassword.Name = "lblLoginPassword";
            this.lblLoginPassword.Size = new Size(0x35, 12);
            this.lblLoginPassword.TabIndex = 1;
            this.lblLoginPassword.Text = "密　码：";
            this.panel1.BackColor = Color.DeepSkyBlue;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = DockStyle.Top;
            this.panel1.Location = new Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0x169, 0x2b);
            this.panel1.TabIndex = 5;
            this.label1.AutoSize = true;
            this.label1.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.label1.ForeColor = Color.White;
            this.label1.Location = new Point(12, 10);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x36, 0x19);
            this.label1.TabIndex = 1;
            this.label1.Text = "登录";
            this.lblMessage.AutoSize = true;
            this.lblMessage.Location = new Point(0x18, 0x92);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new Size(0x1d, 12);
            this.lblMessage.TabIndex = 6;
            this.lblMessage.Text = "ffff";
            base.ClientSize = new Size(0x169, 180);
            base.Controls.Add(this.lblMessage);
            base.Controls.Add(this.panel1);
            base.Controls.Add(this.btnLogin);
            base.Controls.Add(this.txtLoginPassword);
            base.Controls.Add(this.txtLoginEmail);
            base.Controls.Add(this.lblLoginPassword);
            base.Controls.Add(this.lblLoginEmail);
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "FLogin";
            this.Text = "登录-DNSPodClientLite";
            base.FormClosed += new FormClosedEventHandler(this.FLogin_FormClosed);
            base.Load += new EventHandler(this.FLogin_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void InstallService(string serviceName)
        {
            string[] args = new string[] { "DNSPodClientLite.exe" };
            ServiceController controller = new ServiceController(serviceName);
            if (!this.ServiceIsExisted(serviceName))
            {
                try
                {
                    ManagedInstallerClass.InstallHelper(args);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }
            else
            {
                MessageBox.Show("该服务已经存在，不用重复安装。");
            }
        }

        public void NotifyInfo(bool warn, string msg)
        {
            if (this._fdomainlist != null)
            {
                this._fdomainlist.NotifyInfo(warn, msg);
            }
        }

        public void RestartService(string serviceName, int timeoutMilliseconds)
        {
            TimeSpan span;
            Exception exception;
            ServiceController controller = new ServiceController(serviceName);
            try
            {
                span = TimeSpan.FromMilliseconds((double) timeoutMilliseconds);
                controller.Stop();
                controller.WaitForStatus(ServiceControllerStatus.Stopped, span);
            }
            catch (Exception exception1)
            {
                exception = exception1;
                this._logger.Error("服务停止失败:{0}", new object[] { exception.Message });
            }
            try
            {
                span = TimeSpan.FromMilliseconds((double) timeoutMilliseconds);
                controller.Start();
                controller.WaitForStatus(ServiceControllerStatus.Running, span);
            }
            catch (Exception exception2)
            {
                exception = exception2;
                this._logger.Error("服务启动失败:{0}", new object[] { exception.Message });
                MessageBox.Show("重启失败，请手工重启DNSPodLite服务。" + exception.Message);
            }
        }

        private bool ServiceIsExisted(string svcName)
        {
            ServiceController[] services = ServiceController.GetServices();
            foreach (ServiceController controller in services)
            {
                if (controller.ServiceName == svcName)
                {
                    return true;
                }
            }
            return false;
        }

        private void ShowDomainList()
        {
            this._fdomainlist = new FDomainList(this);
            this._fdomainlist.FormClosed += new FormClosedEventHandler(this.form_FormClosed);
            this._fdomainlist.Show();
            base.Hide();
        }

        private void TruncateLog()
        {
            try
            {
                DateTime time = DateTime.Now.AddDays(-7.0);
                string[] files = Directory.GetFiles(Path.Combine(Environment.CurrentDirectory, "log"));
                foreach (string str in files)
                {
                    FileInfo info = new FileInfo(str);
                    if (info.CreationTime < time)
                    {
                        info.Delete();
                    }
                }
            }
            catch (Exception exception)
            {
                this._logger.Error("truncate log han an error:{0}", new object[] { exception });
            }
        }

        public DNSPodClientLite.Api Api { get; set; }

        public DNSPodClientLite.Config Config { get; set; }
    }
}

