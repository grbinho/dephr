using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Service1.Areas.Monitoring.Controllers
{
    public class HealthController : Controller
    {
        // GET: Monitoring/Health
        public ActionResult Index()
        {
            return View();
        }
    }
}