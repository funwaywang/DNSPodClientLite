using DNSPodClientLite.ConfigTool.Properties;
using DNSPodClientLite.Utility;
using System;
using System.IO;
using System.Windows.Forms;

namespace DNSPodClientLite.ConfigTool
{
    public partial class LoginDialog : Form
    {
        private string Title;
        private string BtnLoginText;
        private readonly Logger logger = new Logger("ui");

        public LoginDialog()
        {
            InitializeComponent();

            Title = Text;
            Icon = Resources.App;
            BtnLoginText = BtnLogin.Text;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            try
            {
                if (!ServiceHelper.ServiceIsExisted("DNSPodLite"))
                {
                    Text = $"{Title} - 服务未安装";
                    ServiceHelper.InstallService("DNSPodLite", "DNSPodClientLite.exe");
                }
                else
                {
                    Text = $"{Title} - 服务已安装";
                }

                AppStatus.Default.LoadConfig();
                if (AppStatus.Default.Config != null)
                {
                    TxtEmail.Text = AppStatus.Default.Config.LoginEmail;
                    TxtPassword.Text = AppStatus.Default.Config.LoginPassword;
                    TxtTokenId.Text = AppStatus.Default.Config.LoginTokenId;
                    TxtTokenCode.Text = AppStatus.Default.Config.LoginTokenCode;
                }
            }
            catch (Exception exception)
            {
                logger.Error("flogin.load has an error:{0}", new object[] { exception });
                MessageBox.Show(exception.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                logger.Error("truncate log han an error:{0}", new object[] { exception });
            }
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                BtnLogin.Text = "登录中，请等待...";
                BtnLogin.Enabled = false;
                TruncateLog();

                if (tabControl1.SelectedIndex == 0)
                {
                    AppStatus.Default.UseTokenLoin(TxtTokenId.Text.Trim(), TxtTokenCode.Text.Trim());
                }
                else if (tabControl1.SelectedIndex == 1)
                {
                    AppStatus.Default.UseClassicLogin(TxtEmail.Text.Trim(), TxtPassword.Text.Trim());
                }
                else
                {
                    MessageBox.Show(this, "Unsupported Login Method!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                AppStatus.Default.Ddns.InformationReceived += InformationReceived;
                AppStatus.Default.Ddns.Start();
                AppStatus.Default.Monitor.InformationReceived += InformationReceived;
                AppStatus.Default.Monitor.Start();

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception exception)
            {
                BtnLogin.Enabled = true;
                BtnLogin.Text = BtnLoginText;

                logger.Error("flogin.btnLogin_Click has an error:{0}", new object[] { exception });
                MessageBox.Show(this, exception.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void InformationReceived(object sender, MessageEventArgs e)
        {
            if (IsDisposed || Disposing || !Visible)
            {
                return;
            }

            MessageBox.Show(this, e.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
