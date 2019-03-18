using System;
using System.Net;
using System.Net.Cache;

namespace DNSPodClientLite
{
    public class MyWebClient : WebClient
    {
        private readonly IPEndPoint outIPEndPoint;

        public MyWebClient(IPEndPoint outIp)
        {
            outIPEndPoint = outIp ?? new IPEndPoint(IPAddress.Any, 0);
            CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            HttpWebRequest webRequest = (HttpWebRequest)base.GetWebRequest(address);
            webRequest.ServicePoint.BindIPEndPointDelegate = (servicePoint, remoteEndPoint, retryCount) => outIPEndPoint;
            return webRequest;
        }
    }
}

