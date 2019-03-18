using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Xml.Serialization;

namespace DNSPodClientLite
{
    public enum LoginMethod
    {
        Token,
        Email
    }

    [Serializable]
    public class Config
    {
        public List<DDNSConfig> _Ddnses { get; set; }
        public List<MonitorConfig> _Monitors { get; set; }
        public string LastIp { get; set; }
        public LoginMethod LoginMethod { get; set; }
        public string LoginEmail { get; set; }
        public string LoginPassword { get; set; }
        public string LoginTokenId { get; set; }
        public string LoginTokenCode { get; set; }
        public string NetCardId { get; set; }

        public Config()
        {
            _Monitors = new List<MonitorConfig>();
            _Ddnses = new List<DDNSConfig>();
        }

        public void AddDdns(DDNSConfig item)
        {
            Predicate<DDNSConfig> predicate2 = null;
            Predicate<DDNSConfig> match = null;
            lock (this)
            {
                if (match == null)
                {
                    if (predicate2 == null)
                    {
                        predicate2 = x => x.RecordId == item.RecordId;
                    }
                    match = predicate2;
                }
                DDNSConfig config = _Ddnses.Find(match);
                if (config != null)
                {
                    _Ddnses.Remove(config);
                }
                _Ddnses.Add(item);
            }
        }

        public void AddMonitor(MonitorConfig item)
        {
            Predicate<MonitorConfig> predicate2 = null;
            Predicate<MonitorConfig> match = null;
            lock (this)
            {
                if (match == null)
                {
                    if (predicate2 == null)
                    {
                        predicate2 = x => x.RecordId == item.RecordId;
                    }
                    match = predicate2;
                }
                MonitorConfig config = _Monitors.Find(match);
                if (config != null)
                {
                    _Monitors.Remove(config);
                }
                _Monitors.Add(item);
            }
        }

        public static string DecodePassword(string text, string key)
        {
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }

            byte[] bytes = Convert.FromBase64String(text);
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = (byte)(bytes[i] ^ key[i % key.Length]);
            }
            return Encoding.UTF8.GetString(bytes);
        }

        public static string EncodePassword(string text, string key)
        {
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }

            byte[] bytes = Encoding.UTF8.GetBytes(text);
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = (byte)(bytes[i] ^ key[i % key.Length]);
            }
            return Convert.ToBase64String(bytes);
        }

        public MonitorConfig GetBackMonitor(int recordid)
        {
            Predicate<MonitorConfig> predicate4 = null;
            Predicate<MonitorConfig> match = null;
            lock (this)
            {
                Predicate<MonitorConfig> predicate3 = null;
                Predicate<MonitorConfig> predicate2 = null;
                if (match == null)
                {
                    if (predicate4 == null)
                    {
                        predicate4 = x => x.RecordId == recordid;
                    }
                    match = predicate4;
                }
                MonitorConfig cfg = _Monitors.Find(match);
                if (cfg != null)
                {
                    if (predicate2 == null)
                    {
                        if (predicate3 == null)
                        {
                            predicate3 = x => (((x.Line != cfg.Line) && (x.Status == "ok")) && (x.DomainId == cfg.DomainId)) && (x.Subdomain == cfg.Subdomain);
                        }
                        predicate2 = predicate3;
                    }
                    return _Monitors.Find(predicate2);
                }
                return null;
            }
        }

        public List<DDNSConfig> GetDdnses()
        {
            lock (this)
            {
                return new List<DDNSConfig>(_Ddnses);
            }
        }

        public IPEndPoint GetLocal()
        {
            foreach (NetworkInterface interface2 in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (interface2.Id == NetCardId)
                {
                    if (interface2.OperationalStatus != OperationalStatus.Up)
                    {
                        throw new ApplicationException("绑定的网卡未启动");
                    }
                    foreach (UnicastIPAddressInformation information in interface2.GetIPProperties().UnicastAddresses)
                    {
                        if (information.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            return new IPEndPoint(information.Address, 0);
                        }
                    }
                }
            }
            return new IPEndPoint(IPAddress.Any, 0);
        }

        public List<MonitorConfig> GetMonitors()
        {
            lock (this)
            {
                return new List<MonitorConfig>(_Monitors);
            }
        }

        public static Config Load()
        {
            string path = Path.Combine(Environment.CurrentDirectory, "config.xml");
            if (!File.Exists(path))
            {
                return null;
            }
            XmlSerializer serializer = new XmlSerializer(typeof(Config));
            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                Config config = (Config)serializer.Deserialize(stream);
                config.LoginPassword = DecodePassword(config.LoginPassword, config.LoginEmail);
                return config;
            }
        }

        public static Config Load(string file)
        {
            string path = Path.Combine(Environment.CurrentDirectory, "config.xml");
            if (!File.Exists(path))
            {
                return null;
            }

            File.Copy(path, file, true);
            path = Path.Combine(Environment.CurrentDirectory, file);
            XmlSerializer serializer = new XmlSerializer(typeof(Config));
            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                Config config = (Config)serializer.Deserialize(stream);
                config.LoginPassword = DecodePassword(config.LoginPassword, config.LoginEmail);
                return config;
            }
        }

        public void RemoveDdns(int recordid)
        {
            Predicate<DDNSConfig> predicate2 = null;
            Predicate<DDNSConfig> match = null;
            lock (this)
            {
                if (match == null)
                {
                    if (predicate2 == null)
                    {
                        predicate2 = item => item.RecordId == recordid;
                    }
                    match = predicate2;
                }
                DDNSConfig config = _Ddnses.Find(match);
                if (config != null)
                {
                    _Ddnses.Remove(config);
                }
            }
        }

        public void RemoveMonitor(int recordid)
        {
            Predicate<MonitorConfig> predicate2 = null;
            Predicate<MonitorConfig> match = null;
            lock (this)
            {
                if (match == null)
                {
                    if (predicate2 == null)
                    {
                        predicate2 = item => item.RecordId == recordid;
                    }
                    match = predicate2;
                }
                MonitorConfig config = _Monitors.Find(match);
                if (config != null)
                {
                    _Monitors.Remove(config);
                }
            }
        }

        public void Save()
        {
            lock (this)
            {
                string path = Path.Combine(Environment.CurrentDirectory, "config.xml");
                XmlSerializer serializer = new XmlSerializer(typeof(Config));
                string loginPassword = LoginPassword;
                LoginPassword = EncodePassword(LoginPassword, LoginEmail);
                using (FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    serializer.Serialize(stream, this);
                }
                LoginPassword = loginPassword;
            }
        }

        public class DDNSConfig
        {
            public string Domain { get; set; }

            public int DomainId { get; set; }

            public int RecordId { get; set; }

            public string Subdomain { get; set; }
        }

        public class MonitorConfig
        {
            public string BakValue { get; set; }

            public string Domain { get; set; }

            public int DomainId { get; set; }

            public string Ip { get; set; }

            public string Line { get; set; }

            public int MonitorInteval { get; set; }

            public int Port { get; set; }

            public bool Qiehuan { get; set; }

            public int RecordId { get; set; }

            public bool SmartQiehuan { get; set; }

            public string Status { get; set; }

            public string Subdomain { get; set; }
        }
    }
}

