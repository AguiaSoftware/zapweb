using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ZapWeb.Models;

namespace ZapWeb.Mvc.Controllers
{
    public class HistoricoController : ZapWeb.Lib.Mvc.Controller
    {

        public string Add(Historico historico)
        {
            var rules = new HistoricoRules();

            if (!rules.Adicionar(historico))
            {
                return this.Error(rules.MessageError);
            }

            return this.Success(historico);
        }

        public string All(int condominioId)
        {
            var rules = new HistoricoRules();

            return this.Success(rules.All(condominioId));
        }

    }
}