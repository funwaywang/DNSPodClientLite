using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace DNSPodClientLite
{
    public class ListViewExtender : IDisposable
    {
        private readonly Dictionary<int, ListViewColumn> _columns = new Dictionary<int, ListViewColumn>();
        public Font Font { get; private set; }
        public ListView ListView { get; private set; }

        public ListViewExtender(ListView listView)
        {
            if (listView == null)
            {
                throw new ArgumentNullException("listView");
            }
            if (listView.View != View.Details)
            {
                throw new ArgumentException(null, "listView");
            }
            ListView = listView;
            ListView.OwnerDraw = true;
            ListView.DrawItem += new DrawListViewItemEventHandler(OnDrawItem);
            ListView.DrawSubItem += new DrawListViewSubItemEventHandler(OnDrawSubItem);
            ListView.DrawColumnHeader += new DrawListViewColumnHeaderEventHandler(OnDrawColumnHeader);
            ListView.MouseMove += new MouseEventHandler(OnMouseMove);
            ListView.MouseClick += new MouseEventHandler(OnMouseClick);
            Font = new System.Drawing.Font(ListView.Font.FontFamily, ListView.Font.Size - 2f);
        }

        public void AddColumn(ListViewColumn column)
        {
            if (column == null)
            {
                throw new ArgumentNullException("column");
            }
            column.Extender = this;
            _columns[column.ColumnIndex] = column;
        }

        public virtual void Dispose()
        {
            if (Font != null)
            {
                Font.Dispose();
                Font = null;
            }
        }

        public ListViewColumn GetColumn(int index)
        {
            ListViewColumn column;
            return (_columns.TryGetValue(index, out column) ? column : null);
        }

        public ListViewColumn GetColumnAt(int x, int y, out ListViewItem item, out ListViewItem.ListViewSubItem subItem)
        {
            subItem = null;
            item = ListView.GetItemAt(x, y);
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
                        return GetColumn(i);
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
            ListViewColumn column = GetColumn(e.ColumnIndex);
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
            ListViewColumn column = GetColumnAt(e.X, e.Y, out ListViewItem item, out ListViewItem.ListViewSubItem item2);
            if (column != null)
            {
                column.MouseClick(e, item, item2);
            }
        }

        protected virtual void OnMouseMove(object sender, MouseEventArgs e)
        {
            ListViewColumn column = GetColumnAt(e.X, e.Y, out ListViewItem item, out ListViewItem.ListViewSubItem item2);
            if (column != null)
            {
                column.Invalidate(item, item2);
            }
            else if (item != null)
            {
                ListView.Invalidate(item.Bounds);
            }
        }

        public IEnumerable<ListViewColumn> Columns
        {
            get
            {
                return _columns.Values;
            }
        }
    }
}

