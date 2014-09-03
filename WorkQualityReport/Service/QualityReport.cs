using System.Collections.Generic;
using System.Configuration;
using Microsoft.Practices.Unity;
using WorkQualityReport.Bll;
using WorkQualityReport.IService;
using WorkQualityReport.Model;

namespace WorkQualityReport.Service
{
    public class QualityReport:IQualityReport
    {
        [Dependency]
        public GetReprotDataBll Bll { get; set; }

        public List<WorkReportBuildingEntity> GetWorkReport(int workId, int subGroupId)
        {
             
            return Bll.GetWorkReport(workId, subGroupId);
        }

        public List<WorkDetailInfoEntity> GetAllWorkOrder(OrderClause clause)
        {
            return Bll.GetAllWorkOrder(clause);
        }

        public IList<SysDictionary> QueryDict(string value)
        {
            return Bll.QueryDict(value);
        }

        public bool UpLoadLog(UploadClause logIds)
        {
            return Bll.UpdLoadLog(logIds);
        }
    }
}