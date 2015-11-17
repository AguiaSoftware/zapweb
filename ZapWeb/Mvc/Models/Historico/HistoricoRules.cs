using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZapWeb.Models
{
    public class HistoricoRules : ZapWeb.Lib.Mvc.BusinessLogic
    {

        public bool Adicionar(Historico historico)
        {
            var historicoRepositorio = new HistoricoRepositorio();
            var condominioRepositorio = new CondominioRepositorio();
            var agendaRepositorio = new AgendaRepositorio();
            var condominio = condominioRepositorio.Simple(historico.Condominio.Id);

            historico.Condominio.Rank = historico.Rank;
            condominioRepositorio.UpdateRank(historico.Condominio);

            historico.Usuario = Account.Current.Usuario;
            historicoRepositorio.Insert(historico);

            //agendaRepositorio.Insert(new Agenda()
            //{
            //    Descricao = "<a href='#Condominio/Editar/" + condominio.Id + "'>" + condominio.Nome + "</a>",
            //    Data = historico.ProximoContato,
            //    UnidadeId = condominio.UnidadeId,
            //    Usuario = Account.Current.Usuario
            //});

            return true;
        }

        public List<Historico> All(int condominioId)
        {
            var historicoRepositorico = new HistoricoRepositorio();

            return historicoRepositorico.SimpleByCondominioId(condominioId);
        }

        public void UpdateData(int id, DateTime data)
        {
            var historicoRepositorico = new HistoricoRepositorio();

            historicoRepositorico.UpdateDate(id, data);
        }

    }
}