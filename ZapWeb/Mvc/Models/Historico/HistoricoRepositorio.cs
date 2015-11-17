using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZapWeb.Models
{
    public class HistoricoRepositorio : ZapWeb.Lib.Mvc.RepositorioTemp
    {

        public Historico Insert(Historico historico)
        {
            if (historico.Usuario != null)
            {
                historico.UsuarioId = historico.Usuario.Id;
            }

            if (historico.Condominio != null)
            {
                historico.CondominioId = historico.Condominio.Id;
            }

            this.Db.Insert(historico);

            return historico;
        }

        public List<Historico> SimpleByCondominioId(int condominioId)
        {
            var sql = PetaPoco.Sql.Builder.Append("SELECT *")
                                          .Append("FROM Historico")
                                          .Append("WHERE Historico.CondominioId = @0", condominioId);

            return this.Db.Fetch<Historico>(sql);
        }

        public List<Historico> Search(DateTime start, DateTime end, Unidade unidade)
        {
            var sql = PetaPoco.Sql.Builder.Append("SELECT Historico.*, Condominio.*")
                                          .Append("FROM Historico")
                                          .Append("INNER JOIN Condominio ON Condominio.Id = Historico.CondominioId")
                                          .Append("INNER JOIN Unidade ON Unidade.Id = Condominio.UnidadeId")
                                          .Append("WHERE Historico.ProximoContato BETWEEN @0 AND @1", start, end)
                                          .Append("AND (Unidade.Hierarquia LIKE @0 OR Unidade.Id = @1)", unidade.GetFullLevelHierarquia() + "%", unidade.Id);

            return this.Db.Fetch<Historico, Condominio, Historico>((h, c)=> {

                h.Condominio = c;

                return h;
            }, sql);
        }

    }
}