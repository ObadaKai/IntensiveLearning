﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IntensiveLearning.Database;

namespace IntensiveLearning.Controllers
{
    public class ComplainsController : Controller
    {
        TaalimEntities db = new TaalimEntities();
        // GET: Complains
        public ActionResult Index()
        {
            var typeName = (string)Session["Type"];
            var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
            if (Session["ID"] != null)
            {
                if (TempData["Message"] != null)
                {
                    ViewBag.StateMessage = TempData["Message"];
                }
                if (type.SeeAll == true || type.SeeAllButFinance == true || type.SeeAccToCity==true)
                {
                    ViewBag.TitleSideBar = "Complains";

                    return View(db.Concerns.ToList());
                }
                return RedirectToAction("Default", "Home");
            }
            return RedirectToAction("Index", "Home");

        }
        [HttpGet]
        public ActionResult Create()
        {
            if (Session["ID"] != null)
            {
                ViewBag.TitleSideBar = "Complains";

                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Create(Concern concern)
        {
            try
            {
                concern.id = db.Concerns.OrderByDescending(x => x.id).FirstOrDefault().id + 1;
            }
            catch (Exception)
            {
                concern.id = 1;
            }
            concern.date = DateTime.Now.Date;
            var id = Convert.ToInt32(Session["ID"]);
            concern.Employeeid = id;


            db.Concerns.Add(concern);
            db.SaveChanges();
            TempData["Message"] = "تم تقديم الشكوى";
            return RedirectToAction("Default","Home");
        }
    }
}