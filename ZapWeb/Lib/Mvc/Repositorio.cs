using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZapWeb.Lib.Mvc
{
    public class Repositorio
    {
        public PetaPoco.Database Db { get; set; }

        public Repositorio()
        {
            if (System.Web.HttpContext.Current.Items["ModelDataContext"] == null)
            {
                System.Web.HttpContext.Current.Items["ModelDataContext"] = new PetaPoco.Database(ZapWeb.Lib.DataBaseParam.ConnectionString, ZapWeb.Lib.DataBaseParam.Provider);
            }

            this.Db = (PetaPoco.Database)System.Web.HttpContext.Current.Items["ModelDataContext"];
        }
    }


}