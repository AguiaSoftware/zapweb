using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZapWeb.Models
{
    public class AgendaRules: ZapWeb.Lib.Mvc.BusinessLogic
    {

        public bool Adicionar(Agenda agenda)
        {
            var agendaRepositorio = new AgendaRepositorio();

            agenda.Usuario = Account.Current.Usuario;

            agendaRepositorio.Insert(agenda);

            return true;
        }

        public List<Agenda> Search(DateTime start, DateTime end)
        {
            var agendaRepositorio = new AgendaRepositorio();
            var unidadeRepositorio = new UnidadeRepositorio();
            var unidade = unidadeRepositorio.Fetch(Account.Current.Usuario.Unidade.Id);

            return agendaRepositorio.Search(start, end, unidade);
        }

        public void UpdateData(Agenda agenda)
        {
            var agendaRepositorio = new AgendaRepositorio();

            agendaRepositorio.UpdateData(agenda);
        }

    }
}