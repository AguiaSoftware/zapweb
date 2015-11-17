using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZapWeb.Models
{
    public class MaterialRepositorio : ZapWeb.Lib.Mvc.Repositorio
    {

        public Material Insert(Material material)
        {

            if (material.Usuario != null)
            {
                material.UsuarioId = material.Usuario.Id;
            }

            if (material.Condominio != null)
            {
                material.CondominioId = material.Condominio.Id;
            }

            material.Data = DateTime.Now;

            this.Db.Insert(material);

            this.InsertArquivos(material);

            return material;
        }

        public void Update(Material material)
        {
            this.Db.Update(material);

            this.InsertArquivos(material);
        }
        
        public void InsertArquivos(Material material)
        {
            if (material.Anexos == null) return;

            this.Db.Execute("DELETE FROM MaterialAnexo WHERE MaterialAnexo.MaterialId = @0", material.Id);

            foreach (var anexo in material.Anexos)
            {
                this.Db.Insert("MaterialAnexo", "Id", new {
                    MaterialId = material.Id,
                    AnexoId = anexo.Id
                });
            }
        }

        private List<Arquivo> FetchAnexos(int materialId)
        {
            var sql = PetaPoco.Sql.Builder.Append("SELECT Arquivo.*")
                                          .Append("FROM Arquivo")
                                          .Append("INNER JOIN MaterialAnexo ON MaterialAnexo.AnexoId = Arquivo.Id")
                                          .Append("WHERE MaterialAnexo.MaterialId = @0", materialId);

            return this.Db.Fetch<Arquivo>(sql);
        }

        public List<Material> FetchByCondominioId(int condominioId)
        {
            var sql = PetaPoco.Sql.Builder.Append("SELECT *")
                                          .Append("FROM Material")
                                          .Append("WHERE Material.CondominioId = @0", condominioId);

            var materiais = this.Db.Fetch<Material>(sql);

            foreach (var material in materiais)
            {
                material.Anexos = this.FetchAnexos(material.Id);
            }

            return materiais;
        }

        public Material Fetch(int Id)
        {
            var sql = PetaPoco.Sql.Builder.Append("SELECT *")
                                          .Append("FROM Material")
                                          .Append("WHERE Material.Id = @0", Id);

            var materiais = this.Db.Fetch<Material>(sql);

            foreach (var material in materiais)
            {
                material.Anexos = this.FetchAnexos(material.Id);
            }

            return materiais[0];
        }

        public Material Simple(int id)
        {
            var sql = PetaPoco.Sql.Builder.Append("SELECT *")
                                          .Append("FROM Material")
                                          .Append("WHERE Material.Id = @0", id);

            return this.Db.SingleOrDefault<Material>(sql);
        }

    }
}