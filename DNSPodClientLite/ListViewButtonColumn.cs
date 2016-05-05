namespace DNSPodClientLite
{
    using System;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;
    using System.Windows.Forms.VisualStyles;

    public class ListViewButtonColumn : ListViewColumn
    {
        private Rectangle _hot;

        public ListViewButtonColumn(int columnIndex) : base(columnIndex)
        {
            this._hot = Rectangle.Empty;
        }

        public override void Draw(DrawListViewSubItemEventArgs e)
        {
            if ((this._hot != Rectangle.Empty) && (this._hot != e.Bounds))
            {
                base.ListView.Invalidate(this._hot);
                this._hot = Rectangle.Empty;
            }
            if (this.DrawIfEmpty || !string.IsNullOrEmpty(e.SubItem.Text))
            {
                Point point = e.Item.ListView.PointToClient(Control.MousePosition);
                ButtonRenderer.DrawButton(e.Graphics, e.Bounds, e.SubItem.Text, this.Font, false, PushButtonState.Default);
                if ((base.ListView.GetItemAt(point.X, point.Y) == e.Item) && (e.Item.GetSubItemAt(point.X, point.Y) == e.SubItem))
                {
                    ButtonRenderer.DrawButton(e.Graphics, e.Bounds, e.SubItem.Text, this.Font, true, PushButtonState.Hot);
                    this._hot = e.Bounds;
                }
                else
                {
                    ButtonRenderer.DrawButton(e.Graphics, e.Bounds, e.SubItem.Text, this.Font, false, PushButtonState.Default);
                }
            }
        }

        protected virtual void OnColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            if (e.ColumnIndex == base.ColumnIndex)
            {
                e.Cancel = true;
                e.NewWidth = base.ListView.Columns[e.ColumnIndex].Width;
            }
        }

        public bool DrawIfEmpty { get; set; }

        public override ListViewExtender Extender
        {
            get
            {
                return base.Extender;
            }
            protected internal set
            {
                base.Extender = value;
                if (this.FixedWidth)
                {
                    base.Extender.ListView.ColumnWidthChanging += new ColumnWidthChangingEventHandler(this.OnColumnWidthChanging);
                }
            }
        }

        public bool FixedWidth { get; set; }
    }
}

