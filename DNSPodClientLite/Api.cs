namespace DNSPodClientLite
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Web;
    using System.Xml;

    public class Api
    {
        public Api(string loginEmail, string loginPassword, IPEndPoint local)
        {
            this.LoginEmail = loginEmail;
            this.LoginPassword = loginPassword;
            this.Local = local;
        }

        public void ChangeIP(int domainid, int recordid, string ip)
        {
            Record record = this.GetRecord(domainid, recordid);
            string data = string.Format("domain_id={0}&record_id={1}&sub_domain={2}&record_line={3}&value={4}&record_type={5}&mx={6}&ttl={7}", new object[] { domainid, recordid, record.Name, record.Line, ip, record.RecordType, record.MX, record.TTL });
            Result result = ParseResult(this.GetWebData("Record.Modify", data));
            if (result.Code != 1)
            {
                throw new ApplicationException(result.Message);
            }
        }

        public void Ddns(int domainid, int recordid, string ip)
        {
            Logger logger = new Logger("ddns");
            Record record = this.GetRecord(domainid, recordid);
            string data = string.Format("domain_id={0}&record_id={1}&sub_domain={2}&record_line={3}&value={4}", new object[] { domainid, recordid, record.Name, record.Line, ip });
            Result result = ParseResult(this.GetWebData("Record.Ddns", data));
            logger.Info("change ip 5:{0} {1} {2}", new object[] { ip, data, result.Text });
            if (result.Code != 1)
            {
                throw new ApplicationException(data + result.Message);
            }
        }

        public List<Domain> GetDomainList()
        {
            List<Domain> list = new List<Domain>();
            XmlElement webData = this.GetWebData("Domain.List", null);
            Result result = ParseResult(webData);
            if (result.Code != 1)
            {
                throw new ApplicationException(result.Message);
            }
            XmlNodeList list2 = webData.SelectNodes("domains/item");
            foreach (XmlNode node in list2)
            {
                Domain domain2 = new Domain {
                    DomainId = int.Parse(node["id"].InnerText),
                    Name = node["name"].InnerText
                };
                Domain item = domain2;
                list.Add(item);
            }
            return list;
        }

        public Record GetRecord(int domainid, int recordid)
        {
            string data = string.Format("domain_id={0}&record_id={1}", domainid, recordid);
            XmlElement webData = this.GetWebData("Record.Info", data);
            Result result = ParseResult(webData);
            if (result.Code != 1)
            {
                throw new ApplicationException(data + result.Message);
            }
            XmlNode node = webData.SelectSingleNode("record");
            return new Record { RecordId = int.Parse(node["id"].InnerText), MX = int.Parse(node["mx"].InnerText), TTL = int.Parse(node["ttl"].InnerText), Name = node["sub_domain"].InnerText, Line = node["record_line"].InnerText, Value = node["value"].InnerText, RecordType = node["record_type"].InnerText };
        }

        public List<Record> GetRecordList(int domainid)
        {
            List<Record> list = new List<Record>();
            XmlElement webData = this.GetWebData("Record.List", string.Format("domain_id={0}", domainid));
            Result result = ParseResult(webData);
            if (result.Code != 1)
            {
                throw new ApplicationException(result.Message);
            }
            XmlNodeList list2 = webData.SelectNodes("records/item");
            foreach (XmlNode node in list2)
            {
                Record record2 = new Record {
                    RecordId = int.Parse(node["id"].InnerText),
                    Name = node["name"].InnerText,
                    Line = node["line"].InnerText,
                    Value = node["value"].InnerText,
                    RecordType = node["type"].InnerText
                };
                Record item = record2;
                list.Add(item);
            }
            return list;
        }

        public XmlElement GetWebData(string op, string data)
        {
            data = string.Format("login_email={0}&login_password={1}&format=xml&lang=cn&{2}", HttpUtility.UrlEncode(this.LoginEmail), HttpUtility.UrlEncode(this.LoginPassword), data);
            data.TrimEnd(new char[] { '&' });
            WebClient client = new MyWebClient(this.Local);
            client.Headers.Add("user-agent", "DNSPodClientLite/1.0.0 (onlytiancai@gmail.com)");
            client.Headers.Add("Content-type", "application/x-www-form-urlencoded");
            client.Encoding = Encoding.UTF8;
            string xml = client.UploadString("https://dnsapi.cn/" + op, data);
            XmlDocument document = new XmlDocument();
            document.LoadXml(xml);
            return document["dnspod"];
        }

        private static Result ParseResult(XmlElement doc)
        {
            int num = int.Parse(doc["status"]["code"].InnerText);
            string innerText = doc["status"]["message"].InnerText;
            return new Result { Code = num, Message = innerText, Text = doc.OuterXml };
        }

        public IPEndPoint Local { get; set; }

        public string LoginEmail { get; set; }

        public string LoginPassword { get; set; }

        public class Domain
        {
            public int DomainId { get; set; }

            public string Name { get; set; }
        }

        public class Record
        {
            public bool Enabled { get; set; }

            public bool IsDdns { get; set; }

            public bool IsMonitor { get; set; }

            public string Line { get; set; }

            public int MX { get; set; }

            public string Name { get; set; }

            public int RecordId { get; set; }

            public string RecordType { get; set; }

            public int TTL { get; set; }

            public string Value { get; set; }
        }

        public class Result
        {
            public int Code { get; set; }

            public string Message { get; set; }

            public string Text { get; set; }
        }
    }
}

