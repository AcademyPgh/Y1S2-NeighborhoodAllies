using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

            DashboardViewModel dvm = new DashboardViewModel();
            Utils.Utility userUtil = new Utils.Utility();
            dvm.investmentsOut = db.users.Find(userUtil.UserID(User)).entity.investmentsOut.ToList();
            dvm.projects = db.users.Find(userUtil.UserID(User)).entity.projects.ToList();
            dvm.entity = db.users.Find(userUtil.UserID(User)).entity;
            dvm.investment = new Investment();
            dvm.project = new Project();

            return View(dvm);
        }
    }

    public class DashboardViewModel
    {
        public List<Project> projects { get; set; }
        public List<Investment> investmentsOut { get; set; }
        public Investment investment { get; set; }
        public Project project { get; set; } 
        public Entity entity { get; set; }
    }
}