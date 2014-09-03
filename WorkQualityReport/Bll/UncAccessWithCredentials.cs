using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace WorkQualityReport.Bll
{
    public class UncAccessWithCredentials : IDisposable
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct UseInfo2
        {
            internal String ui2_local;
            internal String ui2_remote;
            internal String ui2_password;
            internal UInt32 ui2_status;
            internal UInt32 ui2_asg_type;
            internal UInt32 ui2_refcount;
            internal UInt32 ui2_usecount;
            internal String ui2_username;
            internal String ui2_domainname;
        }

        [DllImport("NetApi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern UInt32 NetUseAdd(
            String UncServerName,
            UInt32 Level,
            ref UseInfo2 Buf,
            out UInt32 ParmError);

        [DllImport("NetApi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern UInt32 NetUseDel(
            String UncServerName,
            String UseName,
            UInt32 ForceCond);

        private bool disposed = false;

        private string sUNCPath;
        private string sUser;
        private string sPassword;
        private string sDomain;
        private int iLastError;

        /// <summary>
        /// A disposeable class that allows access to a UNC resource with credentials.
        /// </summary>
        public UncAccessWithCredentials()
        {
        }

        /// <summary>
        /// 返回最后系统错误（NetUseAdd 或 NetUseDel），成功返回空字符串
        /// 修改原方法，返回错误对应的描述
        /// </summary>
        public string LastError
        {
            get
            {
                if (this.iLastError == 0)
                {
                    return string.Empty;
                }

                string rtnMessage = string.Empty;
                switch (this.iLastError)
                {
                    case 67:
                        rtnMessage = "找不到网络名";
                        break;
                    case 2250:
                        rtnMessage = "此网络连接不存在";
                        break;
                    case 1219:
                        rtnMessage = "在同一台电脑上使用不同得凭据连接到同一共享资源";
                        break;
                    case 53:
                        rtnMessage = "网络路径不存在";
                        break;
                    default:
                        rtnMessage = "未知错误";
                        break;
                }
                return string.Format("{0}-（Win32 Error Code:{1}）。", rtnMessage, this.iLastError);
            }
        }

        public void Dispose()
        {
           
            if (!this.disposed)
            {
               NetUseDelete();
            }
            this.disposed = true;
            
        }

        /// <summary>
        /// Connects to a UNC path using the credentials supplied.
        /// </summary>
        /// <param name="UNCPath">Fully qualified domain name UNC path</param>
        /// <param name="User">A user with sufficient rights to access the path.</param>
        /// <param name="Domain">Domain of User.</param>
        /// <param name="Password">Password of User</param>
        /// <returns>True if mapping succeeds.  Use LastError to get the system error code.</returns>
        public bool NetUseWithCredentials(string UNCPath, string User, string Domain, string Password)
        {
            this.sUNCPath = UNCPath;
            this.sUser = User;
            this.sPassword = Password;
            this.sDomain = Domain;
            return this.NetUseWithCredentials();
        }

        private bool NetUseWithCredentials()
        {
            uint returncode;
            try
            {
                UseInfo2 useinfo = new UseInfo2();

                useinfo.ui2_remote = this.sUNCPath;
                useinfo.ui2_username = this.sUser;
                useinfo.ui2_domainname = this.sDomain;
                useinfo.ui2_password = this.sPassword;
                useinfo.ui2_asg_type = 0;
                useinfo.ui2_usecount = 1;
                uint paramErrorIndex;
                returncode = NetUseAdd(null, 2, ref useinfo, out paramErrorIndex);
                this.iLastError = (int)returncode;
                return returncode == 0;
            }
            catch
            {
                this.iLastError = Marshal.GetLastWin32Error();
                return false;
            }
        }

        /// <summary>
        /// Ends the connection to the remote resource 
        /// </summary>
        /// <returns>True if it succeeds.  Use LastError to get the system error code</returns>
        public bool NetUseDelete(string unPath=null)
        {
            uint returncode;
            if (unPath==null)
            {
                unPath = this.sUNCPath;
            }
            try
            {
                returncode = NetUseDel(null, unPath, 2);
                this.iLastError = (int)returncode;
                return (returncode == 0);
            }
            catch
            {
                this.iLastError = Marshal.GetLastWin32Error();
                return false;
            }
        }

        ~UncAccessWithCredentials()
        {
            this.Dispose();
        }
    }
}
