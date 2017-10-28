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
    public class PeriodsController : Controller
    {
        private TaalimEntities db = new TaalimEntities();

        // GET: Periods
        public ActionResult Index()
        {
            if (Session["ID"] != null)
            {
                var typeName = (string)Session["Type"];var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
                if (type.SeeAccToCenter == true || type.SeeAccToCity == true || type.SeeAll == true || type.SeeAllButFinance == true || type.SeeTeachers == true)
                {
                    return View(db.Periods.ToList());

                }
                return RedirectToAction("Default", "Home");
            }
            return RedirectToAction("Index", "Home");


        }

        // GET: Periods/Details/5
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
                    Period period = db.Periods.Find(id);
                    if (period == null)
                    {
                        return HttpNotFound();
                    }
                    return View(period);

                }
                return RedirectToAction("Default", "Home");
            }
            return RedirectToAction("Index", "Home");


        }

        // GET: Periods/Create
        public ActionResult Create()
        {
            if (Session["ID"] != null)
            {
                                var typeName = (string)Session["Type"];var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
                if (type.AddSchoolManagingTools == true)
                {
                    return View();

                }
                return RedirectToAction("Default", "Home");
            }
            return RedirectToAction("Index", "Home");


        }

        // POST: Periods/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,StartingTime,Enh2ime")] Period period)
        {
            try
            {
                period.id = db.Periods.OrderByDescending(x => x.id).FirstOrDefault().id + 1;
            }
            catch (Exception)
            {
                period.id = 1;
            }
            if (ModelState.IsValid)
            {
                db.Periods.Add(period);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(period);
        }

        // GET: Periods/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Default", "Home");
            }
            if (Session["ID"] != null)
            {
                                var typeName = (string)Session["Type"];var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
                if (type.AddSchoolManagingTools == true)
                {

                    Period period = db.Periods.Find(id);
                    if (period == null)
                    {
                        return HttpNotFound();
                    }
                    return View(period);

                }
                return RedirectToAction("Default", "Home");
            }
            return RedirectToAction("Index", "Home");


        }

        // POST: Periods/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Name,StartingTime,Enh2ime")] Period period)
        {
            if (ModelState.IsValid)
            {
                db.Entry(period).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(period);
        }

        // GET: Periods/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Default", "Home");
            }
            if (Session["ID"] != null)
            {
                                var typeName = (string)Session["Type"];var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
                if (type.AddSchoolManagingTools == true)
                {
                    Period period = db.Periods.Find(id);
                    if (period == null)
                    {
                        return HttpNotFound();
                    }
                    return View(period);

                }
                return RedirectToAction("Default", "Home");
            }
            return RedirectToAction("Index", "Home");


        }

        // POST: Periods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["ID"] != null)
            {
                                var typeName = (string)Session["Type"];var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
                if (type.AddSchoolManagingTools == true)
                {
                    Period period = db.Periods.Find(id);
                    db.Periods.Remove(period);
                    try
                    {
                        db.SaveChanges();
                    }
                    catch
                    {
                        ViewBag.error = "يوجد مدخلات اخرى متعلقة بهذه الفترة يرجى تغييرها قبل الحذف";
                        return View(period);
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
