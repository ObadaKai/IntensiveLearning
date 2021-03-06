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
    public class ProjectsController : Controller
    {
        private TaalimEntities db = new TaalimEntities();

        // GET: Projects
        public ActionResult Index()
        {
            if (Session["ID"]==null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (TempData["Message"] != null)
            {
                ViewBag.StateMessage = TempData["Message"];
            }
            ViewBag.TitleSideBar = "Projects";

            return View(db.Projects.ToList());
        }

        // GET: Projects/Details/5
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
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            ViewBag.TitleSideBar = "Projects";

            return View(project);
        }

        // GET: Projects/Create
        public ActionResult Create()
        {
            ViewBag.TitleSideBar = "Projects";

            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProjectName,StartDate,EndDate")] Project project)
        {
            if (Session["ID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            try
            {
                project.id = db.Projects.OrderByDescending(x => x.id).FirstOrDefault().id + 1;
            }
            catch (Exception)
            {
                project.id = 1;
            }
            if (ModelState.IsValid)
            {
                db.Projects.Add(project);
                db.SaveChanges();
                TempData["Message"] = "تم الادخال بنجاح";
                return RedirectToAction("Index");
            }
            ViewBag.TitleSideBar = "Projects";

            return View(project);
        }

        // GET: Projects/Edit/5
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
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            ViewBag.TitleSideBar = "Projects";

            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,ProjectName,StartDate,EndDate")] Project project)
        {

            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Message"] = "تم التعديل بنجاح";
                return RedirectToAction("Index");
            }
            ViewBag.TitleSideBar = "Projects";

            return View(project);
        }

        // GET: Projects/Delete/5
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
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            ViewBag.TitleSideBar = "Projects";

            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
            try
            {
                db.SaveChanges();
            }
            catch
            {
                ViewBag.error = "يوجد مدخلات اخرى متعلقة بهذا المشروع يرجى تغييرها قبل الحذف";
                ViewBag.TitleSideBar = "Projects";

                return View(project);
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
