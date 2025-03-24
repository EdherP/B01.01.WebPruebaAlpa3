using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;
using Web_ALPA.Models.Home;
using Web_ALPA.ViewModel;

namespace Web_ALPA.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public ActionResult Login()
        {
            System.Web.HttpContext.Current.Response.AddHeader("Cache-Control", "no-cache, no-store, must-revalidate");
            System.Web.HttpContext.Current.Response.AddHeader("Pragma", "no-cache");
            System.Web.HttpContext.Current.Response.AddHeader("Expires", "0");
            LoginViewModel model = new LoginViewModel()
            {
                Mensaje = "",
                Password = "",
                UserName = ""
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            model = (new LoginModel().SetLogin(model));
            if (model.Mensaje == string.Empty)
            {
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetExpires(DateTime.Now.AddDays(-1));
                Response.Cache.SetNoStore();
                Session["Login"] = model;
                return RedirectToAction("Inicio", "Home");
            }
            ViewBag.oAlert = model.Mensaje;
            return View(model);
        }

        [HttpGet]
        //[ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {           
            //Session.RemoveAll();
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();

            //// clear authentication cookie
            //HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "")
            //{
            //    Expires = DateTime.Now.AddYears(-1)
            //};
            //Response.Cookies.Add(cookie1);

            //// clear session cookie (not necessary for your current problem but i would recommend you do it anyway)
            //SessionStateSection sessionStateSection = (SessionStateSection)WebConfigurationManager.GetSection("system.web/sessionState");
            //HttpCookie cookie2 = new HttpCookie(sessionStateSection.CookieName, "")
            //{
            //    Expires = DateTime.Now.AddYears(-1)
            //};
            //Response.Cookies.Add(cookie2);

            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();

            return RedirectToAction("Login", "Account");
            //if (Request.IsAuthenticated)
            //{
            //    FormsAuthentication.SignOut();

            //    return Redirect("~/");
            //}
            //return View();

        }
    }
}