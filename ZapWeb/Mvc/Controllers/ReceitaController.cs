using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZapWeb.Lib.Mvc;
using ZapWeb.Models;

using Pillar.Mvc;

namespace ZapWeb.Controllers
{
    public class ReceitaController : ZapWeb.Lib.Mvc.Controller
    {

        public string Add(Receita receita) {
            var receitaRules = new ReceitaRules();

            if (!receitaRules.Add(ref receita))
            {
                return Error(receitaRules.MessageError);
            }

            return Success(receita);
        }

        public string Update(Receita receita)
        {
            var receitaRules = new ReceitaRules();

            if (!receitaRules.Update(receita))
            {
                return Error(receitaRules.MessageError);
            }

            return Success(receita);
        }

        public string Excluir(Receita receita)
        {
            var receitaRules = new ReceitaRules();

            if (!receitaRules.Excluir(receita))
            {
                return Error(receitaRules.MessageError);
            }

            return Success(receita);
        }

        public string All(int unidadeId){
            var receitaRules = new ReceitaRules();
            var receitas = receitaRules.All(unidadeId);

            if (receitas == null)
            {
                return Error(receitaRules.MessageError);
            }

            return Success(receitas);
        }

        public string Get(int Id) {
            var receitaRules = new ReceitaRules();
            var receita = receitaRules.Get(Id);

            if (receita == null)
            {
                return Error(receitaRules.MessageError);
            }

            return Success(receita);
        }

        public string Find(int mes, int ano, int unidadeId)
        {
            var receitaRules = new ReceitaRules();
            var receita = receitaRules.Get(mes, ano, unidadeId);

            return Success(receita);
        }
    }
}
