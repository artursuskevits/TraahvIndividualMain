using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace TraahvIndividual.Models
{


    public class AuthorizeUserAttribute : AuthorizeAttribute
    {
        private readonly string _username;

        public AuthorizeUserAttribute(string username)
        {
            _username = username;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.User.Identity.IsAuthenticated)
            {
                return httpContext.User.Identity.Name.Equals(_username, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
        }
    }

}