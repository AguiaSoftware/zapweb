using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using ZapWeb.Lib.Mvc;

namespace ZapWeb.Models
{
    public class DespesaRelatorioRules : BusinessLogic
    {

        public List<DespesaCentroCusto> RelatorioByUnidade(int unidadeId, int mes, int ano) {
            var despesaRelatorioRepositorio = new DespesaRelatorioRepositorio();

            var unidadeRepositorio = new UnidadeRepositorio();
            var unidadeSelecionada = unidadeRepositorio.Fetch(unidadeId);

            if (unidadeSelecionada == null) {
                this.MessageError = "USUARIO_SEM_PERMISSAO";
                return null;
            }

            if (unidadeSelecionada.Id != Account.Current.Usuario.Unidade.Id &&
                !unidadeSelecionada.IsChildren(Account.Current.Usuario.Unidade.Id) &&
                unidadeSelecionada.Tipo != UnidadeTipo.ZAP)
            {
                this.MessageError = "USUARIO_SEM_PERMISSAO";
                return null;
            }

            return despesaRelatorioRepositorio.Relatorio(unidadeId, mes, ano);
        }

        public List<UnidadeCentroCustos> RelatorioByCentral(int centralId, int mes, int ano)
        {
            var despesaRelatorioRepositorio = new DespesaRelatorioRepositorio();

            var unidadeRepositorio = new UnidadeRepositorio();
            var unidadeSelecionada = unidadeRepositorio.Fetch(centralId);

            if (unidadeSelecionada == null)
            {
                this.MessageError = "USUARIO_SEM_PERMISSAO";
                return null;
            }

            if (unidadeSelecionada.Id != Account.Current.Usuario.Unidade.Id &&
                !unidadeSelecionada.IsChildren(Account.Current.Usuario.Unidade.Id) &&
                unidadeSelecionada.Tipo != UnidadeTipo.ZAP)
            {
                this.MessageError = "USUARIO_SEM_PERMISSAO";
                return null;
            }

            var unidadesFilhas = unidadeRepositorio.GetUnidadesFilhas(unidadeSelecionada);
            var unidadeCentroCustos = new List<UnidadeCentroCustos>();
            
            foreach (var filha in unidadesFilhas)
            {
                unidadeCentroCustos.Add(new UnidadeCentroCustos()
                {
                    Unidade = filha,
                    CentroCustos = despesaRelatorioRepositorio.Relatorio(filha.Id, mes, ano)
                });
            }

            return unidadeCentroCustos;
        }

        public List<UnidadeCentroCustos> RelatorioByZap(int mes, int ano)
        {
            var despesaRelatorioRepositorio = new DespesaRelatorioRepositorio();

            var unidadeRepositorio = new UnidadeRepositorio();
            var unidade = unidadeRepositorio.Fetch(Account.Current.Usuario.Unidade.Id);

            if (unidade.Tipo != UnidadeTipo.ZAP)
            {
                this.MessageError = "USUARIO_SEM_PERMISSAO";
                return null;
            }

            var centrais = unidadeRepositorio.FetchCentrais();
            var unidadeCentroCustos = new List<UnidadeCentroCustos>();

            foreach (var central in centrais)
            {
                unidadeCentroCustos.Add(new UnidadeCentroCustos()
                {
                    Unidade = central,
                    CentroCustos = despesaRelatorioRepositorio.DespesaUnidadesByCentral(central, mes, ano)
                });
            }

            return unidadeCentroCustos;
        }

    }
}