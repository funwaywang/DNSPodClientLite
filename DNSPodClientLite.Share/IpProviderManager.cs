using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DNSPodClientLite.Share
{
    public class IpProviderManager
    {
        private static readonly Random random = new Random(DateTime.Now.Millisecond);
        public static readonly IpProviderManager Default = new IpProviderManager();

        private IpProviderManager()
        {
            var ipProviders = LoadProviders();
            IpProviders.AddRange(ipProviders);
        }

        public List<IpProvider> IpProviders { get; private set; } = new List<IpProvider>();

        private List<IpProvider> LoadProviders()
        {
            List<IpProvider> ipProviders = new List<IpProvider>();
            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "IpProviders.txt");
            if (File.Exists(path))
            {
                using (var sr = new StreamReader(path, Encoding.UTF8))
                {
                    var url = sr.ReadLine();
                    while (!string.IsNullOrEmpty(url))
                    {
                        var regex = sr.ReadLine();
                        if (string.IsNullOrEmpty(regex))
                        {
                            break;
                        }

                        ipProviders.Add(new IpProvider(url, regex));

                        url = sr.ReadLine();
                    }
                }
            }

            return ipProviders;
        }

        public IpProvider GetRandomProvider()
        {
            if (IpProviders.Any())
            {
                return IpProviders[random.Next(IpProviders.Count)];
            }
            else
            {
                return null;
            }
        }
    }
}
