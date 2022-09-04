using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DNSPodClientLite.Share
{
    public class IpProvider
    {
        public IpProvider(string resourceUrl, Regex regex)
        {
            ResourceUrl = resourceUrl;
            Regex = regex;
        }

        public IpProvider(string resourceUrl, string regex)
        {
            ResourceUrl = resourceUrl;
            Regex = new Regex(regex, RegexOptions.Compiled);
        }

        public string ResourceUrl { get; private set; }

        public Regex Regex { get; private set; }

        public string GetIp()
        {
            if (string.IsNullOrEmpty(ResourceUrl))
            {
                throw new Exception("Invalid resource url.");
            }

            string input = new WebClient().DownloadString(ResourceUrl);
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            var match = Regex?.Match(input);
            if (match?.Success == true)
            {
                return match.Groups[1].Value;
            }

            return null;
        }

        public async Task<string> GetIpAsync()
        {
            if (string.IsNullOrEmpty(ResourceUrl))
            {
                throw new Exception("Invalid resource url.");
            }

            string input = await new WebClient().DownloadStringTaskAsync(ResourceUrl);
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            var match = Regex?.Match(input);
            if (match?.Success == true)
            {
                return match.Groups[1].Value;
            }

            return null;
        }
    }
}
