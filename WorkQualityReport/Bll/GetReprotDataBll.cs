using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
 
using Microsoft.Practices.Unity;
using WorkQualityReport.Dal;
using WorkQualityReport.IDal;
using WorkQualityReport.Model;
using WorkQualityReport.Utility;

namespace WorkQualityReport.Bll
{
    public class GetReprotDataBll
    {
        [Dependency]
        public IGetReprotDataDal Dal { get; set; }

        public List<WorkReportBuildingEntity> GetWorkReport(int workId, int subGroupId)
        {
            return Dal.GetWorkReport(workId, subGroupId);
        }

        public List<WorkDetailInfoEntity> GetAllWorkOrder(OrderClause clause)
        {
            try
            {
                return Dal.GetAllWorkOrder(clause);
            }
            catch (Exception e)
            {

                LogManager.Instance.Log.Info(e.ToString());
            }
            return new List<WorkDetailInfoEntity>();
 
        }

        public IList<SysDictionary> QueryDict(string value)
        {
            return Dal.QueryDict(value);
        }

        public bool UpdLoadLog(UploadClause logIds)
        {
            try
            {
                return Dal.UpLoadLog(logIds);
            }
            catch (Exception e)
            {

                LogManager.Instance.Log.Info(e.ToString());
            }
            return false;
        }
    }
}
