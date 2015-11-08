using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ZapWeb.Lib.Mvc;
using ZapWeb.Models;

namespace ZapWeb.Controllers
{
    public class AuthController : ZapWeb.Lib.Mvc.Controller
    {
        //
        // GET: /Auth/
        public ActionResult Index()
        {
            if (Account.Current.isAuthenticate)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        public string Entrar(Account account) {
            var authRules = new AuthRules();

            if (authRules.Logar(account)) return Success(account);
            else return Error(authRules.MessageError);
        }

        public ActionResult Sair()
        {
            var authRules = new AuthRules();

            authRules.Sair();

            return RedirectToAction("Index", "Auth");
        }

    }
}
