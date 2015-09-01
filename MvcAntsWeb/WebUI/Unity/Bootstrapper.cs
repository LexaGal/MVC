using System.Web.Mvc;
using Algorithm.Controllers;
using AntsAlg.QapAlg;
using DatabaseAccess.Repository.Abstract;
using DatabaseAccess.Repository.Concrete;
using Microsoft.Practices.Unity;

namespace Algorithm.Unity
{
    public class Bootstrapper
    {
        public static IUnityContainer Initialize()
        {
            var container = BuildUnityContainer();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
            return container;
        }
        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();
            
            container.RegisterType<HomeController>();
            container.RegisterType<IQapAntAlgorithm, QapAntAlgorithm>();
            container.RegisterType<QapGraph>();
            container.RegisterType<IParametersRepository, ParametersRepository>();
            container.RegisterType<IDistMatricesRepository, DistMatricesRepository>();
            container.RegisterType<IFlowMatricesRepository, FlowMatricesRepository>();
            container.RegisterType<IResultsInfoRepository, ResultsInfoRepository>();
            
            MvcUnityContainer.Container = container;
            return container;
        }
    }  
}