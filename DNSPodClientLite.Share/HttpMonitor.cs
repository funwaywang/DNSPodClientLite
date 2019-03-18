using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace DNSPodClientLite
{
    public class HttpMonitor
    {
        private Config _cfg;
        private Logger _logger = new Logger("monitor");
        private const string HTTPDATA = "GET / HTTP/1.1\r\nHOST:{0}\r\n\r\n";
        public event MessageEventHandler InformationReceived;
        public event RecordStatusChangedEventHandler StatusChanged;

        public HttpMonitor(Config cfg)
        {
            _cfg = cfg;
        }

        private string CheckHttp(MonitorHistory h, Config.MonitorConfig item)
        {
            string str = "unknow";
            int result = 0x3e7;
            try
            {
                TcpClient client2 = new TcpClient
                {
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

            InformationReceived?.Invoke(this, new MessageEventArgs($"{DateTime.Now.ToLongTimeString()}：监控结果:ip={item.Ip},status={str},statuscode={result}"));
            return str;
        }

        public void Start()
        {
            List<Config.MonitorConfig> monitors = _cfg.GetMonitors();
            foreach (Config.MonitorConfig config in monitors)
            {
                StartMonitor(config.RecordId);
            }
        }

        public void StartMonitor(int recordid)
        {
            new Thread(new ParameterizedThreadStart(ThreadProcess)) { IsBackground = true }.Start(recordid);
        }

        public void ThreadProcess(object state)
        {
            Predicate<Config.MonitorConfig> predicate2 = null;
            Predicate<Config.MonitorConfig> match = null;
            int recordid = (int)state;
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
                    Config.MonitorConfig item = _cfg.GetMonitors().Find(match);
                    if (item == null)
                    {
                        return;
                    }
                    string status = CheckHttp(h, item);
                    if (status != item.Status)
                    {
                        int num = 0;
                        while (num < 3)
                        {
                            if (CheckHttp(h, item) != status)
                            {
                                break;
                            }
                            num++;
                            Thread.Sleep(0x2710);
                        }
                        if (num >= 2)
                        {
                            StatusChanged?.Invoke(this, new RecordStatusChangedEventArgs(item.RecordId, status));
                        }
                    }
                    Thread.Sleep((item.MonitorInteval * 60) * 0x3e8);
                }
                catch (SocketException)
                {
                }
                catch (Exception exception)
                {
                    _logger.Error("ThreadProcess error:{0}", new object[] { exception });
                }
            }
        }
    }
}

