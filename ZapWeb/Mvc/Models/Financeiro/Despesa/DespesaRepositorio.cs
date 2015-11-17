using Pillar.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZapWeb.Lib.Mvc;

namespace ZapWeb.Models
{
    public class DespesaRepositorio : RepositorioTemp
    {
        public void Add(Despesa despesa) {

            despesa.FornecedorId = despesa.Fornecedor.Id;
            despesa.UnidadeId = despesa.Unidade.Id;
            despesa.UsuarioId = despesa.Usuario.Id;

            this.Db.Insert(despesa);
        }

        public void UpdateItems(Despesa despesa) {

            this.UpdateTotal(despesa);

            this.Db.Execute("DELETE DespesaItem, FinanceiroItem FROM DespesaItem INNER JOIN FinanceiroItem ON FinanceiroItem.Id = DespesaItem.ItemId WHERE DespesaItem.DespesaId = @0", despesa.Id);

            var financeiroItemRepositorio = new FinanceiroItemRepositorio();
            financeiroItemRepositorio.Add(despesa.Items);

            foreach (var item in despesa.Items)
            {
                this.Db.Insert("DespesaItem", "Id", new
                {
                    DespesaId = despesa.Id,
                    ItemId = item.Id
                });
            }
        }

        public void UpdateAnexos(Despesa despesa) {
            this.Db.Execute("DELETE FROM DespesaAnexo WHERE DespesaId = @0", despesa.Id);

            foreach (var anexo in despesa.Anexos)
            {
                this.Db.Insert("DespesaAnexo", "Id", new
                {
                    DespesaId = despesa.Id,
                    AnexoId = anexo.Id
                });
            }
        }

        public void Update(Despesa despesa)
        {
            despesa.FornecedorId = despesa.Fornecedor.Id;
            despesa.UnidadeId = despesa.Unidade.Id;
            despesa.UsuarioId = despesa.Usuario.Id;

            this.Db.Update(despesa);
        }

        public void UpdateTotal(Despesa despesa)
        { 
            despesa.Total = 0;

            for (int i = 0; i < despesa.Items.Count; i++)
            {
                despesa.Total += despesa.Items[i].Qtde * despesa.Items[i].Valor;
            }

            this.Db.Update("Despesa", "Id", new
            {
                Total = despesa.Total,
                Id = despesa.Id
            }); 
        }

        public void Delete(Despesa despesa)
        {
            this.Db.Execute("DELETE FROM Despesa WHERE Despesa.Id = @0", despesa.Id);
            this.Db.Execute("DELETE FROM DespesaAnexo WHERE DespesaAnexo.DespesaId = @0", despesa.Id);
            this.Db.Execute("DELETE FROM DespesaHistorico WHERE DespesaHistorico.DespesaId = @0", despesa.Id);

            this.Db.Execute(PetaPoco.Sql.Builder.Append("DELETE DespesaItem, FinanceiroItem")
                                                .Append("FROM DespesaItem")
                                                .Append("INNER JOIN FinanceiroItem ON FinanceiroItem.Id = DespesaItem.ItemId")
                                                .Append("WHERE DespesaItem.DespesaId = @0", despesa.Id));
        }

        public Despesa Fetch(int Id) {
            var sql = PetaPoco.Sql.Builder.Append("SELECT Despesa.*, Fornecedor.*, Unidade.*, Usuario.*")
                                          .Append("FROM Despesa")
                                          .Append("INNER JOIN Fornecedor ON Fornecedor.Id = Despesa.FornecedorId")
                                          .Append("INNER JOIN Unidade ON Unidade.Id = Despesa.UnidadeId")
                                          .Append("INNER JOIN Usuario ON Usuario.Id = Despesa.UsuarioId")
                                          .Append("WHERE Despesa.Id = @0", Id);

            var despesas = this.Db.Fetch<Despesa, Fornecedor, Unidade, Usuario, Despesa>((d, f, un, us) =>
            {

                d.Fornecedor = f;
                d.Unidade = un;
                d.Usuario = us;

                return d;
            }, sql).ToList();

            if (despesas == null) return null;

            var despesa = despesas[0];

            sql = PetaPoco.Sql.Builder.Append("SELECT Arquivo.*")
                                      .Append("FROM Arquivo")
                                      .Append("INNER JOIN DespesaAnexo ON DespesaAnexo.AnexoId = Arquivo.Id")
                                      .Append("WHERE DespesaAnexo.DespesaId = @0", Id);

            despesa.Anexos = this.Db.Fetch<Arquivo>(sql).ToList();

            sql = PetaPoco.Sql.Builder.Append("SELECT FinanceiroItem.*, CentroCusto.*")
                                      .Append("FROM FinanceiroItem")
                                      .Append("INNER JOIN DespesaItem ON DespesaItem.ItemId = FinanceiroItem.Id")
                                      .Append("INNER JOIN CentroCusto ON CentroCusto.Id = FinanceiroItem.CentroCustoId")
                                      .Append("WHERE DespesaItem.DespesaId = @0", Id);

            despesa.Items = this.Db.Fetch<FinanceiroItem, CentroCusto, FinanceiroItem>((f, c) => {
                f.CentroCusto = c;
                return f;
            }, sql).ToList();

            var despesaHistoricoRepositorio = new DespesaHistoricoRepositorio();
            despesa.Historicos = despesaHistoricoRepositorio.Fetch(despesa);

            return despesa;
        }

        public List<Despesa> Fetch(DespesaPesquisa parametro, Unidade unidade, Paging paging) {
            var sql = PetaPoco.Sql.Builder.Append("SELECT SQL_CALC_FOUND_ROWS Despesa.*, Fornecedor.*, Usuario.*, Unidade.*")
                                          .Append("FROM Despesa")
                                          .Append("INNER JOIN Fornecedor ON Fornecedor.Id = Despesa.FornecedorId")
                                          .Append("INNER JOIN Usuario ON Usuario.Id = Despesa.UsuarioId")
                                          .Append("INNER JOIN Unidade ON Unidade.Id = Despesa.UnidadeId")
                                          .Append("WHERE (Unidade.Hierarquia LIKE @0 OR Unidade.Id = @1)", unidade.GetFullLevelHierarquia() + "%", unidade.Id);

            //5 TODOS
            if (parametro.Status != 5) {
                sql.Append(" AND Despesa.Status = @0", parametro.Status);
            }

            if (parametro.Usuario != null) {
                sql.Append(" AND Despesa.UsuarioId = @0", parametro.Usuario.Id);
            }

            if (parametro.Unidade != null) {
                sql.Append(" AND Despesa.UnidadeId = @0", parametro.Unidade.Id);
            }

            if (parametro.Fornecedor != null) {
                sql.Append(" AND Despesa.FornecedorId = @0", parametro.Fornecedor.Id);
            }

            if (parametro.Numero != null)
            {
                sql.Append(" AND Despesa.Numero = @0", parametro.Numero);
            }

            if (parametro.ValorMenor > 0 && parametro.ValorMaior > 0) {
                sql.Append(" AND Despesa.Total >= @0 AND Despesa.Total <= @1", parametro.ValorMenor, parametro.ValorMaior);
            } else if (parametro.ValorMenor > 0){
                sql.Append(" AND Despesa.Total >= @0", parametro.ValorMenor);
            } else if (parametro.ValorMaior > 0){
                sql.Append(" AND Despesa.Total <= @0", parametro.ValorMaior);
            }

            var dataNull = new DateTime(1, 1, 1, 0, 0, 0);
            if (parametro.DataInicio > dataNull && parametro.DataFim > dataNull) {
                sql.Append(" AND Despesa.Data >= @0 AND Despesa.Data <= @1", parametro.DataInicio, parametro.DataFim);
            } else if (parametro.DataInicio > dataNull)
            {
                sql.Append(" AND Despesa.Data >= @0", parametro.DataInicio);
            } else if (parametro.DataFim > dataNull)
            {
                sql.Append(" AND Despesa.Data <= @0", parametro.DataFim);
            }

            sql.Append("ORDER BY Despesa.Data DESC");
            sql.Append("LIMIT @0, @1", paging.LimitDown, paging.LimitUp);

            this.Db.BeginTransaction();

            var despesas = this.Db.Fetch<Despesa, Fornecedor, Usuario, Unidade, Despesa>((d, f, us, un) =>
            {

                d.Fornecedor = f;
                d.Usuario = us;
                d.Unidade = un;

                return d;
            }, sql);

            paging.total = this.Db.ExecuteScalar<int>("SELECT FOUND_ROWS()");

            this.Db.CompleteTransaction();

            return despesas;
        }
    }
}