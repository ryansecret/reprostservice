using System.Collections.Generic;
using System.ServiceModel;
using WorkQualityReport.Model;

namespace WorkQualityReport.IService
{
    [ServiceContract]
    public interface IQualityReport
    {
        [OperationContract]
        List<WorkReportBuildingEntity> GetWorkReport(int workId, int subGroupId);
        [OperationContract]
        List<WorkDetailInfoEntity> GetAllWorkOrder(OrderClause clause);
        [OperationContract]
        IList<SysDictionary> QueryDict(string value);

        [OperationContract]
        bool UpLoadLog(UploadClause logIds);
    }
}