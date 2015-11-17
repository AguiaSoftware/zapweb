using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ZapWeb.Lib.Mvc;

namespace ZapWeb.Models
{
    public class UsuarioRepositorio : RepositorioTemp
    {

        public void Add(Usuario usuario) {
            usuario.UnidadeId = usuario.Unidade.Id;
            this.Db.Insert(usuario);
        }

        public void Update(Usuario usuario)
        {
            this.Db.Update(usuario);
        }

        public void UpdateUnidade(List<Usuario> usuarios, int unidadeId) {
            if (usuarios == null) return;

            foreach (var usuario in usuarios)
            {
                this.UpdateUnidade(usuario, unidadeId);
            }
        }
        
        public void UpdateUnidade(Usuario usuario, int unidadeId) {
            this.Db.Update("Usuario", "Id", new {
                Id = usuario.Id,
                UnidadeId = unidadeId
            });
        }

        public bool ExistNome(Usuario usuario)
        {
            return this.Db.ExecuteScalar<int>("SELECT COUNT(*) FROM Usuario WHERE Nome = @0 AND Usuario.Id != @1", usuario.Nome, usuario.Id) == 0 ? false : true;
        }

        public List<Usuario> Fetch(string nome, Unidade unidade)
        {
            var sql = PetaPoco.Sql.Builder.Append("SELECT Usuario.*, Unidade.*")
                                          .Append("FROM Usuario")
                                          .Append("INNER JOIN Unidade ON Unidade.Id = Usuario.UnidadeId")
                                          .Append("WHERE (Unidade.Id = @0 OR Unidade.Hierarquia LIKE @1)", unidade.Id, unidade.GetFullLevelHierarquia() + '%')
                                          .Append("AND Usuario.Nome LIKE @0", '%' + nome + '%')
                                          .Append("ORDER BY Usuario.Nome");

            return this.Db.Fetch<Usuario, Unidade, Usuario>((u, un) =>
            {

                u.Unidade = un;

                return u;
            }, sql).ToList();
        }

        public Usuario Fetch(int usuarioId) {
            var sql = PetaPoco.Sql.Builder.Append("SELECT *")
                                          .Append("FROM Usuario")
                                          .Append("WHERE Usuario.Id = @0", usuarioId);

            var usuario = this.Db.SingleOrDefault<Usuario>(sql);

            var unidadeRepositorio = new UnidadeRepositorio();
            usuario.Unidade = unidadeRepositorio.Fetch(usuario.UnidadeId);

            return usuario;
        }

        public List<Usuario> FetchUsuariosByUnidade(Unidade unidade, bool all)
        {
            var sql = PetaPoco.Sql.Builder.Append("SELECT Usuario.*, Unidade.*")
                                          .Append("FROM Usuario")
                                          .Append("INNER JOIN Unidade ON Unidade.Id = Usuario.UnidadeId")
                                          .Append("WHERE Unidade.Id = @0", unidade.Id);

            if (all) {
                sql.Append("OR Unidade.Hierarquia LIKE @0", unidade.GetFullLevelHierarquia() + '%');
            }

            return this.Db.Fetch<Usuario, Unidade, Usuario>((u, un) =>
            {

                u.Unidade = un;

                return u;
            }, sql).ToList();
        }

        public List<Arquivo> FetchArquivos(Usuario usuario)
        {
            var sql = PetaPoco.Sql.Builder.Append("SELECT Arquivo.*")
                                          .Append("FROM Arquivo")
                                          .Append("INNER JOIN UsuarioAnexo ON UsuarioAnexo.AnexoId = Arquivo.Id")
                                          .Append("WHERE UsuarioAnexo.UsuarioId = @0", usuario.Id);

            return this.Db.Fetch<Arquivo>(sql).ToList();
        }

        public void UpdateAnexos(Usuario usuario)
        {
            if (usuario.Anexos == null) return;

            this.Db.Execute("DELETE FROM UsuarioAnexo WHERE UsuarioId = @0", usuario.Id);

            foreach (var anexo in usuario.Anexos)
            {
                this.Db.Insert("UsuarioAnexo", "Id", new
                {
                    UsuarioId = usuario.Id,
                    AnexoId = anexo.Id
                });
            }
        }
    }
}