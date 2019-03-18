using DNSPodClientLite.ConfigTool.Properties;
using DNSPodClientLite.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DNSPodClientLite.ConfigTool
{
    public partial class ControlPanelForm : Form
    {
        private Logger logger;

        public ControlPanelForm()
        {
            InitializeComponent();

            logger = new Logger("ui");
            Icon = Resources.App;
        }

        private void TsbImport_Click(object sender, EventArgs e)
        {
            new FImportRecords().Show();
        }

        private void TsbNetCard_Click(object sender, EventArgs e)
        {
            new FNetCard().Show();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            try
            {
                ImageList list3 = new ImageList
                {
                    ImageSize = new Size(1, 20)
                };
                ImageList list = list3;
                lvDomains.SmallImageList = list;
                ListViewExtender extender = new ListViewExtender(lvDomains);
                ListViewButtonColumn column = new ListViewButtonColumn(1);
                column.Click += new EventHandler<ListViewColumnMouseEventArgs>(OnButtonActionClick);
                extender.AddColumn(column);
                column = new ListViewButtonColumn(2);
                column.Click += new EventHandler<ListViewColumnMouseEventArgs>(OnButtonActionClick);
                extender.AddColumn(column);
                List<DnsPodApi.Domain> domainList = AppStatus.Default.Api.GetDomainList();
                foreach (DnsPodApi.Domain domain in domainList)
                {
                    ListViewItem item2 = new ListViewItem(domain.Name)
                    {
                        Tag = domain
                    };
                    ListViewItem item = item2;
                    item.SubItems.Add("动态解析");
                    item.SubItems.Add("监控");
                    lvDomains.Items.Add(item);
                }
            }
            catch (Exception exception)
            {
                logger.Error("FDomainList_Load has an error:{0}", new object[] { exception });
                MessageBox.Show(this, exception.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);

            ServiceHelper.RestartService("DNSPodLite", 0xbb8, logger);
        }

        public void NotifyInfo(bool warn, string msg)
        {
            if (InvokeRequired)
            {
                DNotifyInfo method = new DNotifyInfo(NotifyInfo);
                Invoke(method, new object[] { warn, msg });
            }
            else
            {
                ListViewItem item2 = new ListViewItem(msg)
                {
                    ToolTipText = msg
                };
                ListViewItem item = item2;
                if (warn)
                {
                    item.BackColor = Color.Red;
                }
                lock (lvMsg)
                {
                    lvMsg.Items.Insert(0, item);
                    if (lvMsg.Items.Count > 0x3e8)
                    {
                        lvMsg.Items.Clear();
                    }
                }
            }
        }

        private void OnButtonActionClick(object sender, ListViewColumnMouseEventArgs e)
        {
            DnsPodApi.Domain tag = (DnsPodApi.Domain)e.Item.Tag;
            if (e.SubItem.Text == "动态解析")
            {
                new FDdns(tag).Show();
            }
            else
            {
                new FMonitor(tag).Show();
            }
        }

        private delegate void DNotifyInfo(bool warn, string msg);
    }
}
