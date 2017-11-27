using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IntensiveLearning.Database;
using IntensiveLearning.Models;

namespace IntensiveLearning.Controllers
{
    public class MissionsController : Controller
    {
        private TaalimEntities db = new TaalimEntities();

        // GET: Missions
        public ActionResult Index()
        {
            if (Session["ID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // GET: Missions/Details/5
        public ActionResult Details(int? id)
        {


            if (Session["ID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return RedirectToAction("Default", "Home");
            }
            Mission mission = db.Missions.Find(id);
            var missionResponses = db.MissionResponses.Where(x => x.MissionID == id).ToList();
            var missionResposesModel = new MissionResposesModel();
            missionResposesModel.mission = mission;
            missionResposesModel.missionResponses = missionResponses;
            if (mission == null)
            {
                return HttpNotFound();
            }
            return View(missionResposesModel);
        }

        // GET: Missions/Create
        public ActionResult Create()
        {
            if (Session["ID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var empid = Convert.ToInt32(Session["ID"]);
            ViewBag.ManagerId = new SelectList(db.Employees.Where(x => x.State != "خارج الخدمة").OrderBy(x => x.id).ToList());
            ViewBag.PeopleInCharge = new MultiSelectList(db.Employees.Where(x => x.State != "خارج الخدمة").OrderBy(x => x.id).ToList());
            return View();
        }

        // POST: Missions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MissionsModel m)
        {
            if (Session["ID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                int id = Convert.ToInt32(Session["ID"]);

                if (m.PeopleInCharge != null)
                {

                    Mission mission = new Mission();
                    try
                    {
                        mission.id = db.Missions.OrderByDescending(x => x.id).FirstOrDefault().id + 1;
                    }
                    catch (Exception)
                    {
                        mission.id = 1;
                    }

                    mission.PersonEntered = id;
                    mission.Manager = m.ManagerId;
                    mission.MissionText = m.mission.MissionText;
                    mission.DateOfEntry = DateTime.Now;
                    mission.TimeOfEntry = DateTime.Now.TimeOfDay;
                    db.Missions.Add(mission);
                    if (m.PeopleInCharge != null)
                    {
                        int picNum;
                        try
                        {
                            picNum = db.MissionPersonInCharges.OrderByDescending(x => x.id).FirstOrDefault().id + 1;
                        }
                        catch (Exception)
                        {
                            picNum = 1;
                        }
                        for (int i = 0; i < m.PeopleInCharge.Length; i++)
                        {
                            MissionPersonInCharge personIncharge = new MissionPersonInCharge();
                            personIncharge.id = picNum;
                            personIncharge.MissionId = mission.id;
                            personIncharge.EmployeeID = m.PeopleInCharge[i];
                            picNum++;
                            db.MissionPersonInCharges.Add(personIncharge);
                        }
                    }
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.PeopleInChargeError = "الرجاء اختيار المسؤلين عن المهمة";
                }
            }
            var empid = Convert.ToInt32(Session["ID"]);
            var pp = db.MissionPersonInCharges.Where(x => x.MissionId == m.mission.id).Select(x => x.EmployeeID).ToList();
            ViewBag.ManagerId = new SelectList(db.Employees.Where(x => x.State != "خارج الخدمة").OrderBy(x => x.id).ToList(), m.mission.Manager);
            ViewBag.PeopleInCharge = new MultiSelectList(ViewBag.PeopleInCharge = db.Employees.Where(x => x.State != "خارج الخدمة").OrderBy(x => x.id).ToList(), pp);
            return View(m);
        }


        // GET: Missions/Edit/5
        public ActionResult Edit(int? id)
        {


            if (Session["ID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return RedirectToAction("Default", "Home");
            }
            var empid = Convert.ToInt32(Session["ID"]);
            Mission mission = db.Missions.Find(id);
            if (mission == null)
            {
                return HttpNotFound();
            }
            if (mission.Manager == empid || mission.PersonEntered == empid)
            {
                MissionsModel missionsModel = new MissionsModel();
                missionsModel.mission = mission;
                var emp = db.Employees.Find(empid);
                var pp = db.MissionPersonInCharges.Where(x => x.MissionId == id).Select(x => x.EmployeeID).ToList();
                ViewBag.ManagerId = new SelectList(db.Employees.Where(x => x.State != "خارج الخدمة").OrderBy(x => x.id).ToList(), mission.Manager);
                ViewBag.PeopleInCharge = new MultiSelectList(ViewBag.PeopleInCharge = db.Employees.Where(x => x.State != "خارج الخدمة").OrderBy(x => x.id).ToList(), pp);
                return View(missionsModel);
            }
            return RedirectToAction("Default", "Home");

        }

        // POST: Missions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MissionsModel m)
        {
            if (Session["ID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            int id = Convert.ToInt32(Session["ID"]);

            if (m.PeopleInCharge != null)
            {

                if (ModelState.IsValid)
                {
                    var mission = db.Missions.Find(m.mission.id);
                    mission.MissionText = m.mission.MissionText;
                    //Missions.Manager = db.Employees.Find(m.managerID).username;
                    mission.Manager = m.ManagerId;
                    mission.DateOfLastModification = DateTime.Now;
                    mission.TimeOfLastModification = DateTime.Now.TimeOfDay;
                    mission.Checked = null;
                    mission.Closed = null;
                    if (mission.NumberOfModifications == null)
                    {
                        mission.NumberOfModifications = 1;
                    }
                    else
                    {
                        mission.NumberOfModifications++;
                    }
                    var b = db.MissionPersonInCharges.Where(c => c.MissionId == m.mission.id).ToList();
                    foreach (var item in b)
                    {
                        db.MissionPersonInCharges.Remove(item);
                    }
                    if (m.PeopleInCharge != null)
                    {
                        int picNum;
                        try
                        {
                            picNum = db.MissionPersonInCharges.OrderByDescending(x => x.id).FirstOrDefault().id + 1;
                        }
                        catch (Exception)
                        {
                            picNum = 1;
                        }
                        for (int i = 0; i < m.PeopleInCharge.Length; i++)
                        {
                            var personIncharge = new MissionPersonInCharge();
                            personIncharge.id = picNum;
                            personIncharge.MissionId = mission.id;
                            personIncharge.EmployeeID = m.PeopleInCharge[i];
                            picNum++;
                            db.MissionPersonInCharges.Add(personIncharge);
                        }
                    }

                    db.Entry(mission).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            else
            {
                ViewBag.PeopleInChargeError = "الرجاء اختيار المسؤلين عن المهمة";
            }
            var empid = Convert.ToInt32(Session["ID"]);
            var pp = db.MissionPersonInCharges.Where(x => x.MissionId == m.mission.id).Select(x => x.EmployeeID).ToList();
            ViewBag.ManagerId = new SelectList(db.Employees.Where(x => x.State == "متوفر").Where(x => x.id != empid).OrderBy(x => x.id).ToList(), m.mission.Manager);
            ViewBag.PeopleInCharge = new MultiSelectList(ViewBag.PeopleInCharge = db.Employees.Where(x => x.State == "متوفر").Where(x => x.id != empid).OrderBy(x => x.id).ToList(), pp);
            return View(m);
        }

        // GET: Missions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mission mission = db.Missions.Find(id);
            var missionResponses = db.MissionResponses.Where(x => x.MissionID == id).ToList();
            var missionResposesModel = new MissionResposesModel();
            missionResposesModel.mission = mission;
            missionResposesModel.missionResponses = missionResponses;
            if (mission == null)
            {
                return HttpNotFound();
            }
            return View(missionResposesModel);
        }

        // POST: Missions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Mission mission = db.Missions.Find(id);
            var mpic = db.MissionPersonInCharges.Where(x => x.MissionId == id).ToList();
            foreach (var item in mpic)
            {
                db.MissionPersonInCharges.Remove(item);
            }
            db.Missions.Remove(mission);

            try
            {
                db.SaveChanges();
            }
            catch
            {
                ViewBag.error = "لا يمكن حذف مهمة قد تم التعليق عليها";
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
