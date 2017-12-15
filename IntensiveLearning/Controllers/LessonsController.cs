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

                var lessons = db.Lessons;
                if (TempData["Message"] != null)
                {
                    ViewBag.StateMessage = TempData["Message"];
                }
                ViewBag.TitleSideBar = "Lessons";

                var empid = Convert.ToInt16(Session["ID"]);
                var emp = db.Employees.Find(empid);
                ViewBag.Stageid = new SelectList(db.Stages, "id", "Stagename");
                ViewBag.Regimentid = new SelectList(db.Regiments, "id", "Name");
                ViewBag.Periodid = new SelectList(db.Periods, "id", "Name");

                if (emp.EmployeeType.SeeAccToCity == true)
                {
                    ViewBag.Centerid = new SelectList(db.Centers.Where(x => x.Cityid == emp.CityID), "id", "Name");
                    return View(lessons.Where(x => x.Center.Cityid == emp.CityID).ToList());
                }
                else if (emp.EmployeeType.SeeAccToCenter == true || emp.EmployeeType.SeeTeachers == true)
                {
                    return View(lessons.Where(x => x.Centerid == emp.Centerid).ToList());
                }
                else
                {
                    ViewBag.Centerid = new SelectList(db.Centers, "id", "Name");

                    return View(lessons.ToList());
                }
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Search(int? Centerid, int? Regimentid, int? Stageid, int? Periodid)
        {
            List<Lesson> lessons = new List<Lesson>();
            List<Lesson> CenterLessons = new List<Lesson>();
            List<Lesson> RegimentLessons = new List<Lesson>();
            List<Lesson> StageLessons = new List<Lesson>();
            List<Lesson> PeriodLessons = new List<Lesson>();

            if (Centerid != null)
            {
                CenterLessons = db.Lessons.Where(x => x.Centerid == Centerid).ToList();
            }
            if (Regimentid != null)
            {
                RegimentLessons = db.Lessons.Where(x => x.Regimentid == Regimentid).ToList();

            }
            if (Stageid != null)
            {
                StageLessons = db.Lessons.Where(x => x.Stageid == Stageid).ToList();
            }
            if (Periodid != null)
            {
                PeriodLessons = db.Lessons.Where(x => x.Periodid == Periodid).ToList();
            }
            if (CenterLessons.Count() > 0)
            {
                lessons = CenterLessons;
            }
            else if (RegimentLessons.Count() > 0)
            {
                lessons = RegimentLessons;

            }
            else if (StageLessons.Count() > 0)
            {
                lessons = StageLessons;

            }
            else
            {
                lessons = PeriodLessons;

            }
            if (Centerid!=null)
            {
                lessons = lessons.Intersect(CenterLessons).ToList();
            }
            if (Regimentid!=null)
            {
                lessons = lessons.Intersect(RegimentLessons).ToList();

            }
            if (Stageid != null)
            {
                lessons = lessons.Intersect(StageLessons).ToList();

            }
            if (Periodid != null)
            {
                lessons = lessons.Intersect(PeriodLessons).ToList();

            }
            var empid = Convert.ToInt16(Session["ID"]);
            var emp = db.Employees.Find(empid);
            ViewBag.Stageid = new SelectList(db.Stages, "id", "Stagename", Stageid);
            ViewBag.Regimentid = new SelectList(db.Regiments, "id", "Name", Regimentid);
            ViewBag.Periodid = new SelectList(db.Periods, "id", "Name", Periodid);
            if (emp.EmployeeType.SeeAccToCity == true)
            {
                ViewBag.Centerid = new SelectList(db.Centers.Where(x => x.Cityid == emp.CityID), "id", "Name", Centerid);
            }
            else if (emp.EmployeeType.SeeAccToCenter == true || emp.EmployeeType.SeeTeachers == true)
            {
            }
            else
            {
                ViewBag.Centerid = new SelectList(db.Centers, "id", "Name", Centerid);
            }

            lessons.RemoveAll(item => item == null);
            lessons.Distinct();
            return View("Index", lessons);
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
                Lesson lesson = db.Lessons.Find(id);
                if (lesson == null)
                {
                    return HttpNotFound();
                }
                ViewBag.TitleSideBar = "Lessons";

                return View(lesson);
            }
            return RedirectToAction("Index", "Home");


        }

        // GET: Lessons/Create
        public ActionResult Create()
        {
            if (Session["ID"] != null)
            {
                var typeName = (string)Session["Type"]; var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
                if (type.SeeAccToCenter == true)
                {
                    ViewBag.Regimentid = new SelectList(db.Regiments, "id", "Name");
                    ViewBag.Stageid = new SelectList(db.Stages, "id", "StageName");
                    ViewBag.Periodid = new SelectList(db.Periods, "id", "Name");
                    ViewBag.Lesson1 = new SelectList(db.Study_subject, "Name", "Name");
                    ViewBag.Lesson2 = new SelectList(db.Study_subject, "Name", "Name");
                    ViewBag.Lesson3 = new SelectList(db.Study_subject, "Name", "Name");
                    ViewBag.Lesson4 = new SelectList(db.Study_subject, "Name", "Name");
                    ViewBag.Lesson5 = new SelectList(db.Study_subject, "Name", "Name");
                    ViewBag.Lesson6 = new SelectList(db.Study_subject, "Name", "Name");
                    ViewBag.Lesson7 = new SelectList(db.Study_subject, "Name", "Name");
                    ViewBag.TitleSideBar = "Lessons";

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
        public ActionResult Create([Bind(Include = "Day,State,Lesson1,Lesson2,Lesson3,Lesson4,Lesson5,Lesson6,Lesson7,Regimentid,Periodid,Stageid")] Lesson lesson)
        {
            var empid = Convert.ToInt32(Session["ID"]);
            var emp = db.Employees.Find(empid);
            try
            {
                lesson.id = db.Lessons.OrderByDescending(x => x.id).FirstOrDefault().id + 1;
            }
            catch (Exception)
            {
                lesson.id = 1;
            }
            lesson.Centerid = emp.Centerid;
            if (ModelState.IsValid)
            {
                db.Lessons.Add(lesson);
                db.SaveChanges();
                TempData["Message"] = "تم الادخال بنجاح";
                return RedirectToAction("Index");
            }
            ViewBag.Lesson1 = new SelectList(db.Study_subject, "Name", "Name", lesson.Lesson1);
            ViewBag.Lesson2 = new SelectList(db.Study_subject, "Name", "Name", lesson.Lesson2);
            ViewBag.Lesson3 = new SelectList(db.Study_subject, "Name", "Name", lesson.Lesson3);
            ViewBag.Lesson4 = new SelectList(db.Study_subject, "Name", "Name", lesson.Lesson4);
            ViewBag.Lesson5 = new SelectList(db.Study_subject, "Name", "Name", lesson.Lesson5);
            ViewBag.Lesson6 = new SelectList(db.Study_subject, "Name", "Name", lesson.Lesson6);
            ViewBag.Lesson7 = new SelectList(db.Study_subject, "Name", "Name", lesson.Lesson7);

            ViewBag.Regimentid = new SelectList(db.Regiments, "id", "Name", lesson.Regimentid);
            ViewBag.Stageid = new SelectList(db.Stages, "id", "StageName", lesson.Stageid);
            ViewBag.Periodid = new SelectList(db.Periods, "id", "Name", lesson.Periodid);
            ViewBag.TitleSideBar = "Lessons";

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
                if (type.SeeAccToCenter == true)
                {





                    Lesson lesson = db.Lessons.Find(id);
                    if (lesson == null)
                    {
                        return HttpNotFound();
                    }
                    ViewBag.Lesson1 = new SelectList(db.Study_subject, "Name", "Name", lesson.Lesson1);
                    ViewBag.Lesson2 = new SelectList(db.Study_subject, "Name", "Name", lesson.Lesson2);
                    ViewBag.Lesson3 = new SelectList(db.Study_subject, "Name", "Name", lesson.Lesson3);
                    ViewBag.Lesson4 = new SelectList(db.Study_subject, "Name", "Name", lesson.Lesson4);
                    ViewBag.Lesson5 = new SelectList(db.Study_subject, "Name", "Name", lesson.Lesson5);
                    ViewBag.Lesson6 = new SelectList(db.Study_subject, "Name", "Name", lesson.Lesson6);
                    ViewBag.Lesson7 = new SelectList(db.Study_subject, "Name", "Name", lesson.Lesson7);

                    ViewBag.Regimentid = new SelectList(db.Regiments, "id", "Name", lesson.Regimentid);
                    ViewBag.Stageid = new SelectList(db.Stages, "id", "StageName", lesson.Stageid);
                    ViewBag.Periodid = new SelectList(db.Periods, "id", "Name", lesson.Periodid);
                    ViewBag.TitleSideBar = "Lessons";

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
        public ActionResult Edit([Bind(Include = "id,Day,State,Lesson1,Lesson2,Lesson3,Lesson4,Lesson5,Lesson6,Lesson7,Regimentid,Periodid,Stageid")] Lesson lesson)
        {
            var empid = Convert.ToInt32(Session["ID"]);
            var emp = db.Employees.Find(empid);
            lesson.Centerid = emp.Centerid;
            if (ModelState.IsValid)
            {
                db.Entry(lesson).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Message"] = "تم التعديل بنجاح";
                return RedirectToAction("Index");
            }
            ViewBag.Lesson1 = new SelectList(db.Study_subject, "Name", "Name", lesson.Lesson1);
            ViewBag.Lesson2 = new SelectList(db.Study_subject, "Name", "Name", lesson.Lesson2);
            ViewBag.Lesson3 = new SelectList(db.Study_subject, "Name", "Name", lesson.Lesson3);
            ViewBag.Lesson4 = new SelectList(db.Study_subject, "Name", "Name", lesson.Lesson4);
            ViewBag.Lesson5 = new SelectList(db.Study_subject, "Name", "Name", lesson.Lesson5);
            ViewBag.Lesson6 = new SelectList(db.Study_subject, "Name", "Name", lesson.Lesson6);
            ViewBag.Lesson7 = new SelectList(db.Study_subject, "Name", "Name", lesson.Lesson7);

            ViewBag.Regimentid = new SelectList(db.Regiments, "id", "Name", lesson.Regimentid);
            ViewBag.Stageid = new SelectList(db.Stages, "id", "StageName", lesson.Stageid);
            ViewBag.Periodid = new SelectList(db.Periods, "id", "Name", lesson.Periodid);
            ViewBag.TitleSideBar = "Lessons";

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
                if (type.SeeAccToCenter == true)
                {
                    Lesson lesson = db.Lessons.Find(id);
                    if (lesson == null)
                    {
                        return HttpNotFound();
                    }
                    ViewBag.TitleSideBar = "Lessons";

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
                        ViewBag.TitleSideBar = "Lessons";

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
