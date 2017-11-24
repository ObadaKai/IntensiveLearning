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
    public class BndsController : Controller
    {
        private TaalimEntities db = new TaalimEntities();

        // GET: Bnds
        public ActionResult Index()
        {
            if (Session["ID"] == null)
            {
                return RedirectToAction("Index", "Home");

            }
            var typeName = (string)Session["Type"];
            var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();

            if (type.SeeAll == true)
            {
                return View(db.Bnds.ToList());

            }
            return RedirectToAction("Default", "Home");
        }

        // GET: Bnds/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["ID"] == null)
            {
                return RedirectToAction("Index", "Home");

            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bnd bnd = db.Bnds.Find(id);
            if (bnd == null)
            {
                return HttpNotFound();
            }
            var typeName = (string)Session["Type"];
            var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();

            if (type.SeeAll == true)
                return View(bnd);

            return RedirectToAction("Default", "Home");

        }

        // GET: Bnds/Create
        public ActionResult Create()
        {
            if (Session["ID"] == null)
            {
                return RedirectToAction("Index", "Home");

            }
            var typeName = (string)Session["Type"];
            var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();

            if (type.Finance == true)
                return View();

            return RedirectToAction("Default", "Home");
        }

        // POST: Bnds/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(/*[Bind(Include = "id,Name,TotalNum,Month1Share,Month2Share,Month3Share,Month4Share,Month5Share,Month6Share,Month7Share,Month8Share,Month9Share,Month10Share,Month11Share,Month12Share")]*/ Bnd bnd)
        {
            if (Session["ID"] == null)
            {
                return RedirectToAction("Index", "Home");

            }
            var typeName = (string)Session["Type"];
            var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();

            if (type.Finance == true)
            {
                bnd.TotalNum = bnd.NumberOfUnits * bnd.PerUnitCost * bnd.PeriodOnMonth;
                bnd.AfterReductionNum = bnd.TotalNum;
                double MonthsNum = (double)(bnd.Month10Share + bnd.Month11Share + bnd.Month12Share + bnd.Month13Share + bnd.Month1Share + bnd.Month2Share + bnd.Month3Share + bnd.Month4Share + bnd.Month5Share + bnd.Month6Share + bnd.Month7Share + bnd.Month8Share + bnd.Month9Share);
                if (MonthsNum != bnd.TotalNum)
                {
                    ViewBag.error = "الرجاء التأكد من تساوي مجموع ميزانيات الاشهر مع الميزانية العامة";
                    return View(bnd);
                }

                try
                {
                    bnd.id = db.Bnds.OrderByDescending(x => x.id).FirstOrDefault().id + 1;
                }
                catch (Exception)
                {
                    bnd.id = 1;
                }

                if (ModelState.IsValid)
                {
                    db.Bnds.Add(bnd);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(bnd);
            }
            return RedirectToAction("Default", "Home");

        }

        // GET: Bnds/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["ID"] == null)
            {
                return RedirectToAction("Index", "Home");

            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bnd bnd = db.Bnds.Find(id);
            if (bnd == null)
            {
                return HttpNotFound();
            }
            var typeName = (string)Session["Type"];
            var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();

            if (type.Finance == true)
                return View(bnd);
            return RedirectToAction("Default", "Home");

        }

        // POST: Bnds/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Name,TotalNum,Month1Share,Month2Share,Month3Share,Month4Share,Month5Share,Month6Share,Month7Share,Month8Share,Month9Share,Month10Share,Month11Share,Month12Share")] Bnd bnd)
        {
            if (Session["ID"] == null)
            {
                return RedirectToAction("Index", "Home");

            }
            var typeName = (string)Session["Type"];
            var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
            if (type.Finance == true)
            {
                if (bnd.Month10Share + bnd.Month11Share + bnd.Month12Share + bnd.Month1Share + bnd.Month2Share + bnd.Month3Share + bnd.Month4Share + bnd.Month5Share + bnd.Month6Share + bnd.Month7Share + bnd.Month8Share + bnd.Month9Share != bnd.TotalNum)
                {
                    ViewBag.error = "الرجاء التأكد من تساوي مجموع ميزانيات الاشهر مع الميزانية العامة";
                    return View(bnd);
                }
                if (ModelState.IsValid)
                {
                    db.Entry(bnd).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(bnd);
            }
            return RedirectToAction("Default", "Home");

        }

        // GET: Bnds/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["ID"] == null)
            {
                return RedirectToAction("Index", "Home");

            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bnd bnd = db.Bnds.Find(id);
            if (bnd == null)
            {
                return HttpNotFound();
            }
            var typeName = (string)Session["Type"];
            var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();

            if (type.Finance == true)
                return View(bnd);
            return RedirectToAction("Default", "Home");

        }

        // POST: Bnds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["ID"] == null)
            {
                return RedirectToAction("Index", "Home");

            }
            var typeName = (string)Session["Type"];
            var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();

            if (type.Finance == true)
            {
                Bnd bnd = db.Bnds.Find(id);
                db.Bnds.Remove(bnd);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Default", "Home");

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
