using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using ZapWeb.Lib.Mvc;

namespace ZapWeb.Models
{
    public class GrupoPermisaoRules : BusinessLogic
    {

        public bool Add(GrupoPermissao grupo)
        {
            var grupoRepositorio = new GrupoPermissaoRepositorio();

            if (!Account.Current.Permissao.Has("ADD_PERMISSAO")) {
                this.MessageError = "USUARIO_SEM_PERMISSAO";
                return false;
            }

            if (grupoRepositorio.ExistNome(grupo)) {
                this.MessageError = "PERMISSAO_EXISTENTE";
                return false;
            }

            grupoRepositorio.Add(grupo);

            return true;
        }

        public bool Update(GrupoPermissao grupo)
        {
            var grupoRepositorio = new GrupoPermissaoRepositorio();

            if (!Account.Current.Permissao.Has("UPDATE_PERMISSAO"))
            {
                this.MessageError = "USUARIO_SEM_PERMISSAO";
                return false;
            }

            if (grupoRepositorio.ExistNome(grupo))
            {
                this.MessageError = "PERMISSAO_EXISTENTE";
                return false;
            }

            grupoRepositorio.Update(grupo);

            return true;
        }

        public bool Remove(GrupoPermissao grupo)
        {
            var grupoRepositorio = new GrupoPermissaoRepositorio();

            if (!Account.Current.Permissao.Has("EXCLUIR_PERMISSAO"))
            {
                this.MessageError = "USUARIO_SEM_PERMISSAO";
                return false;
            }

            var accountRepositorio = new AccountRepositorio();
            if (accountRepositorio.TotalAccountByGrupoPermissao(grupo) > 0)
            {
                this.MessageError = "PERMISSAO_EXCLUIR";
                return false;
            }

            grupoRepositorio.Remove(grupo);

            return true;
        }

        public bool AddPermissoes(GrupoPermissao grupo)
        {
            var permissaoRepositorio = new PermissaoRepositorio();

            if (!Account.Current.Permissao.Has("ADD_PERMISSAO"))
            {
                this.MessageError = "USUARIO_SEM_PERMISSAO";
                return false;
            }

            permissaoRepositorio.Add(grupo.Permissoes, grupo.Id);

            return true;
        }
        
    }
}