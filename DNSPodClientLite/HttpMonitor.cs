namespace DNSPodClientLite
{
    using System;
    using System.Collections.Generic;
    using System.Net.Sockets;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;

    public class HttpMonitor
    {
        private Config _cfg;
        private Logger _logger = new Logger("monitor");
        private const string HTTPDATA = "GET / HTTP/1.1\r\nHOST:{0}\r\n\r\n";
        private Action<string> m_Info;
        private DStatusChanged m_StatusChanged;

        public event Action<string> Info
        {
            add
            {
                Action<string> action2;
                Action<string> info = this.m_Info;
                do
                {
                    action2 = info;
                    Action<string> action3 = (Action<string>) Delegate.Combine(action2, value);
                    info = Interlocked.CompareExchange<Action<string>>(ref this.m_Info, action3, action2);
                }
                while (info != action2);
            }
            remove
            {
                Action<string> action2;
                Action<string> info = this.m_Info;
                do
                {
                    action2 = info;
                    Action<string> action3 = (Action<string>) Delegate.Remove(action2, value);
                    info = Interlocked.CompareExchange<Action<string>>(ref this.m_Info, action3, action2);
                }
                while (info != action2);
            }
        }

        public event DStatusChanged StatusChanged
        {
            add
            {
                DStatusChanged changed2;
                DStatusChanged statusChanged = this.m_StatusChanged;
                do
                {
                    changed2 = statusChanged;
                    DStatusChanged changed3 = (DStatusChanged) Delegate.Combine(changed2, value);
                    statusChanged = Interlocked.CompareExchange<DStatusChanged>(ref this.m_StatusChanged, changed3, changed2);
                }
                while (statusChanged != changed2);
            }
            remove
            {
                DStatusChanged changed2;
                DStatusChanged statusChanged = this.m_StatusChanged;
                do
                {
                    changed2 = statusChanged;
                    DStatusChanged changed3 = (DStatusChanged) Delegate.Remove(changed2, value);
                    statusChanged = Interlocked.CompareExchange<DStatusChanged>(ref this.m_StatusChanged, changed3, changed2);
                }
                while (statusChanged != changed2);
            }
        }

        public HttpMonitor(Config cfg)
        {
            this._cfg = cfg;
        }

        private string CheckHttp(MonitorHistory h, Config.MonitorConfig item)
        {
            string str = "unknow";
            int result = 0x3e7;
            try
            {
                TcpClient client2 = new TcpClient {
                    ReceiveTimeout = 0x1388,
                    SendTimeout = 0x1388
                };
                TcpClient client = client2;
                client.Connect(item.Ip, item.Port);
                NetworkStream stream = client.GetStream();
                string domain = item.Domain;
                if (item.Subdomain != "@")
                {
                    domain = string.Format("{0}.{1}", item.Subdomain, item.Domain);
                }
                string s = string.Format("GET / HTTP/1.1\r\nHOST:{0}\r\n\r\n", domain);
                byte[] bytes = Encoding.ASCII.GetBytes(s);
                stream.Write(bytes, 0, bytes.Length);
                byte[] buffer = new byte[0x400];
                int count = stream.Read(buffer, 0, 0x400);
                if (!int.TryParse(Encoding.ASCII.GetString(buffer, 0, count).Substring(9, 3), out result))
                {
                    str = "down";
                }
                else
                {
                    str = "ok";
                }
                client.Close();
            }
            catch (Exception)
            {
                str = "down";
            }
            h.WriteData(DateTime.Now, str == "down");
            Action<string> info = this.m_Info;
            if (info != null)
            {
                info(string.Format("{0}：监控结果:ip={1},status={2},statuscode={3}", new object[] { DateTime.Now.ToLongTimeString(), item.Ip, str, result }));
            }
            return str;
        }

        public void Start()
        {
            List<Config.MonitorConfig> monitors = this._cfg.GetMonitors();
            foreach (Config.MonitorConfig config in monitors)
            {
                this.StartMonitor(config.RecordId);
            }
        }

        public void StartMonitor(int recordid)
        {
            new Thread(new ParameterizedThreadStart(this.ThreadProcess)) { IsBackground = true }.Start(recordid);
        }

        public void ThreadProcess(object state)
        {
            Predicate<Config.MonitorConfig> predicate2 = null;
            Predicate<Config.MonitorConfig> match = null;
            int recordid = (int) state;
            MonitorHistory h = MonitorHistory.Get(recordid);
            while (true)
            {
                try
                {
                    if (match == null)
                    {
                        if (predicate2 == null)
                        {
                            predicate2 = x => x.RecordId == recordid;
                        }
                        match = predicate2;
                    }
                    Config.MonitorConfig item = this._cfg.GetMonitors().Find(match);
                    if (item == null)
                    {
                        return;
                    }
                    string status = this.CheckHttp(h, item);
                    if (status != item.Status)
                    {
                        int num = 0;
                        while (num < 3)
                        {
                            if (this.CheckHttp(h, item) != status)
                            {
                                break;
                            }
                            num++;
                            Thread.Sleep(0x2710);
                        }
                        if (num >= 2)
                        {
                            DStatusChanged statusChanged = this.m_StatusChanged;
                            if (statusChanged != null)
                            {
                                statusChanged(item.RecordId, status);
                            }
                        }
                    }
                    Thread.Sleep((int) ((item.MonitorInteval * 60) * 0x3e8));
                }
                catch (SocketException)
                {
                }
                catch (Exception exception)
                {
                    this._logger.Error("ThreadProcess error:{0}", new object[] { exception });
                }
            }
        }

        public delegate void DStatusChanged(int recordid, string status);
    }
}

