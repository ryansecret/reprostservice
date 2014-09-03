using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorkQualityReport.Utility
{
    public class LogManager
    {
        public static readonly LogManager Instance = new LogManager();

        private LogManager()
        {
            Log=new ConsoleLog();
        }

        public ILog Log { get; set; }
    }


}

