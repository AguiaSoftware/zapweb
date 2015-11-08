using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ZapWeb.Lib.Mvc;

namespace ZapWeb.Models
{
    public class CentroCustoRepositorio : Repositorio
    {

        public void Add(CentroCusto centroCusto) {
            this.Db.Insert(centroCusto);
        }

        public void Update(CentroCusto centroCusto) {
            var oldNome = this.Db.ExecuteScalar<string>("SELECT Nome FROM CentroCusto WHERE Id = @0", centroCusto.Id);
            var newNome = centroCusto.Nome;

            var centros = this.Db.Fetch<CentroCusto>("SELECT * FROM CentroCusto WHERE Nome LIKE @0", oldNome + ":%").ToList();

            foreach (var centro in centros)
            {
                centro.Nome = centro.Nome.Replace(oldNome, newNome);
                this.Db.Update(centro);
            }

            this.Db.Update(centroCusto);
        }

        public bool Has(CentroCusto centroCusto) {
            return this.Db.ExecuteScalar<bool>("SELECT COUNT(*) FROM CentroCusto WHERE Nome = @0 AND Id != @1", centroCusto.Nome, centroCusto.Id);
        }

        public List<CentroCusto> FetchByTipo(TipoCentroCusto tipo)
        {
            var sql = PetaPoco.Sql.Builder.Append("SELECT CentroCusto.*")
                                          .Append("FROM CentroCusto");

            if(tipo!= TipoCentroCusto.TODOS){
                sql.Append("WHERE CentroCusto.Tipo = @0", tipo);
            }

            sql.Append("ORDER BY CentroCusto.Nome");

            return this.Db.Fetch<CentroCusto>(sql).ToList();
        }

        public List<CentroCusto> FetchAll() {
            var sql = PetaPoco.Sql.Builder.Append("SELECT CentroCusto.*")
                                          .Append("FROM CentroCusto")
                                          .Append("ODER BY CentroCusto.Nome");

            return this.Db.Fetch<CentroCusto>(sql).ToList();
        }

    }
}