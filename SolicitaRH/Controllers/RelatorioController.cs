using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SolicitaRH.Controllers
{
    public class RelatorioController : Controller
    {
        public ActionResult Index(String inicial = "", String final = "")
        {
            return View();
        }

    }
}
