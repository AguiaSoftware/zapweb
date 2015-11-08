using Pillar.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using ZapWeb.Lib.Mvc;

namespace ZapWeb.Models
{
    public class FornecedorRules : BusinessLogic
    {

        public bool Add(Fornecedor fornecedor) {

            if (!Account.Current.Permissao.Has("ADD_FORNECEDOR")) {
                this.MessageError = "USUARIO_SEM_PERMISSAO";
                return false;
            }

            var fornecedorRepositorio = new FornecedorRepositorio();
            if (fornecedorRepositorio.ExisteNome(fornecedor)) {
                this.MessageError = "FORNECEDOR_EXISTENTE";
                return false;
            }
            
            fornecedorRepositorio.Add(fornecedor);

            return true;
        }

        public bool Update(Fornecedor fornecedor)
        {

            if (!Account.Current.Permissao.Has("UPDATE_FORNECEDOR"))
            {
                this.MessageError = "USUARIO_SEM_PERMISSAO";
                return false;
            }

            var fornecedorRepositorio = new FornecedorRepositorio();
            if (fornecedorRepositorio.ExisteNome(fornecedor))
            {
                this.MessageError = "FORNECEDOR_EXISTENTE";
                return false;
            }

            fornecedorRepositorio.Update(fornecedor);

            return true;
        }

        public Fornecedor Get(int Id)
        {

            if (!Account.Current.Permissao.Has("UPDATE_FORNECEDOR"))
            {
                this.MessageError = "USUARIO_SEM_PERMISSAO";
                return null;
            }

            var fornecedorRepositorio = new FornecedorRepositorio();
            var fornecedor = fornecedorRepositorio.Fetch(Id);

            return fornecedor;
        }

        public List<Fornecedor> Search(string nome)
        {
            var fornecedorRepositorio = new FornecedorRepositorio();

            return fornecedorRepositorio.Search(nome);
        }

        public List<Fornecedor> All(FornecedorPesquisa parametros, Paging paging)
        {
            var fornecedorRepositorio = new FornecedorRepositorio();

            return fornecedorRepositorio.FetchAll(parametros, paging);
        }

    }
}