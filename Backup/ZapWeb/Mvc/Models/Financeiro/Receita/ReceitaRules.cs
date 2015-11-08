using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using ZapWeb.Lib.Mvc;

using Pillar.RealTime;

namespace ZapWeb.Models
{
    public class ReceitaRules : BusinessLogic
    {
        public bool Add(ref Receita receita) {

            if (!Account.Current.Permissao.Has("ADD_RECEITA"))
            {
                this.MessageError = "USUARIO_SEM_PERMISSAO";
                return false;
            }

            var unidadeRepositorio = new UnidadeRepositorio();
            var unidade = unidadeRepositorio.Fetch(Account.Current.Usuario.Unidade.Id);

            if (unidade.Tipo != UnidadeTipo.ZAP) {
                this.MessageError = "USUARIO_SEM_PERMISSAO";
                return false;
            }

            var receitaRepositorio = new ReceitaRepositorio();
            var receitaCurrent = receitaRepositorio.Fetch(receita.Mes, receita.Ano, receita.Unidade.Id);

            if (receitaCurrent.Id == 0)
            {
                receitaRepositorio.Add(receita);
            }
            else {
                foreach (var item in receita.Items)
                {
                    receitaCurrent.Items.Add(item);
                }

                receita = receitaCurrent;
                this.Update(receitaCurrent);
            }

            return true;
        }

        public bool Update(Receita receita)
        {

            if (!Account.Current.Permissao.Has("UPDATE_RECEITA"))
            {
                this.MessageError = "USUARIO_SEM_PERMISSAO";
                return false;
            }

            var unidadeRepositorio = new UnidadeRepositorio();
            var unidade = unidadeRepositorio.Fetch(Account.Current.Usuario.Unidade.Id);

            if (unidade.Tipo != UnidadeTipo.ZAP)
            {
                this.MessageError = "USUARIO_SEM_PERMISSAO";
                return false;
            }

            var receitaRepositorio = new ReceitaRepositorio();
            receitaRepositorio.Update(receita);

            return true;
        }

        public Receita Get(int Id) {

            if (!Account.Current.Permissao.Has("UPDATE_RECEITA"))
            {
                this.MessageError = "USUARIO_SEM_PERMISSAO";
                return null;
            }

            var unidadeRepositorio = new UnidadeRepositorio();
            var unidade = unidadeRepositorio.Fetch(Account.Current.Usuario.Unidade.Id);

            if (unidade.Tipo != UnidadeTipo.ZAP)
            {
                this.MessageError = "USUARIO_SEM_PERMISSAO";
                return null;
            }

            var receitaRepositorio = new ReceitaRepositorio();

            return receitaRepositorio.Fetch(Id);
        }

        public Receita Get(int mes, int ano, int unidadeId)
        {
            var receitaRepositorio = new ReceitaRepositorio();

            if (!Account.Current.Permissao.Has("UPDATE_RECEITA"))
            {
                this.MessageError = "USUARIO_SEM_PERMISSAO";
                return null;
            }

            return receitaRepositorio.Fetch(mes, ano, unidadeId);
        }

        public List<Receita> All(int unidadeId) {
            var unidadeRepositorio = new UnidadeRepositorio();
            var unidade = unidadeRepositorio.Fetch(Account.Current.Usuario.Unidade.Id);

            if (unidade.Tipo != UnidadeTipo.ZAP)
            {
                this.MessageError = "USUARIO_SEM_PERMISSAO";
                return null;
            }

            unidade = unidadeRepositorio.Fetch(unidadeId);

            var receitaRepositorio = new ReceitaRepositorio();

            return receitaRepositorio.FetchAll(unidade);
        }
                
    }
}