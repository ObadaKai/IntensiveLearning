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
    public class ExamTypesController : Controller
    {
        private TaalimEntities db = new TaalimEntities();

        // GET: ExamTypes
        public ActionResult Index()
        {
            if (Session["ID"] != null)
            {
                var typeName = (string)Session["Type"];
                var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
                if (type.SeeAccToCenter == true || type.SeeAccToCity == true || type.SeeAll == true || type.SeeAllButFinance == true || type.SeeTeachers == true)
                {

                    if (TempData["Message"] != null)
                    {
                        ViewBag.StateMessage = TempData["Message"];
                    }
                    ViewBag.TitleSideBar = "ExamTypes";

                    return View(db.ExamTypes.ToList());


                }
                return RedirectToAction("Default", "Home");
            }
            return RedirectToAction("Index", "Home");


        }

        // GET: ExamTypes/Details/5
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

                    ExamType examType = db.ExamTypes.Find(id);
                    if (examType == null)
                    {
                        return HttpNotFound();
                    }
                    ViewBag.TitleSideBar = "ExamTypes";

                    return View(examType);
                }
                return RedirectToAction("Default", "Home");
            }
            return RedirectToAction("Index", "Home");


        }

        // GET: ExamTypes/Create
        public ActionResult Create()
        {
            if (Session["ID"] != null)
            {
                var typeName = (string)Session["Type"]; var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
                if (type.AddSchoolManagingTools == true)
                {

                    ViewBag.TitleSideBar = "ExamTypes";

                    return View();



                }
                return RedirectToAction("Default", "Home");
            }
            return RedirectToAction("Index", "Home");


        }

        // POST: ExamTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Type")] ExamType examType)
        {
            try
            {
                examType.id = db.ExamTypes.OrderByDescending(x => x.id).FirstOrDefault().id + 1;
            }
            catch (Exception)
            {
                examType.id = 1;
            }
            if (ModelState.IsValid)
            {
                db.ExamTypes.Add(examType);
                db.SaveChanges();
                TempData["Message"] = "تم الادخال بنجاح";

                return RedirectToAction("Index");
            }
            ViewBag.TitleSideBar = "ExamTypes";

            return View(examType);
        }

        // GET: ExamTypes/Edit/5
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

                    ExamType examType = db.ExamTypes.Find(id);
                    if (examType == null)
                    {
                        return HttpNotFound();
                    }
                    ViewBag.TitleSideBar = "ExamTypes";

                    return View(examType);



                }
                return RedirectToAction("Default", "Home");
            }
            return RedirectToAction("Index", "Home");



        }

        // POST: ExamTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Type")] ExamType examType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(examType).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Message"] = "تم التعديل بنجاح";

                return RedirectToAction("Index");
            }
            ViewBag.TitleSideBar = "ExamTypes";

            return View(examType);
        }

        // GET: ExamTypes/Delete/5
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


                    ExamType examType = db.ExamTypes.Find(id);
                    if (examType == null)
                    {
                        return HttpNotFound();
                    }
                    ViewBag.TitleSideBar = "ExamTypes";

                    return View(examType);





                }
                return RedirectToAction("Default", "Home");
            }
            return RedirectToAction("Index", "Home");


        }

        // POST: ExamTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["ID"] != null)
            {
                var typeName = (string)Session["Type"]; var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
                if (type.AddSchoolManagingTools == true)
                {



                    ExamType examType = db.ExamTypes.Find(id);
                    db.ExamTypes.Remove(examType);
                    try
                    {
                        db.SaveChanges();
                    }
                    catch
                    {
                        ViewBag.error = "يوجد مدخلات اخرى متعلقة بهذا النوع يرجى تغييرها قبل الحذف";
                        ViewBag.TitleSideBar = "ExamTypes";

                        return View(examType);
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
