namespace DNSPodClientLite
{
    using System;
    using System.Windows.Forms;

    internal class DoubleBufferListView : ListView
    {
        public DoubleBufferListView()
        {
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            base.UpdateStyles();
        }
    }
}

