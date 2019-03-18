namespace DNSPodClientLite
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class ListViewColumnMouseEventArgs : MouseEventArgs
    {
        public ListViewColumnMouseEventArgs(MouseEventArgs e, ListViewItem item, ListViewItem.ListViewSubItem subItem) : base(e.Button, e.Clicks, e.X, e.Y, e.Delta)
        {
            this.Item = item;
            this.SubItem = subItem;
        }

        public ListViewItem Item { get; private set; }

        public ListViewItem.ListViewSubItem SubItem { get; private set; }
    }
}

