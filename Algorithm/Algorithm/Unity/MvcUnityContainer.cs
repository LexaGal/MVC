using System.Collections.Generic;
using Algorithm.Authentication;
using AntsAlg.AntsAlgorithm.Algorithm;
using AntsAlg.AntsAlgorithm.Graph;
using AntsAlg.QapAlg;
using DatabaseAccess.Repository.Abstract;
using DatabaseAccess.Repository.Concrete;
using Microsoft.Practices.Unity;

namespace Algorithm.Unity
{
    public static class MvcUnityContainer
    {
        public static void Initialize()
        {
            Container = new UnityContainer();
            Container.RegisterType<IAlgorithm<QapGraph, IList<IVertex>> , QapAntAlgorithm>();
            Container.RegisterType<IParametersRepository, ParametersRepository>();
            Container.RegisterType<IDistMatricesRepository, DistMatricesRepository>();
            Container.RegisterType<IFlowMatricesRepository, FlowMatricesRepository>();
            Container.RegisterType<IResultsInfoRepository, ResultsInfoRepository>();
            Container.RegisterType<IUsersRepository, UsersRepository>();
            Container.RegisterType<IAuthProvider, AuthProvider>();
            Container.RegisterType<ICryptor, PasswordCryptor>();
        }

        public static IUnityContainer Container { get; set; }
    }
}