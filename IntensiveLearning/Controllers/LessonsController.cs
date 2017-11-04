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
    public class LessonsController : Controller
    {
        private TaalimEntities db = new TaalimEntities();

        // GET: Lessons
        public ActionResult Index()
        {
            if (Session["ID"] != null)
            {
                var typeName = (string)Session["Type"]; var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
                if (type.SeeAccToCenter == true || type.SeeAccToCity == true || type.SeeAll == true || type.SeeAllButFinance == true || type.SeeTeachers == true)
                {
                    var lessons = db.Lessons.Include(l => l.Regiment);
                    if (TempData["Message"] != null)
                    {
                        ViewBag.StateMessage = TempData["Message"];
                    }
                    return View(lessons.ToList());

                }
                return RedirectToAction("Default", "Home");
            }
            return RedirectToAction("Index", "Home");


        }

        // GET: Lessons/Details/5
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
                    Lesson lesson = db.Lessons.Find(id);
                    if (lesson == null)
                    {
                        return HttpNotFound();
                    }
                    return View(lesson);

                }
                return RedirectToAction("Default", "Home");
            }
            return RedirectToAction("Index", "Home");


        }

        // GET: Lessons/Create
        public ActionResult Create()
        {
            if (Session["ID"] != null)
            {
                var typeName = (string)Session["Type"]; var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
                if (type.AddSchoolManagingTools == true)
                {
                    ViewBag.Regimentid = new SelectList(db.Regiments, "id", "Name");
                    ViewBag.Lesson1 = new SelectList(db.Study_subject, "id", "Name");
                    ViewBag.Lesson2 = new SelectList(db.Study_subject, "id", "Name");
                    ViewBag.Lesson3 = new SelectList(db.Study_subject, "id", "Name");
                    ViewBag.Lesson4 = new SelectList(db.Study_subject, "id", "Name");
                    ViewBag.Lesson5 = new SelectList(db.Study_subject, "id", "Name");
                    ViewBag.Lesson6 = new SelectList(db.Study_subject, "id", "Name");
                    ViewBag.Lesson7 = new SelectList(db.Study_subject, "id", "Name");
                    return View();

                }
                return RedirectToAction("Default", "Home");
            }
            return RedirectToAction("Index", "Home");


        }

        // POST: Lessons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Day,State,Lesson1,Lesson2,Lesson3,Lesson4,Lesson5,Lesson6,Lesson7,Regimentid")] Lesson lesson)
        {
            var Lesson1ID = Convert.ToInt32(lesson.Lesson1);
            lesson.Lesson1 = db.Study_subject.Find(Lesson1ID).Name;

            var Lesson2ID = Convert.ToInt32(lesson.Lesson2);
            lesson.Lesson2 = db.Study_subject.Find(Lesson2ID).Name;

            var Lesson3ID = Convert.ToInt32(lesson.Lesson3);
            lesson.Lesson3 = db.Study_subject.Find(Lesson3ID).Name;

            var Lesson4ID = Convert.ToInt32(lesson.Lesson4);
            lesson.Lesson4 = db.Study_subject.Find(Lesson4ID).Name;

            var Lesson5ID = Convert.ToInt32(lesson.Lesson5);
            lesson.Lesson5 = db.Study_subject.Find(Lesson5ID).Name;

            var Lesson6ID = Convert.ToInt32(lesson.Lesson6);
            lesson.Lesson6 = db.Study_subject.Find(Lesson6ID).Name;

            var Lesson7ID = Convert.ToInt32(lesson.Lesson7);
            lesson.Lesson7 = db.Study_subject.Find(Lesson7ID).Name;

            try
            {
                lesson.id = db.Lessons.OrderByDescending(x => x.id).FirstOrDefault().id + 1;
            }
            catch (Exception)
            {
                lesson.id = 1;
            }
            if (ModelState.IsValid)
            {
                db.Lessons.Add(lesson);
                db.SaveChanges();
                TempData["Message"] = "تم الادخال بنجاح";
                return RedirectToAction("Index");
            }

            ViewBag.Regimentid = new SelectList(db.Regiments, "id", "Name", lesson.Regimentid);
            return View(lesson);
        }

        // GET: Lessons/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Default", "Home");
            }
            if (Session["ID"] != null)
            {
                var typeName = (string)Session["Type"]; var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
                if (type.AddSchoolManagingTools == true)
                {





                    Lesson lesson = db.Lessons.Find(id);
                    if (lesson == null)
                    {
                        return HttpNotFound();
                    }
                    ViewBag.Regimentid = new SelectList(db.Regiments, "id", "Name", lesson.Regimentid);
                    return View(lesson);

                }
                return RedirectToAction("Default", "Home");
            }
            return RedirectToAction("Index", "Home");


        }

        // POST: Lessons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Day,State,Lesson1,Lesson2,Lesson3,Lesson4,Lesson5,Lesson6,Lesson7,Regimentid")] Lesson lesson)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lesson).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Message"] = "تم التعديل بنجاح";
                return RedirectToAction("Index");
            }
            ViewBag.Regimentid = new SelectList(db.Regiments, "id", "Name", lesson.Regimentid);
            return View(lesson);
        }

        // GET: Lessons/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Default", "Home");
            }
            if (Session["ID"] != null)
            {
                var typeName = (string)Session["Type"]; var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
                if (type.AddSchoolManagingTools == true)
                {





                    Lesson lesson = db.Lessons.Find(id);
                    if (lesson == null)
                    {
                        return HttpNotFound();
                    }
                    return View(lesson);

                }
                return RedirectToAction("Default", "Home");
            }
            return RedirectToAction("Index", "Home");


        }

        // POST: Lessons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["ID"] != null)
            {
                var typeName = (string)Session["Type"]; var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
                if (type.AddSchoolManagingTools == true)
                {





                    Lesson lesson = db.Lessons.Find(id);
                    db.Lessons.Remove(lesson);
                    try
                    {
                        db.SaveChanges();
                    }
                    catch
                    {
                        ViewBag.error = "يوجد مدخلات اخرى متعلقة بهذه الدروس يرجى تغييرها قبل الحذف";
                        return View(lesson);
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
