using System;

namespace DNSPodClientLite
{
    public class IpChangedEventArgs : EventArgs
    {
        public IpChangedEventArgs(string original, string ip)
        {
            Original = original;
            IP = ip;
        }

        public string Original { get; }

        public string IP { get; }
    }

    public delegate void IpChangedEventHandler(object sender, IpChangedEventArgs e);
}
