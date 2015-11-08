using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ZapWeb.Lib.Mvc;

namespace ZapWeb.Models
{
    public class EnderecoRepositorio : Repositorio
    {
        public void Add(Endereco endereco)
        {
            if (endereco.Cidade != null) {
                endereco.CidadeId = endereco.Cidade.Id;
            }

            this.Db.Insert(endereco);
        }

        public void Update(Endereco endereco)
        {
            if (endereco.Cidade != null)
            {
                endereco.CidadeId = endereco.Cidade.Id;
            }

            this.Db.Update(endereco);
        }

        public Endereco Fetch(int Id) {
            var sql = PetaPoco.Sql.Builder.Append("SELECT Endereco.*, Cidade.*")
                                          .Append("FROM Endereco")
                                          .Append("LEFT JOIN Cidade ON Cidade.Id = Endereco.CidadeId")
                                          .Append("WHERE Endereco.Id = @0", Id);

            var endereco = this.Db.Fetch<Endereco, Cidade, Endereco>((e, c)=>{
                e.Cidade = c;
                return e;
            }, sql).ToList();

            if (endereco == null) return null;

            return endereco[0];
        }
    }
}