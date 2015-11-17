using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ZapWeb.Lib.Mvc;

namespace ZapWeb.Models
{
    public class DespesaHistoricoRepositorio : RepositorioTemp
    {
        public void Add(DespesaHistorico historico) {

            historico.DespesaId = historico.Despesa.Id;
            historico.UsuarioId = historico.Usuario.Id;

            this.Db.Insert(historico);
        }

        public List<DespesaHistorico> Fetch(Despesa despesa) {
            var sql = PetaPoco.Sql.Builder.Append("SELECT DespesaHistorico.*")
                                          .Append("FROM DespesaHistorico")
                                          .Append("WHERE DespesaHistorico.DespesaId = @0", despesa.Id);

            return this.Db.Fetch<DespesaHistorico>(sql).ToList();
        }
    }
}