using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ZapWeb.Lib.Mvc;

namespace ZapWeb.Models
{
    public class AccountRepositorio : Repositorio
    {
        public void Add(Account account) {
            account.Ativa = true;
            account.UsuarioId = account.Usuario.Id;
            account.GrupoPermissaoId = account.Permissao.Id;
            account.Tipo = AccountType.DEFAULT;

            this.Db.Insert(account);
        }

        public void Update(Account account)
        {
            account.Ativa = true;
            account.GrupoPermissaoId = account.Permissao.Id;
            account.Tipo = AccountType.DEFAULT;

            this.Db.Update(account);
        }

        public bool ExistUsername(Account account)
        {
            return this.Db.ExecuteScalar<int>("SELECT COUNT(*) FROM Account WHERE Username = @0 AND Account.Id != @1", account.Username, account.Id) == 0 ? false : true;
        }

        public int TotalAccountByGrupoPermissao(GrupoPermissao grupo)
        {
            return this.Db.ExecuteScalar<int>("SELECT COUNT(*) FROM Account WHERE GrupoPermissaoId = @0", grupo.Id);
        }

        public Account Fetch(string username)
        {
            var sql = PetaPoco.Sql.Builder.Append("SELECT *")
                                          .Append("FROM Account")
                                          .Append("WHERE UserName = @0", username);

            return this.Db.SingleOrDefault<Account>(sql);
        }

        public Account FetchByUsuarioId(int Id)
        {
            var sql = PetaPoco.Sql.Builder.Append("SELECT Account.*, GrupoPermissao.*")
                                          .Append("FROM Account")
                                          .Append("INNER JOIN GrupoPermissao ON GrupoPermissao.Id = Account.GrupoPermissaoId")
                                          .Append("WHERE Account.UsuarioId = @0", Id);

            return this.Db.Fetch<Account, GrupoPermissao, Account>((a, g) => {
                
                a.Permissao = g;

                return a;
            }, sql).ToList()[0];
        }

        public List<Account> FetchByUnidadeId(int unidadeId) {
            var sql = PetaPoco.Sql.Builder.Append("SELECT Account.*, Session.*, Usuario.*")
                                          .Append("FROM Account")
                                          .Append("LEFT JOIN Session ON Session.AccountId = Account.Id")
                                          .Append("INNER JOIN Usuario ON Usuario.Id = Account.UsuarioId")
                                          .Append("INNER JOIN Unidade ON Unidade.Id = Usuario.UnidadeId")
                                          .Append("WHERE Unidade.Id = @0", unidadeId);

            List<Account> accounts = new List<Account>();

            this.Db.Fetch<Account, Session, Usuario, Account>((a, s, u) => {

                var _ac = accounts.Find(ac => ac.Id == a.Id);
                if (_ac == null) {
                    _ac = a;
                    _ac.Usuario = u;

                    _ac.Sessions = new List<Session>();

                    accounts.Add(_ac);
                }

                if (s != null)
                {
                    _ac.Sessions.Add(s);    
                }
                
                return a;
            }, sql).ToList();

            return accounts;
        }

        public Account FetchBySession(string session) {
            if (session == null) return null;
            if (session.Length == 0) return null;

            var sql = PetaPoco.Sql.Builder.Append("SELECT Account.*, Usuario.*")
                                          .Append("FROM Account")
                                          .Append("JOIN Session ON Session.AccountId = Account.Id")
                                          .Append("JOIN Usuario ON Usuario.Id = Account.UsuarioId")
                                          .Append("WHERE Session.Presence = @0", session);

            var account = this.Db.Fetch<Account, Usuario, Account>((a, u) => {
                
                a.Usuario = u;
                u.Unidade = new Unidade();
                u.Unidade.Id = u.UnidadeId;

                return a;
            }, sql).ToList()[0];

            if (account == null) return null;
            var grupoPermissaoRepositorio = new GrupoPermissaoRepositorio();
            account.Permissao = grupoPermissaoRepositorio.Fetch(account.GrupoPermissaoId);

            return account;
        }

    }
}