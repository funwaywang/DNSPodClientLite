using System;

namespace DNSPodClientLite
{
    public class MessageEventArgs : EventArgs
    {
        public string Message { get; private set; }

        public MessageEventArgs(string message)
        {
            Message = message;
        }
    }

    public delegate void MessageEventHandler(object sender, MessageEventArgs e);
}
