using Algorithm.Repository.Abstract;
using Algorithm.Repository.Concrete;
using AntsLibrary.Classes;
using Grsu.Lab.Aoc.Contracts;
using Microsoft.Practices.Unity;

namespace Algorithm.Unity
{
    public static class MvcUnityContainer
    {
        private static IUnityContainer _container;
        public static IUnityContainer Container
        {
            get
            {
                _container = new UnityContainer();
                _container.RegisterType<IAlgorithm, AntAlgorithm>();
                _container.RegisterType<IParametersRepository, ParametersRepository>();
                _container.RegisterType<IDistMatricesRepository, DistMatricesRepository>();
                _container.RegisterType<IFlowMatricesRepository, FlowMatricesRepository>();
                _container.RegisterType<IResultsInfoRepository, ResultsInfoRepository>();
                return _container;
            }

            set { _container = value; }
        }
    }
}