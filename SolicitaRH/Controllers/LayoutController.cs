using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SolicitaRH.Controllers
{
    public class LayoutController : Controller
    {
        public ActionResult Index() {
            return View();
        }

        public PartialViewResult _Topo()
        {
            return PartialView();
        }
    }
}
