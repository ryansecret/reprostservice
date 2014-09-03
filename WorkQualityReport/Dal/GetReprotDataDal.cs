#region

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.FtpClient;
using System.Threading;
using System.Web.Hosting;
using NHibernate.Hql.Ast.ANTLR;
using WorkQualityReport.Bll;
using WorkQualityReport.Config;
using WorkQualityReport.IDal;
using WorkQualityReport.Model;
using WorkQualityReport.Utility;
using Wpms.App.Core.Framework.DataAccess;
using Wpms.App.Core.Framework.Entity;
using Wpms.App.Core.Util.Common.Helper;

#endregion

namespace WorkQualityReport.Dal
{
    public partial class GetReprotDataDal : BaseMapDao, IGetReprotDataDal
    {
        public List<WorkReportBuildingEntity> GetWorkReport(int workId, int subGroupId)
        {

            string searchLog = string.Format(@"select d.test_floors,b.building_name,d.building_id,d.building_level,t.*,d.floors_id from log_file_info t,{2}.work_sub_items d,building_info b  where t.work_id={0} and d.sub_group_id={1} and t.log_quality=1 and t.log_type!=1 and t.test_scene in(3,4)   and t.work_id=d.work_id and t.items_id=d.items_id and d.building_id=b.auid(+)", workId, subGroupId, GetSchem());
            
            var logs = QueryBySql<LogFileInfo>(searchLog);

            string sql = string.Format(
                @"select  t.* from work_report_building t where t.work_id={0} and t.s_group_id={1}", workId, subGroupId);
            var workReportBuildings = QueryBySql<WorkReportBuildingEntity>(sql).ToList();

             Func<LogFileInfo, WorkReportLogEntity> convertLog =
                            d =>
                            {
                                return new WorkReportLogEntity()
                                {
                                    LogId = d.LogId,
                                    LogName = d.LogName,
                                    LogServertype = d.LogServertype,
                                    FloorAuid = d.FloorAuid,
                                    WorkId = workId,
                                    SGroupId = subGroupId,
                                    TestScence = d.TestScence
                                };
                            };
            if (workReportBuildings.Count > 0)
            {
                var reportBuilding = workReportBuildings.First();
                sql = string.Format(
                    @"select  t.* from work_report_floor t where t.work_id={0} and t.s_group_id={1}", workId, subGroupId);
                reportBuilding.WorkReportFloors = QueryBySql<WorkReportFloorEntity>(sql).ToList();

                if (reportBuilding.WorkReportFloors.Count > 0)
                {
                    foreach (var floor in reportBuilding.WorkReportFloors)
                    {
                        sql = string.Format(
                            @"select  t.* from work_report_log t where t.work_id={0} and t.s_group_id={1} and floors_code='{2}'",
                            workId, subGroupId, floor.FloorsCode);
                        floor.WorkReportLogs = QueryBySql<WorkReportLogEntity>(sql).OrderBy(d => d.LogId).ToList();
                        var matchLogs = logs.Where(d => d.FloorAuid == floor.FloorsCode).ToList();
                       

                        if (matchLogs.Any())
                        {
                            floor.WorkReportLogs.AddRange(matchLogs.Select(d=>convertLog(d)));
                            LogManager.Instance.Log.Info("——————————————————————————得到符合的");
                        }
                        foreach (var log in floor.WorkReportLogs)
                        {
                            string sqlFloorKpis = string.Format(
                                @"select  t.id,t.log_id,t.kpi_name,t.kpi_range,round(t.kpi_value,10) kpi_value from WORK_REPORT_LOG_KPI t  where t.LOG_ID={0} order by  t.kpi_name",
                                log.LogId);
                            log.Kpis = QueryBySql<WorkReportLogKpiEntity>(sqlFloorKpis).ToList();
                        }
                    }
                    var reportFloors = reportBuilding.WorkReportFloors.Select(d => d.FloorsCode);
                    var outlaws = logs.Where(d => !reportFloors.Contains(d.FloorAuid)).ToList();
                    if (outlaws.Any())
                    {
                        reportBuilding.WorkReportFloors.AddRange((from logGp in outlaws.GroupBy(d => new { d.FloorAuid, d.FloorName })
                        select
                            new WorkReportFloorEntity()
                            {
                                FloorsCode = logGp.Key.FloorAuid,
                                FloorsName = logGp.Key.FloorName,
                                WorkReportLogs = new List<WorkReportLogEntity>(logGp.Select(d => convertLog(d)))
                            }));
                    }
                    
                }
               
            }
            else
            {
                if (logs.Any())
                {

                WorkReportBuildingEntity reportBuilding = new WorkReportBuildingEntity();

                var logReports= (from logGp in logs.GroupBy(d => new {d.FloorAuid, d.FloorName})
                        select
                            new WorkReportFloorEntity()
                            {
                                FloorsCode = logGp.Key.FloorAuid,
                                FloorsName = logGp.Key.FloorName,
                                WorkReportLogs = new List<WorkReportLogEntity>(logGp.Select(d => convertLog(d)))
                            });
                reportBuilding.WorkReportFloors = logReports.ToList();
                var log = logs.First();
                reportBuilding.BuildingName = log.BuildingName;
                reportBuilding.BuildingType = log.BuidingType;
                  
                workReportBuildings.Add(reportBuilding);
                 
                }
               
            }
            return workReportBuildings;
        }

        public IList<SysDictionary> QueryDict(string value)
        {
            string sql = string.Format("select * from data_type_def t where t.group_name='{0}'", value);
            return HelperDal.ExecuteAndGetInstanceList(sql, null, dr =>
            {
                return new SysDictionary
                {
                    CnName = dr["cnname"].ToString(),
                    EnName = dr["enname"].ToString(),
                    Code = Convert.ToInt32(dr["CODE_ID"])
                };
            });
        }

        public List<WorkDetailInfoEntity> GetAllWorkOrder(OrderClause clause)
        {
         
            var schem = ConfigureManager.Logsever.ProvinceConfig.DefaultSchem;
           
            if (GetFilter().ProvinceId != 0)
            {
                var helperDal = new HelperDal();
                schem = helperDal.GetDbUser(Convert.ToInt32(GetFilter().ProvinceId));
            }
            var subGroups =
                QueryBySql<WorkSubItemGroupEntity>(GetPageSql(string.Format(
                    @" select distinct t.id,t.work_id,t.group_name,t.create_date,r.build_auid,count(1) over() recordCount  from {0}.work_sub_item_group t left join work_report_building r on t.work_id=r.work_id and t.id=r.s_group_id where t.WORK_ID like '%{1}%'
  group by t.work_id,t.group_name,t.create_date,t.id,r.build_auid order by t.create_date desc", schem, clause.WorkId),
                    clause));
            if (subGroups.Count > 0)
            {
                var workOrders =
                    QueryBySql<WorkDetailInfoEntity>(
                        string.Format("select * from {1}.work_detail_info t where t.work_id in ({0})",
                            string.Join(",", subGroups.GroupBy(d => d.WorkId).Select(d => d.Key)), schem));

                foreach (var workSubItemGroupEntity in subGroups)
                {
                    WorkSubItemGroupEntity entity = workSubItemGroupEntity;
                    var orders = workOrders.Where(d => d.WorkId == entity.WorkId);
                    if (orders.Any())
                    {
                        var order = orders.First();
                        if (order != null && order.WorkSubItemGroup != null)
                        {
                            LogManager.Instance.Log.Info("sss"+order.WorkId);
                            order = DeepCopy.DeepCopyEntity(order);
                            workOrders.Add(order);
                        }
                        order.WorkSubItemGroup = entity;
                        if (entity.BuildingId != null)
                        {
                            order.HaveQuality = true;
                        }
                    }
                }
                return workOrders.ToList();
            }
            return new List<WorkDetailInfoEntity>();
        }

        public bool UpLoadLog(UploadClause clause)
        {
            
            LogManager.Instance.Log.Info("开始手动上传log");
            if (!UpLoadLogFile(clause))
            {
                LogManager.Instance.Log.Info("手动上传log失败");
                return false;
            }
            LogManager.Instance.Log.Info("手动上传log成功");

           

            ProcessDb(clause);

           

            return true;
        }

        private void DeleteReleatedWorkItems(int workid, int subGroupId, string clientId)
        {
            var connection = HelperDal.GetConnection(clientId);
            var schem = GetSchem();
            var subGroups = string.Format("delete from {0}.work_sub_item_group t where t.WORK_ID={1} and t.id={2}",
                schem, workid, subGroupId);

            HelperDal.ExcecuteNoQurey(subGroups, connection);
            LogManager.Instance.Log.Info("删除work_sub_item_group记录！");
            var delLog = string.Format("delete   from work_report_log t where t.work_id={0} and t.s_group_id={1}",
                workid, subGroupId);
            var deLogKpi =
                string.Format(
                    "delete from work_report_log_kpi t where t.log_id in (select w.log_id from work_report_log w where w.work_id={0} and w.s_group_id={1})",
                    workid, subGroupId);
            var deFloor = string.Format("delete   from work_report_floor t where t.work_id={0} and t.s_group_id={1} ",
                workid, subGroupId);
            var deBuilding =
                string.Format("delete   from work_report_building t where t.work_id={0} and t.s_group_id={1} ", workid,
                    subGroupId);


            HelperDal.ExcecuteNoQurey(deBuilding, connection);
            // LogManager.Instance.Log.Info("删除work_report_building记录！");
            HelperDal.ExcecuteNoQurey(deFloor, connection);
            // LogManager.Instance.Log.Info("删除work_report_floor记录！");
            HelperDal.ExcecuteNoQurey(delLog, connection);
            //  LogManager.Instance.Log.Info("删除work_report_log记录！");
            HelperDal.ExcecuteNoQurey(deLogKpi, connection);
            // LogManager.Instance.Log.Info("删除work_report_log_kpi记录！");
        }

        private string GetSchem()
        {
            var schem = ConfigureManager.Logsever.ProvinceConfig.DefaultSchem;

            if (GetFilter().ProvinceId != 0)
            {
                var helperDal = new HelperDal();
                schem = helperDal.GetDbUser(Convert.ToInt32(GetFilter().ProvinceId));
            }
            return schem;
        }

        public string GetPageSql(string sql, OrderClause clause)
        {
            return string.Format(@"SELECT t.*,count(1) over() recountCount
                              FROM (SELECT T.*, ROWNUM RN FROM ({0}) T WHERE ROWNUM <= {2}) T
                             WHERE RN > {1}", sql, (clause.PageIndex - 1)*clause.PageSize,
                clause.PageIndex*clause.PageSize);
        }

        private bool UpLoadLogFile(UploadClause clause)
        {
            if (clause.LogIds.Count == 0)
            {
                LogManager.Instance.Log.Info("没有符合条件的log!");
                return false;
            }
            LogManager.Instance.Log.Info("开始上传log!");
            string sql =
                string.Format(
                    "select t.auid,t.node_id,t.log_path,t.province_id from log_file_info t where t.auid in ({0})",
                    string.Join(",", clause.LogIds.Select(d => d.Value)));

            var logs = HelperDal.ExecuteAndGetInstanceList(sql, null, dr =>
            {
                return new LogEntity
                {
                    LogId = Convert.ToInt32(dr["auid"]),
                    NodeId = Convert.ToInt32(dr["node_id"]),
                    ProvinceId = Convert.ToInt32(dr["province_id"]),
                    UpLoadPath = dr["log_path"].ToString()
                };
            },ConfigureManager.Logsever.DbConnection);
            if (logs.Count == 0)
            {
                LogManager.Instance.Log.Info("log表里没有相关log");
                return false;
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();

            GetLogContent(logs);
            try
            {
                MoveFile(logs);
            }
            catch (Exception e)
            {
                LogManager.Instance.Log.Info(e.ToString());
                return false;
            }
            LogManager.Instance.Log.Info("上传成功！");
            return true;
        }

        public DataNodeInfoEntity QueryDataNodeInfoEntity(decimal? provinceId)
        {
            var sql = string.Format("select t.nodeid from data_rule t where t.isvalid=1 and t.province_id={0}", provinceId);
            var nodeId = HelperDal.ExcecuteSalar(sql);
            var list = QueryAllByProperty<DataNodeInfoEntity>("NodeId", Convert.ToDecimal(nodeId));
            if (list != null && list.Any())
            {
                return list[0];
            }
            return null;
        }

        public DataNodeInfoEntity QureyProvinceNodeInfo(decimal? nodeId)
        {
            string sql = string.Format("select * from data_node_info t where t.nodeid={0}", nodeId);
            var list = HelperDal.ExecuteAndGetInstanceList<DataNodeInfoEntity>(sql, null, dr =>
            {
                DataNodeInfoEntity dataNode = new DataNodeInfoEntity();
                dataNode.NodeId = Convert.ToInt32(dr["NODEID"]);
                dataNode.NodeIp = dr["NODEIP"].ToString();
                dataNode.CifsAccout = dr["CIFS_ACCOUT"].ToString();
                dataNode.CifsPasswd = dr["CIFS_PASSWD"].ToString();
                dataNode.CifsDir = dr["CIFS_DIR"].ToString();
                return dataNode;
            }, ConfigurationManager.AppSettings["connection"]);
            if (list.Any())
            {
                return list.First();
            }
            return null;
        }
        private bool AutoUpLoadLog(UploadClause clause)
        {
            if (!UpLoadLogFile(clause))
            {
                return false;
            }

            if (!ProcessDb(clause))
            {
                return false;
            }

            return true;
        }

        private bool ProcessDb(UploadClause clause)
        {
            #region

            string logIds = string.Join(",", clause.LogIds);

            string update =
                string.Format(
                    "update  {0}.work_sub_items t set t.is_upload=1 where t.ITEMS_ID in (select d.items_id from log_file_info d where d.auid in ({1}))",
                    GetSchem(), logIds);
            HelperDal.ExcecuteNoQureyWithoutTrans(update);
            string clientId = Guid.NewGuid().ToString();
            using (var con = HelperDal.GetConnection(clientId))
            {
                HelperDal.BeginTrans(clientId);
                try
                {
                    // HelperDal.ExcecuteNoQurey(del, con);

                    string checkSubItem =
                        string.Format(
                            "select count(t.log_id)  from {2}.work_sub_items t where t.work_id={0} and t.sub_group_id ={1} and t.is_upload=0",
                            clause.WorkId, clause.SubGroupId, GetSchem());
                    if (Convert.ToInt32(HelperDal.ExcecuteSalar(checkSubItem)) == 0)
                    {
                        DeleteReleatedWorkItems(clause.WorkId, clause.SubGroupId, clientId);
                    }
                    else
                    {
                        var delLog = string.Format(@"delete from work_report_log d where d.floors_code in 
(select distinct(t.floors_code) from work_report_log t where t.log_id in ({0}) )", logIds);
                        // var delLog = string.Format("delete   from work_report_log t where t.log_id in ({0}) ", logIds);

                        var deLogKpi =
                            string.Format(
                                "delete from work_report_log_kpi t where t.log_id in ({0})", logIds
                                );


                        HelperDal.ExcecuteNoQurey(delLog, con);
                        HelperDal.ExcecuteNoQurey(deLogKpi, con);
                    }
                    HelperDal.CommitTrans(clientId);
                }
                catch (Exception e)
                {
                    LogHelper.GetInstance().Error("清除数据出错", e);
                    HelperDal.RollBackTrans(clientId);
                }
                finally
                {
                    con.Close();
                }
            }
            return true;

            #endregion
        }


        private string GetFilePath(string rawPath, string part)
        {
            var list = part.Split(new[] {'/'}).ToList();

            string filePath = rawPath;
            foreach (var path in list)
            {
                filePath = Path.Combine(filePath, path);
            }
            return filePath;
        }

        private void MoveFile(IList<LogEntity> logs)
        {
            //FtpTrace.AddListener(new EventLogTraceListener("log上传"));
            using (var conn = new FtpClient())
            {
                var serverConfig = ConfigureManager.Logsever;
                conn.Host = serverConfig.IP;
                conn.Credentials = new NetworkCredential(serverConfig.UserName, serverConfig.Pwd);
                conn.DataConnectionType = FtpDataConnectionType.AutoPassive;
                foreach (var logEntity in logs)
                {
                    LogManager.Instance.Log.Info(string.Format("开始上传log:{0}", logEntity.UpLoadPath));
                    var path = Path.GetFileName(logEntity.UpLoadPath);
                    if (logEntity.Content == null)
                    {
                        LogManager.Instance.Log.Info(string.Format("log:{0} 流内容为空", logEntity.LogId));
                        continue;
                    }
                    LogManager.Instance.Log.Info(string.Format("开始读流"));
                    using (var stream = conn.OpenWrite(path))
                    {
                        stream.Write(logEntity.Content, 0, logEntity.Content.Length);
                    }
                    LogManager.Instance.Log.Info(string.Format("上传结束log:{0}，log大小为：{1}", conn.Host,
                        logEntity.Content.Length));
                    logEntity.Success = true;
                }
                LogManager.Instance.Log.Info("上传结束!");
            }
        }
 
    }

    public partial class GetReprotDataDal
    {
        public bool UpLoadLog()
        {
            LogManager.Instance.Log.Info("开始扫描……");
            string sql = string.Format(
                @"select  t.* from work_report_building t");
            var workReportBuildings = QueryBySql<WorkReportBuildingEntity>(sql).ToList();
            var canUpLoad = new List<WorkReportBuildingEntity>();

            foreach (var  buildingEntity in workReportBuildings)
            {
                bool needUpload = true;
                var workId = buildingEntity.WorkId;
                var subGroupId = buildingEntity.SGroupId;
                sql = string.Format(
                    @"select  t.* from work_report_floor t where t.work_id={0} and t.s_group_id={1}", workId, subGroupId);
                buildingEntity.WorkReportFloors = QueryBySql<WorkReportFloorEntity>(sql).ToList();
                foreach (var floor in buildingEntity.WorkReportFloors)
                {
                    sql = string.Format(
                        @"select  t.* from work_report_log t where t.work_id={0} and t.s_group_id={1} and floors_code='{2}'",
                        workId, subGroupId, floor.FloorsCode);
                    var logs = QueryBySql<WorkReportLogEntity>(sql).OrderBy(d => d.LogId).ToList();
                    floor.WorkReportLogs = new List<WorkReportLogEntity>();

                    var data = logs.Where(d => d.LogServertype.Trim() == "1").ToList();
                    var voice = logs.Where(d => d.LogServertype.Trim() == "2").ToList();
                    Action<IEnumerable<WorkReportLogEntity>> check = item =>
                    {
                        if (item.Count() > 0)
                        {
                            var dataMin = item.Min(d => d.Result);
                            if (dataMin == 4)
                            {
                                needUpload = false;
                                floor.WorkReportLogs.Clear();
                            }
                            else
                            {
                                floor.WorkReportLogs.Add(item.First(d => d.Result == dataMin));
                            }
                        }
                    };
                    check(data);
                    check(voice);
                    if (!needUpload)
                    {
                        break;
                    }
                }
                if (needUpload)
                {
                    canUpLoad.Add(buildingEntity);
                }
            }

          
            LogManager.Instance.Log.Info(string.Format("扫描结束，共有{0}次测试，开始上传log", canUpLoad.Count));

            foreach (var workReportBuildingEntity in canUpLoad)
            {
                LogManager.Instance.Log.Info(string.Format("工单{0}开始上传", workReportBuildingEntity.WorkId));
                try
                {
                    var clause = new UploadClause
                    {
                        LogIds =
                            workReportBuildingEntity.WorkReportFloors.Where(d => d.WorkReportLogs.Any())
                                .Select(d => d.WorkReportLogs.First().LogId)
                                .ToList(),
                        WorkId = (int) workReportBuildingEntity.WorkId.Value,
                        SubGroupId = (int) workReportBuildingEntity.SGroupId.Value
                    };


                    AutoUpLoadLog(clause);
                    LogManager.Instance.Log.Info(string.Format("工单{0}开始结束", workReportBuildingEntity.WorkId));
                }
                catch (Exception e)
                {
                    LogManager.Instance.Log.Info(e.ToString());
                    //throw;
                }
            }
            LogManager.Instance.Log.Info(string.Format("上传结束"));
#if DEBUG
          //  Console.Read();
#endif
            return true;
        }
    }

    public partial class GetReprotDataDal
    {
        public void UpLoadLogRecurse()
        {
            string sql =string.Format( @"select t.province_id,t.auid,t.work_id,t.items_id,t.node_id,t.log_path from log_file_info t,{0}.work_sub_items d where t.items_id=d.items_id and d.is_upload=0 
and t.LOG_QUALITY=1 and  mod(t.work_id,10)!=8", GetSchem());
            var list = QueryBySql<LogInfo>(sql);
            var orders= list.GroupBy(d => d.WorkId);
            foreach (var order in orders)
            {
                LogManager.Instance.Log.Info(string.Format("开始上传工单{0},log个数为{1}：", order.Key,order.Count()));
                var logInfos =
                    order.Select(
                        d =>
                            new LogEntity()
                            {
                                NodeId = (int)d.NodeId,
                                LogId = (int) d.LogId,
                                ProvinceId = (int) d.ProvinceId,
                                UpLoadPath = d.LogPath,ItemsId = d.SubItemId
                            }).ToList();
                GetLogContent(logInfos);
                MoveFile(logInfos);
                var needUpdate = logInfos.Where(d => d.Success).Select(d => d.ItemsId).ToList();
                if (needUpdate.Any())
                { 
                    string upDateSubItems = string.Format("update  {0}.work_sub_items t set t.is_upload=1 where t.ITEMS_ID in({1})", GetSchem(), string.Join(",", needUpdate));
                    HelperDal.ExcecuteNoQureyWithoutTrans(upDateSubItems);
                }
                
                LogManager.Instance.Log.Info(string.Format("工单{0}上传结束",order.Key));
            }
        }

        private bool _getShare = true;
        private void GetLogContent(List<LogEntity> logs)
        {
            if (_getShare)
            {
                if (!GetLogByShare(logs))
                {
                    _getShare = false;
                } 
            }
            else
            {
                using (var conn = new FtpClient())
                {
                    var config = ConfigureManager.Logsever.ProvinceConfig;
                    conn.Host = config.IP;
                    conn.Credentials = new NetworkCredential(config.UserName, config.Pwd);
                    conn.DataConnectionType = FtpDataConnectionType.AutoActive;
                    foreach (var logEntity in logs)
                    {
                        LogManager.Instance.Log.Info(string.Format("开始获取log:{0}", logEntity.UpLoadPath));

                        if (conn.FileExists(logEntity.UpLoadPath, FtpListOption.ForceList | FtpListOption.AllFiles))
                        {

                            using (var stream = conn.OpenRead(logEntity.UpLoadPath))
                            {
                                List<byte> contents = new List<byte>();
                                byte[] buf = new byte[1024 * 2];
                                int read = 0;
                                while ((read = stream.Read(buf, 0, buf.Length)) > 0)
                                {
                                    contents.AddRange(buf.ToList().Take(read));
                                }
                                logEntity.Content = contents.ToArray();
                            }
                            LogManager.Instance.Log.Info(string.Format("获取成功log:{0}，log大小为：{1}", conn.Host,
                                logEntity.Content.Length));
                        }
                        else
                        {
                            LogManager.Instance.Log.Info(string.Format("服务器上文件不存在，log路径:{0}", logEntity.UpLoadPath));
                        }

                    }

                }
            }
        }

        private bool GetLogByShare(List<LogEntity> logs)
        {
            using (var unc = new UncAccessWithCredentials())
            {
                foreach (var logEntity in logs)
                {
                    var config = QureyProvinceNodeInfo(logEntity.NodeId);
                    if (config == null)
                    {
                        LogManager.Instance.Log.Info(string.Format("Log{0}找不到文件上传配置路径！", logEntity.LogId));
                        return false;
                    }
                    string serverPath = string.Format(@"\\{0}", config.NodeIp);

                    var basePath = GetFilePath(serverPath, config.CifsDir);
                    LogManager.Instance.Log.Info(string.Format("开始访问！"));
                    try
                    {
                        if (unc.NetUseWithCredentials(basePath, config.CifsAccout, "", config.CifsPasswd))
                        {
                            var filePath = GetFilePath(basePath, logEntity.UpLoadPath);
                            if (File.Exists(filePath))
                            {
                                LogManager.Instance.Log.Info(string.Format("开始读取文件！"));

                                logEntity.Content = File.ReadAllBytes(filePath);

                                LogManager.Instance.Log.Info(string.Format("读取文件结束，大小为{0}！", logEntity.Content.Length));
                            }
                            else
                            {
                                LogManager.Instance.Log.Info(string.Format("服务器上没有相关log(LogId={0})!", logEntity.LogId));
                            }

                        }
                        else
                        {
                            LogManager.Instance.Log.Info(string.Format(" 认证文件失败 ,原因：{3},地址：{0} 用户名：{1} 密码：{2}", basePath, config.CifsAccout, config.CifsPasswd, unc.LastError));
                            return false;
                        }
                    }
                    catch (Exception e)
                    {
                        LogManager.Instance.Log.Info(e.ToString());
                    }

                }
                unc.NetUseDelete();
            }
            return true;
        }
    }
}