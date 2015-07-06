using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;

namespace MvcApplication.Infrastructure
{
    public class CustomAuthenticationModule : IHttpModule
    {

        public void Init(HttpApplication context)
        {
            context.AuthenticateRequest += new EventHandler(this.OnAuthenticate);
        }
        public void Dispose()
        {
        }

        void OnAuthenticate(object sender, EventArgs e)
        {
            HttpApplication app = (HttpApplication)sender;
            HttpContext context = app.Context;
            HttpRequest request = app.Request;
            HttpResponse response = app.Response;
            HttpCookie cookie = null;
            string cookieName = ".ASPXAUTH";

            if (context.User != null)
            {
                return;
            }

            cookie = request.Cookies[cookieName];

            if (null == cookie)
                return;

            if (null == cookie.Value || cookie.Value.Length < 1 )
            {
                context.Request.Cookies.Remove(cookieName);
                return;
            }

            FormsAuthenticationTicket authTicket = null;
            try
            {
                authTicket = FormsAuthentication.Decrypt(cookie.Value);
            }
            catch (ArgumentException)
            {
                context.Request.Cookies.Remove(cookieName);
                return;
                //throw;
            }

            if (authTicket == null)
                return;

            if (authTicket != null && authTicket.Expired)
            {
                context.Request.Cookies.Remove(cookieName);
                return;
            }

            FormsAuthenticationTicket authTicket2 = authTicket;

            if (FormsAuthentication.SlidingExpiration)
                authTicket2 = FormsAuthentication.RenewTicketIfOld(authTicket);

            CustomIdentity userIdentity = new CustomIdentity(authTicket2.Name);
            CustomPrincipal principal = new CustomPrincipal(userIdentity);
            app.Context.User = principal;
            Thread.CurrentPrincipal = principal;

            if (authTicket2 != authTicket)
            {
                String strEnc = FormsAuthentication.Encrypt(authTicket2);
                if (cookie != null)
                    cookie = context.Request.Cookies[FormsAuthentication.FormsCookieName];

                if (cookie == null)
                {
                    cookie = new HttpCookie(FormsAuthentication.FormsCookieName, strEnc);
                    cookie.Path = authTicket2.CookiePath;
                }
                if (authTicket2.IsPersistent)
                    cookie.Expires = authTicket2.Expiration;
                cookie.Value = strEnc;
                if (FormsAuthentication.CookieDomain != null)
                    cookie.Domain = FormsAuthentication.CookieDomain;
                cookie.HttpOnly = true;
                context.Response.Cookies.Remove(cookie.Name);
                context.Response.Cookies.Add(cookie);
            }
        }
    }
}
