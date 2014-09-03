using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Wpms.App.Core.Util.Common.Helper;

namespace WorkQualityReport.Utility
{
    public interface ILog
    {
        void Info(string msg);
    }

    public  class ConsoleLog:ILog
    {
        public ConsoleLog()
        {
            //Trace.Listeners.Add(new ConsoleTraceListener());
        }
        public void Info(string msg)
        {
             
             System.Console.WriteLine(msg);
            //Trace.WriteLine(msg);
            LogHelper.GetInstance().Info(msg);
        }
    }
}
