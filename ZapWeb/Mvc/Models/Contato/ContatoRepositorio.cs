using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ZapWeb.Lib.Mvc;

namespace ZapWeb.Models
{
    public class ContatoRepositorio : Repositorio
    {
        public void Add(Contato contato)
        {
            this.Db.Insert(contato);

            if (contato.Telefones != null) {
                var telefoneRepositorio = new TelefoneRepositorio();
                telefoneRepositorio.Add(contato.Telefones);

                foreach (var telefone in contato.Telefones)
                {
                    this.Db.Insert("ContatoTelefone", "Id", new
                    {
                        ContatoId = contato.Id,
                        TelefoneId = telefone.Id
                    });
                }
            }
            
        }

        public void Update(Contato contato)
        {
            this.Db.Update(contato);

            if (contato.Telefones != null)
            {
                this.RemoveTelefones(contato.Id);

                var telefoneRepositorio = new TelefoneRepositorio();
                telefoneRepositorio.Add(contato.Telefones);

                foreach (var telefone in contato.Telefones)
                {
                    this.Db.Insert("ContatoTelefone", "Id", new
                    {
                        ContatoId = contato.Id,
                        TelefoneId = telefone.Id
                    });
                }
            }

        }

        public void RemoveTelefones(int contatoId) {
            this.Db.Execute("DELETE ContatoTelefone, Telefone FROM ContatoTelefone INNER JOIN Telefone ON Telefone.Id = ContatoTelefone.TelefoneId WHERE ContatoTelefone.ContatoId = @0", contatoId);
        }

        public Contato Fetch(int Id) {
            var sql = PetaPoco.Sql.Builder.Append("SELECT Contato.*, Telefone.*")
                                          .Append("FROM Contato")
                                          .Append("LEFT JOIN ContatoTelefone ON ContatoTelefone.ContatoId = Contato.Id")
                                          .Append("LEFT JOIN Telefone ON Telefone.Id = ContatoTelefone.TelefoneId")
                                          .Append("WHERE Contato.Id = @0", Id)
                                          .Append("ORDER BY Contato.Nome");

            Contato contato = null;
            
            this.Db.Fetch<Contato, Telefone, Contato>((c, t) => {

                if (contato == null) {
                    contato = c;
                    contato.Telefones = new List<Telefone>();
                }

                contato.Telefones.Add(t);

                return c;
            }, sql).ToList();

            return contato;
        }
    }
}