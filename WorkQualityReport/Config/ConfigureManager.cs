using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using WorkQualityReport.Utility;

namespace WorkQualityReport.Config
{
    public class ConfigureManager
    {
      public  static LogseverJtConfig Logsever { get; set; }

        static ConfigureManager()
        {
            
            try
            {
                Logsever = ConfigurationManager.GetSection("logserver") as LogseverJtConfig;
            }
            catch
            {
                Action getCongig = () =>
                {
                    var configuration = WebConfigurationManager.OpenWebConfiguration("/Web.config", "dynamic");
                    Logsever = configuration.GetSection("logserver") as LogseverJtConfig;
                };
                try
                {

                    if (Logsever == null)
                    {

                        Logsever = WebConfigurationManager.GetSection("logserver") as LogseverJtConfig;
                        if (Logsever == null)
                        {
                            throw new ConfigurationErrorsException("缺少配置节点，请配置！");
                        }
                    }
                }
                catch
                {
                    getCongig();
                }
            }
        }
    }
}
