using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using ZapWeb.Lib.Mvc;

using Pillar.RealTime;

namespace ZapWeb.Models
{
    public class ReceitaRelatorioRules : BusinessLogic
    {

        public Receita ReceitaUnidade(Unidade unidade, int mes, int ano)
        {
            var receitaRepositorio = new ReceitaRepositorio();

            if (!Account.Current.Permissao.Has("UPDATE_RECEITA"))
            {
                this.MessageError = "USUARIO_SEM_PERMISSAO";
                return null;
            }

            return receitaRepositorio.Fetch(mes, ano, unidade.Id);
        }

        public List<ReceitaCentral> ReceitasPorCentral(int mes, int ano)
        {
            var receitaRepositorio = new ReceitaRelatorioRepositorio();

            if (!Account.Current.Permissao.Has("UPDATE_RECEITA"))
            {
                this.MessageError = "USUARIO_SEM_PERMISSAO";
                return null;
            }

            return receitaRepositorio.ReceitasPorCentral(mes, ano);
        }

        public List<Receita> ReceitasUnidadesFilhas(Unidade central, int mes, int ano)
        {
            var receitaRepositorio = new ReceitaRelatorioRepositorio();

            if (!Account.Current.Permissao.Has("UPDATE_RECEITA"))
            {
                this.MessageError = "USUARIO_SEM_PERMISSAO";
                return null;
            }

            return receitaRepositorio.ReceitasUnidadesFilhas(central, mes, ano);
        }

        public decimal? TotalPorUnidade(int unidadeId, int mes, int ano)
        {
            var receitaRepositorio = new ReceitaRelatorioRepositorio();
            var unidadeRepositorio = new UnidadeRepositorio();

            var unidade = unidadeRepositorio.Fetch(Account.Current.Usuario.Unidade.Id);

            //if (unidade.Id != unidadeId)
            //{
            //    this.MessageError = "USUARIO_SEM_PERMISSAO";
            //    return null;
            //}

            if (!unidade.IsParent(unidadeId))
            {
                this.MessageError = "USUARIO_SEM_PERMISSAO";
                return null;
            }

            unidade = unidadeRepositorio.Fetch(unidadeId);

            return receitaRepositorio.TotalPorUnidade(unidade, mes, ano);
        }

        public decimal? TotalPorCentral(int centralId, int mes, int ano) {
            var receitaRepositorio = new ReceitaRelatorioRepositorio();
            var unidadeRepositorio = new UnidadeRepositorio();

            var unidade = unidadeRepositorio.Fetch(Account.Current.Usuario.Unidade.Id);

            //if (unidade.Tipo == UnidadeTipo.COS)
            //{
            //    this.MessageError = "USUARIO_SEM_PERMISSAO";
            //    return null;
            //}

            if (unidade.Tipo == UnidadeTipo.CENTRAL && centralId != unidade.Id)
            {
                this.MessageError = "USUARIO_SEM_PERMISSAO";
                return null;
            }

            var central = unidadeRepositorio.Fetch(centralId);

            return receitaRepositorio.TotalPorUnidade(central, mes, ano);
        }

        public decimal? TotalPorZap(int mes, int ano)
        {
            var receitaRepositorio = new ReceitaRelatorioRepositorio();
            var unidadeRepositorio = new UnidadeRepositorio();
            var unidade = unidadeRepositorio.Fetch(Account.Current.Usuario.Unidade.Id);

            if (!Account.Current.Permissao.Has("UPDATE_RECEITA"))
            {
                this.MessageError = "USUARIO_SEM_PERMISSAO";
                return null;
            }

            return receitaRepositorio.TotalPorUnidade(unidade, mes, ano);
        }
                
    }
}