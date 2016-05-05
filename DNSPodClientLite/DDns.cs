namespace DNSPodClientLite
{
    using System;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class DDns
    {
        private Logger _logger = new Logger("ddns");
        private Action<string> m_Info;
        private Action<string> m_IPChanged;

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

        public event Action<string> IPChanged
        {
            add
            {
                Action<string> action2;
                Action<string> iPChanged = this.m_IPChanged;
                do
                {
                    action2 = iPChanged;
                    Action<string> action3 = (Action<string>) Delegate.Combine(action2, value);
                    iPChanged = Interlocked.CompareExchange<Action<string>>(ref this.m_IPChanged, action3, action2);
                }
                while (iPChanged != action2);
            }
            remove
            {
                Action<string> action2;
                Action<string> iPChanged = this.m_IPChanged;
                do
                {
                    action2 = iPChanged;
                    Action<string> action3 = (Action<string>) Delegate.Remove(action2, value);
                    iPChanged = Interlocked.CompareExchange<Action<string>>(ref this.m_IPChanged, action3, action2);
                }
                while (iPChanged != action2);
            }
        }

        public DDns(string lastIp, IPEndPoint local)
        {
            this.LastIp = lastIp;
            this.Local = local;
        }

        public void Start()
        {
            new Thread(new ParameterizedThreadStart(this.ThreadProcess)) { IsBackground = true }.Start();
        }

        public void ThreadProcess(object state)
        {
            while (true)
            {
                try
                {
                    IpHelper helper = new IpHelper(this.LastIp);
                    Thread thread = new Thread(new ParameterizedThreadStart(helper.GetIp));
                    thread.Start();
                    if (!thread.Join(TimeSpan.FromMinutes(1.0)))
                    {
                        this._logger.Error("get ip timeout", new object[0]);
                    }
                    string iP = helper.IP;
                    this._logger.Info("get ip:{0} - {1}", new object[] { this.LastIp, iP });
                    Action<string> info = this.m_Info;
                    if (info != null)
                    {
                        info(string.Format("{0}：动态域名获取本机最新IP：{1}", DateTime.Now.ToLongTimeString(), iP));
                    }
                    if (iP != this.LastIp)
                    {
                        this.LastIp = iP;
                        this._logger.Info("change ip 1:{0}", new object[] { iP });
                        info = this.m_IPChanged;
                        if (info != null)
                        {
                            this._logger.Info("change ip 2:{0}", new object[] { iP });
                            info(iP);
                        }
                    }
                }
                catch (Exception exception)
                {
                    this._logger.Error("ThreadProcess error:{0}-{1}", new object[] { this.Local, exception });
                }
                finally
                {
                    Thread.Sleep(TimeSpan.FromMinutes(3.0));
                }
            }
        }

        public string LastIp { get; private set; }

        public EndPoint Local { get; set; }
    }
}

