using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using ZapWeb.Lib.Mvc;

namespace ZapWeb.Models
{
    public class CentroCustoRules : BusinessLogic
    {

        public bool Add(CentroCusto centroCusto) {

            if (!Account.Current.Permissao.Has("ADD_CENTRO_CUSTO"))
            {
                this.MessageError = "USUARIO_SEM_PERMISSAO";
                return false;
            }

            var centroCustoRepositorio = new CentroCustoRepositorio();
            if (centroCustoRepositorio.Has(centroCusto))
            {
                this.MessageError = "CENTRO_CUSTO_EXISTENTE";
                return false;
            }

            centroCustoRepositorio.Add(centroCusto);

            return true;
        }

        public bool Update(CentroCusto centroCusto)
        {

            if (!Account.Current.Permissao.Has("UPDATE_CENTRO_CUSTO"))
            {
                this.MessageError = "USUARIO_SEM_PERMISSAO";
                return false;
            }

            var centroCustoRepositorio = new CentroCustoRepositorio();
            if (centroCustoRepositorio.Has(centroCusto))
            {
                this.MessageError = "CENTRO_CUSTO_EXISTENTE";
                return false;
            }

            centroCustoRepositorio.Update(centroCusto);

            return true;
        }

        public bool Excluir(CentroCusto centroCusto)
        {
            var centroCustoRepositorio = new CentroCustoRepositorio();

            if (centroCustoRepositorio.HashVinculo(centroCusto))
            {
                this.MessageError = "CENTRO_CUSTO_VINCULADO";
                return false;
            }

            centroCustoRepositorio.Delete(centroCusto);

            return true;
        }

        public List<CentroCusto> Tipo(TipoCentroCusto tipo) {
            var centroCustoRepositorio = new CentroCustoRepositorio();

            return centroCustoRepositorio.FetchByTipo(tipo);
        }

    }
}