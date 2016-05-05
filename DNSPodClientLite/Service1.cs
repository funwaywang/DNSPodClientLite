namespace DNSPodClientLite
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.ServiceProcess;

    internal class Service1 : ServiceBase
    {
        private IContainer components = null;

        public Service1()
        {
            this.InitializeComponent();
        }

        private void _ddns_IPChanged(string ip)
        {
            Logger logger = new Logger("ddns");
            this.Config.LastIp = ip;
            logger.Info("change ip 2.1:{0}", new object[] { ip });
            this.Config.Save();
            logger.Info("change ip 3:{0}", new object[] { ip });
            foreach (DNSPodClientLite.Config.DDNSConfig config in this.Config.GetDdnses())
            {
                logger.Info("change ip 4:{0}", new object[] { config.Subdomain });
                this.Api.Ddns(config.DomainId, config.RecordId, ip);
                logger.Info("动态IP修改:{0}.{1}({2})-{3}", new object[] { config.Subdomain, config.Domain, config.RecordId, ip });
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

        private void InitializeComponent()
        {
            base.ServiceName = "DNSPodLite";
        }

        private void Monitor_StatusChanged(int recordid, string status)
        {
            Logger logger = new Logger("monitor");
            DNSPodClientLite.Config.MonitorConfig config = this.Config.GetMonitors().Find(i => i.RecordId == recordid);
            if (config != null)
            {
                logger.Info("状态变化:{0}.{1}({2}):from {3} to {4}", new object[] { config.Subdomain, config.Domain, config.Ip, config.Status, status });
                config.Status = status;
                this.Config.Save();
                if (config.Qiehuan)
                {
                    if (status == "down")
                    {
                        if (config.SmartQiehuan)
                        {
                            DNSPodClientLite.Config.MonitorConfig backMonitor = this.Config.GetBackMonitor(recordid);
                            if (backMonitor != null)
                            {
                                this.Api.ChangeIP(config.DomainId, recordid, backMonitor.Ip);
                                logger.Info("宕机切换-autobak:{0}.{1}(2):{3}", new object[] { config.Subdomain, config.Domain, config.RecordId, backMonitor.Ip });
                            }
                            else
                            {
                                logger.Info("宕机切换-未找到可用IP:{0}.{1}(2)", new object[] { config.Subdomain, config.Domain, config.RecordId });
                            }
                        }
                        else
                        {
                            this.Api.ChangeIP(config.DomainId, recordid, config.BakValue);
                            logger.Info("宕机切换-to-bakvalue:{0}.{1}(2):{3}", new object[] { config.Subdomain, config.Domain, config.RecordId, config.BakValue });
                        }
                    }
                    else
                    {
                        this.Api.ChangeIP(config.DomainId, recordid, config.Ip);
                        logger.Info("宕机恢复-to-source:{0}.{1}(2):{3}", new object[] { config.Subdomain, config.Domain, config.RecordId, config.Ip });
                    }
                }
            }
        }

        protected override void OnStart(string[] args)
        {
            Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            Logger.Init();
            MonitorHistory.Init();
            Logger logger = new Logger("service");
            logger.Error("begin start", new object[0]);
            try
            {
                this.Config = DNSPodClientLite.Config.Load("service.xml");
                this.Api = new DNSPodClientLite.Api(this.Config.LoginEmail, this.Config.LoginPassword, this.Config.GetLocal());
                this.Ddns = new DDns(this.Config.LastIp, this.Config.GetLocal());
                this.Ddns.IPChanged += new Action<string>(this._ddns_IPChanged);
                this.Ddns.Start();
                this.Monitor = new HttpMonitor(this.Config);
                this.Monitor.StatusChanged += new HttpMonitor.DStatusChanged(this.Monitor_StatusChanged);
                this.Monitor.Start();
                logger.Error("end start", new object[0]);
            }
            catch (Exception exception)
            {
                logger.Error("start error:{0}", new object[] { exception });
            }
        }

        protected override void OnStop()
        {
        }

        public DNSPodClientLite.Api Api { get; set; }

        public DNSPodClientLite.Config Config { get; set; }

        public DDns Ddns { get; set; }

        public HttpMonitor Monitor { get; set; }
    }
}

