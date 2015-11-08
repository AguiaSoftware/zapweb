using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ZapWeb.Lib.Mvc;
using ZapWeb.Models;

namespace ZapWeb.Controllers
{
    public class UnidadeController : ZapWeb.Lib.Mvc.Controller
    {
        public string Add(Unidade unidade) {
            var unidadeRules = new UnidadeRules();

            if (!unidadeRules.Add(unidade)) {
                return Error(unidadeRules.MessageError);
            }

            return Success(unidade);
        }

        public string Update(Unidade unidade) {
            var unidadeRules = new UnidadeRules();

            if (!unidadeRules.Update(unidade))
            {
                return Error(unidadeRules.MessageError);
            }

            return Success(unidade);
        }

        public string Get(int Id) {
            var unidadeRules = new UnidadeRules();

            return Success(unidadeRules.Get(Id));
        }

        public string Search(string nome, UnidadeTipo tipo) {
            var unidadeRules = new UnidadeRules();

            return Success(unidadeRules.Search(nome, tipo));
        }

        public string All(int unidadeId) {
            var unidadeRules = new UnidadeRules();

            return Success(unidadeRules.Unidades(unidadeId));
        }

        public string Excluir(Unidade unidade)
        {
            var unidadeRules = new UnidadeRules();

            if (unidadeRules.Excluir(unidade))
            {
                return Error(unidadeRules.MessageError);
            }

            return Success(unidade);
        }
    }
}
