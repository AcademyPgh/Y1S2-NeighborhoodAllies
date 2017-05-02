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

        //This holds the toolTips for Investment Forms
        private Dictionary<string, string> toolTips = new Dictionary<string, string>()
            {
                {"amount", "The value of the investment or the cash-equivalence of the in-kind donation." },
                {"date", "The date the investment was made"},
                {"inKind", "Check if the investment was an in kind donation (volunteer hours, services, equipment, etc)" },
                {"projectFrom", "The pool of funds from which the investment was made."},
                {"entityTo", "The enitity to which the investment was made"},
                {"projectTo", "The recipients pool of funds into which the investment was made."},
                {"categories", "The categories and metrics you would like the recipient to include in their report." },
                {"ivmDescription", "A brief description of the investment (if in kind, the nature of the donation can be noted here)"},

            };




        // GET: Investments
        [Authorize]
        public ActionResult Index()
        {
            ViewBag.Tooltips = toolTips;
            //return View(db.investments.ToList());
            Utils.Utility uu = new Utils.Utility();
            var entity = db.users.Find(uu.UserID(User)).entity;
            return View(entity);
        }

        // GET: Investments/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            ViewBag.Tooltips = toolTips;
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
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.Tooltips = toolTips;
            //Utils.Utility gets the ID of the user that's logged in
            Utils.Utility userUtil = new Utils.Utility();
            int entityID = db.users.Find(userUtil.UserID(User)).entity.ID;

            //Uses InvestmentViewModel which is at the bottom of this file
            InvestmentViewModel ivm = new InvestmentViewModel();
            
            //pulling in all the entities in the system
            ivm.Entities = db.entities.ToList();
            //new investment because we're creating one
            ivm.Investment = new Models.Investment();
            //sets the default investment date to the current date
            ivm.Investment.date = DateTime.Now;
            //pulling all of the current entity's categories
            ivm.Categories = new List<Category>();
            foreach (var category in db.categories)
            {
                if (category.entityID == entityID)
                {
                    ivm.Categories.Add(category);
                }
            }
            //entityFrom is retrieved based on the user that's logged in
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
        [Authorize]
        public ActionResult Create([Bind(Include = "ID,amount,entityFrom_ID,entityTo_ID,description,date,isInKind, projectFrom_ID,projectTo_ID")] Investment investment, int entityTo_ID, int projectTo_ID, int projectFrom_ID, string categories)
        {
            if (ModelState.IsValid)
            {
                //Utils.Utility gets currently logged in user's ID
                Utils.Utility userUtil = new Utils.Utility();

                //entityTo_ID gets sent in via the forms and then attached to the investment here
                investment.entityTo = db.entities.Find(entityTo_ID);

                //entityFrom uses UserID() from Utils/Utility.cs to get the entity linked to the currently logged in User
                investment.entityFrom = db.users.Find(userUtil.UserID(User)).entity;

                //projectTo_ID and projectFrom_ID get sent in via the forms and then attached to the investment here
                investment.projectTo = db.projects.Find(projectTo_ID);
                investment.projectFrom = db.projects.Find(projectFrom_ID);
                
                //"categories" is a comma-separated string of categories sent in via the forms 
                //(a hidden input, using an ajax call to get the categories from the database)
                //the string is split into an array, and then each one is added to the new investment's category list
                if (categories != "" && categories != null)
                {
                    investment.categories = new List<Category>();
                    foreach (var id in categories.Split(','))
                    {
                        investment.categories.Add(db.categories.Find(Convert.ToInt32(id)));
                    }
                }

                //finally we add the investment to the database and save the changes
                db.investments.Add(investment);
                db.SaveChanges();

                //These if statements are analogous to if projectTo != null / if projectFrom != null
                //If the investment has a projectTo, add this investment to that project's investmentsIn list
                if (projectTo_ID != 0)
                {
                    Project newProject = new Project();
                    newProject = db.projects.Find(projectTo_ID);
                    newProject.investmentsIn.Add(db.investments.Find(investment.ID));
                    db.SaveChanges();
                }
                //If the investment has a projectFrom, add this investment to that project's investmentsOut list
                if (projectFrom_ID != 0)
                {
                    Project newProject = new Project();
                    newProject = db.projects.Find(projectFrom_ID);
                    newProject.investmentsOut.Add(db.investments.Find(investment.ID));
                    db.SaveChanges();
                }

                //Now redirect to the dashboard
                return RedirectToAction("Index", "Dashboard");
            }
            return View(investment);
        }

        // GET: Investments/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            ViewBag.Tooltips = toolTips;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            InvestmentViewModel ivm = new InvestmentViewModel();
            Utils.Utility userUtil = new Utils.Utility();

            ivm.Investment = db.investments.Find(id);
            ivm.Entities = db.entities.ToList();
            ivm.Categories = db.categories.ToList();
            ivm.Investment.entityFrom = db.users.Find(userUtil.UserID(User)).entity;
            ivm.Projects = db.projects.Where(i => i.entity.ID == ivm.Investment.entityFrom.ID).ToList();
            ViewBag.InvestmentCategories = string.Join(",", ivm.Investment.categories.Select(c => c.ID));

            if (ivm.Investment == null)
            {
                return HttpNotFound();
            }

           
            return View(ivm);
        }

        

        // POST: Investments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Authorize]
        //public ActionResult Edit([Bind(Include = "ID,amount,entityFrom_ID,entityTo_ID,date,isInKind,projectFrom_ID,projectTo_ID")] Investment investment)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(investment).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(investment);
        //}


        // POST: Investments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "ID,amount,entityFrom_ID,entityTo_ID,description,date,isInKind, projectFrom_ID,projectTo_ID")] Investment investment, int entityTo_ID, int projectTo_ID, int projectFrom_ID, string categories, int ID)
        {
            if (ModelState.IsValid)
            {
                if (investment != null)
                {
                    //Modified entity state causes us to not be able to update connected categories
                    Investment oldInv = db.investments.Find(ID);
                    Investment inv = db.investments.Find(ID);


                    inv.amount = investment.amount;        
                    inv.entityTo_ID = entityTo_ID;
                    inv.description = investment.description;
                    inv.date = investment.date;
                    inv.isInKind = investment.isInKind;
                    inv.projectFrom_ID = projectFrom_ID;
                    inv.projectTo_ID = projectTo_ID;

                    SaveInvestmentCategories(ID, categories.Split(','));
                    db.SaveChanges();

                    UpdateProjectInvestments(projectTo_ID, projectFrom_ID, ID, oldInv);
                }

                return RedirectToAction("Index", "Dashboard");

            }
            return View(investment);
        }


        private void UpdateProjectInvestments(int projectTo_ID, int projectFrom_ID, int ID, Investment oldInv)
        {
            //These if statements are analogous to if projectTo != null / if projectFrom != null
            //If the investment has a projectTo, add this investment to that project's investmentsIn list
                //Okay we need to send this the ID of the OLD projectTo_ID
                int oldProjectTo_ID = oldInv.projectTo.ID;
                //if the new ProjectTo is not the same as the old ProjectTo
                if (projectTo_ID != oldProjectTo_ID)
                {
                    //Remove "all" items in the list of investmentsIn where the item = the old investment
                    db.projects.Find(oldProjectTo_ID).investmentsIn.RemoveAll(item => item == oldInv);

                    //Add the new version of the investment to the new projectTo's investmentsIn list
                    db.projects.Find(projectTo_ID).investmentsIn.Add(db.investments.Find(ID));
                    db.SaveChanges();
                }
            

            //If the investment has a projectFrom, add this investment to that project's investmentsOut list
                int oldProjectFrom_ID = oldInv.projectFrom.ID;
                //If the projectFrom is changing
                if (projectFrom_ID != oldProjectFrom_ID)
                {
                    //Remove the old investment from the old projectFrom's investmentsOut list
                    db.projects.Find(oldProjectFrom_ID).investmentsOut.RemoveAll(item => item == oldInv);
                    //Add the new investment to the new projectFrom's investmentsOut list
                    db.projects.Find(projectFrom_ID).investmentsOut.Add(db.investments.Find(ID));
                    db.SaveChanges();
                }
            }
        
        private void SaveInvestmentCategories(int id, string[] CategoryIDs)
        {
            List<int> ids = new List<int>();
            List<Category> categoriesToRemove = new List<Category>();
            foreach (var cat in CategoryIDs)
            {
                int i;
                if (int.TryParse(cat, out i))
                {
                    ids.Add(i);
                }
            }

            var investment = db.investments.Find(id);
            foreach (var cat in investment.categories)
            {
                if (ids.Contains(cat.ID))
                {
                    // keep it, remove from the ids list
                    ids.Remove(cat.ID);
                }
                else
                {

                    categoriesToRemove.Add(cat);
                }
            }
            foreach (var rmcat in categoriesToRemove)
            {
                investment.categories.Remove(rmcat);
            }
            foreach (var i in ids)
            {
                investment.categories.Add(db.categories.Find(i));
            }
        }

        // GET: Investments/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            ViewBag.Tooltips = toolTips;
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
        [Authorize]
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

    //Investments require all of this stuff, so this view model is brought into the Investments/Create view
    public class InvestmentViewModel
    {
        public List<Project> Projects { get; set; }
        public List<Entity> Entities { get; set; }
        public Investment Investment { get; set; }
        public List<Category> Categories { get; set; }
    }


}

