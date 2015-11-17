using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZapWeb.Models
{
    public class AdministradoraRepositorio : ZapWeb.Lib.Mvc.Repositorio
    {

        public Administradora Insert(Administradora administradora)
        {
            if (administradora.Endereco != null)
            {
                administradora.EnderecoId = administradora.Endereco.Id;
            }

            this.Db.Insert(administradora);

            this.InsertTelefones(administradora);

            return administradora;
        }

        public void Update(Administradora administradora)
        {
            this.Db.Update(administradora);
        }

        public void InsertTelefones(Administradora administradora)
        {
            if (administradora.Telefones == null) return;

            foreach (var telefone in administradora.Telefones)
            {
                this.Db.Insert("AdministradoraTelefone", "Id", new {
                    AdministradoraId = administradora.Id,
                    TelefoneId = telefone.Id
                });
            }
        }

        public List<Administradora> Search(string nome)
        {
            var sql = PetaPoco.Sql.Builder.Append("SELECT *")
                                          .Append("FROM Administradora")
                                          .Append("WHERE Nome LIKE @0", "%" + nome + "%");

            return this.Db.Fetch<Administradora>(sql);
        }

        public Administradora Fetch(int Id)
        {
            var sql = PetaPoco.Sql.Builder.Append("SELECT *")
                                          .Append("FROM Administradora")
                                          .Append("WHERE Id = @0", Id);

            return this.Db.SingleOrDefault<Administradora>(sql);
        }

    }
}