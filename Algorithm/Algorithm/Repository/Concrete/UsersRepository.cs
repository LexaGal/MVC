using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Algorithm.Authentication;
using Algorithm.Repository.Abstract;

namespace Algorithm.Repository.Concrete
{
    public class UsersRepository : Repository<User>, IUsersRepository
    {
    }
}