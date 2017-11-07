﻿using System;
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
    public class CitiesController : Controller
    {
        private TaalimEntities db = new TaalimEntities();

        // GET: Cities
        public ActionResult Index()
        {
            if (Session["ID"]!=null)
            {
                var empid = Convert.ToInt32(Session["ID"]);
                var type = db.Employees.Find(empid);
                var typename = db.EmployeeTypes.Find(type.Job);
                if (typename.AddCitesAndCenters ==true)
                {
                    var cities = db.Cities.Include(c => c.Project);
                    if (TempData["Message"] != null)
                    {
                        ViewBag.StateMessage = TempData["Message"];
                    }
                    return View(cities.ToList());
                }
                return RedirectToAction("Default","Home");
            }
           return RedirectToAction("Index", "Home");

        }

        // GET: Cities/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            City city = db.Cities.Find(id);
            if (city == null)
            {
                return HttpNotFound();
            }
            return View(city);
        }

        // GET: Cities/Create
        public ActionResult Create()
        {
            if (Session["ID"]!=null)
            {

            
            var empid = Convert.ToInt32(Session["ID"]);
            var type = db.Employees.Find(empid);
            var typename = db.EmployeeTypes.Find(type.Job);
            if (typename.AddCitesAndCenters == true)
            {
                if (typename.SeeAccToCity == true)
                {
                    return RedirectToAction("Default", "Home");
                }
                else
                {
                    ViewBag.ProjectID = new SelectList(db.Projects, "id", "ProjectName");
                    return View();
                }

            }
            return RedirectToAction("Default", "Home");
            }
            return RedirectToAction("Index", "Home");
        }

        // POST: Cities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,CountryName,ProjectID")] City city)
        {

            try
            {
                city.id = db.Cities.OrderByDescending(x => x.id).FirstOrDefault().id + 1;
            }
            catch (Exception)
            {
                city.id = 1;
            }
            city.ProjectID = 1;
            if (ModelState.IsValid)
            {
                db.Cities.Add(city);
                db.SaveChanges();
                TempData["Message"] = "تم الادخال بنجاح";
                return RedirectToAction("Index");
            }

            ViewBag.ProjectID = new SelectList(db.Projects, "id", "ProjectName", city.ProjectID);
            return View(city);
        }

        // GET: Cities/Edit/5
        public ActionResult Edit(int? id)
        {

            if (Session["ID"] != null)
            {


                var empid = Convert.ToInt32(Session["ID"]);
                var type = db.Employees.Find(empid);
                var typename = db.EmployeeTypes.Find(type.Job);
                City city = db.Cities.Find(id);
                if (typename.AddCitesAndCenters == true)
                {
                    if (typename.SeeAccToCity == true)
                    {
                        return RedirectToAction("Default", "Home");
                    }
                    else
                    {
                        ViewBag.ProjectID = new SelectList(db.Projects, "id", "ProjectName", city.ProjectID);
                        return View(city);
                    }

                }
                return RedirectToAction("Default", "Home");
            }
            return RedirectToAction("Index", "Home");
        
           

        }

        // POST: Cities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Name,CountryName,ProjectID")] City city)
        {
            if (ModelState.IsValid)
            {
                db.Entry(city).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Message"] = "تم التعديل بنجاح";

                return RedirectToAction("Index");
            }
            ViewBag.ProjectID = new SelectList(db.Projects, "id", "ProjectName", city.ProjectID);
            return View(city);
        }

        // GET: Cities/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["ID"] != null)
            {


                var empid = Convert.ToInt32(Session["ID"]);
                var type = db.Employees.Find(empid);
                var typename = db.EmployeeTypes.Find(type.Job);
                City city = db.Cities.Find(id);
                if (typename.AddCitesAndCenters == true)
                {
                    if (typename.SeeAccToCity == true)
                    {
                        return RedirectToAction("Default", "Home");
                    }
                    else
                    {
                        ViewBag.ProjectID = new SelectList(db.Projects, "id", "ProjectName", city.ProjectID);
                        return View(city);
                    }

                }
                return RedirectToAction("Default", "Home");
            }
            return RedirectToAction("Index", "Home");
        }

        // POST: Cities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            City city = db.Cities.Find(id);
            db.Cities.Remove(city);
            try
            {
                db.SaveChanges();
            }
            catch
            {
                ViewBag.error = "يوجد مدخلات اخرى متعلقة بهذه المدينة يرجى تغييرها قبل الحذف";
                return View(city);
            }
            return RedirectToAction("Index");
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
