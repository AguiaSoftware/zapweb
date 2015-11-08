using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ZapWeb.Lib.Mvc;

namespace ZapWeb.Models
{
    public class UnidadeRepositorio : Repositorio
    {

        public void Add(Unidade unidade) {
            var zap = this.FetchZapUnidade();

            if (unidade.Cidade != null) {
                unidade.CidadeId = unidade.Cidade.Id;
            }

            unidade.Hierarquia = zap.GetLevelHierarquia();

            this.Db.Insert(unidade);

            this.SetHierarquia(unidade.GetFullLevelHierarquia(), unidade.Unidades);

            var usuarioRepositorio = new UsuarioRepositorio();
            usuarioRepositorio.UpdateUnidade(unidade.Usuarios, unidade.Id);
        }

        public void Update(Unidade unidade)
        {
            if (unidade.Cidade != null)
            {
                unidade.CidadeId = unidade.Cidade.Id;
            }

            unidade.Hierarquia = this.Fetch(unidade.Id).Hierarquia;

            this.Db.Update(unidade);

            this.SetHierarquia(unidade.GetFullLevelHierarquia(), unidade.Unidades);

            var usuarioRepositorio = new UsuarioRepositorio();
            usuarioRepositorio.UpdateUnidade(unidade.Usuarios, unidade.Id);
        }

        public Unidade FetchZapUnidade()
        {
            return this.Db.Single<Unidade>("SELECT * FROM Unidade WHERE Tipo = @0", UnidadeTipo.ZAP);
        }

        private void SetHierarquia(string hierarquia, List<Unidade> unidades) {

            if (unidades == null) return;

            foreach (var unidade in unidades)
            {
                this.Db.Update("Unidade", "Id", new
                {
                    Id = unidade.Id,
                    Hierarquia = hierarquia
                });
            }           
            
        }

        public bool ExistNome(Unidade unidade)
        {
            return this.Db.ExecuteScalar<int>("SELECT COUNT(*) FROM Unidade WHERE Nome = @0 AND Id != @1", unidade.Nome, unidade.Id) == 0 ? false : true;
        }

        public List<Unidade> Search(string nome, Unidade unidade)
        {
            return this.Db.Fetch<Unidade>("SELECT * FROM Unidade WHERE (Unidade.Id = @0 OR Unidade.Hierarquia LIKE @1) AND Unidade.Nome LIKE @2 ORDER BY Unidade.Nome", unidade.Id, unidade.GetFullLevelHierarquia() + '%', '%' + nome + '%').ToList();
        }

        public List<Unidade> Search(string nome, Unidade unidade, UnidadeTipo tipo)
        {
            return this.Db.Fetch<Unidade>("SELECT * FROM Unidade WHERE (Unidade.Id = @0 OR Unidade.Hierarquia LIKE @1) AND (Unidade.Tipo = @3 OR @3 = @4) AND Unidade.Nome LIKE @2 ORDER BY Unidade.Nome", unidade.Id, unidade.GetFullLevelHierarquia() + '%', '%' + nome + '%', tipo, UnidadeTipo.TODOS).ToList();
        }

        public List<Unidade> FetchUnidadesFilhas(Unidade unidade)
        {
            var sql = PetaPoco.Sql.Builder.Append("SELECT Unidade.*, Cidade.*")
                                          .Append("FROM Unidade")
                                          .Append("LEFT JOIN Cidade ON Cidade.Id = Unidade.CidadeId")
                                          .Append("WHERE Unidade.Hierarquia LIKE @0", unidade.GetFullLevelHierarquia() + "%")
                                          .Append("ORDER BY Unidade.Nome");

            return this.Db.Fetch<Unidade, Cidade, Unidade>((u, c) =>
            {
                u.Cidade = c;

                return u;
            }, sql).ToList();
        }

        public List<Unidade> GetUnidadesFilhas(Unidade unidade)
        {
            var sql = PetaPoco.Sql.Builder.Append("SELECT Unidade.*")
                                          .Append("FROM Unidade")
                                          .Append("WHERE Unidade.Hierarquia LIKE @0", unidade.GetFullLevelHierarquia() + "%")
                                          .Append("ORDER BY Unidade.Nome");

            return this.Db.Fetch<Unidade>(sql).ToList();
        }

        public Unidade Fetch(int id) {
            var sql = PetaPoco.Sql.Builder.Append("SELECT *")
                                          .Append("FROM Unidade")
                                          .Append("WHERE Unidade.Id = @0", id);
            
            return this.Db.SingleOrDefault<Unidade>(sql);
        }

        public Unidade FetchByUsuario(int usuarioId)
        {
            var sql = PetaPoco.Sql.Builder.Append("SELECT Unidade.*")
                                          .Append("FROM Unidade")
                                          .Append("INNER JOIN Usuario ON Usuario.UnidadeId = Unidade.Id")
                                          .Append("WHERE Usuario.Id = @0", usuarioId);

            return this.Db.SingleOrDefault<Unidade>(sql);
        }

        public bool IsUnidadeFilha(Unidade pai, Unidade filha) {
            return this.Db.ExecuteScalar<bool>("SELECT COUNT(*) FROM Unidade WHERE Hierarquia LIKE @0 AND Unidade.Id = @1", pai.GetFullLevelHierarquia() + '%', filha.Id);
        }

        public List<Unidade> FetchCentrais()
        {
            var sql = PetaPoco.Sql.Builder.Append("SELECT Unidade.*")
                                          .Append("FROM Unidade")
                                          .Append("WHERE Unidade.Tipo = @0", UnidadeTipo.CENTRAL);

            return this.Db.Fetch<Unidade>(sql);
        }

        public List<Arquivo> FetchArquivos(Unidade unidade) {
            var sql = PetaPoco.Sql.Builder.Append("SELECT Arquivo.*")
                                          .Append("FROM Arquivo")
                                          .Append("INNER JOIN UnidadeAnexo ON UnidadeAnexo.AnexoId = Arquivo.Id")
                                          .Append("WHERE UnidadeAnexo.UnidadeId = @0", unidade.Id);

            return this.Db.Fetch<Arquivo>(sql).ToList();
        }

        public void UpdateAnexos(Unidade unidade)
        {
            if (unidade.Anexos == null) return;

            this.Db.Execute("DELETE FROM UnidadeAnexo WHERE UnidadeId = @0", unidade.Id);

            foreach (var anexo in unidade.Anexos)
            {
                this.Db.Insert("UnidadeAnexo", "Id", new
                {
                    UnidadeId = unidade.Id,
                    AnexoId = anexo.Id
                });
            }
        }

        public void Excluir(Unidade unidade) {
            var zap = this.FetchZapUnidade();
            var fullHierarquia = unidade.GetFullLevelHierarquia() + '%';

            //deleta a undidade
            this.Db.Delete(unidade);

            //deleta os arquivos da unidade
            this.Db.Execute("DELETE UnidadeAnexo, Arquivo FROM UnidadeAnexo INNER JOIN Arquivo ON Arquivo.Id = UnidadeAnexo.AnexoId WHERE UnidadeAnexo.UnidadeId = @0", unidade.Id);

            //deleta todos os usuarios da unidade
            this.Db.Execute("DELETE FROM Usuario WHERE Usuario.UnidadeId = @0", unidade.Id);

            //deleta todas as unidades vinculadas
            this.Db.Execute("DELETE FROM Unidade WHERE Hierarquia LIKE @0", fullHierarquia);

            //deleta todos os arquivos das unidades vinculadas
            this.Db.Execute(PetaPoco.Sql.Builder.Append("DELETE UnidadeAnexo, Arquivo")
                                                .Append("FROM UnidadeAnexo")
                                                .Append("INNER JOIN Arquivo ON Arquivo.Id = UnidadeAnexo.AnexoId")
                                                .Append("INNER JOIN Unidade ON Unidade.Id = UnidadeAnexo.UnidadeId")
                                                .Append("WHERE Unidade.Hierarquia LIKE @0", fullHierarquia));

            //deleta todos os usuarios das unidades vinculadas
            this.Db.Execute(PetaPoco.Sql.Builder.Append("DELETE Usuario")
                                                .Append("FROM Usuario")
                                                .Append("INNER JOIN Unidade ON Unidade.Id = Usuario.UnidadeId")
                                                .Append("WHERE Unidade.Hierarquia LIKE @0", fullHierarquia));

            
            
        }
    }
}