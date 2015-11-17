using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZapWeb.Models
{
    public class CampanhaRepositorio : ZapWeb.Lib.Mvc.Repositorio<Campanha>
    {

        public Campanha Insert(Campanha campanha)
        {

            if (campanha.Usuario != null)
            {
                campanha.UsuarioId = campanha.Usuario.Id;
            }

            if (campanha.Condominio != null)
            {
                campanha.CondominioId = campanha.Condominio.Id;
            }

            campanha.Data = DateTime.Now;

            this.Db.Insert(campanha);

            this.InsertArquivos(campanha);

            return campanha;
        }

        public void Update(Campanha campanha)
        {
            this.Db.Update(campanha);

            this.InsertArquivos(campanha);
        }

        public void Delete(Campanha campanha)
        {
            this.Db.Delete(campanha);

            this.Db.Execute("DELETE FROM CampanhaAnexo WHERE CampanhaAnexo.CampanhaId = @0", campanha.Id);
        }

        private List<Arquivo> FetchAnexos(int campanhaId)
        {
            var sql = PetaPoco.Sql.Builder.Append("SELECT Arquivo.*")
                                          .Append("FROM Arquivo")
                                          .Append("INNER JOIN CampanhaAnexo ON CampanhaAnexo.AnexoId = Arquivo.Id")
                                          .Append("WHERE CampanhaAnexo.CampanhaId = @0", campanhaId);

            return this.Db.Fetch<Arquivo>(sql);
        }

        public CampanhaRepositorio FetchByCondominioId(int condominioId)
        {
            var sql = PetaPoco.Sql.Builder.Append("SELECT *")
                                          .Append("FROM Campanha")
                                          .Append("WHERE Campanha.CondominioId = @0", condominioId);

            ResultSet = this.Db.Fetch<Campanha>(sql);
            
            return this;
        }
        
        public CampanhaRepositorio Simple(int id)
        {
            var sql = PetaPoco.Sql.Builder.Append("SELECT *")
                                          .Append("FROM Campanha")
                                          .Append("WHERE Campanha.Id = @0", id);

            ResultSet = this.Db.Fetch<Campanha>(sql);

            return this;
        }

        public CampanhaRepositorio IncludeAnexos()
        {
            foreach (var campanha in ResultSet)
            {
                campanha.Anexos = this.FetchAnexos(campanha.Id);
            }

            return this;
        }

        private void InsertArquivos(Campanha campanha)
        {
            if (campanha.Anexos == null) return;

            this.Db.Execute("DELETE FROM CampanhaAnexo WHERE CampanhaAnexo.CampanhaId = @0", campanha.Id);

            foreach (var anexo in campanha.Anexos)
            {
                this.Db.Insert("CampanhaAnexo", "Id", new
                {
                    CampanhaId = campanha.Id,
                    AnexoId = anexo.Id
                });
            }
        }

    }
}