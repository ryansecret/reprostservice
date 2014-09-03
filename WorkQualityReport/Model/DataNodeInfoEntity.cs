using Castle.ActiveRecord;
using Wpms.App.Core.Framework.Entity;

namespace WorkQualityReport.Model
{
    [ActiveRecord("DATA_NODE_INFO", Lazy = false)]
    public class DataNodeInfoEntity : BaseEntity
    {
        [PrimaryKey(Column = "NODEID")]
        public virtual decimal? NodeId
        {
            get;
            set;
        }
        [Property(Column = "NODENAME", NotNull = false)]
        public virtual string NodeName
        {
            get;
            set;
        }
        [Property(Column = "NODEIP", NotNull = false)]
        public virtual string NodeIp
        {
            get;
            set;
        }
        [Property(Column = "CIFS_ACCOUT", NotNull = false)]
        public virtual string CifsAccout
        {
            get;
            set;
        }
        [Property(Column = "CIFS_PASSWD", NotNull = false)]
        public virtual string CifsPasswd
        {
            get;
            set;
        }
        [Property(Column = "CIFS_DIR", NotNull = false)]
        public virtual string CifsDir
        {
            get;
            set;
        }
        [Property(Column = "NFS_DIR", NotNull = false)]
        public virtual string NfsDir
        {
            get;
            set;
        }
    }
}
