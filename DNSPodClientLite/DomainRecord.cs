namespace DNSPodClientLite
{
    using System;
    using System.Runtime.CompilerServices;

    public class DomainRecord
    {
        public string name { get; set; }

        public string priority { get; set; }

        public string ttl { get; set; }

        public string type { get; set; }

        public string value { get; set; }
    }
}

