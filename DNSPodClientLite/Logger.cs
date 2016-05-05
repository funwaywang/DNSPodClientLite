namespace DNSPodClientLite
{
    using System;
    using System.IO;
    using System.Threading;

    public class Logger
    {
        private readonly string _filename;
        private static string _logdir;

        public Logger(string filename)
        {
            this._filename = filename;
            if (string.IsNullOrEmpty(_logdir))
            {
                throw new ApplicationException("please init frist");
            }
        }

        public void Error(string msg, params object[] args)
        {
            msg = FormatMsg(msg, args, "Error");
            this.WriteLine(msg);
        }

        private static string FormatMsg(string msg, object[] args, string level)
        {
            try
            {
                msg = string.Format(msg, args);
                msg = string.Format("[{0}]({1})-{2}", level, DateTime.Now, msg);
                return msg;
            }
            catch (Exception)
            {
                return msg;
            }
        }

        public void Info(string msg, params object[] args)
        {
            msg = FormatMsg(msg, args, "Info");
            this.WriteLine(msg);
        }

        public static void Init()
        {
            _logdir = Path.Combine(Environment.CurrentDirectory, "log");
            if (!Directory.Exists(_logdir))
            {
                Directory.CreateDirectory(_logdir);
            }
        }

        public void Warn(string msg, params object[] args)
        {
            msg = FormatMsg(msg, args, "Warn");
            this.WriteLine(msg);
        }

        private void WriteLine(string msg)
        {
            Type type;
            Monitor.Enter(type = base.GetType());
            try
            {
                string str = string.Format("{0}-{1}.log", this._filename, DateTime.Now.ToString("yyyy-MM-dd"));
                using (StreamWriter writer = new StreamWriter(Path.Combine(_logdir, str), true))
                {
                    writer.WriteLine(msg);
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                Monitor.Exit(type);
            }
        }
    }
}

