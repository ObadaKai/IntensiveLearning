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
    public class EmployeeTypesController : Controller
    {
        private TaalimEntities db = new TaalimEntities();

        // GET: EmployeeTypes
        public ActionResult Index()
        {
            if (Session["ID"] != null)
            {
                var typeName = (string)Session["Type"];
                var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
                if (type.SeeAll == true || type.SeeAllButFinance == true || type.AddNewEmployeeType == true)
                {
                    if (TempData["Message"] != null)
                    {
                        ViewBag.StateMessage = TempData["Message"];
                    }
                    ViewBag.TitleSideBar = "EmployeeTypes";

                    return View(db.EmployeeTypes.ToList());
                }
                return RedirectToAction("Default", "Home");
            }
            return RedirectToAction("Index", "Home");


        }

        // GET: EmployeeTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Default", "Home");
            }
            if (Session["ID"] != null)
            {
                var typeName = (string)Session["Type"]; var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
                if (type.SeeAll == true || type.SeeAllButFinance == true || type.AddNewEmployeeType == true)
                {
                    EmployeeType employeeType = db.EmployeeTypes.Find(id);
                    if (employeeType == null)
                    {
                        return HttpNotFound();
                    }
                    ViewBag.TitleSideBar = "EmployeeTypes";

                    return View(employeeType);
                }
                return RedirectToAction("Default", "Home");
            }
            return RedirectToAction("Index", "Home");


        }

        // GET: EmployeeTypes/Create
        public ActionResult Create()
        {
            if (Session["ID"] != null)
            {
                var typeName = (string)Session["Type"]; var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
                if (type.AddNewEmployeeType == true)
                {
                    ViewBag.TitleSideBar = "EmployeeTypes";

                    return View();
                }
                return RedirectToAction("Default", "Home");
            }
            return RedirectToAction("Index", "Home");


        }

        // POST: EmployeeTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(/*[Bind(Include = "Type,Basics,Managment,Guidence,Teaching,Observing")]*/ EmployeeType employeeType)
        {
            try
            {
                employeeType.id = db.EmployeeTypes.OrderByDescending(x => x.id).FirstOrDefault().id + 1;
            }
            catch (Exception)
            {
                employeeType.id = 1;
            }
            if (ModelState.IsValid)
            {
                db.EmployeeTypes.Add(employeeType);
                db.SaveChanges();
                TempData["Message"] = "تم الادخال بنجاح";

                return RedirectToAction("Index");
            }
            ViewBag.TitleSideBar = "EmployeeTypes";

            return View(employeeType);
        }

        // GET: EmployeeTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Default", "Home");
            }
            if (Session["ID"] != null)
            {
                var typeName = (string)Session["Type"]; var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
                if (type.AddNewEmployeeType == true)
                {

                    EmployeeType employeeType = db.EmployeeTypes.Find(id);
                    if (employeeType == null)
                    {
                        return HttpNotFound();
                    }
                    ViewBag.TitleSideBar = "EmployeeTypes";

                    return View(employeeType);


                }
                return RedirectToAction("Default", "Home");
            }
            return RedirectToAction("Index", "Home");


        }

        // POST: EmployeeTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(/*[Bind(Include = "id,Type,Basics,Managment,Guidence,Teaching,Observing")]*/ EmployeeType employeeType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employeeType).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Message"] = "تم التعديل بنجاح";

                return RedirectToAction("Index");
            }
            ViewBag.TitleSideBar = "EmployeeTypes";

            return View(employeeType);
        }

        // GET: EmployeeTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Default", "Home");
            }
            if (Session["ID"] != null)
            {
                var typeName = (string)Session["Type"]; var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
                if (type.AddNewEmployeeType == true)
                {

                    EmployeeType employeeType = db.EmployeeTypes.Find(id);
                    if (employeeType == null)
                    {
                        return HttpNotFound();
                    }
                    ViewBag.TitleSideBar = "EmployeeTypes";

                    return View(employeeType);
                }
                return RedirectToAction("Default", "Home");
            }
            return RedirectToAction("Index", "Home");


        }

        // POST: EmployeeTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["ID"] != null)
            {
                var typeName = (string)Session["Type"]; var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
                if (type.AddNewEmployeeType == true)
                {
                    EmployeeType employeeType = db.EmployeeTypes.Find(id);
                    db.EmployeeTypes.Remove(employeeType);
                    try
                    {
                        db.SaveChanges();
                    }
                    catch
                    {
                        ViewBag.error = "يوجد مدخلات اخرى متعلقة بهذا النوع يرجى تغييرها قبل الحذف";
                        ViewBag.TitleSideBar = "EmployeeTypes";

                        return View(employeeType);
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
