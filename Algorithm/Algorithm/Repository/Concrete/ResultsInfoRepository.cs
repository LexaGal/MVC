using System;
using System.Data;
using System.Linq;
using System.Transactions;
using Algorithm.AOPAttributes;
using Algorithm.AOPAttributes.Caching;
using Algorithm.Database;
using Algorithm.DomainModels;
using Algorithm.Repository.Abstract;

namespace Algorithm.Repository.Concrete
{
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
                return true;
            }
            return false;
        }

        [TimingAspect]
        [CacheableResult]
        public IQueryable<ResultInfo> GetAllById(int id, string type)
        {
            switch (type)
            {
                case "Parameters":
                    return Context.ResultsInfo.Where(ri => ri.ParametersId == id).AsQueryable();

                case "DistMatrix":
                    return Context.ResultsInfo.Where(ri => ri.DistMatrixId == id).AsQueryable();

                case "FlowMatrix":
                    return Context.ResultsInfo.Where(ri => ri.FlowMatrixId == id).AsQueryable();
            }
            return null;
        }
    }
}