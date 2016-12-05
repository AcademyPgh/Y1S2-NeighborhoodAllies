using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImpactMap.Models
{
    public class DashboardController : Controller
    {
        private ImpactMapDbContext db = new ImpactMapDbContext();

        // GET: Investments
        public ActionResult Index()
        {
            return View(db.investments.ToList());
        }
    }
}