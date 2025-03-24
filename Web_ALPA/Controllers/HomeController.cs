using Neodynamic.SDK.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_ALPA.Models.Home;
using Web_ALPA.ViewModel;

namespace Web_ALPA.Controllers
{
    public class HomeController : Controller
    {
        private readonly LoginModel objMenux;
        public HomeController()
        {
            objMenux = new LoginModel();
        }
        // GET: Home
        public ActionResult Inicio()
        {
            if ((LoginViewModel)Session["Login"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            System.Web.HttpContext.Current.Response.AddHeader("Cache-Control", "no-cache, no-store, must-revalidate");
            System.Web.HttpContext.Current.Response.AddHeader("Pragma", "no-cache");
            System.Web.HttpContext.Current.Response.AddHeader("Expires", "0");
            ViewBag.SubMenu = "Inicio";
            ViewBag.Menu = "Bienvenido";
            return View("Inicio");

        }

        public ActionResult AnotherLink()
        {
            return View("Inicio");
        }

        [ChildActionOnly]
        public ActionResult LoadMenu()
        {
            if ((LoginViewModel)Session["Login"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                var ListLoadMenuTemp = objMenux.ListarLoad_Menu();
                return PartialView(ListLoadMenuTemp.ToList());
            }
        }
    }
}