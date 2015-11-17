using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZapWeb.Models
{
    public class SindicoRules : ZapWeb.Lib.Mvc.BusinessLogic
    {

        public Sindico Adicionar(Sindico sindico)
        {
            var sindicoRepositorio = new SindicoRepositorio();
            var telefoneRepositorio = new TelefoneRepositorio();
            var enderecoRepositorio = new EnderecoRepositorio();

            enderecoRepositorio.Insert(sindico.Endereco);
            telefoneRepositorio.Insert(sindico.Telefones);

            sindicoRepositorio.Insert(sindico);

            return sindico;
        }

        public List<Sindico> Search(string nome)
        {
            var sindicoRepositorio = new SindicoRepositorio();

            return sindicoRepositorio.Simple(nome);
        }

    }
}