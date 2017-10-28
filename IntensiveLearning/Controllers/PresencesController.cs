using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IntensiveLearning.Database;

namespace IntensiveLearning.Controllers
{
    public class PresencesController : Controller
    {
        private TaalimEntities db = new TaalimEntities();

        // GET: Presences
        public ActionResult Index()
        {
            if (Session["ID"] != null)
            {
                var typeName = (string)Session["Type"]; var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
                if (type.SeeAccToCenter == true || type.SeeAccToCity == true || type.SeeAll == true || type.SeeAllButFinance == true || type.SeeTeachers == true)
                {
                    var presences = db.Presences.Include(p => p.Student);
                    return View(presences.ToList());
                }
                return RedirectToAction("Default", "Home");
            }
            return RedirectToAction("Index", "Home");


        }

        // GET: Presences/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Default", "Home");
            }
            if (Session["ID"] != null)
            {
                var typeName = (string)Session["Type"]; var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
                if (type.SeeAccToCenter == true || type.SeeAccToCity == true || type.SeeAll == true || type.SeeAllButFinance == true || type.SeeTeachers == true)
                {
                    Presence presence = db.Presences.Find(id);
                    if (presence == null)
                    {
                        return HttpNotFound();
                    }
                    return View(presence);



                }
                return RedirectToAction("Default", "Home");
            }
            return RedirectToAction("Index", "Home");


        }

        // GET: Presences/Create
        public ActionResult Create()
        {
            if (Session["ID"] != null)
            {
                var typeName = (string)Session["Type"]; var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
                if (type.AddPresence == true)
                {
                    ViewBag.Studentid = new SelectList(db.Students, "id", "Name");
                    return View();



                }
                return RedirectToAction("Default", "Home");
            }
            return RedirectToAction("Index", "Home");


        }

        // POST: Presences/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Date,Desc,Studentid,Lesson1,Lesson2,Lesson3,Lesson4,Lesson5,Lesson6,Lesson7")] Presence presence)
        {
            try
            {
                presence.id = db.Presences.OrderByDescending(x => x.id).FirstOrDefault().id + 1;
            }
            catch (Exception)
            {
                presence.id = 1;
            }
            if (ModelState.IsValid)
            {
                db.Presences.Add(presence);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Studentid = new SelectList(db.Students, "id", "Name", presence.Studentid);
            return View(presence);


        }

        // GET: Presences/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Default", "Home");
            }
            if (Session["ID"] != null)
            {
                var typeName = (string)Session["Type"]; var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
                if (type.AddPresence == true)
                {
                    Presence presence = db.Presences.Find(id);
                    if (presence == null)
                    {
                        return HttpNotFound();
                    }
                    ViewBag.Studentid = new SelectList(db.Students, "id", "Name", presence.Studentid);
                    return View(presence);



                }
                return RedirectToAction("Default", "Home");
            }
            return RedirectToAction("Index", "Home");


        }

        // POST: Presences/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Date,Desc,Studentid,Lesson1,Lesson2,Lesson3,Lesson4,Lesson5,Lesson6,Lesson7")] Presence presence)
        {
            if (ModelState.IsValid)
            {
                db.Entry(presence).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Studentid = new SelectList(db.Students, "id", "Name", presence.Studentid);
            return View(presence);
        }

        // GET: Presences/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Default", "Home");
            }
            if (Session["ID"] != null)
            {
                var typeName = (string)Session["Type"]; var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
                if (type.AddPresence == true)
                {
                    Presence presence = db.Presences.Find(id);
                    if (presence == null)
                    {
                        return HttpNotFound();
                    }
                    return View(presence);



                }
                return RedirectToAction("Default", "Home");
            }
            return RedirectToAction("Index", "Home");


        }

        // POST: Presences/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["ID"] != null)
            {
                var typeName = (string)Session["Type"]; var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
                if (type.AddPresence == true)
                {
                    Presence presence = db.Presences.Find(id);
                    db.Presences.Remove(presence);
                    try
                    {
                        db.SaveChanges();
                    }
                    catch
                    {
                        ViewBag.error = "يوجد مدخلات اخرى متعلقة بهذا الحضور يرجى تغييرها قبل الحذف";
                        return View(presence);
                    }
                    return RedirectToAction("Index");



                }
                return RedirectToAction("Default", "Home");
            }
            return RedirectToAction("Index", "Home");


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
}
