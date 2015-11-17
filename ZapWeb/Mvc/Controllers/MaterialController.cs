using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ZapWeb.Models;

namespace ZapWeb.Mvc.Controllers
{
    public class MaterialController : ZapWeb.Lib.Mvc.Controller
    {

        public string Add(Material material)
        {
            var rules = new MaterialRules();

            if (!rules.Adicionar(material))
            {
                return this.Error(rules.MessageError);
            }

            return this.Success(material);
        }

        public string Update(Material material)
        {
            var rules = new MaterialRules();

            if (!rules.Update(material))
            {
                return this.Error(rules.MessageError);
            }

            return this.Success(material);
        }

        public string All(int condominioId)
        {
            var rules = new MaterialRules();

            return this.Success(rules.All(condominioId));
        }

        public string Get(int Id)
        {
            var rules = new MaterialRules();
            var material = rules.Get(Id);

            if(material == null)
            {
                return this.Error(rules.MessageError);
            }

            return this.Success(material);
        }

        public ActionResult Download(int materialId, string nome, string hash)
        {
            var rules = new MaterialRules();
            var filename = rules.GetPdfFilename(materialId, hash);
            
            System.Net.Mime.ContentDisposition cd = new System.Net.Mime.ContentDisposition
            {
                FileName = nome + ".pdf",
                Inline = true,
            };

            Response.AppendHeader("Content-Disposition", cd.ToString());

            return File(filename, "application/force-download");
        }

    }
}