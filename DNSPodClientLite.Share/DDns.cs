using DNSPodClientLite.Share;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace DNSPodClientLite
{
    public class DDns
    {
        private Logger _logger = new Logger("ddns");
        public event MessageEventHandler InformationReceived;
        public event IpChangedEventHandler IPChanged;

        public DDns(string lastIp, IPEndPoint local)
        {
            LastIp = lastIp;
            Local = local;
        }

        public void Start()
        {
            new Thread(new ParameterizedThreadStart(ThreadProcess)) { IsBackground = true }.Start();
        }

        public void ThreadProcess(object state)
        {
            while (true)
            {
                try
                {
                    IpHelper helper = new IpHelper(LastIp);
                    Thread thread = new Thread(new ParameterizedThreadStart(helper.GetIp));
                    thread.Start();
                    if (!thread.Join(TimeSpan.FromMinutes(1.0)))
                    {
                        _logger.Error("get ip timeout", new object[0]);
                    }
                    string ip = helper.IP;
                    UpdateIp(ip);
                }
                catch (Exception exception)
                {
                    _logger.Error("ThreadProcess error:{0}-{1}", new object[] { Local, exception });
                }
                finally
                {
                    Thread.Sleep(TimeSpan.FromMinutes(3.0));
                }
            }
        }

        private void UpdateIp(string ip)
        {
            _logger.Info("get ip:{0} - {1}", new object[] { LastIp, ip });
            InformationReceived?.Invoke(this, new MessageEventArgs($"{DateTime.Now.ToLongTimeString()}：动态域名获取本机最新IP：{ip}"));
            if (ip != LastIp)
            {
                var ipChangedArgs = new IpChangedEventArgs(LastIp, ip);
                LastIp = ip;
                _logger.Info("change ip 1:{0}", new object[] { ip });
                IPChanged?.Invoke(this, ipChangedArgs);
            }
        }

        public string LastIp { get; private set; }

        public EndPoint Local { get; set; }

        public async Task<string> RefreshIpAsync()
        {
            var provider = IpProviderManager.Default.GetRandomProvider();
            if (provider == null)
            {
                throw new Exception("Could not get any ip provider");
            }

            var ip = await provider.GetIpAsync();
            LastIp = ip;
            UpdateIp(ip);
            return ip;
        }
    }
}

