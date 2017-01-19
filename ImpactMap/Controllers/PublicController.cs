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

        //For Investments/Edit, gets categories already attached to current investment
        public ActionResult CurrInvestmentCategories(int investmentID)
        {
            List<Category> currentCategories = new List<Category>();
            foreach (var category in db.investments.Find(investmentID).categories)
            {
                currentCategories.Add(category);
            }
            var result = JsonConvert.SerializeObject(currentCategories, Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            return Content(result, "application/json");
        }
        //For Investments/Edity, gets categories NOT attached to current investment
        public ActionResult NonCurrInvestmentCategories(int investmentID)
        {
            Utils.Utility userUtil = new Utils.Utility();
            List<Category> currentCategories = new List<Category>();
            foreach (var category in db.investments.Find(investmentID).categories)
            {
                currentCategories.Add(category);
            }
            List<Category> nonCurrentCategories = new List<Category>();
            foreach (var category in db.categories)
            {
                if (category.entityID == db.users.Find(userUtil.UserID(User)).entity.ID && !currentCategories.Contains(category))
                {
                    nonCurrentCategories.Add(category);
                }
            }
            var result = JsonConvert.SerializeObject(nonCurrentCategories, Formatting.None,
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