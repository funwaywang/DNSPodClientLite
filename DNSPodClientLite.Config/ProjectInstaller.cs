using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace DNSPodClientLite
{
    [RunInstaller(true)]
    public class ProjectInstaller : Installer
    {
        private IContainer components = null;
        private ServiceInstaller serviceInstaller1;
        private ServiceProcessInstaller serviceProcessInstaller1;

        public ProjectInstaller()
        {
            InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            serviceProcessInstaller1 = new ServiceProcessInstaller
            {
                Account = ServiceAccount.LocalSystem,
                Password = null,
                Username = null
            };
            serviceInstaller1 = new ServiceInstaller
            {
                Description = "DNSPodClientLite service",
                ServiceName = "DNSPodLite",
                StartType = ServiceStartMode.Automatic
            };

            Installers.AddRange(new Installer[] { serviceProcessInstaller1, serviceInstaller1 });
        }
    }
}

