using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ZapWeb.Lib.Mvc;

namespace ZapWeb.Models
{
    public class CidadeRepositorio : Repositorio
    {

        public List<Cidade> Search(string nome) {
            return this.Db.Fetch<Cidade>("SELECT * FROM Cidade WHERE Nome LIKE @0 ORDER BY Nome LIMIT 20", nome + '%').ToList();
        }

        public Cidade Fetch(int id) {
            return this.Db.SingleOrDefault<Cidade>("SELECT * FROM Cidade WHERE Id = @0", id);
        }

    }
}