using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ImpactMap.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using Geocoding;
using Geocoding.Google;

namespace ImpactMap.Controllers
{
    public class EntitiesController : Controller
    {
        private ImpactMapDbContext db = new ImpactMapDbContext();

        // GET: Entities
        public ActionResult Index()
        {
            return View(db.entities.ToList());
        }

        // GET: Entities/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Entity entity = db.entities.Find(id);
            if (entity == null)
            {
                return HttpNotFound();
            }
            return View(entity);
        }

        // GET: Entities/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Entities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,name,description,address1,address2,city,state,zip,lat,lng")] Entity entity)
        {
            if (ModelState.IsValid)
            {
                Utils.Utility userUtil = new Utils.Utility();
                IGeocoder geocoder = new GoogleGeocoder() { ApiKey = "AIzaSyDOH51wduQKexTyFXGy0tdDqfXw47XIrjA" };
                IEnumerable<Address> addresses = geocoder.Geocode(entity.address1 + " " + entity.address2 + " " + entity.city + " " + entity.state + " " + entity.zip);
                
              
                entity.lat = Convert.ToString(addresses.First().Coordinates.Latitude);
                entity.lng = Convert.ToString(addresses.First().Coordinates.Longitude);
                var user = db.users.Find(userUtil.UserID(User));
                db.entities.Add(entity);
                db.SaveChanges();
                user.entity = entity;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(entity);
        }

        // GET: Entities/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Entity entity = db.entities.Find(id);
            if (entity == null)
            {
                return HttpNotFound();
            }
            return View(entity);
        }

        // POST: Entities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,name,description,address1,address2,city,state,zip,lat,lng")] Entity entity)
        {
            if (ModelState.IsValid)
            {
                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(entity);
        }

        // GET: Entities/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Entity entity = db.entities.Find(id);
            if (entity == null)
            {
                return HttpNotFound();
            }
            return View(entity);
        }

        // POST: Entities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Entity entity = db.entities.Find(id);
            db.entities.Remove(entity);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        //It's my understanding that this ID is Investments.entityOut.ID which is chosen in the dropdown...
        //How are we sending it to this action? Should there be an ajax post function on change?
        public ActionResult GetProjectsOut(int ID)
        {
            Entity recipient = db.entities.Find(ID);
            List<Project> projectList = recipient.projects.ToList();
            var result = JsonConvert.SerializeObject(projectList, Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            return Content(result, "application/json");
        }

        public string GetAddress(int ID)
        {
            Entity addressHaver = db.entities.Find(ID);
            string address1 = addressHaver.address1;
            string address2 = addressHaver.address2;
            string city = addressHaver.city;
            string state = addressHaver.state;
            string zip = addressHaver.zip;
            string FullAddress = address1 + "+" + address2 + "+" + city + "+" + state + "+" + zip;
            return FullAddress;
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
    public class EntityViewModel
    {
        public List<Project> Projects { get; set; }
        public List<Entity> Entities { get; set; }
        public Investment Investment { get; set; }
    }
}
