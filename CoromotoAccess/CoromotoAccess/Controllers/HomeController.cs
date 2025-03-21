﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CoromotoAccess.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        //cambiar idioma 7 marzo
        
        public ActionResult ChangeLanguage(string lang)
        {
            HttpCookie langCookie = new HttpCookie("Language", lang);
            langCookie.Expires = DateTime.Now.AddYears(1);
            Response.Cookies.Add(langCookie);

            return Redirect(Request.UrlReferrer.ToString());
        }
    }
}