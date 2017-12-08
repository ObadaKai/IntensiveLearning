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
    public class RegimentsController : Controller
    {
        private TaalimEntities db = new TaalimEntities();

        // GET: Regiments
        public ActionResult Index()
        {
            if (Session["ID"] != null)
            {
                                var typeName = (string)Session["Type"];var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
                if (type.SeeAccToCenter == true || type.SeeAccToCity == true || type.SeeAll == true || type.SeeAllButFinance == true || type.SeeTeachers == true)
                {

                    var regiments = db.Regiments;
                    if (TempData["Message"] != null)
                    {
                        ViewBag.StateMessage = TempData["Message"];
                    }
                    ViewBag.TitleSideBar = "Regiments";

                    return View(regiments.ToList());

                }
                return RedirectToAction("Default", "Home");
            }
            return RedirectToAction("Index", "Home");


        }

        // GET: Regiments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Default", "Home");
            }
            if (Session["ID"] != null)
            {
                                var typeName = (string)Session["Type"];var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
                if (type.SeeAccToCenter == true || type.SeeAccToCity == true || type.SeeAll == true || type.SeeAllButFinance == true || type.SeeTeachers == true)
                {




                    Regiment regiment = db.Regiments.Find(id);
                    if (regiment == null)
                    {
                        return HttpNotFound();
                    }
                    ViewBag.TitleSideBar = "Regiments";

                    return View(regiment);

                }
                return RedirectToAction("Default", "Home");
            }
            return RedirectToAction("Index", "Home");


        }

        // GET: Regiments/Create
        public ActionResult Create()
        {
            if (Session["ID"] != null)
            {
                                var typeName = (string)Session["Type"];var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
                if (type.SeeAccToCenter == true)
                {




                    ViewBag.Periodid = new SelectList(db.Periods, "id", "Name");
                    ViewBag.TitleSideBar = "Regiments";

                    return View();

                }
                return RedirectToAction("Default", "Home");
            }
            return RedirectToAction("Index", "Home");


        }

        // POST: Regiments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Day1,Day2,Day3,Day4,Day5,Day6,Day7,Desc,Periodid")] Regiment regiment)
        {
            try
            {
                regiment.id = db.Regiments.OrderByDescending(x => x.id).FirstOrDefault().id + 1;
            }
            catch (Exception)
            {
                regiment.id = 1;
            }
            if (ModelState.IsValid)
            {
                db.Regiments.Add(regiment);
                db.SaveChanges();
                TempData["Message"] = "تم الادخال بنجاح";
                return RedirectToAction("Index");
            }
            ViewBag.Periodid = new SelectList(db.Periods, "id", "Name",regiment.Periodid);
            ViewBag.TitleSideBar = "Regiments";

            return View(regiment);
        }

        // GET: Regiments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Default", "Home");
            }
            if (Session["ID"] != null)
            {
                                var typeName = (string)Session["Type"];var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
                if (type.SeeAccToCenter == true)
                {




                    Regiment regiment = db.Regiments.Find(id);
                    if (regiment == null)
                    {
                        return HttpNotFound();
                    }
                    ViewBag.Periodid = new SelectList(db.Periods, "id", "Name", regiment.Periodid);
                    ViewBag.TitleSideBar = "Regiments";

                    return View(regiment);

                }
                return RedirectToAction("Default", "Home");
            }
            return RedirectToAction("Index", "Home");


        }

        // POST: Regiments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Name,Day1,Day2,Day3,Day4,Day5,Day6,Day7,Desc,Periodid")] Regiment regiment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(regiment).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Message"] = "تم التعديل بنجاح";
                return RedirectToAction("Index");
            }
            ViewBag.Periodid = new SelectList(db.Periods, "id", "Name", regiment.Periodid);
            ViewBag.TitleSideBar = "Regiments";

            return View(regiment);
        }

        // GET: Regiments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Default", "Home");
            }
            if (Session["ID"] != null)
            {
                                var typeName = (string)Session["Type"];var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
                if (type.SeeAccToCenter == true)
                {




                    Regiment regiment = db.Regiments.Find(id);
                    if (regiment == null)
                    {
                        return HttpNotFound();
                    }
                    ViewBag.TitleSideBar = "Regiments";

                    return View(regiment);

                }
                return RedirectToAction("Default", "Home");
            }
            return RedirectToAction("Index", "Home");


        }

        // POST: Regiments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["ID"] != null)
            {
                                var typeName = (string)Session["Type"];var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
                if (type.SeeAccToCenter == true)
                {




                    Regiment regiment = db.Regiments.Find(id);
                    db.Regiments.Remove(regiment);
                    try
                    {
                        db.SaveChanges();
                    }
                    catch
                    {
                        ViewBag.TitleSideBar = "Regiments";

                        ViewBag.error = "يوجد مدخلات اخرى متعلقة بهذا الفوج يرجى تغييرها قبل الحذف";
                        return View(regiment);
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
