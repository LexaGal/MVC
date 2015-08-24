using System.Web;
using System.Web.Mvc;
using Algorithm.Authentication;

namespace Algorithm
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            //filters.Add(new CustomHandleErrorAttribute());
        }
    }
}