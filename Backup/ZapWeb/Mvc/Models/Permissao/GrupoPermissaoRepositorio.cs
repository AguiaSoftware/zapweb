using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ZapWeb.Lib.Mvc;

namespace ZapWeb.Models
{
    public class GrupoPermissaoRepositorio : Repositorio
    {

        public void Add(GrupoPermissao grupo) {
            this.Db.Insert(grupo);
        }

        public void Update(GrupoPermissao grupo)
        {
            this.Db.Update(grupo);
        }

        public void Remove(GrupoPermissao grupo)
        {
            this.Db.Delete(grupo);
        }

        public bool ExistNome(GrupoPermissao grupo)
        {
            return this.Db.ExecuteScalar<int>("SELECT COUNT(*) FROM GrupoPermissao WHERE Nome = @0 AND Id != @1", grupo.Nome, grupo.Id) == 0 ? false : true;
        }

        public List<GrupoPermissao> FetchAll()
        {
            var sql = PetaPoco.Sql.Builder.Append("SELECT GrupoPermissao.*, Permissao.*")
                                          .Append("FROM GrupoPermissao")
                                          .Append("LEFT JOIN Permissao ON Permissao.GrupoId = GrupoPermissao.Id")
                                          .Append("ORDER BY GrupoPermissao.Nome");

            var grupos = new List<GrupoPermissao>();

            this.Db.Fetch<GrupoPermissao, Permissao, GrupoPermissao>((gp, p) =>
            {

                var grupo = grupos.Find(item => item.Id == gp.Id);

                if (grupo == null)
                {
                    grupo = gp;
                    grupo.Permissoes = new List<Permissao>();
                    grupos.Add(grupo);
                }

                if (p.Nome != null) {
                    grupo.Permissoes.Add(p);
                }                

                return gp;
            }, sql).ToList();

            return grupos;
        }

        public GrupoPermissao Fetch(int grupoId) {
            var sql = PetaPoco.Sql.Builder.Append("SELECT GrupoPermissao.*, Permissao.*")
                                          .Append("FROM GrupoPermissao")
                                          .Append("JOIN Permissao ON Permissao.GrupoId = GrupoPermissao.Id")
                                          .Append("WHERE GrupoPermissao.Id = @0", grupoId)
                                          .Append("ORDER BY GrupoPermissao.Nome");

            GrupoPermissao grupo = null;

            this.Db.Fetch<GrupoPermissao, Permissao, GrupoPermissao>((gp, p) => {

                if (grupo == null) {
                    grupo = gp;
                    grupo.Permissoes = new List<Permissao>();
                }

                grupo.Permissoes.Add(p);

                return gp;
            }, sql).ToList();

            if (grupo == null) {
                grupo = new GrupoPermissao();
                grupo.Permissoes = new List<Permissao>();
            }

            return grupo;
        }

    }
}