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
    public class InvestmentsController : Controller
    {
        private ImpactMapDbContext db = new ImpactMapDbContext();

        // GET: Investments
        public ActionResult Index()
        {
            return View(db.investments.ToList());
        }

        // GET: Investments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Investment investment = db.investments.Find(id);
            if (investment == null)
            {
                return HttpNotFound();
            }
            return View(investment);
        }

        // GET: Investments/Create
        public ActionResult Create()
        {
            InvestmentViewModel ivm = new InvestmentViewModel();
            ivm.Projects = db.projects.ToList();
            ivm.Entities = db.entities.ToList();
            ivm.Investment = new Models.Investment();
            ivm.Categories = db.categories.ToList();
            return View(ivm);
        }


        // POST: Investments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,amount,entityFrom_ID,entityTo_ID,date,isInKind,volunteerHours,projectFrom_ID,projectTo_ID")] Investment investment, int entityTo_ID, int projectTo_ID, int category_ID)
        {
            if (ModelState.IsValid)
            {
                Utils.Utility userUtil = new Utils.Utility();
                investment.entityTo = db.entities.Find(entityTo_ID);
                investment.projectTo = db.projects.Find(projectTo_ID);
                //investment.categories = db.categories.Find(category_ID);
                //Uses UserID() from Utils/Utility.cs
                investment.entityFrom = db.users.Find(userUtil.UserID(User)).entity;
                db.investments.Add(investment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(investment);
        }

        // GET: Investments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Investment investment = db.investments.Find(id);
            if (investment == null)
            {
                return HttpNotFound();
            }
            return View(investment);
        }

        // POST: Investments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,amount,entityFrom_ID,entityTo_ID,date,isInKind,volunteerHours,projectFrom_ID,projectTo_ID")] Investment investment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(investment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(investment);
        }

        // GET: Investments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Investment investment = db.investments.Find(id);
            if (investment == null)
            {
                return HttpNotFound();
            }
            return View(investment);
        }

        // POST: Investments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Investment investment = db.investments.Find(id);
            db.investments.Remove(investment);
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

    public class InvestmentViewModel
    {
        public List<Project> Projects { get; set; }
        public List<Entity> Entities { get; set; }
        public Investment Investment { get; set; }
        public List<Category> Categories { get; set; }
    }

}

