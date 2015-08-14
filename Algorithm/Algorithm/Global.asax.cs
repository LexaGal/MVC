using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Algorithm.Unity;
using AntsLibrary.Classes;
using Grsu.Lab.Aoc.Contracts;
using log4net;
using Microsoft.Practices.Unity;

namespace Algorithm
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static ILog Log;
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            DependencyResolver.SetResolver(new UnityDependencyResolver(MvcUnityContainer.Container));
            
            Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            log4net.Config.XmlConfigurator.Configure();
            
            //Initialize IoC container/Unity
            //Bootstrapper.Initialise();
            //Register our custom controller factory
            //ControllerBuilder.Current.SetControllerFactory(typeof(MvcControllerFactory));       
        }
    }
}