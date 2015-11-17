using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ZapWeb.Lib.Mvc;

namespace ZapWeb.Models
{
    public class ArquivoRepositorio : RepositorioTemp
    {

        public void Add(Arquivo arquivo) {
            this.Db.Insert(arquivo);
        }

        public void Remove(Arquivo arquivo) {
            TableDependency.Resolve(this.Db, "DELETE", "Arquivo", arquivo.Id);
            this.Db.Execute("DELETE FROM Arquivo WHERE Id = @0", arquivo.Id);
        }

        public Arquivo FetchByHash(string hash) {
            var sql = PetaPoco.Sql.Builder.Append("SELECT Arquivo.*")
                                          .Append("FROM Arquivo")
                                          .Append("WHERE Arquivo.Hash = @0", hash)
                                          .Append("ORDER BY Arquivo.Nome");

            return this.Db.SingleOrDefault<Arquivo>(sql);
        }
    }
}