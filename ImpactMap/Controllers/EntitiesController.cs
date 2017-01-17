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
            Utils.Utility userUtil = new Utils.Utility();
            User user = db.users.Find(userUtil.UserID(User));
            
            if (user.entity != null)
            {
                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                return View();
            }
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
                //Instantiating userUtil to get the ID of currently logged in user
                Utils.Utility userUtil = new Utils.Utility();
                
                //Using geocoder from Google to get latitude and longitude from entity address
                IGeocoder geocoder = new GoogleGeocoder() { ApiKey = "AIzaSyDOH51wduQKexTyFXGy0tdDqfXw47XIrjA" };
                IEnumerable<Address> addresses = geocoder.Geocode(entity.address1 + " " + entity.address2 + " " + entity.city + " " + entity.state + " " + entity.zip);
                entity.lat = Convert.ToString(addresses.First().Coordinates.Latitude);
                entity.lng = Convert.ToString(addresses.First().Coordinates.Longitude);
                
                //user is the currently logged in user; attach the entity we're making to the user
                var user = db.users.Find(userUtil.UserID(User));
                db.entities.Add(entity);
                db.SaveChanges();
                user.entity = entity;
                db.SaveChanges();

                //Creating a default project each time that an entity is created
                Project project = new Project();
                project.name = "General Fund";
                project.description = "General Pool of Funds - unassigned to a project";
                project.entity = entity;
                db.projects.Add(project);
                db.SaveChanges();
                //Add the project to this entity's list of projects
                entity.projects.Add(project);
                db.SaveChanges();

                //Once the entity is created, redirect to the dashboard IF there are categories in the system
                //If there aren't, that means no base categories exist and the user will be redirected to make one
                List<Category> categoriesList = db.categories.ToList();
                if (categoriesList.Count > 0)
                {
                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    return RedirectToAction("CreateBase", "Categories");
                }
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
            Utils.Utility userUtil = new Utils.Utility();

            IGeocoder geocoder = new GoogleGeocoder() { ApiKey = "AIzaSyDOH51wduQKexTyFXGy0tdDqfXw47XIrjA" };
            IEnumerable<Address> addresses = geocoder.Geocode(entity.address1 + " " + entity.address2 + " " + entity.city + " " + entity.state + " " + entity.zip);

            if (ModelState.IsValid)
            {
                entity.lat = Convert.ToString(addresses.First().Coordinates.Latitude);
                entity.lng = Convert.ToString(addresses.First().Coordinates.Longitude);

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

        //This Action connects to ajax in the header (_Layout.cshtml) to show notifications
        public ActionResult GetReportNotifs(int ID)
        {
            Entity currEntity = db.entities.Find(ID);
            List<Project> projectList = currEntity.projects.ToList();
            List<Project> projectsWithoutReports = new List<Project>();
            foreach (var project in projectList)
            {
                if (project.investmentsIn.Count > 0 && project.report == null)
                {
                    projectsWithoutReports.Add(project);
                }
            }
            var result = JsonConvert.SerializeObject(projectsWithoutReports, Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            return Content(result, "application/json");
        }
        //Entity ID is needed to get the projects for the report notifications
        public ActionResult GetCurrEntity()
        {
            Utils.Utility userUtil = new Utils.Utility();
            Entity currEntity = db.entities.Find(userUtil.UserID(User));
            var result = JsonConvert.SerializeObject(currEntity, Formatting.None,
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
