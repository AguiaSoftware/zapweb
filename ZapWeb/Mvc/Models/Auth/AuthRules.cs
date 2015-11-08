using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using ZapWeb.Lib.Mvc;

namespace ZapWeb.Models
{
    public class AuthRules : BusinessLogic
    {

        public bool Logar(Account account) {
            var accountRepositorio = new AccountRepositorio();
            var accountCurrent = accountRepositorio.Fetch(account.Username);

            //usuario nao existe
            if (accountCurrent == null)
            {
                this.MessageError = "USUARIO_SENHA_INCORRETA";
                return false;
            }

            //senha errada
            if (accountCurrent.Password != account.Password)
            {
                this.MessageError = "USUARIO_SENHA_INCORRETA";
                return false;
            }

            //usuario cancelado
            if (!accountCurrent.Ativa) {
                this.MessageError = "USUARIO_CANCELADO";
                return false;
            }

            var sessionRepositorio = new SessionRepositorio();
            var session = new Session();
            session.Account = accountCurrent;

            sessionRepositorio.Add(session);

            FormsAuthentication.SetAuthCookie(session.Presence, true);

            return true;
        }

        public void Sair() {
            var sessionRepositorio = new SessionRepositorio();
            sessionRepositorio.Remove(Account.Current.Presence);

            FormsAuthentication.SignOut();
        }

    }
}