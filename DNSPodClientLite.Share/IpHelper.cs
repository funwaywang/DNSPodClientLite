using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace DNSPodClientLite
{
    public class IpHelper
    {
        private static readonly Regex _regBaidu = new Regex(@"<strong>(\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})<\/strong>", RegexOptions.IgnoreCase);
        private static readonly Regex _regIp138 = new Regex(@"<center>.*?(\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}).*<\/center>", RegexOptions.IgnoreCase);

        public IpHelper(string oldip)
        {
            IP = oldip;
        }

        public void GetIp(object state)
        {
            try
            {
                IP = GetIpFromDNSPod();
            }
            catch
            {
                try
                {
                    IP = GetIpFromIp138();
                }
                catch
                {
                    try
                    {
                        IP = GetIpFromBaidu();
                    }
                    catch (Exception exception)
                    {
                        new Logger("ddns").Error("GetIp error:{0}", new object[] { exception });
                    }
                }
            }
        }

        private static string GetIpByWeb(string url, Regex reg, string domainName)
        {
            string input = new WebClient().DownloadString(url);
            MatchCollection matchs = reg.Matches(input);
            foreach (Match match in matchs)
            {
                return match.Groups[1].Value;
            }
            throw new ApplicationException(string.Format("get ip error from {0}", domainName));
        }

        public static string GetIpFromBaidu()
        {
            string url = "http://www.baidu.com/s?wd=ip&rsv_bp=0&rsv_spt=3&inputT=456";
            return GetIpByWeb(url, _regBaidu, "baidu");
        }

        public static string GetIpFromDNSPod()
        {
            string str;
            TcpClient client = new TcpClient();
            try
            {
                client.Connect("ns1.dnspod.net", 0x1a0a);
                byte[] buffer = new byte[0x200];
                int count = client.GetStream().Read(buffer, 0, 0x200);
                str = Encoding.ASCII.GetString(buffer, 0, count);
            }
            finally
            {
                client.Close();
            }
            return str;
        }

        public static string GetIpFromIp138()
        {
            string url = "http://www.ip138.com/ip2city.asp";
            return GetIpByWeb(url, _regIp138, "ip138");
        }

        public string IP { get; set; }
    }
}

