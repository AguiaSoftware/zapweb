using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ZapWeb.Lib.Mvc;
using ZapWeb.Models;

namespace ZapWeb.Controllers
{
    public class GrupoPermissaoController : ZapWeb.Lib.Mvc.Controller
    {
        public string Add(GrupoPermissao grupo) {
            var grupoRules = new GrupoPermisaoRules();

            if (!grupoRules.Add(grupo)) {
                return Error(grupoRules.MessageError);
            }

            return Success(grupo);
        }

        public string Update(GrupoPermissao grupo)
        {
            var grupoRules = new GrupoPermisaoRules();

            if (!grupoRules.Update(grupo))
            {
                return Error(grupoRules.MessageError);
            }

            return Success(grupo);
        }

        public string Remove(GrupoPermissao grupo) {
            var grupoRules = new GrupoPermisaoRules();

            if (!grupoRules.Remove(grupo))
            {
                return Error(grupoRules.MessageError);
            }

            return Success(grupo);
        }

        public string addPermissoes(GrupoPermissao grupo) {
            var grupoRules = new GrupoPermisaoRules();

            if (!grupoRules.AddPermissoes(grupo))
            {
                return Error(grupoRules.MessageError);
            }

            return Success(grupo);
        }

        public string All() {
            var permissaoRepositorio = new GrupoPermissaoRepositorio();
            var permissoes = permissaoRepositorio.FetchAll();

            return this.Success(permissoes);
        }
    }
}
