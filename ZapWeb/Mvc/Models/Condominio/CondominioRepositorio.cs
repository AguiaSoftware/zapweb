using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZapWeb.Models
{
    public class CondominioRepositorio : ZapWeb.Lib.Mvc.RepositorioTemp
    {
        
        public Condominio Insert(Condominio condominio)
        {

            if (condominio.Administradora != null)
            {
                condominio.AdministradoraId = condominio.Administradora.Id;
            }

            if (condominio.Sindico != null)
            {
                condominio.SindicoId = condominio.Sindico.Id;
            }

            if (condominio.Zelador != null)
            {
                condominio.ZeladorId = condominio.Zelador.Id;
            }

            if (condominio.Endereco != null)
            {
                condominio.EnderecoId = condominio.Endereco.Id;
            }

            if (condominio.Unidade != null)
            {
                condominio.UnidadeId = condominio.Unidade.Id;
            }

            this.Db.Insert(condominio);

            return condominio;
        }

        public void Update(Condominio condominio)
        {

            if (condominio.Administradora != null)
            {
                condominio.AdministradoraId = condominio.Administradora.Id;
            }

            if (condominio.Sindico != null)
            {
                condominio.SindicoId = condominio.Sindico.Id;
            }

            if (condominio.Zelador != null)
            {
                condominio.ZeladorId = condominio.Zelador.Id;
            }

            if (condominio.Endereco != null)
            {
                condominio.EnderecoId = condominio.Endereco.Id;
            }

            if (condominio.Unidade != null)
            {
                condominio.UnidadeId = condominio.Unidade.Id;
            }

            this.Db.Update(condominio);
        }

        public void UpdateRank(Condominio condominio)
        {
            this.Db.Update("Condominio", "Id", new
            {
                Id = condominio.Id,
                Rank = condominio.Rank
            });
        }

        public Condominio Simple(int Id)
        {
            var sql = PetaPoco.Sql.Builder.Append("SELECT *")
                                          .Append("FROM Condominio")
                                          .Append("WHERE Condominio.Id = @0", Id);

            return this.Db.SingleOrDefault<Condominio>(sql);
        }

        public List<Condominio> Search(CondominioSearch param, Unidade unidade)
        {
            var sql = PetaPoco.Sql.Builder.Append("SELECT Condominio.*, Endereco.*, Cidade.*, Unidade.*")
                                          .Append("FROM Condominio")
                                          .Append("INNER JOIN Endereco ON Endereco.Id = Condominio.EnderecoId")
                                          .Append("LEFT JOIN Cidade ON Cidade.Id = Endereco.CidadeId")
                                          .Append("INNER JOIN Unidade ON Unidade.Id = Condominio.UnidadeId")
                                          .Append("AND (Unidade.Hierarquia LIKE @0 OR Unidade.Id = @1)", unidade.GetFullLevelHierarquia() + "%", unidade.Id);

            if (param.Rank > 0)
            {
                sql.Where("Condominio.Rank = @0", param.Rank);
            }           
            
            if(param.Nome != null && param.Nome.Length > 0)
            {
                sql.Where("Condominio.Nome LIKE @0", "%" + param.Nome + "%");
            }

            if (param.Administradora != null)
            {
                sql.Where("Condominio.AdministradoraId = @0", param.Administradora.Id);
            }

            if (param.Unidade != null)
            {
                sql.Where("Condominio.UnidadeId = @0", param.Unidade.Id);
            }

            if (param.Endereco != null)
            {

                if (param.Endereco.Cep != null && param.Endereco.Cep.Length > 0)
                {
                    sql.Where("Endereco.Cep LIKE @0", "%" + param.Endereco.Cep + "%");
                }

                if (param.Endereco.Numero != null && param.Endereco.Numero.Length > 0)
                {
                    sql.Where("Endereco.Numero LIKE @0", "%" + param.Endereco.Numero + "%");
                }

                if (param.Endereco.Rua!=null && param.Endereco.Rua.Length > 0)
                {
                    sql.Where("Endereco.Rua LIKE @0", "%" + param.Endereco.Rua + "%");
                }

                if (param.Endereco.Bairro != null && param.Endereco.Bairro.Length > 0)
                {
                    sql.Where("Endereco.Bairro LIKE @0", "%" + param.Endereco.Bairro + "%");
                }

                if (param.Endereco.Cidade != null)
                {
                    sql.Where("Endereco.CidadeId = @0", param.Endereco.Cidade.Id);
                }
            }

            return this.Db.Fetch<Condominio, Endereco, Cidade, Unidade, Condominio>((c, e, cd, u)=> {

                c.Endereco = e;
                c.Endereco.Cidade = cd;
                c.Unidade = u;

                return c;
            }, sql);
        }

    }
}