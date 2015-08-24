using Algorithm.Authentication;
using Algorithm.Repository.Abstract;
using Algorithm.Repository.Concrete;
using AntsLibrary.Classes;
using Grsu.Lab.Aoc.Contracts;
using Microsoft.Practices.Unity;
using NHibernate.Event;

namespace Algorithm.Unity
{
    public static class MvcUnityContainer
    {
        private static IUnityContainer _container;

        public static void Initialize()
        {
             _container = new UnityContainer();
            _container.RegisterType<IAlgorithm, AntAlgorithm>();
            _container.RegisterType<IParametersRepository, ParametersRepository>();
            _container.RegisterType<IDistMatricesRepository, DistMatricesRepository>();
            _container.RegisterType<IFlowMatricesRepository, FlowMatricesRepository>();
            _container.RegisterType<IResultsInfoRepository, ResultsInfoRepository>();
            _container.RegisterType<IUsersRepository, UsersRepository>();
            _container.RegisterType<IAuthProvider, AuthProvider>();
            _container.RegisterType<ICryptor, PasswordCryptor>();
        }

        public static IUnityContainer Container
        {
            get { return _container; }
            set { _container = value; }
        }
    }
}