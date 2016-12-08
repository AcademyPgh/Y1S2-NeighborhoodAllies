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

            dvm.Investments = db.investments.ToList();
            dvm.Projects = db.projects.ToList();
            dvm.Investment = new Investment();
            dvm.Project = new Project();

            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //if (dvm.Entity == null)
            //{
            //    return HttpNotFound();
            //}


            //Utils.Utility userUtil = new Utils.Utility();
            //dvm.Entity = db.users.Find(userUtil.UserID(1)).entity;

            return View(dvm);
        }
    }

    public class DashboardViewModel
    {
        public List<Project> Projects { get; set; }
        public List<Investment> Investments { get; set; }
        public Investment Investment { get; set; }
        public Project Project { get; set; } 
        public Entity Entity { get; set; }
    }
}