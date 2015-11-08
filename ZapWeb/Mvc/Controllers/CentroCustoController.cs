using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ZapWeb.Lib.Mvc;
using ZapWeb.Models;

namespace ZapWeb.Controllers
{
    public class CentroCustoController : ZapWeb.Lib.Mvc.Controller
    {

        public string Add(CentroCusto centroCusto) {
            var centroCustoRules = new CentroCustoRules();

            if (!centroCustoRules.Add(centroCusto)) {
                return Error(centroCustoRules.MessageError);
            }

            return Success(centroCusto);
        }

        public string Update(CentroCusto centroCusto)
        {
            var centroCustoRules = new CentroCustoRules();

            if (!centroCustoRules.Update(centroCusto))
            {
                return Error(centroCustoRules.MessageError);
            }

            return Success(centroCusto);
        }

        public string Excluir(CentroCusto centroCusto)
        {
            var centroCustoRules = new CentroCustoRules();

            if (!centroCustoRules.Excluir(centroCusto))
            {
                return Error(centroCustoRules.MessageError);
            }

            return Success(centroCusto);
        }

        public string All(TipoCentroCusto tipo)
        {
            var centroCustoRules = new CentroCustoRules();

            return Success(centroCustoRules.Tipo(tipo));
        }

    }
}
