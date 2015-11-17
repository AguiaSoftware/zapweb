using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZapWeb.Models
{
    public class AgendaRepositorio : ZapWeb.Lib.Mvc.RepositorioTemp
    {

        public Agenda Insert(Agenda agenda)
        {
            if (agenda.Unidade != null)
            {
                agenda.UnidadeId = agenda.Unidade.Id;
            }

            if (agenda.Usuario != null)
            {
                agenda.UsuarioId = agenda.Usuario.Id;
            }

            var agendaDisponivel = this.GetHorarioDisponivel(agenda);
            if (agendaDisponivel == null)
            {
                agenda.Data = agenda.Data.AddHours(8);
            }
            else
            {
                var hour = agendaDisponivel.Data.Hour + 1;

                if (hour > 17)
                {
                    agenda.Data = agenda.Data.AddHours(17);
                }
                else
                {
                    agenda.Data = agenda.Data.AddHours(hour);
                }
                
            }

            

            this.Db.Insert(agenda);

            return agenda;
        }

        private Agenda GetHorarioDisponivel(Agenda agenda)
        {
            var sql = PetaPoco.Sql.Builder.Append("SELECT Agenda.*")
                                          .Append("FROM Agenda")
                                          .Append("WHERE Agenda.UnidadeId = @0 AND Data = @1", agenda.UnidadeId, agenda.Data)
                                          .Append("ORDER BY Data DESC")
                                          .Append("LIMIT 1");

            return this.Db.SingleOrDefault<Agenda>(sql);
        }

        public void UpdateData(Agenda agenda)
        {
            this.Db.Update("Agenda", "Id", new
            {
                Id = agenda.Id,
                Data = agenda.Data
            });
        }

        public List<Agenda> Search(DateTime start, DateTime end, Unidade unidade)
        {
            var sql = PetaPoco.Sql.Builder.Append("SELECT Agenda.*")
                                          .Append("FROM Agenda")
                                          .Append("INNER JOIN Unidade ON Unidade.Id = Agenda.UnidadeId")
                                          .Append("WHERE Agenda.Data BETWEEN @0 AND @1", start, end)
                                          .Append("AND (Unidade.Hierarquia LIKE @0 OR Unidade.Id = @1)", unidade.GetFullLevelHierarquia() + "%", unidade.Id);

            return this.Db.Fetch<Agenda>(sql);
        }

    }
}