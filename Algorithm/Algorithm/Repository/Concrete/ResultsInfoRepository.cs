using System.Linq;
using System.Transactions;
using Algorithm.Database;
using Algorithm.DomainModels;
using Algorithm.Repository.Abstract;

namespace Algorithm.Repository.Concrete
{
    public class ResultsInfoRepository : Repository<ResultInfo>, IResultsInfoRepository
    {
        public new bool Add(ResultInfo value)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
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
                    scope.Complete();
                    return true;
                }
                if (Edit(ri.Id, value))
                {
                    return true;
                }
            }
            return false;
        }

        public new bool Edit(int id, ResultInfo value)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                ResultInfo ri = Context.ResultsInfo.Find(id);
                if (ri != null)
                {
                    ri.CopyFrom(value);
                    Context.SaveChanges();
                    Dispose();
                    scope.Complete();
                    return true;
                }
            }
            return false;
        }
    }
}