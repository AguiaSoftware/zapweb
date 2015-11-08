using Pillar.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZapWeb.Lib.Mvc;
using ZapWeb.Models;

namespace ZapWeb.Controllers
{
    public class FornecedorController : ZapWeb.Lib.Mvc.Controller
    {
        public string Add(Fornecedor fornecedor)
        {
            var fornecedorRules = new FornecedorRules();

            if (!fornecedorRules.Add(fornecedor)) {
                return Error(fornecedorRules.MessageError);
            }

            return Success(fornecedor);
        }

        public string Update(Fornecedor fornecedor)
        {
            var fornecedorRules = new FornecedorRules();

            if (!fornecedorRules.Update(fornecedor))
            {
                return Error(fornecedorRules.MessageError);
            }

            return Success(fornecedor);
        }

        public string Get(int Id)
        {
            var fornecedorRules = new FornecedorRules();
            var fornecedor=fornecedorRules.Get(Id);

            if (fornecedor == null)
            {
                return Error(fornecedorRules.MessageError);
            }

            return Success(fornecedor);
        }

        public string All(FornecedorPesquisa parametros, int totalPerPage, int page) {
            var fornecedorRules = new FornecedorRules();
            
            var paging = new Paging()
            {
                page = page,
                totalPerPage = totalPerPage
            };

            return Success(fornecedorRules.All(parametros, paging), paging);
        }

        public string Search(string nome) {
            var fornecedorRules = new FornecedorRules();
            
            return Success(fornecedorRules.Search(nome));
        }
    }
}
