using System.Collections.Generic;
using System.Linq;
using Aop.AopAspects;
using Aop.AopAspects.Caching;
using Aop.AopAspects.Logging;
using DatabaseAccess.Repository.Abstract;
using Entities.DatabaseModels;

namespace DatabaseAccess.Repository.Concrete
{
    [AuthentificationAspect]   
    [LogAspect]
    public class ResultsInfoRepository : Repository<ResultInfo>, IResultsInfoRepository
    {
        [RunInTransactionAspect]
        public new bool Add(ResultInfo value)
        {
            ResultInfo ri = Context.ResultsInfo
                .SingleOrDefault(r =>
                    r.ParametersId == value.ParametersId &&
                    r.DistMatrixId == value.DistMatrixId &&
                    r.FlowMatrixId == value.FlowMatrixId);

            if (ri == null)
            {
                Context.ResultsInfo.Add(value);
                Context.SaveChanges();
                Dispose();
                return true;
            }
            if (Edit(ri.Id, value))
            {
                return true;
            }
            return false;
        }

        [RunInTransactionAspect]
        public new bool Edit(int id, ResultInfo value)
        {
            ResultInfo ri = Context.ResultsInfo.Find(id);
            if (ri != null)
            {
                ri.CopyFrom(value);
                Context.SaveChanges();
                Dispose();
                return true;
            }
            return false;
        }

        [CacheableResultAspect]
        public IList<ResultInfo> GetAllById(int parametersId, int distMatrixId, int flowMatrixId, string type)
        {
            switch (type)
            {
                case "Parameters":
                    return Context.ResultsInfo.Where(ri => ri.ParametersId == parametersId).ToList();
                                                                                 
                case "DistMatrix":                                               
                    return Context.ResultsInfo.Where(ri => ri.DistMatrixId == distMatrixId).ToList();
                                                                                 
                case "FlowMatrix":                                               
                    return Context.ResultsInfo.Where(ri => ri.FlowMatrixId == flowMatrixId).ToList();
            }
            return null;
        }
    }
}