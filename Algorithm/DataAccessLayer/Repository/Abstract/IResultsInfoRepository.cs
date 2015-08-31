using System.Collections.Generic;
using Entities.DatabaseModels;

namespace DatabaseAccess.Repository.Abstract
{
    public interface IResultsInfoRepository : IRepository<ResultInfo>
    {
        IList<ResultInfo> GetAllById(int parametersId, int distMatrixId, int flowMatrixId, string type);
    }
}