using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNSPodClientLite
{
    public class RecordStatusChangedEventArgs : EventArgs
    {
        public RecordStatusChangedEventArgs(int recordId, string status)
        {
            RecordId = recordId;
            Status = status;
        }

        public int RecordId { get; }
        public string Status { get; }
    }

    public delegate void RecordStatusChangedEventHandler(object sender, RecordStatusChangedEventArgs e);
}
