namespace DNSPodClientLite
{
    using System;
    using System.Net;
    using System.Net.Cache;

    internal class MyWebClient : WebClient
    {
        private IPEndPoint m_OutIPEndPoint;

        public MyWebClient(IPEndPoint outIp)
        {
            if (outIp == null)
            {
                this.m_OutIPEndPoint = new IPEndPoint(IPAddress.Any, 0);
            }
            this.m_OutIPEndPoint = outIp;
            base.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            HttpWebRequest webRequest = (HttpWebRequest) base.GetWebRequest(address);
            webRequest.ServicePoint.BindIPEndPointDelegate = (servicePoint, remoteEndPoint, retryCount) => this.m_OutIPEndPoint;
            return webRequest;
        }
    }
}

