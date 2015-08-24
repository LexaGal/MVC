using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Providers.Entities;
using Algorithm.AOPAttributes;
using Algorithm.Authentication;
using Algorithm.Repository.Abstract;
using User = Algorithm.Authentication.User;

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
        [LogAspect]
        public ActionResult LogIn(LoginModel loginModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if ((_authProvider.Authenticate(loginModel.Name, loginModel.Password)) != null)
                {
                    Session["client"] = loginModel;
                    return Redirect(returnUrl ?? Url.Action("DatabaseItems", "Home"));
                }
            }
            ModelState.AddModelError("LoginModel", "Пользователя с таким логином и паролем нет");
            return View(loginModel);
        }

        public ActionResult Register()
        {
            return View(new RegisterModel());
        }


        [HttpPost]
        [LogAspect]
        public ActionResult Register(RegisterModel registerModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if ((_authProvider.Authenticate(registerModel.Name, registerModel.Password)) != null)
                {
                    ModelState.AddModelError("RegisterModel", "Пользователь с таким логином и паролем уже есть");
                }
                _authProvider.Register(registerModel.Name, registerModel.Password);
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