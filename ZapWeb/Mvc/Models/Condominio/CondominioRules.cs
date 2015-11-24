using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZapWeb.Models
{
    public class CondominioRules : ZapWeb.Lib.Mvc.BusinessLogic
    {

        public Condominio Adicionar(Condominio condominio)
        {
            var enderecoRepositorio = new EnderecoRepositorio();
            var contatoRepositorio = new ContatoRepositorio();
            var condominioRepositorio = new CondominioRepositorio();
            
            if (!Account.Current.Permissao.Has("ADD_CONDOMINIO"))
            {
                this.MessageError = "USUARIO_SEM_PERMISSAO";
                return null;
            }

            enderecoRepositorio.Insert(condominio.Endereco);

            contatoRepositorio.Insert(condominio.Sindico);
            contatoRepositorio.Insert(condominio.Zelador);

            condominio.DataCadastro = DateTime.Now;
            condominioRepositorio.Insert(condominio);

            return condominio;
        }

        public bool Update(Condominio condominio) {
            var condominioRepositorio = new CondominioRepositorio();
            var enderecoRepositorio = new EnderecoRepositorio();
            var contatoRepositorio = new ContatoRepositorio();

            if (!Account.Current.Permissao.Has("UPDATE_CONDOMINIO"))
            {
                this.MessageError = "USUARIO_SEM_PERMISSAO";
                return false;
            }

            var currentCondominio = condominioRepositorio.Simple(condominio.Id);
            var unidadeRepositorio = new UnidadeRepositorio();
            var unidade = unidadeRepositorio.Fetch(currentCondominio.UnidadeId);

            if (!unidade.IsInTreeView())
            {
                this.MessageError = "USUARIO_SEM_PERMISSAO";
                return false;
            }
            
            contatoRepositorio.Update(condominio.Sindico);
            contatoRepositorio.Update(condominio.Zelador);
            enderecoRepositorio.Update(condominio.Endereco);

            condominioRepositorio.Update(condominio);

            return true;
        }

        public Condominio Get(int Id)
        {
            var condominioRepositorio = new CondominioRepositorio();
            var unidadeRepositorio = new UnidadeRepositorio();
            var enderecoRepositorio = new EnderecoRepositorio();
            var contatoRepositorio = new ContatoRepositorio();
            var administradoraRepositorio = new AdministradoraRepositorio();

            if (!Account.Current.Permissao.Has("UPDATE_CONDOMINIO"))
            {
                this.MessageError = "USUARIO_SEM_PERMISSAO";
                return null;
            }

            var condominio = condominioRepositorio.Simple(Id);
            condominio.Unidade = unidadeRepositorio.Fetch(condominio.UnidadeId);
            condominio.Endereco = enderecoRepositorio.Fetch(condominio.EnderecoId);
            condominio.Sindico = contatoRepositorio.Fetch(condominio.SindicoId);
            condominio.Zelador = contatoRepositorio.Fetch(condominio.ZeladorId);
            condominio.Administradora = administradoraRepositorio.Simple(condominio.AdministradoraId).Get();

            if (!condominio.Unidade.IsInTreeView())
            {
                this.MessageError = "USUARIO_SEM_PERMISSAO";
                return null;
            }

            return condominio;

        }

        public List<Condominio> Search(CondominioSearch param)
        {
            var condominioRepositorio = new CondominioRepositorio();
            var unidadeRepositorio = new UnidadeRepositorio();
            var unidade = unidadeRepositorio.Fetch(Account.Current.Usuario.Unidade.Id);

            return condominioRepositorio.Search(param, unidade);
        }

        public List<Condominio> Imprimir(string ids)
        {
            var condominioRepositorio = new CondominioRepositorio();
            var unidadeRepositorio = new UnidadeRepositorio();
            var enderecoRepositorio = new EnderecoRepositorio();
            var contatoRepositorio = new ContatoRepositorio();
            var administradoraRepositorio = new AdministradoraRepositorio();

            var list = ids.Split(',');
            var intList = new List<int>();

            foreach (var item in list)
            {
                intList.Add(int.Parse(item));
            }

            var condominios = condominioRepositorio.Simple(intList);
            foreach (var condominio in condominios)
            {
                condominio.Unidade = unidadeRepositorio.Fetch(condominio.UnidadeId);
                condominio.Endereco = enderecoRepositorio.Fetch(condominio.EnderecoId);
                condominio.Sindico = contatoRepositorio.Fetch(condominio.SindicoId);
                condominio.Zelador = contatoRepositorio.Fetch(condominio.ZeladorId);
                condominio.Administradora = administradoraRepositorio.Simple(condominio.AdministradoraId).Get();
            }

            return condominios;
        }

    }
}