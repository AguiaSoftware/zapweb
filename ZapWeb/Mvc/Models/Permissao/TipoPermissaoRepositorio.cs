using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ZapWeb.Lib.Mvc;

namespace ZapWeb.Models
{
    public class TipoPermissaoRepositorio : RepositorioTemp
    {
        public List<TipoPermissao> FetchAll() {
            var sql = PetaPoco.Sql.Builder.Append("SELECT * FROM TipoPermissao ORDER BY Ordem");

            return this.Db.Fetch<TipoPermissao>(sql).ToList();
        }
    }
}