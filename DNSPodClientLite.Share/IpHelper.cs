using DNSPodClientLite.Share;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DNSPodClientLite
{
    public class IpHelper
    {
        private static readonly Regex _regBaidu = new Regex(@"<strong>(\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})<\/strong>", RegexOptions.IgnoreCase);
        private static readonly Regex _regIp138 = new Regex(@"<center>.*?(\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}).*<\/center>", RegexOptions.IgnoreCase);
        private static readonly string[] invalidIps = "localhost,127.0.0.1".Split(',');

        public IpHelper(string oldip)
        {
            IP = oldip;
        }

        public string IP { get; set; }

        public void GetIp(object state)
        {
            try
            {
                var provider = IpProviderManager.Default.GetRandomProvider();
                if (provider == null)
                {
                    throw new Exception("Could not get any ip provider");
                }

                var ip = provider.GetIp();
                if (string.IsNullOrEmpty(ip) || invalidIps.Contains(ip) || IsPrivateIp(ip))
                {
                    throw new Exception($"Invalid ip get: [{ip}]");
                }

                IP = ip;
            }
            catch (Exception ex)
            {
                new Logger("ddns").Error("GetIp error:{0}, provider: ", new object[] { ex });
            }

            //try
            //{
            //    IP = GetIpFromDNSPod();
            //}
            //catch
            //{
            //    try
            //    {
            //        IP = GetIpFromIp138();
            //    }
            //    catch
            //    {
            //        try
            //        {
            //            IP = GetIpFromBaidu();
            //        }
            //        catch (Exception exception)
            //        {
            //            new Logger("ddns").Error("GetIp error:{0}", new object[] { exception });
            //        }
            //    }
            //}
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

        public static bool IsPrivateIp(string ipAddress)
        {
            int[] ipParts = ipAddress.Split(new String[] { "." }, StringSplitOptions.RemoveEmptyEntries)
                                     .Select(s => int.Parse(s)).ToArray();
            // in private ip range
            if (ipParts[0] == 10 ||
                (ipParts[0] == 192 && ipParts[1] == 168) ||
                (ipParts[0] == 172 && (ipParts[1] >= 16 && ipParts[1] <= 31)))
            {
                return true;
            }

            // IP Address is probably public.
            // This doesn't catch some VPN ranges like OpenVPN and Hamachi.
            return false;
        }
    }
}

