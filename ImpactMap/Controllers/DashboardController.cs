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
            //Utils.Utility userUtil = new Utils.Utility();
            //project.entity = db.users.Find(userUtil.UserID(User)).entity;

            //Utils.Utility userUtil = new Utils.Utility();
            //var user = db.users.Find(userUtil.UserID(User));
            //db.entities.Add(entity);
            //db.SaveChanges();
            //user.entity = entity;
            //db.SaveChanges();
            //return RedirectToAction("Index");

            DashboardViewModel dvm = new DashboardViewModel();
            return View(dvm);
        }
    }

    public class DashboardViewModel
    {
        public List<Project> Projects { get; set; }
        public List<Investment> Investments { get; set; }
        public Investment Investment { get; set; }
        public Project Project { get; set; } 
    }
}