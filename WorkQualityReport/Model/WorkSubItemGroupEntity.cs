#region

using System;
using System.Runtime.Serialization;
using Castle.ActiveRecord;
using NHibernate.Mapping;
using Wpms.App.Core.Framework.Entity;

#endregion

namespace WorkQualityReport.Model
{
    [ActiveRecord("WORK_SUB_ITEM_GROUP", Lazy = false)]
    [DataContract]
    public class WorkSubItemGroupEntity : BaseEntity
    {
        [PrimaryKey("ID", SequenceName = "SEQ_WORK_SUB_ITEM_GROUP")]
        [DataMember]
        public virtual decimal? Id { get; set; }

        [Property(Column = "WORK_ID", NotNull = true)]
        [DataMember]
        public virtual decimal? WorkId { get; set; }

        [Property(Column = "GROUP_NAME", NotNull = false)]
        [DataMember]
        public virtual string GroupName { get; set; }



        [Property(Column = "build_auid", NotNull = false)]
        [DataMember]
        public virtual string BuildingId { get; set; }
        [DataMember]
        public virtual decimal? GroupId { get; set; }

         
        [DataMember]
        public virtual decimal? GroupStatus { get; set; }

     
        [DataMember]
        public virtual string Creator { get; set; }

        [Property(Column = "CREATE_DATE", NotNull = false)]
        [DataMember]
        public virtual DateTime? CreateDate { get; set; }

       
        [DataMember]
        public virtual string Description { get; set; }

        
        [DataMember]
        public virtual string TestPerson { get; set; }

        
        [DataMember]
        public virtual decimal? NodeId { get; set; }
        [DataMember]
        [Property(Column = "recordcount", NotNull = false)]
        public int RecordCount { get; set; }
    }
}