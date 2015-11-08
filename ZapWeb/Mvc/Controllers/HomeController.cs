using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ZapWeb.Lib.Mvc;
using ZapWeb.Models;

namespace ZapWeb.Controllers
{
    public class HomeController : ZapWeb.Lib.Mvc.Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            if (!Account.Current.isAuthenticate) {
                return RedirectToAction("Index", "Auth");
            }

            var usuarioRepositorio = new UsuarioRepositorio();
            var usuario = usuarioRepositorio.Fetch(Account.Current.UsuarioId);

            var grupoPermissaoRepositorio = new GrupoPermissaoRepositorio();
            usuario.Permissoes = grupoPermissaoRepositorio.Fetch(Account.Current.GrupoPermissaoId);
            
            ViewData["usuario"] = usuario;

            return View();
        }

    }
}
