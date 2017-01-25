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
        [Authorize]
        public ActionResult Index()
        {

            //DashboardViewModel dvm = new DashboardViewModel();
            //Utils.Utility userUtil = new Utils.Utility();
            //var currEntity = db.users.Find(userUtil.UserID(User));
            //dvm.investmentsOut = currEntity.entity.investmentsOut.ToList();
            //dvm.projects = currEntity.entity.projects.ToList();
            //dvm.entity = currEntity.entity;
            //dvm.investment = new Investment();
            //dvm.project = new Project();

            Utils.Utility uu = new Utils.Utility();
            List<Category> categoriesList = db.categories.ToList();

            //Redirects to Categories/CreateBase if there are no categories (aka a base category is needed)
            if (categoriesList.Count < 1)
            {
                return RedirectToAction("CreateBase", "Categories");
            }
            //Redirects to Dashboard if there is an entity attached to the currently logged in user
            if (db.users.Find(uu.UserID(User)).entity != null)
            {
                var entity = db.users.Find(uu.UserID(User)).entity;
                return View(entity);
            }
            //If not, redirect to create an entity

                return RedirectToAction("Create", "Entities");

            
        }
    }

    public class DashboardViewModel
    {
        public Entity entity { get; set; }
    }
}