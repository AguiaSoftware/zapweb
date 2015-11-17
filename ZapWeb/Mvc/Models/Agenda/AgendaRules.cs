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
            var historicoRepositorio = new HistoricoRepositorio();
            var historicos = historicoRepositorio.Search(start, end, unidade);
            var agendas = new List<Agenda>();

            if(unidade.Tipo== UnidadeTipo.CENTRAL || unidade.Tipo== UnidadeTipo.ZAP)
            {
                foreach (var h in historicos)
                {
                    agendas.Add(new Agenda()
                    {
                        Id = h.Id,
                        Data = h.ProximoContato,
                        Url = "#Condominio/Editar/" + h.Condominio.Id,
                        Descricao = h.Condominio.Nome
                    });
                }
            }

            return agendas;
        }

        public void UpdateData(Agenda agenda)
        {
            var agendaRepositorio = new AgendaRepositorio();

            agendaRepositorio.UpdateData(agenda);
        }

    }
}