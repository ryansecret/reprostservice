#region

using System.Collections.Generic;
using System.Runtime.Serialization;
using Castle.ActiveRecord;
using Wpms.App.Core.Framework.Entity;

#endregion

namespace WorkQualityReport.Model
{
    [ActiveRecord("WORK_REPORT_BUILDING", Lazy = false)]
    [DataContract]
    public class WorkReportBuildingEntity : BaseEntity
    {
        [PrimaryKey("ID", SequenceName = "SEQ_WORK_REPORT_BUILDING")]
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

        [Property(Column = "BUILD_AUID", NotNull = false)]
        [DataMember]
        public virtual string BuildAuid { get; set; }

        [Property(Column = "BUILDING_NAME", NotNull = false)]
        [DataMember]
        public virtual string BuildingName { get; set; }

        [Property(Column = "BUILDING_TYPE", NotNull = false)]
        [DataMember]
        public virtual decimal? BuildingType { get; set; }

        [Property(Column = "BUILDING_USE", NotNull = false)]
        [DataMember]
        public virtual string BuildingUse { get; set; }

        public bool CanUpLoad { get; set; }

        [DataMember]
        public List<WorkReportFloorEntity> WorkReportFloors { get; set; } 
    }

    [ActiveRecord]
    public class LogInfo : BaseEntity
    {

        [Property(Column = "province_id", NotNull = true)]
        public virtual decimal? ProvinceId { get; set; }

        [PrimaryKey("auid")]
       
        public virtual decimal? LogId { get; set; }

        [Property(Column = "work_id", NotNull = true)]
        public virtual decimal? WorkId { get; set; }

        [Property(Column = "items_id", NotNull = true)]
        public virtual decimal? SubItemId { get; set; }

        [Property(Column = "node_id", NotNull = true)]
        public virtual decimal? NodeId { get; set; }

        [Property(Column = "log_path", NotNull = true)]
        public virtual string LogPath { get; set; }
    }
}