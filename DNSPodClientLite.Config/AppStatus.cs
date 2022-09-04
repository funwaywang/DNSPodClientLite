using DNSPodClientLite.Share;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNSPodClientLite
{
    public class AppStatus
    {
        public static readonly AppStatus Default = new AppStatus();
        public DnsPodApi Api { get; private set; }
        public Config Config { get; private set; }
        public DDns Ddns { get; private set; }
        public HttpMonitor Monitor { get; private set; }

        private AppStatus()
        {
        }

        public void LoadConfig()
        {
            Config = Config.Load();
            if (Config == null)
            {
                Config = new Config();
            }
        }

        public void UseTokenLoin(string tokenId, string tokenCode)
        {
            LoadConfig();
            Config.LoginMethod = LoginMethod.Token;
            Config.LoginTokenId = tokenId;
            Config.LoginTokenCode = tokenCode;
            Config.Save();

            Reset();
        }

        public void UseClassicLogin(string email, string password)
        {
            LoadConfig();
            Config.LoginMethod = LoginMethod.Token;
            Config.LoginEmail = email;
            Config.LoginPassword = password;
            Config.Save();

            Reset();
        }

        private void Reset()
        {
            Api = new DnsPodApi(Config, Config.GetLocal());
            Ddns = new DDns(Config.LastIp, Config.GetLocal());
            Monitor = new HttpMonitor(Config);
        }
    }
}
