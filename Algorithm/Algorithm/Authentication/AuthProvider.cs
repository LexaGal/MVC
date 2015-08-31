using System.Configuration;
using System.Linq;
using System.Web;
using DatabaseAccess.Repository.Abstract;
using Entities.DatabaseModels;

namespace Algorithm.Authentication
{
    public class AuthProvider : IAuthProvider
    {
        private readonly IUsersRepository _usersRepository;
        private readonly ICryptor _cryptor;

        public AuthProvider(IUsersRepository usersRepository, ICryptor cryptor)
        {
            _usersRepository = usersRepository;
            _cryptor = cryptor;
        }

        public User Authenticate(string username, string password)
        {
            if (username == "admin" & password == ConfigurationManager.AppSettings["admin"])
            {
                return new User(username, _cryptor.Encrypt(password));
            }
            User user =_usersRepository.GetAll().ToList().SingleOrDefault(c =>
                c.Name == username && c.PasswordHash == _cryptor.Encrypt(password));
            return user;
        }

        public User Register(string username, string password)
        {
            User user = new User(username, _cryptor.Encrypt(password));
            _usersRepository.Add(user);
            return user;    
        }
    }
}