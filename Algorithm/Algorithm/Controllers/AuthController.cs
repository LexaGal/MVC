using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Algorithm.Authentication;
using Algorithm.Repository.Abstract;

namespace Algorithm.Controllers
{
    public class AuthController : Controller
    {
        private IClientsRepository _clientsRepository;
        private IAuthProvider _authProvider;

        public AuthController(IClientsRepository clientsRepository, IAuthProvider authProvider)
        {
            _clientsRepository = clientsRepository;
            _authProvider = authProvider;
        }

        public ActionResult Index()
        {
            Session["client"] = null;
            return View();
        }

        [HttpPost]
        public ActionResult Index(Client client, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if ((_authProvider.Authenticate(client.Name, client.Password)) != null)
                {
                    Session["client"] = client;
                    return Redirect(returnUrl ?? Url.Action("DatabaseItems", "Home"));
                }
            }
            return View();
        }
    }
}