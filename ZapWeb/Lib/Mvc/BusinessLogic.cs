using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZapWeb.Lib.Mvc
{
    public class BusinessLogic
    {
        private string _MessageError;
        public string MessageError
        {
            get
            {
                return _MessageError;
            }
            set
            {
                _MessageError = Pillar.Mvc.Application.Language.Get(value);
            }
        }
    }
}