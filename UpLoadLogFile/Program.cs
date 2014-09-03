using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework;
using IBatisNet.Common.Logging;
using WorkQualityReport.Config;
using WorkQualityReport.Dal;

namespace UpLoadLogFile
{
    class Program
    {
        static void Main(string[] args)
        {
             
            var source = (IConfigurationSource)System.Configuration.ConfigurationManager.GetSection("activerecord");
            Assembly assembly = Assembly.Load("WorkQualityReport");
            ActiveRecordStarter.Initialize(assembly, source);

           
            GetReprotDataDal getReprotDataDal = new GetReprotDataDal();
            getReprotDataDal.UpLoadLog();
        }
    }
}
