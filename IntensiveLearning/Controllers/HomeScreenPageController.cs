using IntensiveLearning.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IntensiveLearning.Controllers
{
    public class HomeScreenPageController : Controller
    {
        TaalimEntities db = new TaalimEntities();
        // GET: HomeScreenPage
        public ActionResult Index()
        {
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"];
            }
            return View();
        }
        public ActionResult AboutIntenlearn()
        {
            return View();
        }
        public ActionResult AboutMidad()
        {
            return View();
        }
        public ActionResult RegisterNewStudent()
        {
            ViewBag.Centerid = new SelectList(db.Centers, "id", "Name");
            ViewBag.Cityid = new SelectList(db.Cities, "id", "Name");
            ViewBag.Stageid = new SelectList(db.Stages, "id", "StageName");
            ViewBag.Periodid = new SelectList(db.Periods, "id", "Name");
            ViewBag.CType = new SelectList(db.Stages, "StageName", "StageName");

            return View();
        }
        [HttpPost]
        public ActionResult RegisterNewStudent(NonUserAddRequest nonUser)
        {
            ViewBag.Centerid = new SelectList(db.Centers, "id", "Name");
            ViewBag.Cityid = new SelectList(db.Cities, "id", "Name");
            ViewBag.Stageid = new SelectList(db.Stages, "id", "StageName");
            ViewBag.Periodid = new SelectList(db.Periods, "id", "Name");
            ViewBag.CType = new SelectList(db.Stages, "StageName", "StageName");
            if (ModelState.IsValid)
            {
                db.NonUserAddRequests.Add(nonUser);
                db.SaveChanges();
                TempData["Message"] = "لقد تم وضع  اسمك في قوائم الانتظار";
                return RedirectToAction("Index");
            }
            return View();
        }
        public ActionResult StudentInfo()
        {
            return View();
        }
    }
}