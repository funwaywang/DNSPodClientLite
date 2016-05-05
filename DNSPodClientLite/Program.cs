namespace DNSPodClientLite
{
    using System;
    using System.ServiceProcess;
    using System.Windows.Forms;

    internal static class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            if (Environment.UserInteractive)
            {
                Logger.Init();
                MonitorHistory.Init();
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FLogin());
            }
            else
            {
                ServiceBase.Run(new Service1());
            }
        }
    }
}

