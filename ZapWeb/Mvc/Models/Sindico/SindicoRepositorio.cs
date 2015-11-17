using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZapWeb.Models
{
    public class SindicoRepositorio : ZapWeb.Lib.Mvc.RepositorioTemp
    {

        public Sindico Insert(Sindico sindico)
        {

            if (sindico.Endereco != null)
            {
                sindico.EnderecoId = sindico.Endereco.Id;
            }

            this.Db.Insert(sindico);

            this.InsertTelefones(sindico);

            return sindico;
        }

        public List<Sindico> Simple(string nome)
        {
            var sql = PetaPoco.Sql.Builder.Append("SELECT *")
                                          .Append("FROM Sindico")
                                          .Append("WHERE Nome LIKE @0", "%" + nome + "%");

            return this.Db.Fetch<Sindico>(sql);

        }

        private void InsertTelefones(Sindico sindico)
        {
            if (sindico.Telefones == null) return;

            foreach (var telefone in sindico.Telefones)
            {
                this.Db.Insert("SindicoTelefone", "Id", new {
                    SindicoId = sindico.Id,
                    TelefoneId = telefone.Id
                });
            }
        }        

    }
}