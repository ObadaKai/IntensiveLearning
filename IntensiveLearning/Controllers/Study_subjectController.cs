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
    public class Study_subjectController : Controller
    {
        private TaalimEntities db = new TaalimEntities();

        // GET: Study_subject
        public ActionResult Index()
        {
            if (Session["ID"] != null)
            {
                var typeName = (string)Session["Type"]; var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
                if (type.SeeAccToCenter == true || type.SeeAccToCity == true || type.SeeAll == true || type.SeeAllButFinance == true || type.SeeTeachers == true)
                {

                    return View(db.Study_subject.ToList());
                }
                return RedirectToAction("Default", "Home");
            }
            return RedirectToAction("Index", "Home");


        }

        // GET: Study_subject/Details/5
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

                    Study_subject study_subject = db.Study_subject.Find(id);
                    if (study_subject == null)
                    {
                        return HttpNotFound();
                    }
                    return View(study_subject);
                }
                return RedirectToAction("Default", "Home");
            }
            return RedirectToAction("Index", "Home");


        }

        // GET: Study_subject/Create
        public ActionResult Create()
        {

            if (Session["ID"] != null)
            {
                var typeName = (string)Session["Type"]; var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
                if (type.AddSchoolManagingTools == true)
                {

                    return View();
                }
                return RedirectToAction("Default", "Home");
            }
            return RedirectToAction("Index", "Home");


        }

        // POST: Study_subject/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Desc,FullMark,LeastMark,State")] Study_subject study_subject)
        {
            try
            {
                study_subject.id = db.Study_subject.OrderByDescending(x => x.id).FirstOrDefault().id + 1;
            }
            catch (Exception)
            {
                study_subject.id = 1;
            }
            if (ModelState.IsValid)
            {
                db.Study_subject.Add(study_subject);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(study_subject);
        }

        // GET: Study_subject/Edit/5
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

                    Study_subject study_subject = db.Study_subject.Find(id);
                    if (study_subject == null)
                    {
                        return HttpNotFound();
                    }
                    return View(study_subject);
                }
                return RedirectToAction("Default", "Home");
            }
            return RedirectToAction("Index", "Home");


        }

        // POST: Study_subject/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Name,Desc,FullMark,LeastMark,State")] Study_subject study_subject)
        {
            if (ModelState.IsValid)
            {
                db.Entry(study_subject).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(study_subject);
        }

        // GET: Study_subject/Delete/5
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

                    Study_subject study_subject = db.Study_subject.Find(id);
                    if (study_subject == null)
                    {
                        return HttpNotFound();
                    }
                    return View(study_subject);
                }
                return RedirectToAction("Default", "Home");
            }
            return RedirectToAction("Index", "Home");


        }

        // POST: Study_subject/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["ID"] != null)
            {
                var typeName = (string)Session["Type"]; var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
                if (type.AddSchoolManagingTools == true)
                {

                    Study_subject study_subject = db.Study_subject.Find(id);
                    db.Study_subject.Remove(study_subject);
                    try
                    {
                        db.SaveChanges();
                    }
                    catch
                    {
                        ViewBag.error = "يوجد مدخلات اخرى متعلقة بهذا المادة يرجى تغييرها قبل الحذف";
                        return View(study_subject);
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
