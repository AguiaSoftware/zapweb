using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using ZapWeb.Lib.Mvc;

namespace ZapWeb.Models
{
    public class UsuarioRules : BusinessLogic
    {

        public bool Add(Usuario usuario) {

            if (!Account.Current.Permissao.Has("ADD_USUARIO")) {
                this.MessageError = "USUARIO_SEM_PERMISSAO";
                return false;
            }

            var unidadeRepositorio = new UnidadeRepositorio();
            var usuarioRepositorio = new UsuarioRepositorio();
            var accountRepositorio = new AccountRepositorio();
            var zap = unidadeRepositorio.FetchZapUnidade();

            if (usuarioRepositorio.ExistNome(usuario)) {
                this.MessageError = "USUARIO_EXISTENTE_NOME";
                return false;
            }

            if (accountRepositorio.ExistUsername(usuario.Account))
            {
                this.MessageError = "USERNAME_EXISTENTE_NOME";
                return false;
            }

            usuario.Unidade = zap;
            usuarioRepositorio.Add(usuario);
            usuarioRepositorio.UpdateAnexos(usuario);

            usuario.Account.Usuario = usuario;
            
            accountRepositorio.Add(usuario.Account);

            usuario.Account.Usuario = null;

            var notificacaoRepositorio = new NotificacaoUsuarioRepositorio();
            notificacaoRepositorio.Add(new NotificacaoUsuario()
            {
                UsuarioId = usuario.Id,
                Total = 0
            });

            return true;
        }

        public bool Update(Usuario usuario)
        {

            if (!Account.Current.Permissao.Has("UPDATE_USUARIO"))
            {
                this.MessageError = "USUARIO_SEM_PERMISSAO";
                return false;
            }

            var usuarioRepositorio = new UsuarioRepositorio();
            var accountRepositorio = new AccountRepositorio();

            if (usuarioRepositorio.ExistNome(usuario))
            {
                this.MessageError = "USUARIO_EXISTENTE_NOME";
                return false;
            }

            if (accountRepositorio.ExistUsername(usuario.Account))
            {
                this.MessageError = "USERNAME_EXISTENTE_NOME";
                return false;
            }

            usuarioRepositorio.Update(usuario);
            usuarioRepositorio.UpdateAnexos(usuario);

            accountRepositorio.Update(usuario.Account);

            usuario.Account.Usuario = null;

            return true;
        }

        public Usuario Get(int Id) {
            var usuarioRepositorio = new UsuarioRepositorio();
            var accountRepositorio = new AccountRepositorio();

            var usuario = usuarioRepositorio.Fetch(Id);
            usuario.Account = accountRepositorio.FetchByUsuarioId(usuario.Id);
            usuario.Anexos = usuarioRepositorio.FetchArquivos(usuario);

            return usuario;
        }

        public List<Usuario> Search(string nome)
        {
            var unidadeRepositorio = new UnidadeRepositorio();
            var usuarioRepositorio = new UsuarioRepositorio();
            var unidade = unidadeRepositorio.Fetch(Account.Current.Usuario.Unidade.Id);

            return usuarioRepositorio.Fetch(nome, unidade);
        }

        public List<Usuario> All(int unidadeId) {
            var usuarioRepositorio = new UsuarioRepositorio();
            var unidadeRepositorio = new UnidadeRepositorio();
            var unidade = unidadeRepositorio.Fetch(unidadeId);

            return usuarioRepositorio.FetchUsuariosByUnidade(unidade, true);
        }

    }
}