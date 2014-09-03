using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorkQualityReport.Model
{
    public class UploadClause
    {
        public List<decimal?> LogIds { get; set; }

        public int WorkId { get; set; }

        public int SubGroupId { get; set; }
    }
}
