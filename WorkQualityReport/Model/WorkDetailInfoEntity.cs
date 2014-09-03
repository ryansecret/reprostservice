#region

using System;
using System.Runtime.Serialization;
using Castle.ActiveRecord;
using Wpms.App.Core.Framework.Entity;

#endregion

namespace WorkQualityReport.Model
{
    [ActiveRecord("WORK_DETAIL_INFO", Lazy = false)]
    [DataContract]
    public class WorkDetailInfoEntity : BaseEntity
    {
        [PrimaryKey("WORK_ID", SequenceName = "SEQ_WORK_DETAIL_INFO")]
        [DataMember]
        public virtual decimal? WorkId { get; set; }

        [Property(Column = "MANUAL_ID", NotNull = false)]
        [DataMember]
        public virtual string ManualId { get; set; }

        [Property(Column = "WORK_NAME", NotNull = false)]
        [DataMember]
        public virtual string WorkName { get; set; }

        [Property(Column = "WORK_AREA", NotNull = false)]
        [DataMember]
        public virtual decimal? WorkArea { get; set; }

        [Property(Column = "WORK_TYPE", NotNull = false)]
        [DataMember]
        public virtual decimal? WorkType { get; set; }

        [Property(Column = "CREATE_USERID", NotNull = false)]
        [DataMember]
        public virtual string CreateUserid { get; set; }

        [Property(Column = "CREATE_USER_NAME", NotNull = false)]
        [DataMember]
        public virtual string CreateUserName { get; set; }

        [Property(Column = "CREATE_TIME", NotNull = false)]
        [DataMember]
        public virtual decimal? CreateTime { get; set; }

        [Property(Column = "PLAN_STIME", NotNull = false)]
        [DataMember]
        public virtual decimal? PlanStime { get; set; }

        [Property(Column = "PLAN_ETIME", NotNull = false)]
        [DataMember]
        public virtual decimal? PlanEtime { get; set; }

        [Property(Column = "ACT_STIME", NotNull = false)]
        [DataMember]
        public virtual decimal? ActStime { get; set; }

        [Property(Column = "ACT_ETIME", NotNull = false)]
        [DataMember]
        public virtual decimal? ActEtime { get; set; }

        [Property(Column = "SENDER_USERID", NotNull = false)]
        [DataMember]
        public virtual string SenderUserid { get; set; }

        [Property(Column = "SENDER_USER_NAME", NotNull = false)]
        [DataMember]
        public virtual string SenderUserName { get; set; }

        [Property(Column = "SENDER_EMAIL", NotNull = false)]
        [DataMember]
        public virtual string SenderEmail { get; set; }

        [Property(Column = "SENDER_TEL", NotNull = false)]
        [DataMember]
        public virtual string SenderTel { get; set; }

        [Property(Column = "TESTER_USERID", NotNull = false)]
        [DataMember]
        public virtual string TesterUserid { get; set; }

        [Property(Column = "TESTER_USER_NAME", NotNull = false)]
        [DataMember]
        public virtual string TesterUserName { get; set; }

        [Property(Column = "TESTER_EMAIL", NotNull = false)]
        [DataMember]
        public virtual string TesterEmail { get; set; }

        [Property(Column = "TESTER_TEL", NotNull = false)]
        [DataMember]
        public virtual string TesterTel { get; set; }

        [Property(Column = "PROVINCE_ID", NotNull = false)]
        [DataMember]
        public virtual decimal? ProvinceId { get; set; }

        [Property(Column = "CITY_ID", NotNull = false)]
        [DataMember]
        public virtual decimal? CityId { get; set; }

        [Property(Column = "AREA_ID", NotNull = false)]
        [DataMember]
        public virtual decimal? AreaId { get; set; }

        [Property(Column = "TEST_SITE", NotNull = false)]
        [DataMember]
        public virtual string TestSite { get; set; }

        [Property(Column = "TEST_BUILDING", NotNull = false)]
        [DataMember]
        public virtual string TestBuilding { get; set; }

        [Property(Column = "ADDRESS", NotNull = false)]
        [DataMember]
        public virtual string Address { get; set; }

        [Property(Column = "SITE_SUM", NotNull = false)]
        [DataMember]
        public virtual decimal? SiteSum { get; set; }

        [Property(Column = "BUILDING_SUM", NotNull = false)]
        [DataMember]
        public virtual decimal? BuildingSum { get; set; }

        [Property(Column = "KM_SUM", NotNull = false)]
        [DataMember]
        public virtual decimal? KmSum { get; set; }

        [Property(Column = "WORK_STATE", NotNull = false)]
        [DataMember]
        public virtual decimal? WorkState { get; set; }

        [Property(Column = "IS_DELAY", NotNull = false)]
        [DataMember]
        public virtual decimal? IsDelay { get; set; }

        [Property(Column = "DSTART_TIME", NotNull = false)]
        [DataMember]
        public virtual decimal? DstartTime { get; set; }

        [Property(Column = "DEND_TIME", NotNull = false)]
        [DataMember]
        public virtual decimal? DendTime { get; set; }

        [Property(Column = "DELAY_REASON", NotNull = false)]
        [DataMember]
        public virtual string DelayReason { get; set; }

        [Property(Column = "NET_TYPE", NotNull = false)]
        [DataMember]
        public virtual decimal? NetType { get; set; }

        [Property(Column = "COMP_ID", NotNull = false)]
        [DataMember]
        public virtual decimal? CompId { get; set; }

        [Property(Column = "COMP_NAME", NotNull = false)]
        [DataMember]
        public virtual string CompName { get; set; }

        [Property(Column = "FACTORY", NotNull = false)]
        [DataMember]
        public virtual string Factory { get; set; }

        [Property(Column = "ISRECIVED", NotNull = false)]
        [DataMember]
        public virtual decimal? Isrecived { get; set; }

        [Property(Column = "ISCOMPLETE", NotNull = false)]
        [DataMember]
        public virtual decimal? Iscomplete { get; set; }

        [Property(Column = "REMARK", NotNull = false)]
        [DataMember]
        public virtual string Remark { get; set; }

        [Property(Column = "LEVEL_VAL", NotNull = false)]
        [DataMember]
        public virtual decimal? LevelVal { get; set; }

        [Property(Column = "NEW_BUILDING_NAME", NotNull = false)]
        [DataMember]
        public virtual string NewBuildingName { get; set; }

        [Property(Column = "IS_NEW_BUILDING", NotNull = false)]
        [DataMember]
        public virtual decimal? IsNewBuilding { get; set; }

        [Property(Column = "TEST_TIME", NotNull = false)]
        [DataMember]
        public virtual decimal? TestTime { get; set; }

        [Property(Column = "GPS_DISTANCE", NotNull = false)]
        [DataMember]
        public virtual decimal? GpsDistance { get; set; }

        [Property(Column = "ISNOTTESTED", NotNull = false)]
        [DataMember]
        public virtual decimal? Isnottested { get; set; }

        [Property(Column = "DBCREATETIME", NotNull = false)]
        [DataMember]
        public virtual DateTime? Dbcreatetime { get; set; }

        [Property(Column = "ISAUTOCREATE", NotNull = false)]
        [DataMember]
        public virtual decimal? Isautocreate { get; set; }

        [Property(Column = "PROBLEMTYPE", NotNull = false)]
        [DataMember]
        public virtual decimal? Problemtype { get; set; }

        [Property(Column = "NOAUDITCOMPLETE", NotNull = false)]
        [DataMember]
        public virtual decimal? Noauditcomplete { get; set; }

        [Property(Column = "ADMIN_REGION", NotNull = false)]
        [DataMember]
        public virtual decimal? AdminRegion { get; set; }
        [DataMember]
        public WorkSubItemGroupEntity WorkSubItemGroup { get; set; }
        [DataMember]
        public bool HaveQuality { get; set; }
    }
}