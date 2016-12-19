using ImpactMap.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImpactMap.Controllers
{
    public class MapController : Controller
    {
        private ImpactMapDbContext db = new ImpactMapDbContext();
        // GET: Map
        public ActionResult Index()
        {
            Utils.Utility userUtil = new Utils.Utility();
            //MapViewModel mvm = new MapViewModel();
            Entity currentEntity = db.users.Find(userUtil.UserID(User)).entity;
            return View(currentEntity);
        }
    }

    public class MapViewModel
    {
        public List<Entity> Entities { get; set; }
        public User currentUser { get; set; }
    }
}