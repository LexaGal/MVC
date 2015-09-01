using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Algorithm.Unity;
using Aop.AopAspects.Logging;
using log4net;
using log4net.Config;
using log4net.Repository.Hierarchy;
using Logger = Aop.AopAspects.Logging.Logger;

namespace Algorithm
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        public static ILog Log;
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            MvcUnityContainer.Initialize();
            TypeConverter.Initialize();
            Logger.Configure();

            DependencyResolver.SetResolver(new UnityDependencyResolver(MvcUnityContainer.Container));
            
            //Initialize IoC container/Unity
            //Bootstrapper.Initialize();
            //Register our custom controller factory
            //ControllerBuilder.Current.SetControllerFactory(typeof(MvcControllerFactory));       
        }
    }
}