namespace DNSPodClientLite
{
    using System;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows.Forms;

    public abstract class ListViewColumn
    {
        private EventHandler<ListViewColumnMouseEventArgs> m_click;

        public event EventHandler<ListViewColumnMouseEventArgs> Click
        {
            add
            {
                EventHandler<ListViewColumnMouseEventArgs> handler2;
                EventHandler<ListViewColumnMouseEventArgs> click = this.m_click;
                do
                {
                    handler2 = click;
                    EventHandler<ListViewColumnMouseEventArgs> handler3 = (EventHandler<ListViewColumnMouseEventArgs>) Delegate.Combine(handler2, value);
                    click = Interlocked.CompareExchange<EventHandler<ListViewColumnMouseEventArgs>>(ref this.m_click, handler3, handler2);
                }
                while (click != handler2);
            }
            remove
            {
                EventHandler<ListViewColumnMouseEventArgs> handler2;
                EventHandler<ListViewColumnMouseEventArgs> click = this.m_click;
                do
                {
                    handler2 = click;
                    EventHandler<ListViewColumnMouseEventArgs> handler3 = (EventHandler<ListViewColumnMouseEventArgs>) Delegate.Remove(handler2, value);
                    click = Interlocked.CompareExchange<EventHandler<ListViewColumnMouseEventArgs>>(ref this.m_click, handler3, handler2);
                }
                while (click != handler2);
            }
        }

        protected ListViewColumn(int columnIndex)
        {
            if (columnIndex < 0)
            {
                throw new ArgumentException(null, "columnIndex");
            }
            this.ColumnIndex = columnIndex;
        }

        public abstract void Draw(DrawListViewSubItemEventArgs e);
        public virtual void Invalidate(ListViewItem item, ListViewItem.ListViewSubItem subItem)
        {
            if (this.Extender != null)
            {
                this.Extender.ListView.Invalidate(subItem.Bounds);
            }
        }

        public virtual void MouseClick(MouseEventArgs e, ListViewItem item, ListViewItem.ListViewSubItem subItem)
        {
            if (this.m_click != null)
            {
                this.m_click(this, new ListViewColumnMouseEventArgs(e, item, subItem));
            }
        }

        public int ColumnIndex { get; private set; }

        public virtual ListViewExtender Extender { get; protected internal set; }

        public virtual System.Drawing.Font Font
        {
            get
            {
                return ((this.Extender == null) ? null : this.Extender.Font);
            }
        }

        public System.Windows.Forms.ListView ListView
        {
            get
            {
                return ((this.Extender == null) ? null : this.Extender.ListView);
            }
        }
    }
}

