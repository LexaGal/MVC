using DatabaseAccess.Repository.Abstract;
using Entities.DatabaseModels;

namespace DatabaseAccess.Repository.Concrete
{
    public class DistMatricesRepository : Repository<DistMatrix>, IDistMatricesRepository
    {}
}