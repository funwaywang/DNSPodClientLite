using DNSPodClientLite.ConfigTool;
using System;
using System.Windows.Forms;

namespace DNSPodClientLite
{
    static class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            Logger.Init();
            MonitorHistory.Init();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var dialog = new LoginDialog();
            if(dialog.ShowDialog() == DialogResult.OK)
            {
                Application.Run(new ControlPanelForm());
            }
        }
    }
}

