using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ZapWeb.Lib.Mvc;

namespace ZapWeb.Models
{
    public class TelefoneRepositorio : Repositorio
    {
        public void Add(List<Telefone> telefones)
        {
            if (telefones == null) return;

            foreach (var telefone in telefones)
            {
                this.Db.Insert(telefone);
            }
        }
    }
}