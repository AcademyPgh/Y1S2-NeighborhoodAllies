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
            Utils.Utility userUtil = new Utils.Utility();
            InvestmentViewModel ivm = new InvestmentViewModel();
            
            ivm.Entities = db.entities.ToList();
            ivm.Investment = new Models.Investment();
            ivm.Categories = db.categories.ToList();
            //entityFrom is auto retrieved based on the user that's logged in
            ivm.Investment.entityFrom = db.users.Find(userUtil.UserID(User)).entity;
            //projectsFrom is populated based on entityFrom's projects
            ivm.Projects = db.projects.Where(i => i.entity.ID == ivm.Investment.entityFrom.ID).ToList();
            return View(ivm);
        }


        // POST: Investments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,amount,entityFrom_ID,entityTo_ID,description,date,isInKind,volunteerHours,projectFrom_ID,projectTo_ID")] Investment investment, int entityTo_ID, int projectTo_ID, int projectFrom_ID, string categories)
        {
            if (ModelState.IsValid)
            {
                //entityFrom uses UserID() from Utils/Utility.cs to get the entity linked to the currently logged in User
                Utils.Utility userUtil = new Utils.Utility();
                investment.entityTo = db.entities.Find(entityTo_ID);
                investment.entityFrom = db.users.Find(userUtil.UserID(User)).entity;

                investment.projectTo = db.projects.Find(projectTo_ID);
                investment.projectFrom = db.projects.Find(projectFrom_ID);

                investment.categories = new List<Category>();
                foreach (var id in categories.Split(','))
                {
                    investment.categories.Add(db.categories.Find(Convert.ToInt32(id)));
                }
                
                db.investments.Add(investment);
                db.SaveChanges();

                if (projectTo_ID != 0)
                {
                    Project newProject = new Project();
                    newProject = db.projects.Find(projectTo_ID);
                    newProject.investmentsIn.Add(db.investments.Find(investment.ID));
                    db.SaveChanges();
                }

                if (projectFrom_ID != 0)
                {
                    Project newProject = new Project();
                    newProject = db.projects.Find(projectFrom_ID);
                    newProject.investmentsOut.Add(db.investments.Find(investment.ID));
                    db.SaveChanges();
                }

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

