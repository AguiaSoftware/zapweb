using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using ZapWeb.Lib.Mvc;

using Pillar.Mvc;

namespace ZapWeb.Models
{
    public class ArquivoRules : BusinessLogic
    {

        public Arquivo Add(HttpPostedFileBase file) {

            if (!Account.Current.Permissao.Has("ADD_ARQUIVO")) {
                this.MessageError = "USUARIO_SEM_PERMISSAO";
                return null;
            }

            var arquivo = new Arquivo();
            arquivo.Nome = file.FileName;
            arquivo.Hash = this.UUID();
            arquivo.Size = file.ContentLength;
            arquivo.Tipo = file.ContentType;

            var arquivoRepositorio = new ArquivoRepositorio();
            arquivoRepositorio.Add(arquivo);

            file.SaveAs(Application.Path("/Public/files/" + arquivo.Hash));

            return arquivo;
        }

        public bool Remove(Arquivo arquivo){

            if (!Account.Current.Permissao.Has("REMOVE_ARQUIVO"))
            {
                this.MessageError = "USUARIO_SEM_PERMISSAO";
                return false;
            }

            var arquivoRepositorio = new ArquivoRepositorio();
            arquivoRepositorio.Remove(arquivo);

            return true;
        }

        public Arquivo GetByHash(string hash) {
            var arquivoRepositorio = new ArquivoRepositorio();

            return arquivoRepositorio.FetchByHash(hash);
        }

        private string UUID() {
            return Guid.NewGuid().ToString();
        }

    }
}