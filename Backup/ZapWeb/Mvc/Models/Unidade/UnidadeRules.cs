using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using ZapWeb.Lib.Mvc;

namespace ZapWeb.Models
{
    public class UnidadeRules : BusinessLogic
    {

        public bool Add(Unidade unidade) {

            if (!Account.Current.Permissao.Has("ADD_UNIDADE")) {
                this.MessageError = "USUARIO_SEM_PERMISSAO";
                return false;
            }

            var unidadeRepositorio = new UnidadeRepositorio();
            if (unidadeRepositorio.ExistNome(unidade)) {
                this.MessageError = "UNIDADE_EXISTENTE_NOME";
                return false;
            }

            unidadeRepositorio.Add(unidade);
            unidadeRepositorio.UpdateAnexos(unidade);

            return true;
        }

        public bool Update(Unidade unidade)
        {

            if (!Account.Current.Permissao.Has("UPDATE_UNIDADE"))
            {
                this.MessageError = "USUARIO_SEM_PERMISSAO";
                return false;
            }

            var unidadeRepositorio = new UnidadeRepositorio();
            if (unidadeRepositorio.ExistNome(unidade))
            {
                this.MessageError = "UNIDADE_EXISTENTE_NOME";
                return false;
            }

            unidadeRepositorio.Update(unidade);
            unidadeRepositorio.UpdateAnexos(unidade);

            return true;
        }

        public Unidade Get(int Id) {
            var unidadeRepositorio = new UnidadeRepositorio();
            var usuarioRepositorio = new UsuarioRepositorio();

            var unidade = unidadeRepositorio.Fetch(Id);
            var unidadeUsuario = unidadeRepositorio.Fetch(Account.Current.Usuario.Unidade.Id);

            if (!unidadeRepositorio.IsUnidadeFilha(unidadeUsuario, unidade) && unidade.Id != unidadeUsuario.Id)
            {
                return null;
            }
           
            unidade.Usuarios = usuarioRepositorio.FetchUsuariosByUnidade(unidade, false);
            unidade.Unidades = unidadeRepositorio.FetchUnidadesFilhas(unidade);

            var cidadeRepositorio = new CidadeRepositorio();
            
            unidade.Cidade = cidadeRepositorio.Fetch(unidade.CidadeId);
            unidade.Anexos = unidadeRepositorio.FetchArquivos(unidade);

            return unidade;
        }

        public List<Unidade> Search(string nome) {
            var unidadeRepositorio = new UnidadeRepositorio();
            var unidade = unidadeRepositorio.Fetch(Account.Current.Usuario.Unidade.Id);

            return unidadeRepositorio.Search(nome, unidade);
        }

        public List<Unidade> Search(string nome, UnidadeTipo tipo)
        {
            var unidadeRepositorio = new UnidadeRepositorio();
            var unidade = unidadeRepositorio.Fetch(Account.Current.Usuario.Unidade.Id);

            return unidadeRepositorio.Search(nome, unidade, tipo);
        }

        public List<Unidade> Unidades(int unidadeId) {
            var unidadeRepositorio = new UnidadeRepositorio();
            var unidade = unidadeRepositorio.Fetch(unidadeId);
            var unidades = unidadeRepositorio.FetchUnidadesFilhas(unidade);

            unidades.Add(unidade);

            return unidades;
        }

        public bool Excluir(Unidade unidade) {
            var unidadeRepositorio = new UnidadeRepositorio();

            if (!Account.Current.Usuario.Permissoes.Has("EXCLUIR_UNIDADE")) {
                this.MessageError = "USUARIO_SEM_PERMISSAO";
                return false;
            }

            unidadeRepositorio.Excluir(unidade);

            return true;
        }

    }
}