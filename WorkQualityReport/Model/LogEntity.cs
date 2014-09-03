using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WorkQualityReport.Model
{
    public  class LogEntity
    {
        public int LogId { get; set; }

        public int NodeId { get; set; }

        public string UpLoadPath { get; set; }

        public int ProvinceId { get; set; }

    

        public byte[] Content { get; set; }

        public byte[] Compare { get; set; }

        public bool Success { get; set; }
        public decimal? ItemsId { get; set; }
    }
}
