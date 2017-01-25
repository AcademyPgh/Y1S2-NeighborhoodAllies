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
    public class ReportsController : Controller
    {
        private ImpactMapDbContext db = new ImpactMapDbContext();

        // GET: Reports
        [Authorize]
        public ActionResult Index()
        {
            return View(db.reports.ToList());
        }

        // GET: Reports/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Report report = db.reports.Find(id);
            if (report == null)
            {
                return HttpNotFound();
            }
            return View(report);
        }

        // GET: Reports/Create
        [Authorize]
        public ActionResult Create(int? ID)
        {
            
            //ViewBag.ID = ID;
            ReportViewModel rvm = new ReportViewModel();
            rvm.project = db.projects.Find(ID);
            return View(rvm);
        }

        // POST: Reports/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "ID,completed,reportText,dueDate")] Report report, int project_ID, string metricIDs, string resultTexts)
        {
            if (ModelState.IsValid)
            {
                report.dueDate = DateTime.Now;

                string[] resultTextArray = resultTexts.Split(',');
                int i = 0;


                if (metricIDs != "")
                {
                    foreach (var metricID in metricIDs.Split(','))
                    {
                        int mID = Convert.ToInt32(metricID);
                        MetricResult mr = new MetricResult();
                        mr.report = report;
                        mr.metric = db.metrics.Find(mID);
                        mr.resultText = resultTextArray[i];
                        i++;
                        db.metricResults.Add(mr);
                        db.SaveChanges();
                    }

                }

                db.projects.Find(project_ID).report = report;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(report);
        }

        // GET: Reports/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Report report = db.reports.Find(id);
            if (report == null)
            {
                return HttpNotFound();
            }
            return View(report);
        }

        // POST: Reports/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,project_ID,completed,reportText,dueDate")] Report report)
        {
            if (ModelState.IsValid)
            {
                db.Entry(report).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(report);
        }

        // GET: Reports/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Report report = db.reports.Find(id);
            if (report == null)
            {
                return HttpNotFound();
            }
            return View(report);
        }

        // POST: Reports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            Report report = db.reports.Find(id);
            db.reports.Remove(report);
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

    public class ReportViewModel
    {
        public Project project { get; set; }
        public Report report { get; set; }
    }
}
