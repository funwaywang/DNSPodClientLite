namespace DNSPodClientLite.ConfigTool
{
    partial class ControlPanelForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lvDomains = new DNSPodClientLite.DoubleBufferListView();
            this.name = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ddns = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.monitor = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lvMsg = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.TsbNetCard = new System.Windows.Forms.ToolStripButton();
            this.TsbImport = new System.Windows.Forms.ToolStripButton();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lvDomains);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(330, 491);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "域名列表";
            // 
            // lvDomains
            // 
            this.lvDomains.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.name,
            this.ddns,
            this.monitor});
            this.lvDomains.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvDomains.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lvDomains.FullRowSelect = true;
            this.lvDomains.Location = new System.Drawing.Point(3, 17);
            this.lvDomains.Margin = new System.Windows.Forms.Padding(4);
            this.lvDomains.Name = "lvDomains";
            this.lvDomains.Size = new System.Drawing.Size(324, 471);
            this.lvDomains.TabIndex = 0;
            this.lvDomains.UseCompatibleStateImageBehavior = false;
            this.lvDomains.View = System.Windows.Forms.View.Details;
            // 
            // name
            // 
            this.name.Text = "域名";
            this.name.Width = 150;
            // 
            // ddns
            // 
            this.ddns.Text = "动态解析";
            this.ddns.Width = 80;
            // 
            // monitor
            // 
            this.monitor.Text = "宕机监控";
            this.monitor.Width = 80;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lvMsg);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(656, 491);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "信息列表";
            // 
            // lvMsg
            // 
            this.lvMsg.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.lvMsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvMsg.Location = new System.Drawing.Point(3, 17);
            this.lvMsg.Name = "lvMsg";
            this.lvMsg.Size = new System.Drawing.Size(650, 471);
            this.lvMsg.TabIndex = 0;
            this.lvMsg.UseCompatibleStateImageBehavior = false;
            this.lvMsg.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "信息";
            this.columnHeader1.Width = 500;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer1.Size = new System.Drawing.Size(990, 491);
            this.splitContainer1.SplitterDistance = 330;
            this.splitContainer1.TabIndex = 12;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TsbNetCard,
            this.TsbImport});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(990, 25);
            this.toolStrip1.TabIndex = 9;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // TsbNetCard
            // 
            this.TsbNetCard.Image = global::DNSPodClientLite.ConfigTool.Properties.Resources.network;
            this.TsbNetCard.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TsbNetCard.Name = "TsbNetCard";
            this.TsbNetCard.Size = new System.Drawing.Size(76, 22);
            this.TsbNetCard.Text = "网卡设置";
            this.TsbNetCard.Click += new System.EventHandler(this.TsbNetCard_Click);
            // 
            // TsbImport
            // 
            this.TsbImport.Image = global::DNSPodClientLite.ConfigTool.Properties.Resources.import;
            this.TsbImport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TsbImport.Name = "TsbImport";
            this.TsbImport.Size = new System.Drawing.Size(78, 22);
            this.TsbImport.Text = "导入记录";
            this.TsbImport.Click += new System.EventHandler(this.TsbImport_Click);
            // 
            // ControlPanelForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(990, 516);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "ControlPanelForm";
            this.Text = "控制面板";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox1;
        private DoubleBufferListView lvDomains;
        private System.Windows.Forms.ColumnHeader name;
        private System.Windows.Forms.ColumnHeader ddns;
        private System.Windows.Forms.ColumnHeader monitor;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListView lvMsg;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton TsbNetCard;
        private System.Windows.Forms.ToolStripButton TsbImport;
    }
}