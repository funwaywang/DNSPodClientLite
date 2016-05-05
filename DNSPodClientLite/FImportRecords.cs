namespace DNSPodClientLite
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;

    public class FImportRecords : Form
    {
        private ToolStripButton btnImportZonefile;
        private ColumnHeader columnHeader2;
        private ColumnHeader columnHeader3;
        private ColumnHeader columnHeader4;
        private ColumnHeader columnHeader5;
        private ColumnHeader columnHeader6;
        private IContainer components = null;
        private ToolStripStatusLabel lblStatus;
        private ListView listView1;
        private MyPanel pnl;
        private StatusStrip statusStrip1;
        private ToolStrip toolStrip1;
        private static string[] zonefile_record_arr = new string[] { "; A Records", "; CNAME Records", "; MX Records", "; TXT Records", "; AAAA Records" };

        public FImportRecords()
        {
            this.InitializeComponent();
        }

        private void Alert(string msg)
        {
            if (base.InvokeRequired)
            {
                Action<string> method = new Action<string>(this.Alert);
                base.Invoke(method, new object[] { msg });
            }
            else
            {
                MessageBox.Show(this, msg, "警告");
            }
        }

        private void bindRecord(IEnumerable<DomainRecord> records)
        {
            if (records != null)
            {
                foreach (DomainRecord record in records)
                {
                    ListViewItem item = new ListViewItem(record.type) {
                        Tag = record,
                        Checked = true
                    };
                    ListViewItem.ListViewSubItem item2 = new ListViewItem.ListViewSubItem {
                        Text = record.name
                    };
                    item.SubItems.Add(item2);
                    item2 = new ListViewItem.ListViewSubItem {
                        Text = record.value
                    };
                    item.SubItems.Add(item2);
                    item2 = new ListViewItem.ListViewSubItem {
                        Text = record.ttl
                    };
                    item.SubItems.Add(item2);
                    item2 = new ListViewItem.ListViewSubItem {
                        Text = record.priority
                    };
                    item.SubItems.Add(item2);
                    this.listView1.Items.Add(item);
                }
            }
        }

        private void btnImportZonefile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog {
                Filter = "ZoneFile(*.*)|*.*",
                ValidateNames = true,
                CheckFileExists = true,
                CheckPathExists = true
            };
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                this.StartWait();
                List<DomainRecord> list = new List<DomainRecord>();
                BackgroundWorker worker = new BackgroundWorker();
                string filename = dialog.FileName;
                string domain = string.Empty;
                worker.DoWork += delegate (object a, DoWorkEventArgs b) {
                    try
                    {
                        using (StreamReader reader = new StreamReader(filename, Encoding.UTF8))
                        {
                            domain = handleReader(list, reader);
                            string str = reader.ReadLine();
                            while (str != null)
                            {
                                if (str == "")
                                {
                                    str = reader.ReadLine();
                                }
                                else
                                {
                                    string[] strArray = str.Split(new char[] { '\t' });
                                    string str3 = strArray[1];
                                    if (string.IsNullOrEmpty(str3))
                                    {
                                        str3 = "@";
                                    }
                                    DomainRecord record2 = new DomainRecord {
                                        name = str3,
                                        priority = "10"
                                    };
                                    DomainRecord item = record2;
                                    int num = 1;
                                    for (int j = 2; j < strArray.Length; j++)
                                    {
                                        string str4 = strArray[j];
                                        if (!string.IsNullOrEmpty(str4))
                                        {
                                            switch (num)
                                            {
                                                case 1:
                                                    item.type = str4;
                                                    num++;
                                                    break;

                                                case 2:
                                                    item.value = str4;
                                                    num++;
                                                    break;

                                                case 3:
                                                    item.ttl = str4;
                                                    num++;
                                                    break;
                                            }
                                        }
                                    }
                                    list.Add(item);
                                    str = reader.ReadLine();
                                }
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        this.Alert(exception.Message);
                    }
                };
                worker.RunWorkerCompleted += delegate (object a, RunWorkerCompletedEventArgs b) {
                    this.listView1.Items.Clear();
                    this.bindRecord(list);
                    this.StopWait();
                };
                worker.RunWorkerAsync();
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private static string handleReader(List<DomainRecord> list, StreamReader reader)
        {
            string str2 = reader.ReadLine();
            string str = str2.Substring(10);
            string str3 = "";
            while (str2 != null)
            {
                if (str2 == "")
                {
                    str3 = "";
                    str2 = reader.ReadLine();
                }
                else
                {
                    Array.Sort<string>(zonefile_record_arr);
                    if (Array.BinarySearch<string>(zonefile_record_arr, str2) != -1)
                    {
                        str3 = str2;
                        str2 = reader.ReadLine();
                        continue;
                    }
                    if ((str3 != "") && (str3 != "; NS Records"))
                    {
                        DomainRecord record;
                        string[] strArray = str2.Split(new char[] { '\t' });
                        if (strArray.Length == 5)
                        {
                            DomainRecord record2 = new DomainRecord {
                                type = strArray[3],
                                name = strArray[0],
                                value = strArray[4],
                                ttl = strArray[1]
                            };
                            record = record2;
                            list.Add(record);
                        }
                        else
                        {
                            DomainRecord record3 = new DomainRecord {
                                type = strArray[3],
                                name = strArray[0],
                                value = strArray[5],
                                ttl = strArray[1],
                                priority = strArray[4]
                            };
                            record = record3;
                            list.Add(record);
                        }
                    }
                    str2 = reader.ReadLine();
                }
            }
            return str;
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(FImportRecords));
            this.statusStrip1 = new StatusStrip();
            this.toolStrip1 = new ToolStrip();
            this.listView1 = new ListView();
            this.columnHeader2 = new ColumnHeader();
            this.columnHeader3 = new ColumnHeader();
            this.columnHeader4 = new ColumnHeader();
            this.columnHeader5 = new ColumnHeader();
            this.columnHeader6 = new ColumnHeader();
            this.btnImportZonefile = new ToolStripButton();
            this.lblStatus = new ToolStripStatusLabel();
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            base.SuspendLayout();
            this.statusStrip1.Items.AddRange(new ToolStripItem[] { this.lblStatus });
            this.statusStrip1.Location = new Point(0, 0x15c);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new Size(0x28e, 0x16);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            this.toolStrip1.Items.AddRange(new ToolStripItem[] { this.btnImportZonefile });
            this.toolStrip1.Location = new Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new Size(0x28e, 0x19);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            this.listView1.CheckBoxes = true;
            this.listView1.Columns.AddRange(new ColumnHeader[] { this.columnHeader2, this.columnHeader3, this.columnHeader4, this.columnHeader5, this.columnHeader6 });
            this.listView1.Dock = DockStyle.Fill;
            this.listView1.Location = new Point(0, 0x19);
            this.listView1.Name = "listView1";
            this.listView1.Size = new Size(0x28e, 0x143);
            this.listView1.TabIndex = 2;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = View.Details;
            this.columnHeader2.Text = "类型";
            this.columnHeader3.Text = "记录名称";
            this.columnHeader3.Width = 200;
            this.columnHeader4.Text = "记录值";
            this.columnHeader4.Width = 150;
            this.columnHeader5.Text = "TTL";
            this.columnHeader6.Text = "优先级";
            this.columnHeader6.TextAlign = HorizontalAlignment.Right;
            this.btnImportZonefile.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.btnImportZonefile.Image = (Image) manager.GetObject("btnImportZonefile.Image");
            this.btnImportZonefile.ImageTransparentColor = Color.Magenta;
            this.btnImportZonefile.Name = "btnImportZonefile";
            this.btnImportZonefile.Size = new Size(0x51, 0x16);
            this.btnImportZonefile.Text = "导入Zonefile";
            this.btnImportZonefile.Click += new EventHandler(this.btnImportZonefile_Click);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new Size(0x83, 0x11);
            this.lblStatus.Text = "toolStripStatusLabel1";
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x28e, 370);
            base.Controls.Add(this.listView1);
            base.Controls.Add(this.toolStrip1);
            base.Controls.Add(this.statusStrip1);
            base.Name = "FImportRecords";
            this.Text = "FImportRecords";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void StartWait()
        {
            if (base.InvokeRequired)
            {
                ThreadStart method = new ThreadStart(this.StartWait);
                base.Invoke(method);
            }
            else
            {
                this.pnl = new MyPanel();
                this.pnl.Dock = DockStyle.Fill;
                this.pnl.BackColor = Color.FromArgb(150, Color.LightYellow);
                this.pnl.Parent = this;
                base.Controls.Add(this.pnl);
                this.pnl.BringToFront();
                this.lblStatus.Text = "请等待";
                this.Refresh();
            }
        }

        private void StopWait()
        {
            if (base.InvokeRequired)
            {
                ThreadStart method = new ThreadStart(this.StopWait);
                base.Invoke(method);
            }
            else
            {
                if (base.Controls.Contains(this.pnl))
                {
                    base.Controls.Remove(this.pnl);
                }
                this.lblStatus.Text = "就绪";
                this.Refresh();
            }
        }
    }
}

