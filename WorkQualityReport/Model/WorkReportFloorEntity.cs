#region

using System.Collections.Generic;
using System.Runtime.Serialization;
using Castle.ActiveRecord;
using Wpms.App.Core.Framework.Entity;

#endregion

namespace WorkQualityReport.Model
{
    [ActiveRecord("WORK_REPORT_FLOOR", Lazy = false)]
    [DataContract]
    public class WorkReportFloorEntity : BaseEntity
    {
       [PrimaryKey("ID", SequenceName = "SEQ_WORK_REPORT_FLOOR")]
        [DataMember]
        public virtual decimal? Id { get; set; }

        [Property(Column = "WORK_ID", NotNull = true)]
        [DataMember]
        public virtual decimal? WorkId { get; set; }

        [Property(Column = "S_GROUP_ID", NotNull = false)]
        [DataMember]
        public virtual decimal? SGroupId { get; set; }

        [Property(Column = "RESULT", NotNull = false)]
        [DataMember]
        public virtual decimal? Result { get; set; }

        [Property(Column = "FLOORS_TYPE", NotNull = false)]
        [DataMember]
        public virtual string FloorsType { get; set; }

        [Property(Column = "FLOORS_CODE", NotNull = false)]
        [DataMember]
        public virtual string FloorsCode { get; set; }

        [Property(Column = "FLOORS_NAME", NotNull = false)]
        [DataMember]
        public virtual string FloorsName { get; set; }

        
        [DataMember]
        public List<WorkReportLogEntity> WorkReportLogs { get; set; } 
    }
}