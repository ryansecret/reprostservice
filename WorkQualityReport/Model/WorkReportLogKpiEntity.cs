#region

using System.Runtime.Serialization;
using Castle.ActiveRecord;
using IBatisNet.Common.Logging;
using Wpms.App.Core.Framework.Entity;

#endregion

namespace WorkQualityReport.Model
{
    [ActiveRecord("WORK_REPORT_LOG_KPI", Lazy = false)]
    [DataContract]
    public class WorkReportLogKpiEntity : BaseEntity
    {
        [PrimaryKey("ID", SequenceName = "SEQ_WORK_REPORT_LOG_KPI")]
        [DataMember]
        public virtual decimal? Id { get; set; }

        [Property(Column = "LOG_ID", NotNull = false)]
        [DataMember]
        public virtual decimal? LogId { get; set; }

        [Property(Column = "KPI_NAME", NotNull = false)]
        [DataMember]
        public virtual string KpiName { get; set; }

        [Property(Column = "KPI_VALUE", NotNull = false)]
        [DataMember]
        public virtual decimal? KpiValue { get; set; }

        [Property(Column = "KPI_RANGE", NotNull = false)]
        [DataMember]
        public virtual string KpiRange { get; set; }

        [DataMember]
        public virtual decimal? TestScence { get; set; }
    }
}