using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LostandFound.Tools
{
    public class Tools
    {
        #region Create Cookie
        public HttpCookie CreateCookie(string Name,string Value)
        {
            HttpCookie aCookie = new HttpCookie(Name);
            aCookie.Value = Value;
            aCookie.Expires = DateTime.Now.AddDays(1);
            return aCookie;
        }
        #endregion

        #region Delete Cookie
        public HttpCookie DeleteCookie(string Name)
        {
            HttpCookie aCookie = new HttpCookie(Name);
            aCookie.Expires = DateTime.Now.AddDays(-1);
            return aCookie;
        }
        #endregion
    }
}