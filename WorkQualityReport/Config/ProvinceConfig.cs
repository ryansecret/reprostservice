using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace WorkQualityReport.Config
{
    public class ProvinceConfig : ConfigurationElement
    {
        [ConfigurationProperty("dbConnection", IsRequired = true)]
        public string DbConnection
        {
            get
            {
                return (string) base["dbConnection"];
            }
            set
            {
                base["dbConnection"] = value;
            }
        }

        [ConfigurationProperty("ip", IsRequired = true)]
        public string IP
        {
            get
            {
                return (string) base["ip"];
            }
            set
            {
                base["ip"] = value;
            }
        }


        [ConfigurationProperty("ftpUserName", IsRequired = true)]
        public string UserName
        {
            get { return (string)base["ftpUserName"]; ; }
            set { base["ftpUserName"] = value; }
        }

        [ConfigurationProperty("ftpPwd", IsRequired = true)]
        public string Pwd
        {
            get { return (string)base["ftpPwd"]; ; }
            set { base["ftpPwd"] = value; }
        }

        [ConfigurationProperty("defaultSchem", IsRequired = true)]
        public string DefaultSchem
        {
            get { return (string) base["defaultSchem"]; }
            set { base["defaultSchem"] = value; }
        }
    }
}
