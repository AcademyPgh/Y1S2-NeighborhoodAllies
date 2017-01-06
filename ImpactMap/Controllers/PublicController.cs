using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ImpactMap.Models;
using Newtonsoft.Json;

namespace ImpactMap.Controllers
{
    public class PublicController : Controller
    {
        private ImpactMapDbContext db = new ImpactMapDbContext();

        // GET: Public
        //We probably don't need this at all
        public ActionResult Index()
        {
            var result = JsonConvert.SerializeObject(db.investments.ToList(), Formatting.None,
                       new JsonSerializerSettings
                       {
                           ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                       });

            return Content(result, "application/json");
        }

        public ActionResult Categories()
        {
            var result = JsonConvert.SerializeObject(db.categories.ToList(), Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

            return Content(result, "application/json");
        }

        public ActionResult UserCategories()
        {
            Utils.Utility userUtil = new Utils.Utility();
            List<Category> userCategories = new List<Category>();
            int currEntityId = db.users.Find(userUtil.UserID(User)).entity.ID;

            foreach (var category in db.categories)
            {
                if (category.entityID == currEntityId)
                {
                    userCategories.Add(category);
                }
            }
            var result = JsonConvert.SerializeObject(userCategories, Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

            return Content(result, "application/json");
        }

        //unnecessary ?
        //public ActionResult CurrentEntity()
        //{
        //    Utils.Utility userUtil = new Utils.Utility();
        //    var result = JsonConvert.SerializeObject(db.users.Find(userUtil.UserID(User)).entity.ID, Formatting.None, 
        //        new JsonSerializerSettings
        //        {
        //            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        //        });
        //    return Content(result, "application/json");
        //}

        public ActionResult getCurrentProject()
        {
            return View();
        }
    }
}