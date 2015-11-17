using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ZapWeb.Lib.Mvc;

namespace ZapWeb.Models
{
    public class PermissaoRepositorio : RepositorioTemp
    {

        public void Add(List<Permissao> permissoes, int grupoPermissaoId) {

            if (permissoes == null) return;

            this.Db.Execute("DELETE FROM Permissao WHERE GrupoId = @0", grupoPermissaoId);

            for (int i = 0; i < permissoes.Count; i++)
            {
                permissoes[i].GrupoId = grupoPermissaoId;
                this.Db.Insert(permissoes[i]);
            }
        }

    }
}