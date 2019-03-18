using System;
using System.Collections.Generic;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DNSPodClientLite.Utility
{
    public static class ServiceHelper
    {
        public static void InstallService(string serviceName, string executable)
        {
            string[] args = new string[] { executable };
            ServiceController controller = new ServiceController(serviceName);
            if (!ServiceIsExisted(serviceName))
            {
                try
                {
                    ManagedInstallerClass.InstallHelper(args);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("该服务已经存在, 不用重复安装.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public static void RestartService(string serviceName, int timeoutMilliseconds, Logger logger)
        {
            TimeSpan span;
            Exception exception;
            ServiceController controller = new ServiceController(serviceName);
            try
            {
                span = TimeSpan.FromMilliseconds(timeoutMilliseconds);
                controller.Stop();
                controller.WaitForStatus(ServiceControllerStatus.Stopped, span);
            }
            catch (Exception exception1)
            {
                exception = exception1;
                logger.Error("服务停止失败:{0}", new object[] { exception.Message });
            }
            try
            {
                span = TimeSpan.FromMilliseconds(timeoutMilliseconds);
                controller.Start();
                controller.WaitForStatus(ServiceControllerStatus.Running, span);
            }
            catch (Exception exception2)
            {
                exception = exception2;
                logger.Error("服务启动失败:{0}", new object[] { exception.Message });
                MessageBox.Show("重启失败，请手工重启DNSPodLite服务。" + exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static bool ServiceIsExisted(string svcName)
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
    }
}
