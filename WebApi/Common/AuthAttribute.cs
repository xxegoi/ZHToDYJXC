using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace WebApi.Common
{
    public class AuthAttribute : AuthorizeAttribute
    {

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var token = ConfigurationManager.AppSettings["token"];

            var json = HttpContext.Current.Request.Params["token"] == null ? string.Empty : HttpContext.Current.Request.Params["token"];

            if (string.Empty != json)
            {
                return true;
            }

            return false;
        }
    }
}