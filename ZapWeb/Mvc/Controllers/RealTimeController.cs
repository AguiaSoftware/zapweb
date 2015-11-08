using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ZapWeb.Models;

namespace ZapWeb.Mvc.Controllers
{
    public class RealTimeController : ZapWeb.Lib.Mvc.Controller
    {
        public void Set(string connectionId) {
            var realTimeRepositorio = new RealTimeRepositorio();
            var realtime = realTimeRepositorio.Fetch(Account.Current.Presence);
            if (realtime == null)
            {
                realtime = new RealTime() { 
                    ConnectionId=connectionId,
                    SessionId=Account.Current.Presence                    
                };

                realTimeRepositorio.Add(realtime);
            }
            else {
                realtime.ConnectionId = connectionId;
                realTimeRepositorio.Update(realtime);
            }
        }

    }
}
