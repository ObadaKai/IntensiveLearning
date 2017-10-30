using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IntensiveLearning.Database;
using IntensiveLearning.Models;

namespace IntensiveLearning.Controllers
{
    public class EmployeesController : Controller
    {
        private TaalimEntities db = new TaalimEntities();

        // GET: Employees
        public ActionResult Index()
        {
            if (Session["ID"] != null)
            {
                var typeName = (string)Session["Type"];
                var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
                var empid = Convert.ToInt32(Session["ID"]);
                var emp = db.Employees.Find(empid);
                if (type.AddNewEmployeeType == true)
                {

                    var employees = db.Employees.Where(x => x.EmployeeType.Manager == true).Include(e => e.Center).Include(e => e.EmployeeType).Include(e => e.Period);
                    return View(employees.ToList());
                }
                if (type.SeeAll == true || type.SeeAllButFinance == true)
                {
                    var employees = db.Employees.Include(e => e.Center).Include(e => e.EmployeeType).Include(e => e.Period);
                    return View(employees.ToList());
                }

                if (type.SeeAccToCity == true)
                {

                    var employees = db.Employees.Where(x => x.CityID == emp.CityID).Include(e => e.Center).Include(e => e.EmployeeType).Include(e => e.Period);
                    return View(employees.ToList());
                }
                if (type.SeeAccToCenter == true)
                {
                    var employees = db.Employees.Where(x => x.Centerid == emp.Centerid).Include(e => e.Center).Include(e => e.EmployeeType).Include(e => e.Period);
                    return View(employees.ToList());
                }
                if (type.SeeTeachers == true)
                {
                    var employees = db.Employees.Where(x => x.EmployeeType.SeeTeachers == true && x.Centerid == emp.Centerid).Include(e => e.Center).Include(e => e.EmployeeType).Include(e => e.Period);
                    return View(employees.ToList());
                }
                return RedirectToAction("Default", "Home");
            }
            return RedirectToAction("Index", "Home");
        }

        // GET: Employees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Default", "Home");
            }
            if (Session["ID"] != null)
            {
                var typeName = (string)Session["Type"];
                var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
                var empid = Convert.ToInt32(Session["ID"]);
                var emp = db.Employees.Find(empid);
                Employee employee = db.Employees.Find(id);
                if (type.SeeAll == true || type.SeeAllButFinance == true || type.AddNewEmployeeType == true)
                {
                    if (employee == null)
                    {
                        return HttpNotFound();
                    }
                    return View(employee);
                }
                if (type.SeeAccToCity == true)
                {
                    if (employee == null)
                    {
                        return HttpNotFound();
                    }
                    if (employee.CityID == emp.CityID)
                    {
                        return View(employee);
                    }
                }
                if (type.SeeAccToCenter == true)
                {
                    if (employee == null)
                    {
                        return HttpNotFound();
                    }
                    if (employee.Centerid == emp.Centerid)
                    {
                        return View(employee);
                    }
                }
                if (type.SeeTeachers == true)
                {
                    if (employee == null)
                    {
                        return HttpNotFound();
                    }

                    if (employee.Centerid == emp.Centerid && employee.EmployeeType.SeeTeachers == true)
                    {
                        return View(employee);
                    }
                }


                return RedirectToAction("Default", "Home");
            }
            return RedirectToAction("Index", "Home");


        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            if (Session["ID"] != null)
            {
                var typeName = (string)Session["Type"]; var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();

                var CAddManagers = new SelectList(db.Centers, "id", "Name");
                var CAddCOManagers = new SelectList(db.Centers, "id", "Name");
                var CAddSchoolManagers = new SelectList(db.Centers, "id", "Name");
                var CAddSchoolEmployees = new SelectList(db.Centers, "id", "Name");



                var JAddManagers = new SelectList(db.EmployeeTypes, "id", "Type");
                var JAddCOManagers = new SelectList(db.EmployeeTypes, "id", "Type");
                var JAddSchoolManagers = new SelectList(db.EmployeeTypes, "id", "Type");
                var JAddSchoolEmployees = new SelectList(db.EmployeeTypes, "id", "Type");

                var TosendCenters = new SelectList(db.Centers, "id", "Name");
                var TosendJobs = new SelectList(db.EmployeeTypes, "id", "Type");

                if (type.AddManagers != null)
                {
                    if (type.AddManagers == true)
                    {

                        CAddManagers = new SelectList(db.Centers, "id", "Name");
                        JAddManagers = new SelectList(db.EmployeeTypes.Where(x => x.Manager == true), "id", "Type");
                    }



                }
                else
                {
                    CAddManagers = null;
                    JAddManagers = null;
                }
                if (type.AddCOManagers != null)
                {
                    if (type.AddCOManagers == true)
                    {

                        CAddCOManagers = new SelectList(db.Centers, "id", "Name");
                        JAddCOManagers = new SelectList(db.EmployeeTypes.Where(x => x.CoManager == true), "id", "Type");

                    }
                }
                else
                {
                    CAddCOManagers = null;
                    JAddCOManagers = null;
                }
                if (type.AddSchoolManagers != null)
                {
                    if (type.AddSchoolManagers == true)
                    {

                        var Sid = Convert.ToInt32(Session["ID"]);
                        try
                        {
                            var emp = db.Employees.Find(Sid).CityID;
                            CAddSchoolManagers = new SelectList(db.Centers.Where(x => x.Cityid == emp), "id", "Name");
                            JAddSchoolManagers = new SelectList(db.EmployeeTypes.Where(x => x.SchoolManager == true), "id", "Type");
                        }
                        catch
                        {
                            CAddSchoolManagers = new SelectList(db.Centers, "id", "Name");
                            JAddSchoolManagers = new SelectList(db.EmployeeTypes.Where(x => x.SchoolManager == true), "id", "Type");
                        }


                    }
                }
                else
                {
                    CAddSchoolManagers = null;
                    JAddSchoolManagers = null;
                }
                if (type.AddSchoolEmployees != null)
                {
                    if (type.AddSchoolEmployees == true)
                    {

                        var Sid = Convert.ToInt32(Session["ID"]);
                        var emp = db.Employees.Find(Sid).Centerid;
                        CAddSchoolEmployees = new SelectList(db.Centers.Where(x => x.id == emp), "id", "Name");
                        JAddSchoolEmployees = new SelectList(db.EmployeeTypes.Where(x => x.NormalEmployee == true), "id", "Type");

                    }
                }
                else
                {
                    CAddSchoolEmployees = null;
                    JAddSchoolEmployees = null;
                }
                if (type.AddManagers == null && type.AddCOManagers == null && type.AddSchoolManagers == null && type.AddSchoolEmployees == null)
                {
                    return RedirectToAction("Default", "Home");
                }

                if (JAddManagers != null)
                {
                    TosendJobs = JAddManagers;
                }
                else if (JAddCOManagers != null)
                {
                    TosendJobs = JAddCOManagers;
                }
                else if (JAddSchoolManagers != null)
                {
                    TosendJobs = JAddSchoolManagers;
                }
                else if (JAddSchoolEmployees != null)
                {
                    TosendJobs = JAddSchoolEmployees;

                }

                if (JAddManagers != null)
                {
                    TosendJobs.Union(JAddManagers);
                }
                if (JAddCOManagers != null)
                {
                    TosendJobs.Union(JAddCOManagers);
                }
                if (JAddSchoolManagers != null)
                {
                    TosendJobs.Union(JAddSchoolManagers);
                }
                if (JAddSchoolEmployees != null)
                {
                    TosendJobs.Union(JAddSchoolEmployees);
                }


                if (CAddManagers != null)
                {
                    TosendCenters = CAddManagers;
                }
                else if (CAddCOManagers != null)
                {
                    TosendCenters = CAddCOManagers;
                }
                else if (CAddSchoolManagers != null)
                {
                    TosendCenters = CAddSchoolManagers;
                }
                else if (CAddSchoolEmployees != null)
                {
                    TosendCenters = CAddSchoolEmployees;

                }

                if (CAddManagers != null)
                {
                    TosendCenters.Union(CAddManagers);
                }
                if (CAddCOManagers != null)
                {
                    TosendCenters.Union(CAddCOManagers);
                }
                if (CAddSchoolManagers != null)
                {
                    TosendCenters.Union(CAddSchoolManagers);
                }
                if (CAddSchoolEmployees != null)
                {
                    TosendCenters.Union(CAddSchoolEmployees);
                }


                ViewBag.Centerid = TosendCenters;
                ViewBag.Job = TosendJobs;
                ViewBag.Periodid = new SelectList(db.Periods, "id", "Name");
                return View();


            }
            return RedirectToAction("Index", "Home");


        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "name,surname,BDate,Certificate,CType,State,Centerid,Periodid,SDate,Job,Username,Salary")] Employee employee, HttpPostedFileBase file)
        {
            if (db.Employees.Where(x => x.Username == employee.Username).Count() > 0)
            {
                ViewBag.error = "اسم المستخم مستخدم مسبقا";
                return View(employee);
            }

            try
            {

                if (file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/App_Data/Employees"), fileName);
                    file.SaveAs(path);
                    employee.Proof = path;
                }
                else
                {
                    ViewBag.error = "يرجى ارفاق الاثبات كملف خارجي";
                    return View(employee);
                }
            }
            catch
            {

            }
            try
            {
                employee.id = db.Employees.OrderByDescending(x => x.id).FirstOrDefault().id + 1;
            }
            catch (Exception)
            {
                employee.id = 1;
            }
            employee.Password = Helper.ComputeHash("123", "SHA512", null);
            if (ModelState.IsValid)
            {
                db.Employees.Add(employee);
                try
                {
                    // Your code...
                    // Could also be before try if you know the exception occurs in SaveChanges

                    db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }
                return RedirectToAction("Index");
            }

            var typeName = (string)Session["Type"];
            var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();

            var CAddManagers = new SelectList(db.Centers, "id", "Name");
            var CAddCOManagers = new SelectList(db.Centers, "id", "Name");
            var CAddSchoolManagers = new SelectList(db.Centers, "id", "Name");
            var CAddSchoolEmployees = new SelectList(db.Centers, "id", "Name");



            var JAddManagers = new SelectList(db.EmployeeTypes, "id", "Type");
            var JAddCOManagers = new SelectList(db.EmployeeTypes, "id", "Type");
            var JAddSchoolManagers = new SelectList(db.EmployeeTypes, "id", "Type");
            var JAddSchoolEmployees = new SelectList(db.EmployeeTypes, "id", "Type");

            var TosendCenters = new SelectList(db.Centers, "id", "Name");
            var TosendJobs = new SelectList(db.EmployeeTypes, "id", "Type");

            if (type.AddManagers != null)
            {
                if (type.AddManagers == true)
                {

                    CAddManagers = new SelectList(db.Centers, "id", "Name");
                    JAddManagers = new SelectList(db.EmployeeTypes.Where(x => x.Manager == true), "id", "Type");
                }



            }
            if (type.AddCOManagers != null)
            {
                if (type.AddCOManagers == true)
                {

                    CAddCOManagers = new SelectList(db.Centers, "id", "Name");
                    JAddCOManagers = new SelectList(db.EmployeeTypes.Where(x => x.CoManager == true), "id", "Type");

                }
            }
            if (type.AddSchoolManagers != null)
            {
                if (type.AddSchoolManagers == true)
                {

                    var Sid = Convert.ToInt32(Session["ID"]);
                    try
                    {
                        var emp = db.Employees.Find(Sid).CityID;
                        CAddSchoolManagers = new SelectList(db.Centers.Where(x => x.Cityid == emp), "id", "Name");
                        JAddSchoolManagers = new SelectList(db.EmployeeTypes.Where(x => x.SchoolManager == true), "id", "Type");
                    }
                    catch
                    {

                    }
                    CAddSchoolManagers = new SelectList(db.Centers, "id", "Name");
                    JAddSchoolManagers = new SelectList(db.EmployeeTypes.Where(x => x.SchoolManager == true), "id", "Type");

                }
            }
            if (type.AddSchoolEmployees != null)
            {
                if (type.AddSchoolEmployees == true)
                {

                    var Sid = Convert.ToInt32(Session["ID"]);
                    var emp = db.Employees.Find(Sid).Centerid;
                    CAddSchoolEmployees = new SelectList(db.Centers.Where(x => x.id == emp), "id", "Name");
                    JAddSchoolEmployees = new SelectList(db.EmployeeTypes.Where(x => x.NormalEmployee == true), "id", "Type");

                }
            }
            if (type.AddManagers == null && type.AddCOManagers == null && type.AddSchoolManagers == null && type.AddSchoolEmployees == null)
            {
                return RedirectToAction("Default", "Home");
            }

            if (JAddManagers != null)
            {
                TosendJobs = JAddManagers;
            }
            else if (JAddCOManagers != null)
            {
                TosendJobs = JAddCOManagers;
            }
            else if (JAddSchoolManagers != null)
            {
                TosendJobs = JAddSchoolManagers;
            }
            else if (JAddSchoolEmployees != null)
            {
                TosendJobs = JAddSchoolEmployees;

            }

            if (JAddManagers != null)
            {
                TosendJobs.Union(JAddManagers);
            }
            if (JAddCOManagers != null)
            {
                TosendJobs.Union(JAddCOManagers);
            }
            if (JAddSchoolManagers != null)
            {
                TosendJobs.Union(JAddSchoolManagers);
            }
            if (JAddSchoolEmployees != null)
            {
                TosendJobs.Union(JAddSchoolEmployees);
            }


            if (CAddManagers != null)
            {
                TosendCenters = CAddManagers;
            }
            else if (CAddCOManagers != null)
            {
                TosendCenters = CAddCOManagers;
            }
            else if (CAddSchoolManagers != null)
            {
                TosendCenters = CAddSchoolManagers;
            }
            else if (CAddSchoolEmployees != null)
            {
                TosendCenters = CAddSchoolEmployees;

            }

            if (CAddManagers != null)
            {
                TosendCenters.Union(CAddManagers);
            }
            if (CAddCOManagers != null)
            {
                TosendCenters.Union(CAddCOManagers);
            }
            if (CAddSchoolManagers != null)
            {
                TosendCenters.Union(CAddSchoolManagers);
            }
            if (CAddSchoolEmployees != null)
            {
                TosendCenters.Union(CAddSchoolEmployees);
            }


            ViewBag.Centerid = TosendCenters;
            ViewBag.Job = TosendJobs;
            ViewBag.Periodid = new SelectList(db.Periods, "id", "Name");
            return View(employee);
        }

        // GET: Employees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Default", "Home");
            }
            if (Session["ID"] != null)
            {
                var typeName = (string)Session["Type"]; var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();


                Employee employee = db.Employees.Find(id);


                var CAddManagers = new SelectList(db.Centers, "id", "Name");
                var CAddCOManagers = new SelectList(db.Centers, "id", "Name");
                var CAddSchoolManagers = new SelectList(db.Centers, "id", "Name");
                var CAddSchoolEmployees = new SelectList(db.Centers, "id", "Name");



                var JAddManagers = new SelectList(db.EmployeeTypes, "id", "Type");
                var JAddCOManagers = new SelectList(db.EmployeeTypes, "id", "Type");
                var JAddSchoolManagers = new SelectList(db.EmployeeTypes, "id", "Type");
                var JAddSchoolEmployees = new SelectList(db.EmployeeTypes, "id", "Type");

                var TosendCenters = new SelectList(db.Centers, "id", "Name");
                var TosendJobs = new SelectList(db.EmployeeTypes, "id", "Type");

                if (type.AddManagers != null)
                {
                    if (type.AddManagers == true)
                    {

                        CAddManagers = new SelectList(db.Centers, "id", "Name");
                        JAddManagers = new SelectList(db.EmployeeTypes.Where(x => x.Manager == true), "id", "Type");
                    }



                }
                if (type.AddCOManagers != null)
                {
                    if (type.AddCOManagers == true)
                    {

                        CAddCOManagers = new SelectList(db.Centers, "id", "Name");
                        JAddCOManagers = new SelectList(db.EmployeeTypes.Where(x => x.CoManager == true), "id", "Type");

                    }
                }
                if (type.AddSchoolManagers != null)
                {
                    if (type.AddSchoolManagers == true)
                    {

                        var Sid = Convert.ToInt32(Session["ID"]);
                        try
                        {
                            var emp = db.Employees.Find(Sid).CityID;
                            CAddSchoolManagers = new SelectList(db.Centers.Where(x => x.Cityid == emp), "id", "Name");
                            JAddSchoolManagers = new SelectList(db.EmployeeTypes.Where(x => x.SchoolManager == true), "id", "Type");
                        }
                        catch
                        {

                        }
                        CAddSchoolManagers = new SelectList(db.Centers, "id", "Name");
                        JAddSchoolManagers = new SelectList(db.EmployeeTypes.Where(x => x.SchoolManager == true), "id", "Type");

                    }
                }
                if (type.AddSchoolEmployees != null)
                {
                    if (type.AddSchoolEmployees == true)
                    {

                        var Sid = Convert.ToInt32(Session["ID"]);
                        var emp = db.Employees.Find(Sid).Centerid;
                        CAddSchoolEmployees = new SelectList(db.Centers.Where(x => x.id == emp), "id", "Name");
                        JAddSchoolEmployees = new SelectList(db.EmployeeTypes.Where(x => x.NormalEmployee == true), "id", "Type");

                    }
                }
                if (type.AddManagers == null && type.AddCOManagers == null && type.AddSchoolManagers == null && type.AddSchoolEmployees == null)
                {
                    return RedirectToAction("Default", "Home");
                }

                if (JAddManagers != null)
                {
                    TosendJobs = JAddManagers;
                }
                else if (JAddCOManagers != null)
                {
                    TosendJobs = JAddCOManagers;
                }
                else if (JAddSchoolManagers != null)
                {
                    TosendJobs = JAddSchoolManagers;
                }
                else if (JAddSchoolEmployees != null)
                {
                    TosendJobs = JAddSchoolEmployees;

                }

                if (JAddManagers != null)
                {
                    TosendJobs.Union(JAddManagers);
                }
                if (JAddCOManagers != null)
                {
                    TosendJobs.Union(JAddCOManagers);
                }
                if (JAddSchoolManagers != null)
                {
                    TosendJobs.Union(JAddSchoolManagers);
                }
                if (JAddSchoolEmployees != null)
                {
                    TosendJobs.Union(JAddSchoolEmployees);
                }


                if (CAddManagers != null)
                {
                    TosendCenters = CAddManagers;
                }
                else if (CAddCOManagers != null)
                {
                    TosendCenters = CAddCOManagers;
                }
                else if (CAddSchoolManagers != null)
                {
                    TosendCenters = CAddSchoolManagers;
                }
                else if (CAddSchoolEmployees != null)
                {
                    TosendCenters = CAddSchoolEmployees;

                }

                if (CAddManagers != null)
                {
                    TosendCenters.Union(CAddManagers);
                }
                if (CAddCOManagers != null)
                {
                    TosendCenters.Union(CAddCOManagers);
                }
                if (CAddSchoolManagers != null)
                {
                    TosendCenters.Union(CAddSchoolManagers);
                }
                if (CAddSchoolEmployees != null)
                {
                    TosendCenters.Union(CAddSchoolEmployees);
                }

                ViewBag.Centerid = TosendCenters;
                ViewBag.Job = TosendJobs;
                ViewBag.Periodid = new SelectList(db.Periods, "id", "Name");
                return View(employee);

            }
            return RedirectToAction("Index", "Home");


        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,surname,BDate,Certificate,CType,State,Centerid,Periodid,SDate,EDate,Job,Salary")] Employee employee, HttpPostedFileBase file)
        {
            var typeName = (string)Session["Type"]; var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
            try
            {
                if (file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/App_Data/Employees"), fileName);
                    Employee mystd = db.Employees.Find(employee.id);
                    string EmpProof = mystd.Proof;
                    if ((System.IO.File.Exists(EmpProof)))
                    {
                        System.IO.File.Delete(EmpProof);
                    }
                    file.SaveAs(path);
                    employee.Proof = path;
                    db.Entry(mystd).State = EntityState.Detached;
                }
                ViewBag.Message = "Upload successful";
            }
            catch
            {

            }

            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var CAddManagers = new SelectList(db.Centers, "id", "Name");
            var CAddCOManagers = new SelectList(db.Centers, "id", "Name");
            var CAddSchoolManagers = new SelectList(db.Centers, "id", "Name");
            var CAddSchoolEmployees = new SelectList(db.Centers, "id", "Name");



            var JAddManagers = new SelectList(db.EmployeeTypes, "id", "Type");
            var JAddCOManagers = new SelectList(db.EmployeeTypes, "id", "Type");
            var JAddSchoolManagers = new SelectList(db.EmployeeTypes, "id", "Type");
            var JAddSchoolEmployees = new SelectList(db.EmployeeTypes, "id", "Type");

            var TosendCenters = new SelectList(db.Centers, "id", "Name");
            var TosendJobs = new SelectList(db.EmployeeTypes, "id", "Type");

            if (type.AddManagers != null)
            {
                if (type.AddManagers == true)
                {

                    CAddManagers = new SelectList(db.Centers, "id", "Name");
                    JAddManagers = new SelectList(db.EmployeeTypes.Where(x => x.Manager == true), "id", "Type");
                }



            }
            if (type.AddCOManagers != null)
            {
                if (type.AddCOManagers == true)
                {

                    CAddCOManagers = new SelectList(db.Centers, "id", "Name");
                    JAddCOManagers = new SelectList(db.EmployeeTypes.Where(x => x.CoManager == true), "id", "Type");

                }
            }
            if (type.AddSchoolManagers != null)
            {
                if (type.AddSchoolManagers == true)
                {

                    var Sid = Convert.ToInt32(Session["ID"]);
                    try
                    {
                        var emp = db.Employees.Find(Sid).CityID;
                        CAddSchoolManagers = new SelectList(db.Centers.Where(x => x.Cityid == emp), "id", "Name");
                        JAddSchoolManagers = new SelectList(db.EmployeeTypes.Where(x => x.SchoolManager == true), "id", "Type");
                    }
                    catch
                    {

                    }
                    CAddSchoolManagers = new SelectList(db.Centers, "id", "Name");
                    JAddSchoolManagers = new SelectList(db.EmployeeTypes.Where(x => x.SchoolManager == true), "id", "Type");

                }
            }
            if (type.AddSchoolEmployees != null)
            {
                if (type.AddSchoolEmployees == true)
                {

                    var Sid = Convert.ToInt32(Session["ID"]);
                    var emp = db.Employees.Find(Sid).Centerid;
                    CAddSchoolEmployees = new SelectList(db.Centers.Where(x => x.id == emp), "id", "Name");
                    JAddSchoolEmployees = new SelectList(db.EmployeeTypes.Where(x => x.NormalEmployee == true), "id", "Type");

                }
            }
            if (type.AddManagers == null && type.AddCOManagers == null && type.AddSchoolManagers == null && type.AddSchoolEmployees == null)
            {
                return RedirectToAction("Default", "Home");
            }

            if (JAddManagers != null)
            {
                TosendJobs = JAddManagers;
            }
            else if (JAddCOManagers != null)
            {
                TosendJobs = JAddCOManagers;
            }
            else if (JAddSchoolManagers != null)
            {
                TosendJobs = JAddSchoolManagers;
            }
            else if (JAddSchoolEmployees != null)
            {
                TosendJobs = JAddSchoolEmployees;

            }

            if (JAddManagers != null)
            {
                TosendJobs.Union(JAddManagers);
            }
            if (JAddCOManagers != null)
            {
                TosendJobs.Union(JAddCOManagers);
            }
            if (JAddSchoolManagers != null)
            {
                TosendJobs.Union(JAddSchoolManagers);
            }
            if (JAddSchoolEmployees != null)
            {
                TosendJobs.Union(JAddSchoolEmployees);
            }


            if (CAddManagers != null)
            {
                TosendCenters = CAddManagers;
            }
            else if (CAddCOManagers != null)
            {
                TosendCenters = CAddCOManagers;
            }
            else if (CAddSchoolManagers != null)
            {
                TosendCenters = CAddSchoolManagers;
            }
            else if (CAddSchoolEmployees != null)
            {
                TosendCenters = CAddSchoolEmployees;

            }

            if (CAddManagers != null)
            {
                TosendCenters.Union(CAddManagers);
            }
            if (CAddCOManagers != null)
            {
                TosendCenters.Union(CAddCOManagers);
            }
            if (CAddSchoolManagers != null)
            {
                TosendCenters.Union(CAddSchoolManagers);
            }
            if (CAddSchoolEmployees != null)
            {
                TosendCenters.Union(CAddSchoolEmployees);
            }
            ViewBag.Centerid = TosendCenters;
            ViewBag.Job = TosendJobs;
            ViewBag.Periodid = new SelectList(db.Periods, "id", "Name");
            return View(employee);
        }

        // GET: Employees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Default", "Home");
            }
            if (Session["ID"] != null)
            {
                var typeName = (string)Session["Type"]; var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
                if (type.AddManagers == true)
                {
                    Employee employee = db.Employees.Find(id);
                    if (employee == null)
                    {
                        return HttpNotFound();
                    }

                    ViewBag.Centerid = new SelectList(db.Centers, "id", "Name");
                    ViewBag.Job = new SelectList(db.EmployeeTypes.Where(x => x.Manager == true), "id", "Type");
                    ViewBag.Periodid = new SelectList(db.Periods, "id", "Name");
                    return View(employee);

                }
                if (type.AddCOManagers == true)
                {
                    Employee employee = db.Employees.Find(id);
                    if (employee == null)
                    {
                        return HttpNotFound();
                    }

                    ViewBag.Centerid = new SelectList(db.Centers, "id", "Name");
                    ViewBag.Job = new SelectList(db.EmployeeTypes.Where(x => x.CoManager == true), "id", "Type");
                    ViewBag.Periodid = new SelectList(db.Periods, "id", "Name");
                    return View(employee);

                }
                if (type.AddSchoolManagers == true)
                {

                    var Sid = Convert.ToInt32(Session["ID"]);
                    Employee employee = db.Employees.Find(id);
                    if (employee == null)
                    {
                        return HttpNotFound();
                    }

                    try
                    {
                        var emp = db.Employees.Where(x => x.id == Sid).FirstOrDefault().CityID;
                        ViewBag.Centerid = new SelectList(db.Centers.Where(x => x.Cityid == emp), "id", "Name");
                        ViewBag.Job = new SelectList(db.EmployeeTypes.Where(x => x.SchoolManager == true), "id", "Type");
                        ViewBag.Periodid = new SelectList(db.Periods, "id", "Name");
                        return View(employee);
                    }
                    catch
                    {

                    }
                    ViewBag.Centerid = new SelectList(db.Centers, "id", "Name");
                    ViewBag.Job = new SelectList(db.EmployeeTypes.Where(x => x.SchoolManager == true), "id", "Type");
                    ViewBag.Periodid = new SelectList(db.Periods, "id", "Name");
                    return View(employee);

                }
                if (type.AddSchoolEmployees == true)
                {
                    Employee employee = db.Employees.Find(id);
                    if (employee == null)
                    {
                        return HttpNotFound();
                    }

                    var Sid = Convert.ToInt32(Session["ID"]);
                    var emp = db.Employees.Where(x => x.id == Sid).FirstOrDefault().Centerid;
                    ViewBag.Centerid = new SelectList(db.Centers.Where(x => x.id == emp), "id", "Name");
                    ViewBag.Job = new SelectList(db.EmployeeTypes.Where(x => x.NormalEmployee == true), "id", "Type");
                    ViewBag.Periodid = new SelectList(db.Periods, "id", "Name");
                    return View(employee);

                }


                return RedirectToAction("Default", "Home");
            }
            return RedirectToAction("Index", "Home");

        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["ID"] != null)
            {
                var typeName = (string)Session["Type"];
                var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();

                if (type.AddManagers == true)
                {
                    Employee employee = db.Employees.Find(id);
                    if (employee == null)
                    {
                        return HttpNotFound();
                    }
                    if (employee.EmployeeType.Manager == true)
                    {

                        db.Employees.Remove(employee);
                        try
                        {
                            db.SaveChanges();
                        }
                        catch
                        {
                            ViewBag.error = "يوجد مدخلات اخرى متعلقة بهذا الموظف يرجى تغييرها قبل الحذف";
                            return View();
                        }
                        return RedirectToAction("Index");
                    }
                }

                if (type.AddCOManagers == true)
                {
                    Employee employee = db.Employees.Find(id);
                    if (employee == null)
                    {
                        return HttpNotFound();
                    }
                    if (employee.EmployeeType.CoManager == true)
                    {

                        db.Employees.Remove(employee);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }

                if (type.AddSchoolManagers == true)
                {
                    Employee employee = db.Employees.Find(id);
                    if (employee == null)
                    {
                        return HttpNotFound();
                    }
                    if (employee.EmployeeType.SchoolManager == true)
                    {

                        db.Employees.Remove(employee);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }

                if (type.AddSchoolEmployees == true)
                {
                    Employee employee = db.Employees.Find(id);
                    if (employee == null)
                    {
                        return HttpNotFound();
                    }
                    if (employee.EmployeeType.NormalEmployee == true)
                    {

                        db.Employees.Remove(employee);
                        try
                        {
                            db.SaveChanges();
                        }
                        catch
                        {
                            ViewBag.error = "يوجد مدخلات خرى متعلقة بهذا الموظف يرجى تغييرها قبل الحذف";
                            return View(employee);
                        }
                        return RedirectToAction("Index");
                    }


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
