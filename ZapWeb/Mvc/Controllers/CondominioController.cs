﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ZapWeb.Models;

namespace ZapWeb.Mvc.Controllers
{
    public class CondominioController : ZapWeb.Lib.Mvc.Controller
    {

        public string Add(Condominio condominio)
        {
            var rules = new CondominioRules();

            if (rules.Adicionar(condominio) == null)
            {
                return this.Error(rules.MessageError);
            }

            return this.Success(condominio);
        }

        public string Update(Condominio condominio)
        {
            var rules = new CondominioRules();

            if (!rules.Update(condominio))
            {
                return this.Error(rules.MessageError);
            }

            return this.Success(condominio);
        }

        public string Get(int Id)
        {
            var rules = new CondominioRules();
            var condominio = rules.Get(Id);

            if (condominio == null)
            {
                return this.Error(rules.MessageError);
            }

            return this.Success(condominio);
        }

        public string All(CondominioSearch param)
        {
            var rules = new CondominioRules();

            return this.Success(rules.Search(param));
        }

    }
}