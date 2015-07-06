using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Security;

namespace MvcApplication.Infrastructure
{
    public class CustomAuthentication
    {
        private string cookieName = ".ASPXAUTH";
        public HttpContext HttpContext { get; set; }
        //HttpContext context = HttpContext.Current;

        //public User Login(string userName, string Password, bool isPersistent)
        //{
        //    if (retUser != null)
        //    {
        //        CreateCookie(userName, isPersistent);
        //    }
        //    return retUser;
        //}

        //public User Login(string userName)
        //{
        //    if (retUser != null)
        //    {
        //        CreateCookie(userName);
        //    }
        //    return retUser;
        //}

        private void CreateCookie(string userName, bool isPersistent = false)
        {
            var ticket = new FormsAuthenticationTicket(
                  1,
                  userName,
                  DateTime.Now,
                  DateTime.Now.Add(FormsAuthentication.Timeout),
                  isPersistent,
                  string.Empty,
                  FormsAuthentication.FormsCookiePath);

            // Encrypt the ticket.
            var encTicket = FormsAuthentication.Encrypt(ticket);

            // Create the cookie.
            var AuthCookie = new HttpCookie(cookieName)
            {
                Value = encTicket,
                Expires = DateTime.Now.Add(FormsAuthentication.Timeout)
            };
            HttpContext.Response.Cookies.Set(AuthCookie);
        }

        public void LogOut()
        {
            var httpCookie = HttpContext.Response.Cookies[cookieName];
            if (httpCookie != null)
            {
                httpCookie.Value = string.Empty;
            }
        }
    }
}