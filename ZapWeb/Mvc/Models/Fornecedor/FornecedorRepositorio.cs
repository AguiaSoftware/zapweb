using Pillar.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZapWeb.Lib.Mvc;

namespace ZapWeb.Models
{
    public class FornecedorRepositorio : RepositorioTemp
    {
        public void Add(Fornecedor fornecedor) {
            this.Db.Insert(fornecedor);

            if (fornecedor.Endereco != null) {
                var enderecoRepositorio = new EnderecoRepositorio();
                enderecoRepositorio.Insert(fornecedor.Endereco);

                this.Db.Insert("FornecedorEndereco", "Id", new
                {
                    FornecedorId = fornecedor.Id,
                    EnderecoId = fornecedor.Endereco.Id
                });
            }

            if (fornecedor.Contato != null) {
                var contatoRepositorio = new ContatoRepositorio();
                contatoRepositorio.Insert(fornecedor.Contato);

                this.Db.Insert("FornecedorContato", "Id", new
                {
                    FornecedorId = fornecedor.Id,
                    ContatoId = fornecedor.Contato.Id
                });
            }
        }

        public void Update(Fornecedor fornecedor)
        {
            this.Db.Update(fornecedor);

            if (fornecedor.Endereco != null)
            {
                var enderecoRepositorio = new EnderecoRepositorio();
                enderecoRepositorio.Update(fornecedor.Endereco);
            }

            if (fornecedor.Contato != null)
            {
                var contatoRepositorio = new ContatoRepositorio();
                contatoRepositorio.Update(fornecedor.Contato);
            }
        }

        public Fornecedor Fetch(int Id) {
            var sql = PetaPoco.Sql.Builder.Append("SELECT Fornecedor.*")
                                          .Append("FROM Fornecedor")
                                          .Append("WHERE Fornecedor.Id = @0", Id);

            var fornecedor = this.Db.SingleOrDefault<Fornecedor>(sql);
            var enderecoId = this.Db.ExecuteScalar<int>("SELECT EnderecoId FROM FornecedorEndereco WHERE FornecedorId = @0", Id);
            var contatoId = this.Db.ExecuteScalar<int>("SELECT ContatoId FROM FornecedorContato WHERE FornecedorId = @0", Id);

            var enderecoRepositorio = new EnderecoRepositorio();
            fornecedor.Endereco = enderecoRepositorio.Fetch(enderecoId);

            var contatoRepositorio = new ContatoRepositorio();
            fornecedor.Contato = contatoRepositorio.Fetch(contatoId);
            
            return fornecedor;
        }

        public bool ExisteNome(Fornecedor fornecedor) {
            return this.Db.ExecuteScalar<int>("SELECT COUNT(*) FROM Fornecedor WHERE (RazaoSocial = @0 OR Fantasia = @0 OR RazaoSocial = @1 OR Fantasia = @1) AND Id != @2", fornecedor.RazaoSocial, fornecedor.Fantasia, fornecedor.Id) > 0 ? true : false;
        }

        public List<Fornecedor> Search(string nome)
        {
            var sql = PetaPoco.Sql.Builder.Append("SELECT Fornecedor.*, Endereco.*, Cidade.*")
                                          .Append("FROM Fornecedor")
                                          .Append("JOIN FornecedorEndereco ON FornecedorEndereco.FornecedorId = Fornecedor.Id")
                                          .Append("JOIN Endereco ON Endereco.Id = FornecedorEndereco.EnderecoId")
                                          .Append("LEFT JOIN Cidade ON Cidade.Id = Endereco.CidadeId")
                                          .Append("WHERE RazaoSocial LIKE @0 OR Fantasia LIKE @0", "%" + nome + "%")
                                          .Append("ORDER BY Fornecedor.Fantasia");

            return this.Db.Fetch<Fornecedor, Endereco, Cidade, Fornecedor>((f, e, c) =>
            {
                e.Cidade = c;
                f.Endereco = e;

                return f;
            }, sql).ToList();
        }
        public List<Fornecedor> FetchAll(FornecedorPesquisa parametros, Paging paging)
        {
            var sql = PetaPoco.Sql.Builder.Append("SELECT SQL_CALC_FOUND_ROWS Fornecedor.*, Endereco.*, Cidade.*")
                                          .Append("FROM Fornecedor")
                                          .Append("JOIN FornecedorEndereco ON FornecedorEndereco.FornecedorId = Fornecedor.Id")
                                          .Append("JOIN Endereco ON Endereco.Id = FornecedorEndereco.EnderecoId")
                                          .Append("LEFT JOIN Cidade ON Cidade.Id = Endereco.CidadeId")
                                          .Append("WHERE 1 = 1");

            if (parametros.RazaoSocial != null) {
                sql.Append("AND Fornecedor.RazaoSocial LIKE @0", '%' + parametros.RazaoSocial + '%');
            }

            if (parametros.Fantasia != null)
            {
                sql.Append("AND Fornecedor.Fantasia LIKE @0", '%' + parametros.Fantasia + '%');
            }

            if (parametros.Cnpj != null)
            {
                sql.Append("AND Fornecedor.Cnpj LIKE @0", '%' + parametros.Cnpj + '%');
            }

            sql.Append("ORDER BY Fornecedor.Fantasia");
            sql.Append("LIMIT @0, @1", paging.LimitDown, paging.LimitUp);
            
            this.Db.BeginTransaction();

            var fornecedores = this.Db.Fetch<Fornecedor, Endereco, Cidade, Fornecedor>((f, e, c) => {
                e.Cidade = c;
                f.Endereco = e;

                return f;
            }, sql);

            paging.total = this.Db.ExecuteScalar<int>("SELECT FOUND_ROWS()");

            this.Db.CompleteTransaction();

            return fornecedores.ToList();
        }
    }
}