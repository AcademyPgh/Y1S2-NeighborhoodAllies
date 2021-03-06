﻿using System;
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
    public class ProjectsController : Controller
    {
        private ImpactMapDbContext db = new ImpactMapDbContext();

        //This holds the toolTips for Projects Forms
        private Dictionary<string, string> toolTips = new Dictionary<string, string>()
            {
                {"name", "The name of the project" },
                {"projDescription", "A brief description of the project" }

            };

        // GET: Projects
        [Authorize]
        public ActionResult Index()
        {

            ViewBag.Tooltips = toolTips;
            Utils.Utility uu = new Utils.Utility();
            var entity = db.users.Find(uu.UserID(User)).entity;
            return View(entity.projects.ToList());
        }

        // GET: Projects/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            ViewBag.Tooltips = toolTips;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // GET: Projects/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.Tooltips = toolTips;
            ProjectViewModel pvm = new ProjectViewModel();
            pvm.Project = new Models.Project();
            pvm.Entities = db.entities.ToList();
            return View(pvm);
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "ID,name,description")] Project project)
        {
            if (ModelState.IsValid)
            {
                Utils.Utility userUtil = new Utils.Utility();
                project.entity = db.users.Find(userUtil.UserID(User)).entity;
                db.projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("Index", "Dashboard");
            }

            return View(project);
        }

        // GET: Projects/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            ViewBag.Tooltips = toolTips;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "ID,name,description")] Project project)
        {
 
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(project);
        }

        // GET: Projects/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            ViewBag.Tooltips = toolTips;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.projects.Find(id);
            db.projects.Remove(project);
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

    public class ProjectViewModel
    {
        public Project Project { get; set; }
        public List<Entity> Entities { get; set; }
        public List<Investment> Investments { get; set; }
    }

}
