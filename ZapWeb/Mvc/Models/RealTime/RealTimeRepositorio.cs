using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ZapWeb.Lib.Mvc;

namespace ZapWeb.Models
{
    public class RealTimeRepositorio : RepositorioTemp
    {
        public RealTime Fetch(string sessionId) {
            var sql = PetaPoco.Sql.Builder.Append("SELECT RealTime.*")
                                          .Append("FROM RealTime")
                                          .Append("WHERE SessionId = @0", sessionId)
                                          .Append("ORDER BY Id DESC")
                                          .Append("LIMIT 1");

            return this.Db.SingleOrDefault<RealTime>(sql);
        }

        public void Update(RealTime realtime) {
            this.Db.Update(realtime);
        }

        public void Add(RealTime realtime) {
            this.Db.Insert(realtime);
        }

    }
}