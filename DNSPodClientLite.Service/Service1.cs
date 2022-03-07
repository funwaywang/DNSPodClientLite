using DNSPodClientLite;
using System;
using System.ComponentModel;
using System.ServiceProcess;

namespace DnsPodClient.Service
{
    internal class Service1 : ServiceBase
    {
        private IContainer components = null;

        public Service1()
        {
            InitializeComponent();
        }

        public DnsPodApi Api { get; set; }
        public Config Config { get; set; }
        public DDns Ddns { get; set; }
        public HttpMonitor Monitor { get; set; }

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
            ServiceName = "DNSPodLite";
        }

        private void Monitor_StatusChanged(object sender, RecordStatusChangedEventArgs e)
        {
            Logger logger = new Logger("monitor");
            Config.MonitorConfig config = Config.GetMonitors().Find(i => i.RecordId == e.RecordId);
            if (config != null)
            {
                logger.Info("状态变化:{0}.{1}({2}):from {3} to {4}", new object[] { config.Subdomain, config.Domain, config.Ip, config.Status, e.Status });
                config.Status = e.Status;
                Config.Save();
                if (config.Qiehuan)
                {
                    if (e.Status == "down")
                    {
                        if (config.SmartQiehuan)
                        {
                            Config.MonitorConfig backMonitor = Config.GetBackMonitor(e.RecordId);
                            if (backMonitor != null)
                            {
                                Api.ChangeIP(config.DomainId, e.RecordId, backMonitor.Ip);
                                logger.Info("宕机切换-autobak:{0}.{1}(2):{3}", new object[] { config.Subdomain, config.Domain, config.RecordId, backMonitor.Ip });
                            }
                            else
                            {
                                logger.Info("宕机切换-未找到可用IP:{0}.{1}(2)", new object[] { config.Subdomain, config.Domain, config.RecordId });
                            }
                        }
                        else
                        {
                            Api.ChangeIP(config.DomainId, e.RecordId, config.BakValue);
                            logger.Info("宕机切换-to-bakvalue:{0}.{1}(2):{3}", new object[] { config.Subdomain, config.Domain, config.RecordId, config.BakValue });
                        }
                    }
                    else
                    {
                        Api.ChangeIP(config.DomainId, e.RecordId, config.Ip);
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
            logger.Info("begin start", new object[0]);
            try
            {
                Config = Config.Load("service.xml");
                Api = new DnsPodApi(Config, Config.GetLocal());
                Ddns = new DDns(Config.LastIp, Config.GetLocal());
                Ddns.IPChanged += Ddns_IPChangedNotified;
                Ddns.Start();

                Monitor = new HttpMonitor(Config);
                Monitor.StatusChanged += Monitor_StatusChanged;
                Monitor.Start();

                logger.Info("end start", new object[0]);
            }
            catch (Exception exception)
            {
                logger.Error("start error:{0}", new object[] { exception });
            }
        }

        private void Ddns_IPChangedNotified(object sender, IpChangedEventArgs e)
        {
            Logger logger = new Logger("ddns");
            logger.Info("changing ip to {0}", new object[] { e.IP });

            try
            {
                foreach (Config.DDNSConfig config in Config.GetDdnses())
                {
                    logger.Info("change ip 4:{0}", new object[] { config.Subdomain });
                    Api.UpdateDns(config.DomainId, config.RecordId, e.IP);
                    logger.Info("动态IP修改:{0}.{1}({2})-{3}", new object[] { config.Subdomain, config.Domain, config.RecordId, e.IP });
                }

                Config.LastIp = e.IP;
                Config.Save();
                logger.Info("ip changed! {0}", new object[] { e.IP });
            }
            catch (Exception exception)
            {
                logger.Info("Update DNS fail.");
                logger.Error(exception.Message);
            }
        }

        protected override void OnStop()
        {
        }
    }
}

