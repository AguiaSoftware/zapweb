using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZapWeb.Lib.Mvc;
using ZapWeb.Models;

using Pillar.Mvc;
using Pillar.Util;

namespace ZapWeb.Controllers
{
    public class DespesaRelatorioController : ZapWeb.Lib.Mvc.Controller
    {
        public string RelatorioUnidade(int unidadeId, int mes, int ano)
        {
            var despesaRules = new DespesaRelatorioRules();
            var relatorio = despesaRules.RelatorioByUnidade(unidadeId, mes, ano);

            if (relatorio == null)
            {
                return Error(despesaRules.MessageError);
            }

            var unidadeRepositorio = new UnidadeRepositorio();
            var unidade = unidadeRepositorio.Fetch(unidadeId);

            return Success(new
            {
                Unidade = unidade,
                Dados = relatorio
            });
        }

        public string RelatorioCentral(int centralId, int mes, int ano)
        {
            var despesaRules = new DespesaRelatorioRules();
            var relatorio = despesaRules.RelatorioByCentral(centralId, mes, ano);

            if (relatorio == null)
            {
                return Error(despesaRules.MessageError);
            }

            var unidadeRepositorio = new UnidadeRepositorio();
            var unidade = unidadeRepositorio.Fetch(centralId);

            return Success(new
            {
                Unidade = unidade,
                Dados = relatorio
            });
        }

        public string RelatorioZap(int mes, int ano)
        {
            var despesaRules = new DespesaRelatorioRules();
            var relatorio = despesaRules.RelatorioByZap(mes, ano);

            if (relatorio == null)
            {
                return Error(despesaRules.MessageError);
            }

            return Success(new
            {            
                Dados = relatorio
            });
        }

    }
}
