using System.Web.Mvc;
using Algorithm.Authentication;
using Algorithm.Models;
using DatabaseAccess.Repository.Abstract;
using Entities.DatabaseModels;

namespace Algorithm.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthProvider _authProvider;
        private readonly IUsersRepository _usersRepository;
        private readonly ICryptor _cryptor;

        public AuthController(IAuthProvider authProvider, IUsersRepository usersRepository, ICryptor cryptor)
        {
            _authProvider = authProvider;
            _usersRepository = usersRepository;
            _cryptor = cryptor;
        }

        public ActionResult LogIn()
        {
            Session["client"] = null;
            return View();
        }

        [HttpPost]
        public ActionResult LogIn(LoginModel loginModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = _authProvider.Authenticate(loginModel.Name, loginModel.Password);
                if (user != null)
                {
                    Session["client"] = user;
                    return Redirect(returnUrl ?? Url.Action("DatabaseItems", "Home"));
                }
            }
            ModelState.AddModelError("LoginModel", "Пользователя с таким логином и паролем нет");
            return View();
        }

        public ActionResult Register()
        {
            return View(new RegisterModel());
        }


        [HttpPost]
        public ActionResult Register(RegisterModel registerModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = _authProvider.Authenticate(registerModel.Name, registerModel.Password);
                if (user != null)
                {
                    ModelState.AddModelError("RegisterModel", "Пользователь с таким логином и паролем уже есть");
                    return View();
                }
                
                user = _authProvider.Register(registerModel.Name, registerModel.Password);
                Session["client"] = user;
                return Redirect(returnUrl ?? Url.Action("DatabaseItems", "Home"));
            }
            return View(registerModel);
        }

        public ActionResult LogOut()
        {
            Session["client"] = null;
            return View("LogIn");
        }

    }
}