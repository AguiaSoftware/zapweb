using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZapWeb.Models
{
    public class HistoricoRepositorio : ZapWeb.Lib.Mvc.Repositorio
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

    }
}