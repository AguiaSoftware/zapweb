using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ZapWeb.Lib.Mvc;
using ZapWeb.Models;

namespace ZapWeb.Controllers
{
    public class PermissaoController : ZapWeb.Lib.Mvc.Controller
    {
        public string Tipos() {
            var permissaoRepositorio = new TipoPermissaoRepositorio();
            var permissoes = permissaoRepositorio.FetchAll();

            return this.Success(permissoes);
        }
    }
}
