using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorkQualityReport.Model
{
    public  class OrderClause
    {

        public string WorkId { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
