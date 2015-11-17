using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZapWeb.Models
{
    public class AdministradoraRules : ZapWeb.Lib.Mvc.BusinessLogic
    {

        public Administradora Adicionar(Administradora administradora)
        {
            var adminRepositorio = new AdministradoraRepositorio();
            var enderecoRepositorio = new EnderecoRepositorio();
            var telefoneRepositorio = new TelefoneRepositorio();

            enderecoRepositorio.Insert(administradora.Endereco);
            telefoneRepositorio.Insert(administradora.Telefones);

            adminRepositorio.Insert(administradora);

            return administradora;
        }

        public bool Update(Administradora administradora)
        {
            var adminRepositorio = new AdministradoraRepositorio();
            var enderecoRepositorio = new EnderecoRepositorio();
            var telefoneRepositorio = new TelefoneRepositorio();

            telefoneRepositorio.Insert(administradora.Telefones);

            adminRepositorio.Update(administradora);
            enderecoRepositorio.Update(administradora.Endereco);

            return true;
        }

        public List<Administradora> Search(string nome)
        {
            var adminRepositorio = new AdministradoraRepositorio();

            return adminRepositorio.Search(nome).GetList();
        }

        public Administradora Get(int Id)
        {
            var adminRepositorio = new AdministradoraRepositorio();
            var administradora = adminRepositorio.Simple(Id)
                                                 .IncludeTelefones()
                                                 .IncludeEndereco()
                                                 .Get();
            
            return administradora;
        }

    }
}