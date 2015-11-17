using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ZapWeb.Lib.Mvc;

namespace ZapWeb.Models
{
    public class FinanceiroItemRepositorio : RepositorioTemp
    {
        public void Add(List<FinanceiroItem> items) {

            foreach (var item in items)
            {
                if (item.CentroCusto != null)
                {
                    item.CentroCustoId = item.CentroCusto.Id;
                }

                this.Db.Insert(item);
            }
        }
    }
}