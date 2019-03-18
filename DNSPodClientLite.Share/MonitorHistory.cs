using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DNSPodClientLite
{
    public class MonitorHistory
    {
        private static Dictionary<int, MonitorHistory> _history = new Dictionary<int, MonitorHistory>();
        private static string _logdir;
        private readonly int _recordid;
        private const ushort HighestBitIs1 = 0x8000;
        private const ushort Low15BitsIs1 = 0x7fff;

        public MonitorHistory(int recordid)
        {
            _recordid = recordid;
        }

        public static MonitorHistory Get(int recordid)
        {
            lock (typeof(MonitorData))
            {
                MonitorHistory history;
                if (!_history.TryGetValue(recordid, out history))
                {
                    history = new MonitorHistory(recordid);
                    _history.Add(recordid, history);
                }
                return history;
            }
        }

        public List<MonitorData> GetOneDayHistorys(DateTime day)
        {
            List<MonitorData> list = new List<MonitorData>();
            string str = string.Format("mon{0}-{1}.log", _recordid, DateTime.Now.ToString("yyyy-MM-dd"));
            str = Path.Combine(_logdir, str);
            if (!File.Exists(str))
            {
                return list;
            }
            List<ushort> list2 = new List<ushort>();
            lock (this)
            {
                FileStream input = new FileStream(str, FileMode.Open, FileAccess.Read);
                using (BinaryReader reader = new BinaryReader(input, Encoding.UTF8))
                {
                    while (true)
                    {
                        try
                        {
                            ushort item = reader.ReadUInt16();
                            list2.Add(item);
                        }
                        catch (IOException)
                        {
                            goto Label_00B5;
                        }
                    }
                }
            }
        Label_00B5:
            foreach (ushort num2 in list2)
            {
                DateTime today = DateTime.Today;
                int num3 = num2 & 0x7fff;
                today = today.AddMinutes(num3);
                bool flag = (num2 & 0x8000) == 0x8000;
                MonitorData data2 = new MonitorData
                {
                    Time = today,
                    Down = flag
                };
                MonitorData data = data2;
                list.Add(data);
            }
            return list;
        }

        public static void Init()
        {
            _logdir = Path.Combine(Environment.CurrentDirectory, "log");
            if (!Directory.Exists(_logdir))
            {
                Directory.CreateDirectory(_logdir);
            }
        }

        public void WriteData(DateTime time, bool down)
        {
            string str = string.Format("mon{0}-{1}.log", _recordid, DateTime.Now.ToString("yyyy-MM-dd"));
            str = Path.Combine(_logdir, str);
            ushort totalMinutes = (ushort)time.TimeOfDay.TotalMinutes;
            if (down)
            {
                totalMinutes = (ushort)(totalMinutes | 0x8000);
            }
            lock (this)
            {
                FileStream output = new FileStream(str, FileMode.Append, FileAccess.Write);
                using (BinaryWriter writer = new BinaryWriter(output, Encoding.UTF8))
                {
                    writer.Write(totalMinutes);
                }
            }
        }
    }
}

