using ImageServiceWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace ImageServiceWeb.Controllers
{
    public class FirstController : Controller
    {
        static ImageWebModel imageWebModel = new ImageWebModel();
        // GET: First
        public ActionResult Index()
        {
            return View();
        }

        
        public ActionResult ImageWeb()
        {
            return View(imageWebModel);
        }
    }
}