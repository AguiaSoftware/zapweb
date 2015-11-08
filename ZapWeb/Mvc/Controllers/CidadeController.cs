using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ZapWeb.Lib.Mvc;
using ZapWeb.Models;

namespace ZapWeb.Controllers
{
    public class CidadeController : ZapWeb.Lib.Mvc.Controller
    {
        public string Search(string nome) {
            var cidadeRepositorio = new CidadeRepositorio();

            return Success(cidadeRepositorio.Search(nome));
        }
    }
}
