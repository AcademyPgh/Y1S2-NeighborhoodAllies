using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ImpactMap.Models;

namespace ImpactMap.Controllers
{
    public class CategoriesController : Controller
    {
        private ImpactMapDbContext db = new ImpactMapDbContext();

        private Dictionary<string, string> toolTips = new Dictionary<string, string>()
            {
                {"amount", "The value of the investment or the cash-equivalence of the in-kind donation." },
                {"categories", "The categories and metrics you would like the recipient to report back on." },
                {"ivmDescription", "A brief description of the investment (if in kind, the nature of the donation can be noted here)"},

            };

        // GET: Categories
        [Authorize]
        public ActionResult Index()
        {
            Utils.Utility userUtil = new Utils.Utility();
            int entityID = db.users.Find(userUtil.UserID(User)).entity.ID;

            CategoryViewModel cvm = new CategoryViewModel();
            cvm.UserCategories = new List<Category>();
            //List<Category> userCategories = new List<Category>();
            foreach (var category in db.categories)
            {
                if (category.entityID == entityID)
                {
                    cvm.UserCategories.Add(category);
                }
            }
            cvm.BaseCategories = db.categories.Where(bcat => bcat.isBase == true).ToList();
            //List<Category> categories = db.categories.ToList();
            return View(cvm);
        }

        // GET: Categories/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Categories/Create
        [Authorize]
        public ActionResult Create()
        {
            //Utils.Utility userUtil = new Utils.Utility();
            CategoryViewModel cvm = new CategoryViewModel();
             cvm.Entities = db.entities.ToList();
             cvm.Category = new Models.Category();
             cvm.Metrics = new Models.Metric();

            return View(cvm);
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "ID,name")] Category category, Metric metric, string newMetrics, int baseID)
        {
            if (ModelState.IsValid)
            {
                Utils.Utility userUtil = new Utils.Utility();
                category.entityID = db.users.Find(userUtil.UserID(User)).entity.ID;
                category.isBase = false;
                category.baseID = baseID;
                db.categories.Add(category);
                db.SaveChanges();
                var catID = category.ID;

                if (newMetrics != "")
                {
                    foreach (var metricName in newMetrics.Split(','))
                    {
                        metric.name = metricName;
                        metric.categoryID = catID;
                        db.metrics.Add(metric);
                        db.SaveChanges();
                        Category currentCategory = db.categories.Find(catID);
                        currentCategory.metrics.Add(metric);
                        db.SaveChanges();
                    }
                }

                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: Categories/CreateBase
        [Authorize]
        public ActionResult CreateBase()
        {
            //Utils.Utility userUtil = new Utils.Utility();
            CategoryViewModel cvm = new CategoryViewModel();
            cvm.Entities = db.entities.ToList();
            cvm.Category = new Models.Category();
            cvm.Metrics = new Models.Metric();

            return View(cvm);
        }

        // POST: Categories/CreateBase
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult CreateBase([Bind(Include = "ID,name")] Category category, Metric metric, string newMetrics)
        {
            if (ModelState.IsValid)
            {
                //Utils.Utility userUtil = new Utils.Utility();
                category.isBase = true;
                db.categories.Add(category);
                db.SaveChanges();
                var catID = category.ID;

                if (newMetrics != "")
                {
                    foreach (var metricName in newMetrics.Split(','))
                    {
                        metric.name = metricName;
                        metric.categoryID = catID;
                        db.metrics.Add(metric);
                        db.SaveChanges();
                        Category currentCategory = db.categories.Find(catID);
                        currentCategory.metrics.Add(metric);
                        db.SaveChanges();
                    }
                }

                return RedirectToAction("CreateBase", "Categories");
            }

            return View(category);
        }

        // GET: Categories/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Category category = db.categories.Find(id);
            //if (category == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(category);


            CategoryViewModel cvm = new CategoryViewModel();
            Utils.Utility userUtil = new Utils.Utility();

            //public Metric Metrics { get; set; }
            //public Category Category { get; set; }
            //public List<Category> BaseCategories { get; set; }
            //public List<Entity> Entities { get; set; }

            cvm.Category = db.categories.Find(id);
            cvm.BaseCategories = db.categories.Where(bcat => bcat.isBase == true).ToList();
           
            //ivm.Investment.entityFrom = db.users.Find(userUtil.UserID(User)).entity;
            //ivm.Projects = db.projects.Where(i => i.entity.ID == ivm.Investment.entityFrom.ID).ToList();
    

            if (cvm.Category == null)
            {
                return HttpNotFound();
            }


            return View(cvm);
        






    }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "ID,name")] int baseID, Category category, Metric metric, string metricsToAdd, string metricsToRemove)
        {
            if (ModelState.IsValid)
            {
                Category cat = db.categories.Find(category.ID);
                cat.name = category.name;

                cat.baseID = baseID;

                if (metricsToAdd != "")
                {
                    AddMetrics(category.ID, metric, metricsToAdd.Split(','));
                }

                if (metricsToRemove != "")
                {
                    SaveMetrics(category.ID, metricsToRemove.Split(','));
                }

                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(category);
        }

        private void AddMetrics(int id, Metric metric, string[] newMetrics)
        {
           
            Category currentCategory = db.categories.Find(id);
            foreach (var newMetric in newMetrics)
            {
                metric.name = newMetric;
                metric.categoryID = id;
                db.metrics.Add(metric);
                db.SaveChanges();
                //currentCategory.metrics.Add(metric);
                //db.SaveChanges();
            }
        }

        private void SaveMetrics(int id, string[] MetricIDs)
        {
            List<int> ids = new List<int>();
            List<Metric> metricsToRemove = new List<Metric>();
            foreach (var metricID in MetricIDs)
            {
                int i;
                if(int.TryParse(metricID, out i))
                {
                    ids.Add(i);
                }
            }

            var category = db.categories.Find(id);
            foreach (var metric in category.metrics)
            {
                if (ids.Contains(metric.ID))
                {
                    metricsToRemove.Add(metric);
                }
            }


            foreach (var met in metricsToRemove)
            {

                db.Entry(met).State = EntityState.Deleted;
                category.metrics.Remove(met);
                
            }

           
        }

        // GET: Categories/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = db.categories.Find(id);
            db.categories.Remove(category);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }

    public class CategoryViewModel
    {
        public Metric Metrics { get; set; }
        public Category Category { get; set; }
        public List<Category> UserCategories { get; set; }
        public List<Category> BaseCategories { get; set; }
        public List<Entity> Entities { get; set; }

    }
}
