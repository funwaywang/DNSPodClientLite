namespace DNSPodClientLite
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class ListViewExtender : IDisposable
    {
        private readonly Dictionary<int, ListViewColumn> _columns = new Dictionary<int, ListViewColumn>();

        public ListViewExtender(System.Windows.Forms.ListView listView)
        {
            if (listView == null)
            {
                throw new ArgumentNullException("listView");
            }
            if (listView.View != View.Details)
            {
                throw new ArgumentException(null, "listView");
            }
            this.ListView = listView;
            this.ListView.OwnerDraw = true;
            this.ListView.DrawItem += new DrawListViewItemEventHandler(this.OnDrawItem);
            this.ListView.DrawSubItem += new DrawListViewSubItemEventHandler(this.OnDrawSubItem);
            this.ListView.DrawColumnHeader += new DrawListViewColumnHeaderEventHandler(this.OnDrawColumnHeader);
            this.ListView.MouseMove += new MouseEventHandler(this.OnMouseMove);
            this.ListView.MouseClick += new MouseEventHandler(this.OnMouseClick);
            this.Font = new System.Drawing.Font(this.ListView.Font.FontFamily, this.ListView.Font.Size - 2f);
        }

        public void AddColumn(ListViewColumn column)
        {
            if (column == null)
            {
                throw new ArgumentNullException("column");
            }
            column.Extender = this;
            this._columns[column.ColumnIndex] = column;
        }

        public virtual void Dispose()
        {
            if (this.Font != null)
            {
                this.Font.Dispose();
                this.Font = null;
            }
        }

        public ListViewColumn GetColumn(int index)
        {
            ListViewColumn column;
            return (this._columns.TryGetValue(index, out column) ? column : null);
        }

        public ListViewColumn GetColumnAt(int x, int y, out ListViewItem item, out ListViewItem.ListViewSubItem subItem)
        {
            subItem = null;
            item = this.ListView.GetItemAt(x, y);
            if (item != null)
            {
                subItem = item.GetSubItemAt(x, y);
                if (subItem == null)
                {
                    return null;
                }
                for (int i = 0; i < item.SubItems.Count; i++)
                {
                    if (item.SubItems[i] == subItem)
                    {
                        return this.GetColumn(i);
                    }
                }
            }
            return null;
        }

        protected virtual void OnDrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawDefault = true;
        }

        protected virtual void OnDrawItem(object sender, DrawListViewItemEventArgs e)
        {
        }

        protected virtual void OnDrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            ListViewColumn column = this.GetColumn(e.ColumnIndex);
            if (column == null)
            {
                e.DrawDefault = true;
            }
            else
            {
                column.Draw(e);
            }
        }

        protected virtual void OnMouseClick(object sender, MouseEventArgs e)
        {
            ListViewItem item;
            ListViewItem.ListViewSubItem item2;
            ListViewColumn column = this.GetColumnAt(e.X, e.Y, out item, out item2);
            if (column != null)
            {
                column.MouseClick(e, item, item2);
            }
        }

        protected virtual void OnMouseMove(object sender, MouseEventArgs e)
        {
            ListViewItem item;
            ListViewItem.ListViewSubItem item2;
            ListViewColumn column = this.GetColumnAt(e.X, e.Y, out item, out item2);
            if (column != null)
            {
                column.Invalidate(item, item2);
            }
            else if (item != null)
            {
                this.ListView.Invalidate(item.Bounds);
            }
        }

        public IEnumerable<ListViewColumn> Columns
        {
            get
            {
                return this._columns.Values;
            }
        }

        public System.Drawing.Font Font { get; private set; }

        public System.Windows.Forms.ListView ListView { get; private set; }
    }
}

