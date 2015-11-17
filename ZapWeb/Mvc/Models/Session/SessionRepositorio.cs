using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ZapWeb.Lib.Mvc;

namespace ZapWeb.Models
{
    public class SessionRepositorio : RepositorioTemp
    {

        public void Add(Session session) {
            session.Presence = this.UUID();

            session.AccountId = session.Account.Id;
            session.Data = DateTime.Now;

            this.Db.Insert(session);
        }

        public void Remove(string session) {
            var sql = PetaPoco.Sql.Builder.Append("DELETE FROM Session WHERE Presence = @0", session);
            this.Db.Execute(sql);
        }

        private string UUID()
        {
            return this.Db.ExecuteScalar<string>("SELECT UUID();");
        }

    }
}