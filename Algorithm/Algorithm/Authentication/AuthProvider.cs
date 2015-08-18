using System.Configuration;
using System.Linq;
using System.Web.Providers.Entities;
using System.Web.Security;
using Algorithm.Repository.Abstract;
using Algorithm.Repository.Concrete;
using Algorithm.Unity;

namespace Algorithm.Authentication
{
    public class AuthProvider : IAuthProvider
    {
        private readonly IClientsRepository _clientsRepository;

        public AuthProvider(IClientsRepository clientsRepository)
        {
            _clientsRepository = clientsRepository;
        }

        public Client Authenticate(string username, string password)
        {
            if (username == "admin" & password == ConfigurationManager.AppSettings["admin"])
            {
                return new Client
                {
                    Name = username,
                    Password = password
                };
            }

            //((ClientsRepository) (new UnityDependencyResolver(MvcUnityContainer.Container)).GetService(typeof(IClientsRepository)))

            Client client =_clientsRepository.GetAll().ToList()
                .SingleOrDefault(c => c.Name == username && c.Password == password);
            return client;
        }
    }
}