using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;
using WorkQualityReport.Model;

namespace WorkQualityReport.IDal
{
    public interface IGetReprotDataDal
    {
        IList<SysDictionary> QueryDict(string value);
        List<WorkReportBuildingEntity> GetWorkReport(int workId, int subGroupId);

        List<WorkDetailInfoEntity> GetAllWorkOrder(OrderClause clause);

        bool UpLoadLog(UploadClause clause);

    }
}
