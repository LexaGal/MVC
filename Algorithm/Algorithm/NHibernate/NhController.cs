using System.Linq;
using System.Web.Mvc;
using NHibernate;
using NHibernate.Linq;

namespace Algorithm.NHibernate
{
    public class NhController : Controller
    {
        //
        // GET: /Nh/

        public ActionResult Index()
        {
            using (ISession session = NHibertnateSession.OpenSession())
            {
                var employees = session.Query<Employee>().ToList();
                return View(employees);
            }
        }

    }
}
