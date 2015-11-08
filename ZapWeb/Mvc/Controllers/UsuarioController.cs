using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ZapWeb.Lib.Mvc;
using ZapWeb.Models;

namespace ZapWeb.Controllers
{
    public class UsuarioController : ZapWeb.Lib.Mvc.Controller
    {
        public string Add(Usuario usuario) {
            var usuarioRules = new UsuarioRules();

            if (!usuarioRules.Add(usuario)) {
                return Error(usuarioRules.MessageError);
            }

            return Success(usuario);
        }

        public string Update(Usuario usuario)
        {
            var usuarioRules = new UsuarioRules();

            if (!usuarioRules.Update(usuario))
            {
                return Error(usuarioRules.MessageError);
            }

            return Success(usuario);
        }

        public string Search(string nome) {
            var usuarioRules = new UsuarioRules();

            return Success(usuarioRules.Search(nome));
        }

        public string Get(int id) {
            var usuarioRules = new UsuarioRules();

            return Success(usuarioRules.Get(id));
        }

        public string All(int unidadeId)
        {
            var usuarioRules = new UsuarioRules();

            return Success(usuarioRules.All(unidadeId));
        }
    }
}
