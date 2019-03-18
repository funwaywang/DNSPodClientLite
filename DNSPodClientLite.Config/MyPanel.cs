namespace DNSPodClientLite
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public class MyPanel : Panel
    {
        public MyPanel()
        {
            base.SetStyle(ControlStyles.Opaque, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.FillRectangle(new SolidBrush(this.BackColor), 0, 0, base.Size.Width, base.Size.Height);
        }
    }
}

