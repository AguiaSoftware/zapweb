using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZapWeb.Models
{
    public class AdministradoraRepositorio : ZapWeb.Lib.Mvc.Repositorio<Administradora>
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
            if (administradora.Endereco != null)
            {
                administradora.EnderecoId = administradora.Endereco.Id;
            }

            this.InsertTelefones(administradora);

            this.Db.Update(administradora);
        }

        public void InsertTelefones(Administradora administradora)
        {
            if (administradora.Telefones == null) return;

            this.Db.Execute("DELETE FROM AdministradoraTelefone WHERE AdministradoraId = @0", administradora.Id);

            foreach (var telefone in administradora.Telefones)
            {
                this.Db.Insert("AdministradoraTelefone", "Id", new {
                    AdministradoraId = administradora.Id,
                    TelefoneId = telefone.Id
                });
            }
        }

        public AdministradoraRepositorio Search(string nome)
        {
            var sql = PetaPoco.Sql.Builder.Append("SELECT *")
                                          .Append("FROM Administradora")
                                          .Append("WHERE Nome LIKE @0", "%" + nome + "%");

            ResultSet = this.Db.Fetch<Administradora>(sql);

            return this;
        }

        public AdministradoraRepositorio Simple(int Id)
        {
            var sql = PetaPoco.Sql.Builder.Append("SELECT *")
                                          .Append("FROM Administradora")
                                          .Append("WHERE Id = @0", Id);

            ResultSet = this.Db.Fetch<Administradora>(sql);

            return this;
        }

        public AdministradoraRepositorio IncludeTelefones()
        {            

            foreach (var administradora in ResultSet)
            {
                var sql = PetaPoco.Sql.Builder.Append("SELECT Telefone.*")
                                              .Append("FROM Administradora")
                                              .Append("INNER JOIN AdministradoraTelefone ON AdministradoraTelefone.AdministradoraId = AdministradoraId")
                                              .Append("INNER JOIN Telefone ON Telefone.Id = AdministradoraTelefone.TelefoneId")
                                              .Append("WHERE Administradora.Id = @0", administradora.Id);

                administradora.Telefones = this.Db.Fetch<Telefone>(sql);
            }

            return this;
        }

        public AdministradoraRepositorio IncludeEndereco()
        {
            var enderecoRepositorio = new EnderecoRepositorio();
            foreach (var administradora in ResultSet)
            {
                administradora.Endereco = enderecoRepositorio.Fetch(administradora.EnderecoId);
            }

            return this;
        }

    }
}