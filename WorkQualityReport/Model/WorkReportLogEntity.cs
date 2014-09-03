#region

using System.Collections.Generic;
using System.Runtime.Serialization;
using Castle.ActiveRecord;
using NHibernate.Mapping;
using Wpms.App.Core.Framework.Entity;

#endregion

namespace WorkQualityReport.Model
{
    [ActiveRecord("WORK_REPORT_LOG", Lazy = false)]
    [DataContract]
    public class WorkReportLogEntity : BaseEntity
    {
        [DataMember]
        public bool IsSelected { get; set; } 
         [PrimaryKey("ID", SequenceName = "SEQ_WORK_REPORT_LOG")]
        [DataMember]
        public virtual decimal? Id { get; set; }

        [Property(Column = "WORK_ID", NotNull = true)]
        [DataMember]
        public virtual decimal? WorkId { get; set; }

        [Property(Column = "S_GROUP_ID", NotNull = false)]
        [DataMember]
        public virtual decimal? SGroupId { get; set; }

        [Property(Column = "LOG_ID", NotNull = false)]
        [DataMember]
        public virtual decimal? LogId { get; set; }

        [Property(Column = "LOG_NAME", NotNull = false)]
        [DataMember]
        public virtual string LogName { get; set; }

        [Property(Column = "LOG_SERVERTYPE", NotNull = false)]
        [DataMember]
        public virtual string LogServertype { get; set; }

        [Property(Column = "RESULT", NotNull = false)]
        [DataMember]
        public virtual decimal? Result { get; set; }
        [DataMember]
        public virtual decimal? TestScence { get; set; }

        [Property(Column = "FLOORS_CODE", NotNull = false)]
        [DataMember]
        public virtual string FloorAuid { get; set; }
        [DataMember]
        public List<WorkReportLogKpiEntity> Kpis { get; set; }
    }

    [ActiveRecord]
    public class LogFileInfo : BaseEntity
    {
        [PrimaryKey(Column = "auid")]
        [DataMember]
        public virtual decimal? LogId { get; set; }

        [Property(Column = "log_pfile", NotNull = false)]
        [DataMember]
        public virtual string LogName { get; set; }

        [Property(Column = "log_type", NotNull = false)]
        [DataMember]
        public virtual string LogServertype { get; set; }

        [Property(Column = "building_name", NotNull = false)]
        public virtual  string BuildingName { get; set; }


        [Property(Column = "building_level", NotNull = false)]
        public virtual decimal? BuidingType { get; set; }


        [Property(Column = "floors_id", NotNull = false)]
        [DataMember]
        public virtual string FloorAuid { get; set; }



        [Property(Column = "test_floors", NotNull = false)]
        [DataMember]
        public virtual string FloorName { get; set; }

        [Property(Column = "test_scene", NotNull = false)]
        [DataMember]
        public virtual decimal? TestScence { get; set; }
    }
}