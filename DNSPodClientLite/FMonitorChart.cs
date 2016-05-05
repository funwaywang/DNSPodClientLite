namespace DNSPodClientLite
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class FMonitorChart : Form
    {
        private IContainer components;
        private Label label1;
        private int MarginLeft;
        private const int MaxWidth = 720;
        private Config.MonitorConfig monitorCfg;
        private const int OneChartMaxHours = 12;
        private Panel panel1;

        public FMonitorChart()
        {
            this.components = null;
            this.MarginLeft = 10;
            this.components = null;
            this.InitializeComponent();
        }

        public FMonitorChart(Config.MonitorConfig monitorCfg) : this()
        {
            this.monitorCfg = monitorCfg;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void drawMonitorData(Point p, bool down)
        {
            Point point = new Point(p.X, p.Y - 20);
            Graphics graphics = base.CreateGraphics();
            Pen pen = new Pen(down ? Color.Red : Color.Green, 2f);
            graphics.DrawLine(pen, p, point);
        }

        private void drawRule(int startHour, int top)
        {
            Graphics graphics = base.CreateGraphics();
            Pen pen = new Pen(Color.Black, 1f);
            graphics.DrawLine(pen, this.MarginLeft, top, 720 + this.MarginLeft, top);
            graphics.FillRectangle(new SolidBrush(Color.White), this.MarginLeft, top - 20, 720, 20);
            for (int i = startHour; i <= (startHour + 12); i++)
            {
                int num2 = ((i - startHour) * 60) + this.MarginLeft;
                graphics.DrawLine(pen, num2, top - 20, num2, top + 10);
                graphics.DrawString(i.ToString() + ":00", new Font("04b_08", 6f), new SolidBrush(Color.Black), (float) num2, (float) (top + 10));
            }
        }

        private void FMonitorChart_Load(object sender, EventArgs e)
        {
            this.Text = string.Format("{0}.{1}({2})的监控图表", this.monitorCfg.Subdomain, this.monitorCfg.Domain, this.monitorCfg.Ip);
            this.label1.Text = this.Text;
        }

        private void FMonitorChart_Paint(object sender, PaintEventArgs e)
        {
            Graphics graphics = base.CreateGraphics();
            int y = 180;
            graphics.FillRectangle(new SolidBrush(Color.White), this.MarginLeft, y, 10, 10);
            graphics.FillRectangle(new SolidBrush(Color.Green), this.MarginLeft + 100, y, 10, 10);
            graphics.FillRectangle(new SolidBrush(Color.Red), this.MarginLeft + 200, y, 10, 10);
            Font font = new Font("04b_08", 6f);
            Brush brush = new SolidBrush(Color.Black);
            graphics.DrawString("无数据", font, brush, (float) (this.MarginLeft + 20), (float) y);
            graphics.DrawString("正常", font, brush, (float) (this.MarginLeft + 120), (float) y);
            graphics.DrawString("宕机", font, brush, (float) (this.MarginLeft + 220), (float) y);
            MonitorHistory history = new MonitorHistory(this.monitorCfg.RecordId);
            this.drawRule(0, 100);
            this.drawRule(12, 150);
            List<MonitorData> oneDayHistorys = history.GetOneDayHistorys(DateTime.Now);
            foreach (MonitorData data in oneDayHistorys)
            {
                this.drawMonitorData(this.getTimePoint(data.Time), data.Down);
            }
        }

        private Point getTimePoint(DateTime time)
        {
            int hours = time.TimeOfDay.Hours;
            int y = 0;
            int num3 = 0;
            if ((hours >= 0) && (hours < 12))
            {
                y = 100;
                num3 = 0;
            }
            else if ((hours >= 12) && (hours < 0x18))
            {
                y = 150;
                num3 = 720;
            }
            int num4 = ((int) time.TimeOfDay.TotalMinutes) - num3;
            return new Point(num4 + this.MarginLeft, y);
        }

        private void InitializeComponent()
        {
            this.panel1 = new Panel();
            this.label1 = new Label();
            this.panel1.SuspendLayout();
            base.SuspendLayout();
            this.panel1.BackColor = Color.DeepSkyBlue;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = DockStyle.Top;
            this.panel1.Location = new Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0x2f2, 40);
            this.panel1.TabIndex = 7;
            this.label1.AutoSize = true;
            this.label1.Font = new Font("宋体", 15.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.label1.ForeColor = Color.White;
            this.label1.Location = new Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x5e, 0x15);
            this.label1.TabIndex = 1;
            this.label1.Text = "宕机监控";
            base.ClientSize = new Size(0x2f2, 0xde);
            base.Controls.Add(this.panel1);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "FMonitorChart";
            this.Text = "FMonitorChart";
            base.Load += new EventHandler(this.FMonitorChart_Load);
            base.Paint += new PaintEventHandler(this.FMonitorChart_Paint);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            base.ResumeLayout(false);
        }
    }
}

