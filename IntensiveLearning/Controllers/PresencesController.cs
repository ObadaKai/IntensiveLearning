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
                    if (TempData["Message"] != null)
                    {
                        ViewBag.StateMessage = TempData["Message"];
                    }
                ViewBag.TitleSideBar = "Presences";

                return View();
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
                    ViewBag.TitleSideBar = "Presences";

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
                var id = Convert.ToInt16(Session["ID"]);
                var emp = db.Employees.Find(id);
                if (type.AddPresence == true)
                {
                    if (emp.Centerid != null)
                    {
                        ViewBag.Studentid = db.Students.Where(x => x.Centerid == emp.Centerid).ToList();

                    }
                    else if (emp.CityID != null)
                    {
                        ViewBag.Studentid = db.Students.Where(x => x.Center.Cityid == emp.CityID).ToList();

                    }
                    else
                    {
                        ViewBag.Studentid = db.Students.ToList();
                    }
                    ViewBag.TitleSideBar = "Presences";

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
            presence.AddedBy = Convert.ToInt32(Session["ID"]);
            presence.AddingDate = DateTime.Now.Date;
            presence.AddingTime = DateTime.Now.TimeOfDay;

            if (ModelState.IsValid)
            {
                db.Presences.Add(presence);
                db.SaveChanges();
                TempData["Message"] = "تم الادخال بنجاح";
                return RedirectToAction("Index");
            }

            var typeName = (string)Session["Type"]; var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
            var id = Convert.ToInt16(Session["ID"]);
            var emp = db.Employees.Find(id);
            if (type.AddPresence == true)
            {
                if (emp.Centerid != null)
                {
                    ViewBag.Studentid = db.Students.Where(x => x.Centerid == emp.Centerid).ToList();

                }
                else if (emp.CityID != null)
                {
                    ViewBag.Studentid = db.Students.Where(x => x.Center.Cityid == emp.CityID).ToList();

                }
                else
                {
                    ViewBag.Studentid = db.Students.ToList();
                }
                



            }
            ViewBag.TitleSideBar = "Presences";

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
                    var empid = Convert.ToInt16(Session["ID"]);
                    var emp = db.Employees.Find(empid);
                    if (emp.Centerid != null)
                    {
                        ViewBag.Studentid = db.Students.Where(x => x.Centerid == emp.Centerid).ToList();

                    }
                    else if (emp.CityID != null)
                    {
                        ViewBag.Studentid = db.Students.Where(x => x.Center.Cityid == emp.CityID).ToList();

                    }
                    else
                    {
                        ViewBag.Studentid = db.Students.ToList();
                    }
                    ViewBag.TitleSideBar = "Presences";

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
                TempData["Message"] = "تم التعديل بنجاح";
                return RedirectToAction("Index");
            }
            var typeName = (string)Session["Type"]; var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();

            var empid = Convert.ToInt16(Session["ID"]);
            var emp = db.Employees.Find(empid);
            if (type.AddPresence == true)
            {
                if (emp.Centerid != null)
                {
                    ViewBag.Studentid = db.Students.Where(x => x.Centerid == emp.Centerid).ToList();

                }
                else if (emp.CityID != null)
                {
                    ViewBag.Studentid = db.Students.Where(x => x.Center.Cityid == emp.CityID).ToList();

                }
                else
                {
                    ViewBag.Studentid = db.Students.ToList();
                }





            }
            ViewBag.TitleSideBar = "Presences";

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
                    ViewBag.TitleSideBar = "Presences";

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
                        ViewBag.TitleSideBar = "Presences";

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
