using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using PetaPoco;

namespace ZapWeb.Lib.Mvc
{
    public class Repositorio<T>
    {
        public Database Db { get; set; }
        public string Message { get; set; }
        public int Code { get; set; }
        protected List<T> ResultSet;

        public Repositorio()
        {
            if (System.Web.HttpContext.Current.Items["ModelDataContext"] == null)
            {
                System.Web.HttpContext.Current.Items["ModelDataContext"] = new PetaPoco.Database(ZapWeb.Lib.DataBaseParam.ConnectionString, ZapWeb.Lib.DataBaseParam.Provider);
            }

            this.Db = (PetaPoco.Database)System.Web.HttpContext.Current.Items["ModelDataContext"];
        }

        public Repositorio(string connectionString, string providerName)
        {
            this.Db = new Database(connectionString, providerName);
        }

        public T Get()
        {
            if (ResultSet == null) return default(T);
            if (ResultSet.Count == 0) return default(T);

            return ResultSet[0];
        }

        public List<T> GetList()
        {
            return ResultSet;
        }
        
    }

    public class RepositorioTemp
    {
        public PetaPoco.Database Db { get; set; }

        public RepositorioTemp()
        {
            if (System.Web.HttpContext.Current.Items["ModelDataContext"] == null)
            {
                System.Web.HttpContext.Current.Items["ModelDataContext"] = new PetaPoco.Database(ZapWeb.Lib.DataBaseParam.ConnectionString, ZapWeb.Lib.DataBaseParam.Provider);
            }

            this.Db = (PetaPoco.Database)System.Web.HttpContext.Current.Items["ModelDataContext"];
        }
    }


}