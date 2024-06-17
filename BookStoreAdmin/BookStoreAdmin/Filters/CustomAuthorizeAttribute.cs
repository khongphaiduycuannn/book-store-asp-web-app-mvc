using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStoreAdmin.Filters
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.Request.IsAuthenticated)
            {
                filterContext.Result = new RedirectResult("~/accounts/Login");
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var sessionUsername = filterContext.HttpContext.Session["Username"];
            if (sessionUsername == null)
            {
                filterContext.Result = new RedirectResult("~/accounts/Login");
            }
            else
            {
                base.OnAuthorization(filterContext);
            }
        }
    }
}