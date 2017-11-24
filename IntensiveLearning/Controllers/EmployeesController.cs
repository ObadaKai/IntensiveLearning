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
using System.IO.Compression;

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
                if (TempData["Message"] != null)
                {
                    ViewBag.StateMessage = TempData["Message"];
                }

                return View();
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

                var CAddManagers = db.Centers.ToList();
                var CAddCOManagers = db.Centers.ToList();
                var CAddSchoolManagers = db.Centers.ToList();
                var CAddSchoolEmployees = db.Centers.ToList();


                var JAddManagers = db.EmployeeTypes.ToList();
                var JAddCOManagers = db.EmployeeTypes.ToList();
                var JAddSchoolManagers = db.EmployeeTypes.ToList();
                var JAddSchoolEmployees = db.EmployeeTypes.ToList();
                var JAddNone = db.EmployeeTypes.Where(x => x.Manager == null && x.SchoolManager == null && x.NormalEmployee == null && x.CoManager == null).ToList();

                var TosendCenters = db.Centers.ToList();
                var TosendJobs = db.EmployeeTypes.ToList();

                if (type.AddManagers != null)
                {
                    if (type.AddManagers == true)
                    {

                        CAddManagers = db.Centers.ToList();
                        JAddManagers = db.EmployeeTypes.Where(x => x.Manager == true).ToList();
                    }

                    else
                    {
                        CAddManagers = null;
                        JAddManagers = null;
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

                        CAddCOManagers = db.Centers.ToList();
                        JAddCOManagers = db.EmployeeTypes.Where(x => x.CoManager == true).ToList();

                    }
                    else
                    {
                        CAddCOManagers = null;
                        JAddCOManagers = null;
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
                            if (type.SeeAccToCity == true)
                            {
                                var emp = db.Employees.Find(Sid).CityID;
                                CAddSchoolManagers = db.Centers.Where(x => x.Cityid == emp).ToList();
                                JAddSchoolManagers = db.EmployeeTypes.Where(x => x.SchoolManager == true || x.NormalEmployee == true).ToList();

                            }
                            else
                            {
                                CAddSchoolManagers = db.Centers.ToList();
                                JAddSchoolManagers = db.EmployeeTypes.Where(x => x.SchoolManager == true || x.SeeAccToCity == true || x.NormalEmployee == true).ToList();
                            }
                        }
                        catch
                        {
                            CAddSchoolManagers = db.Centers.ToList();
                            JAddSchoolManagers = db.EmployeeTypes.Where(x => x.SchoolManager == true).ToList();
                        }


                    }
                    else
                    {
                        CAddSchoolManagers = null;
                        JAddSchoolManagers = null;
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
                        CAddSchoolEmployees = db.Centers.Where(x => x.id == emp).ToList();
                        JAddSchoolEmployees = db.EmployeeTypes.Where(x => x.NormalEmployee == true).ToList();

                    }
                    else
                    {
                        CAddSchoolEmployees = null;
                        JAddSchoolEmployees = null;
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
                    TosendJobs = TosendJobs.Union(JAddManagers).ToList();
                }
                if (JAddCOManagers != null)
                {
                    TosendJobs = TosendJobs.Union(JAddCOManagers).ToList();
                }
                if (JAddSchoolManagers != null)
                {
                    TosendJobs = TosendJobs.Union(JAddSchoolManagers).ToList();
                }
                if (JAddSchoolEmployees != null)
                {
                    TosendJobs = TosendJobs.Union(JAddSchoolEmployees).ToList();
                }
                TosendJobs = TosendJobs.Union(JAddNone).ToList();

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
                    TosendCenters = TosendCenters.Union(CAddManagers).ToList();
                }
                if (CAddCOManagers != null)
                {
                    TosendCenters = TosendCenters.Union(CAddCOManagers).ToList();
                }
                if (CAddSchoolManagers != null)
                {
                    TosendCenters = TosendCenters.Union(CAddSchoolManagers).ToList();
                }
                if (CAddSchoolEmployees != null)
                {
                    TosendCenters = TosendCenters.Union(CAddSchoolEmployees).ToList();
                }



                ViewBag.Cityid = new SelectList(db.Cities, "id", "Name");
                ViewBag.Centerid = new SelectList(TosendCenters, "id", "Name");
                ViewBag.Jobs = TosendJobs;
                ViewBag.Periodid = new SelectList(db.Periods, "id", "Name");
                return View();


            }
            return RedirectToAction("Index", "Home");


        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(/*[Bind(Include = "name,surname,BDate,Certificate,CType,State,Centerid,Periodid,SDate,Job,Username,Salary,FathersName,Sex")]*/ Employee employee, IEnumerable<HttpPostedFileBase> file)
        {
            if (Session["ID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var typeName = (string)Session["Type"];
            var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
            var jobs = db.EmployeeTypes.Find(employee.Job);
            bool sendImageError = false;
            if (ModelState.IsValid)
            {
                bool proceed = true;
                if (jobs.Manager == true)
                {
                    employee.Centerid = null;
                    employee.CityID = null;
                }
                if (jobs.CoManager == true && jobs.SeeAccToCity != true)
                {
                    employee.Centerid = null;
                    employee.CityID = null;
                }
                if (jobs.CoManager == true && jobs.SeeAccToCity == true)
                {
                    if (employee.CityID == null)
                    {
                        ViewBag.error = "يرجى اختيار المدينة";
                        return View(employee);
                    }
                    employee.Centerid = null;
                }
                if (jobs.SeeAccToCenter == true || jobs.SeeTeachers == true)
                {
                    if (employee.Centerid == null)
                    {
                        ViewBag.error = "يرجى اختيار المركز";
                        return View(employee);
                    }
                    employee.CityID = null;
                }
                if (db.Employees.Where(x => x.Username == employee.Username).Count() > 0)
                {
                    ViewBag.error = "اسم المستخم مستخدم مسبقا";
                    return View(employee);
                }


                try
                {
                    employee.id = db.Employees.OrderByDescending(x => x.id).FirstOrDefault().id + 1;
                }
                catch (Exception)
                {
                    employee.id = 1;
                }

                int prooveid;
                try
                {
                    prooveid = db.Prooves.OrderByDescending(x => x.id).FirstOrDefault().id + 1;
                }
                catch (Exception)
                {
                    prooveid = 1;
                }
                try
                {

                    if (file.Count() > 0)
                    {
                        var oldImages = db.Prooves.Where(x => x.EmployeeID == employee.id).ToList();

                        foreach (var image in oldImages)
                        {

                            db.Prooves.Remove(image);
                        }

                        if ((Directory.Exists(Server.MapPath("~/App_Data/Employees") + "\\" + employee.id)))
                        {
                            try
                            {
                                Directory.Delete(Server.MapPath("~/App_Data/Employees") + "\\" + employee.id, true);
                            }
                            catch (IOException)
                            {
                                Directory.Delete(Server.MapPath("~/App_Data/Employees") + "\\" + employee.id, true);
                            }
                            catch (UnauthorizedAccessException)
                            {
                                Directory.Delete(Server.MapPath("~/App_Data/Employees") + "\\" + employee.id, true);
                            }
                        }

                        if (!Directory.Exists(Server.MapPath("~/App_Data/Employees") + "\\" + employee.id))
                        {
                            Directory.CreateDirectory(Server.MapPath("~/App_Data/Employees") + "\\" + employee.id);
                        }

                    }

                    foreach (var item in file)
                    {
                        if (item.ContentLength > 0)
                        {
                            var fileName = Path.GetFileName(item.FileName);
                            if (!Directory.Exists(Server.MapPath("~/App_Data/Employees") + "\\" + employee.id))
                            {
                                Directory.CreateDirectory(Server.MapPath("~/App_Data/Employees") + "\\" + employee.id);
                            }
                            var path = Path.Combine(Server.MapPath("~/App_Data/Employees/" + employee.id), fileName);
                            item.SaveAs(path);
                            Proove proove = new Proove();
                            proove.Path = path;
                            proove.id = prooveid;
                            proove.EmployeeID = employee.id;
                            db.Prooves.Add(proove);
                            prooveid++;

                        }
                        else
                        {

                        }
                    }
                    try
                    {
                        var startPath = Server.MapPath("~/App_Data/Employees" + "\\" + employee.id);

                        if (!Directory.Exists(Server.MapPath("~/App_Data/Employees" + "\\" + "ZipFolder")))
                        {
                            Directory.CreateDirectory(Server.MapPath("~/App_Data/Employees" + "\\" + "ZipFolder"));
                        }
                        var zipPath = Server.MapPath("~/App_Data/Employees" + "\\" + "ZipFolder") + "\\" + employee.id + ".zip";

                        if (System.IO.File.Exists(zipPath))
                        {
                            System.IO.File.Delete(zipPath);
                        }
                        try
                        {
                            ZipFile.CreateFromDirectory(startPath, zipPath);
                        }
                        catch { }
                        employee.Proof = zipPath;
                    }
                    catch { }


                }
                catch
                {

                }
                employee.AddedBy = Convert.ToInt32(Session["ID"]);
                employee.AddingDate = DateTime.Now.Date;
                employee.AddingTime = DateTime.Now.TimeOfDay;

                employee.State = "متوفر";
                employee.Password = Helper.ComputeHash("123", "SHA512", null);

                var id = Convert.ToInt32(Session["ID"]);
                var emp = db.Employees.Find(id);
                if (proceed)
                {

                    db.Employees.Add(employee);
                    try
                    {
                        // Your code...
                        // Could also be before try if you know the exception occurs in SaveChanges

                        db.SaveChanges();
                        TempData["Message"] = "تم الادخال بنجاح";

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
            }



            var CAddManagers = db.Centers.ToList();
            var CAddCOManagers = db.Centers.ToList();
            var CAddSchoolManagers = db.Centers.ToList();
            var CAddSchoolEmployees = db.Centers.ToList();



            var JAddManagers = db.EmployeeTypes.ToList();
            var JAddCOManagers = db.EmployeeTypes.ToList();
            var JAddSchoolManagers = db.EmployeeTypes.ToList();
            var JAddSchoolEmployees = db.EmployeeTypes.ToList();
            var JAddNone = db.EmployeeTypes.Where(x => x.Manager == null && x.SchoolManager == null && x.NormalEmployee == null && x.CoManager == null).ToList();

            var TosendCenters = db.Centers.ToList();
            var TosendJobs = db.EmployeeTypes.ToList();

            if (type.AddManagers != null)
            {
                if (type.AddManagers == true)
                {

                    CAddManagers = db.Centers.ToList();
                    JAddManagers = db.EmployeeTypes.Where(x => x.Manager == true).ToList();
                }

                else
                {
                    CAddManagers = null;
                    JAddManagers = null;
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

                    CAddCOManagers = db.Centers.ToList();
                    JAddCOManagers = db.EmployeeTypes.Where(x => x.CoManager == true).ToList();

                }
                else
                {
                    CAddCOManagers = null;
                    JAddCOManagers = null;
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
                        if (type.SeeAccToCenter == true || type.SeeAccToCity == true)
                        {
                            var emp = db.Employees.Find(Sid).CityID;
                            CAddSchoolManagers = db.Centers.Where(x => x.Cityid == emp).ToList();
                            JAddSchoolManagers = db.EmployeeTypes.Where(x => x.SchoolManager == true || x.NormalEmployee == true).ToList();

                        }
                        else
                        {
                            CAddSchoolManagers = db.Centers.ToList();
                            JAddSchoolManagers = db.EmployeeTypes.Where(x => x.SchoolManager == true || x.SeeAccToCity == true || x.NormalEmployee == true).ToList();
                        }
                    }
                    catch
                    {
                        CAddSchoolManagers = db.Centers.ToList();
                        JAddSchoolManagers = db.EmployeeTypes.Where(x => x.SchoolManager == true).ToList();
                    }


                }
                else
                {
                    CAddSchoolManagers = null;
                    JAddSchoolManagers = null;
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
                    CAddSchoolEmployees = db.Centers.Where(x => x.id == emp).ToList();
                    JAddSchoolEmployees = db.EmployeeTypes.Where(x => x.NormalEmployee == true).ToList();

                }
                else
                {
                    CAddSchoolEmployees = null;
                    JAddSchoolEmployees = null;
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
                TosendJobs = TosendJobs.Union(JAddManagers).ToList();
            }
            if (JAddCOManagers != null)
            {
                TosendJobs = TosendJobs.Union(JAddCOManagers).ToList();
            }
            if (JAddSchoolManagers != null)
            {
                TosendJobs = TosendJobs.Union(JAddSchoolManagers).ToList();
            }
            if (JAddSchoolEmployees != null)
            {
                TosendJobs = TosendJobs.Union(JAddSchoolEmployees).ToList();
            }
            TosendJobs = TosendJobs.Union(JAddNone).ToList();


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
                TosendCenters = TosendCenters.Union(CAddManagers).ToList();
            }
            if (CAddCOManagers != null)
            {
                TosendCenters = TosendCenters.Union(CAddCOManagers).ToList();
            }
            if (CAddSchoolManagers != null)
            {
                TosendCenters = TosendCenters.Union(CAddSchoolManagers).ToList();
            }
            if (CAddSchoolEmployees != null)
            {
                TosendCenters = TosendCenters.Union(CAddSchoolEmployees).ToList();
            }


            ViewBag.Cityid = new SelectList(db.Cities, "id", "Name", employee.CityID);

            ViewBag.Centerid = new SelectList(TosendCenters, "id", "Name", employee.Centerid);
            ViewBag.Jobs = TosendJobs;
            ViewBag.SelectedEmployee = employee;
            ViewBag.Periodid = new SelectList(db.Periods, "id", "Name", employee.Periodid);
            if (sendImageError)
            {
                ViewBag.error = "يرجى ارفاق الاثبات كملف خارجي";
            }
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

                if (employee.EmployeeType.Manager == true && type.AddManagers != true)
                {
                    return RedirectToAction("Default", "Home");
                }
                if (employee.EmployeeType.CoManager == true && type.AddCOManagers != true)
                {
                    return RedirectToAction("Default", "Home");
                }
                if (employee.EmployeeType.SchoolManager == true && type.AddSchoolManagers != true)
                {
                    return RedirectToAction("Default", "Home");
                }
                if (employee.EmployeeType.NormalEmployee == true && type.AddSchoolEmployees != true)
                {
                    return RedirectToAction("Default", "Home");
                }
                var CAddManagers = db.Centers.ToList();
                var CAddCOManagers = db.Centers.ToList();
                var CAddSchoolManagers = db.Centers.ToList();
                var CAddSchoolEmployees = db.Centers.ToList();



                var JAddManagers = db.EmployeeTypes.ToList();
                var JAddCOManagers = db.EmployeeTypes.ToList();
                var JAddSchoolManagers = db.EmployeeTypes.ToList();
                var JAddSchoolEmployees = db.EmployeeTypes.ToList();
                var JAddNone = db.EmployeeTypes.Where(x => x.Manager == null && x.SchoolManager == null && x.NormalEmployee == null && x.CoManager == null).ToList();

                var TosendCenters = db.Centers.ToList();
                var TosendJobs = db.EmployeeTypes.ToList();

                if (type.AddManagers != null)
                {
                    if (type.AddManagers == true)
                    {

                        CAddManagers = db.Centers.ToList();
                        JAddManagers = db.EmployeeTypes.Where(x => x.Manager == true).ToList();
                    }

                    else
                    {
                        CAddManagers = null;
                        JAddManagers = null;
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

                        CAddCOManagers = db.Centers.ToList();
                        JAddCOManagers = db.EmployeeTypes.Where(x => x.CoManager == true).ToList();

                    }
                    else
                    {
                        CAddCOManagers = null;
                        JAddCOManagers = null;
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
                            if (type.SeeAccToCenter == true || type.SeeAccToCity == true)
                            {
                                var emp = db.Employees.Find(Sid).CityID;
                                CAddSchoolManagers = db.Centers.Where(x => x.Cityid == emp).ToList();
                                JAddSchoolManagers = db.EmployeeTypes.Where(x => x.SchoolManager == true || x.NormalEmployee == true).ToList();

                            }
                            else
                            {
                                CAddSchoolManagers = db.Centers.ToList();
                                JAddSchoolManagers = db.EmployeeTypes.Where(x => x.SchoolManager == true || x.SeeAccToCity == true || x.NormalEmployee == true).ToList();
                            }
                        }
                        catch
                        {
                            CAddSchoolManagers = db.Centers.ToList();
                            JAddSchoolManagers = db.EmployeeTypes.Where(x => x.SchoolManager == true).ToList();
                        }


                    }
                    else
                    {
                        CAddSchoolManagers = null;
                        JAddSchoolManagers = null;
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
                        CAddSchoolEmployees = db.Centers.Where(x => x.id == emp).ToList();
                        JAddSchoolEmployees = db.EmployeeTypes.Where(x => x.NormalEmployee == true).ToList();

                    }
                    else
                    {
                        CAddSchoolEmployees = null;
                        JAddSchoolEmployees = null;
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
                    TosendJobs = TosendJobs.Union(JAddManagers).ToList();
                }
                if (JAddCOManagers != null)
                {
                    TosendJobs = TosendJobs.Union(JAddCOManagers).ToList();
                }
                if (JAddSchoolManagers != null)
                {
                    TosendJobs = TosendJobs.Union(JAddSchoolManagers).ToList();
                }
                if (JAddSchoolEmployees != null)
                {
                    TosendJobs = TosendJobs.Union(JAddSchoolEmployees).ToList();
                }
                TosendJobs = TosendJobs.Union(JAddNone).ToList();


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
                    TosendCenters = TosendCenters.Union(CAddManagers).ToList();
                }
                if (CAddCOManagers != null)
                {
                    TosendCenters = TosendCenters.Union(CAddCOManagers).ToList();
                }
                if (CAddSchoolManagers != null)
                {
                    TosendCenters = TosendCenters.Union(CAddSchoolManagers).ToList();
                }
                if (CAddSchoolEmployees != null)
                {
                    TosendCenters = TosendCenters.Union(CAddSchoolEmployees).ToList();
                }

                ViewBag.Cityid = new SelectList(db.Cities, "id", "Name", employee.CityID);

                ViewBag.Centerid = new SelectList(TosendCenters, "id", "Name", employee.Centerid);
                ViewBag.Jobs = TosendJobs;
                ViewBag.SelectedJob = employee.Job;
                ViewBag.Periodid = new SelectList(db.Periods, "id", "Name", employee.Periodid);
                return View(employee);
            }
            return RedirectToAction("Index", "Home");


        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(/*[Bind(Include = "id,name,surname,BDate,Certificate,CType,State,Centerid,Periodid,SDate,EDate,Job,Salary,Sex,Father")]*/ Employee employee, IEnumerable<HttpPostedFileBase> file)
        {
            if (Session["ID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            bool proceed = true;
            var jobs = db.EmployeeTypes.Find(employee.Job);
            if (jobs.Manager == true)
            {
                employee.Centerid = null;
                employee.CityID = null;
            }
            if (jobs.CoManager == true && jobs.SeeAccToCity != true)
            {
                employee.Centerid = null;
                employee.CityID = null;
            }
            if (jobs.CoManager == true && jobs.SeeAccToCity == true)
            {
                if (employee.CityID == null)
                {
                    ViewBag.error = "يرجى اختيار المدينة";
                    return View(employee);
                }
                employee.Centerid = null;
            }
            if (jobs.SeeAccToCenter == true || jobs.SeeTeachers == true)
            {
                if (employee.Centerid == null)
                {
                    ViewBag.error = "يرجى اختيار المركز";
                    return View(employee);
                }
                employee.CityID = null;
            }
            if (ModelState.IsValid)
            {


                int prooveid;
                try
                {
                    prooveid = db.Prooves.OrderByDescending(x => x.id).FirstOrDefault().id + 1;
                }
                catch (Exception)
                {
                    prooveid = 1;
                }
                try
                {

                    if (file.Count() > 1 || file.Count() > 0 && file.FirstOrDefault() != null)
                    {
                        var oldImages = db.Prooves.Where(x => x.EmployeeID == employee.id).ToList();

                        foreach (var image in oldImages)
                        {

                            db.Prooves.Remove(image);
                        }

                        if ((Directory.Exists(Server.MapPath("~/App_Data/Employees") + "\\" + employee.id)))
                        {
                            try
                            {
                                Directory.Delete(Server.MapPath("~/App_Data/Employees") + "\\" + employee.id, true);
                            }
                            catch (IOException)
                            {
                                Directory.Delete(Server.MapPath("~/App_Data/Employees") + "\\" + employee.id, true);
                            }
                            catch (UnauthorizedAccessException)
                            {
                                Directory.Delete(Server.MapPath("~/App_Data/Employees") + "\\" + employee.id, true);
                            }
                        }

                        if (!Directory.Exists(Server.MapPath("~/App_Data/Employees") + "\\" + employee.id))
                        {
                            Directory.CreateDirectory(Server.MapPath("~/App_Data/Employees") + "\\" + employee.id);
                        }



                        foreach (var item in file)
                        {
                            if (item.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(item.FileName);
                                if (!Directory.Exists(Server.MapPath("~/App_Data/Employees") + "\\" + employee.id))
                                {
                                    Directory.CreateDirectory(Server.MapPath("~/App_Data/Employees") + "\\" + employee.id);
                                }
                                var path = Path.Combine(Server.MapPath("~/App_Data/Employees/" + employee.id), fileName);
                                item.SaveAs(path);
                                Proove proove = new Proove();
                                proove.Path = path;
                                proove.id = prooveid;
                                proove.EmployeeID = employee.id;
                                db.Prooves.Add(proove);
                                prooveid++;

                            }
                            else
                            {

                            }
                        }

                        try
                        {
                            var startPath = Server.MapPath("~/App_Data/Employees" + "\\" + employee.id);

                            if (!Directory.Exists(Server.MapPath("~/App_Data/Employees" + "\\" + "ZipFolder")))
                            {
                                Directory.CreateDirectory(Server.MapPath("~/App_Data/Employees" + "\\" + "ZipFolder"));
                            }
                            var zipPath = Server.MapPath("~/App_Data/Employees" + "\\" + "ZipFolder") + "\\" + employee.id + ".zip";

                            if (System.IO.File.Exists(zipPath))
                            {
                                System.IO.File.Delete(zipPath);
                            }
                            try
                            {
                                ZipFile.CreateFromDirectory(startPath, zipPath);
                            }
                            catch { }
                            employee.Proof = zipPath;
                        }
                        catch { }
                    }
                }
                catch
                {

                }


                if (employee.EDate != null)
                {
                    if (employee.EDate < DateTime.Now.Date)
                    {
                        employee.State = "خارج الخدمة";
                    }
                    else
                    {
                        employee.State = "متوفر";

                    }
                }
                if (employee.InsideOrOutside != null)
                {
                    employee.InsideOrOutside.Trim();
                }
                employee.Password = Helper.ComputeHash("123", "SHA512", null);
                if (proceed)
                {

                    db.Entry(employee).State = EntityState.Modified;
                    db.SaveChanges();

                    TempData["Message"] = "تم التعديل بنجاح";
                    return RedirectToAction("Index");
                }
            }

            var typeName = (string)Session["Type"]; var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();

            var CAddManagers = db.Centers.ToList();
            var CAddCOManagers = db.Centers.ToList();
            var CAddSchoolManagers = db.Centers.ToList();
            var CAddSchoolEmployees = db.Centers.ToList();



            var JAddManagers = db.EmployeeTypes.ToList();
            var JAddCOManagers = db.EmployeeTypes.ToList();
            var JAddSchoolManagers = db.EmployeeTypes.ToList();
            var JAddSchoolEmployees = db.EmployeeTypes.ToList();
            var JAddNone = db.EmployeeTypes.Where(x => x.Manager == null && x.SchoolManager == null && x.NormalEmployee == null && x.CoManager == null).ToList();

            var TosendCenters = db.Centers.ToList();
            var TosendJobs = db.EmployeeTypes.ToList();

            if (type.AddManagers != null)
            {
                if (type.AddManagers == true)
                {

                    CAddManagers = db.Centers.ToList();
                    JAddManagers = db.EmployeeTypes.Where(x => x.Manager == true).ToList();
                }

                else
                {
                    CAddManagers = null;
                    JAddManagers = null;
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

                    CAddCOManagers = db.Centers.ToList();
                    JAddCOManagers = db.EmployeeTypes.Where(x => x.CoManager == true).ToList();

                }
                else
                {
                    CAddCOManagers = null;
                    JAddCOManagers = null;
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
                        if (type.SeeAccToCenter == true || type.SeeAccToCity == true)
                        {
                            var emp = db.Employees.Find(Sid).CityID;
                            CAddSchoolManagers = db.Centers.Where(x => x.Cityid == emp).ToList();
                            JAddSchoolManagers = db.EmployeeTypes.Where(x => x.SchoolManager == true || x.NormalEmployee == true).ToList();

                        }
                        else
                        {
                            CAddSchoolManagers = db.Centers.ToList();
                            JAddSchoolManagers = db.EmployeeTypes.Where(x => x.SchoolManager == true || x.SeeAccToCity == true || x.NormalEmployee == true).ToList();
                        }
                    }
                    catch
                    {
                        CAddSchoolManagers = db.Centers.ToList();
                        JAddSchoolManagers = db.EmployeeTypes.Where(x => x.SchoolManager == true).ToList();
                    }


                }
                else
                {
                    CAddSchoolManagers = null;
                    JAddSchoolManagers = null;
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
                    CAddSchoolEmployees = db.Centers.Where(x => x.id == emp).ToList();
                    JAddSchoolEmployees = db.EmployeeTypes.Where(x => x.NormalEmployee == true).ToList();

                }
                else
                {
                    CAddSchoolEmployees = null;
                    JAddSchoolEmployees = null;
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
                TosendJobs = TosendJobs.Union(JAddManagers).ToList();
            }
            if (JAddCOManagers != null)
            {
                TosendJobs = TosendJobs.Union(JAddCOManagers).ToList();
            }
            if (JAddSchoolManagers != null)
            {
                TosendJobs = TosendJobs.Union(JAddSchoolManagers).ToList();
            }
            if (JAddSchoolEmployees != null)
            {
                TosendJobs = TosendJobs.Union(JAddSchoolEmployees).ToList();
            }
            TosendJobs = TosendJobs.Union(JAddNone).ToList();


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
                TosendCenters = TosendCenters.Union(CAddManagers).ToList();
            }
            if (CAddCOManagers != null)
            {
                TosendCenters = TosendCenters.Union(CAddCOManagers).ToList();
            }
            if (CAddSchoolManagers != null)
            {
                TosendCenters = TosendCenters.Union(CAddSchoolManagers).ToList();
            }
            if (CAddSchoolEmployees != null)
            {
                TosendCenters = TosendCenters.Union(CAddSchoolEmployees).ToList();
            }


            ViewBag.Cityid = new SelectList(db.Cities, "id", "Name", employee.CityID);

            ViewBag.Centerid = new SelectList(TosendCenters, "id", "Name", employee.Centerid);
            ViewBag.Jobs = TosendJobs;
            ViewBag.SelectedJob = employee.Job;
            ViewBag.Periodid = new SelectList(db.Periods, "id", "Name", employee.Periodid);
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

                    return View(employee);

                }
                if (type.AddCOManagers == true)
                {
                    Employee employee = db.Employees.Find(id);
                    if (employee == null)
                    {
                        return HttpNotFound();
                    }

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
                        return View(employee);
                    }
                    catch
                    {

                    }
                    return View(employee);

                }
                if (type.AddSchoolEmployees == true)
                {
                    Employee employee = db.Employees.Find(id);
                    if (employee == null)
                    {
                        return HttpNotFound();
                    }

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


                        var path = Server.MapPath("~\\App_Data\\Employees\\");
                        var prooves = db.Prooves.Where(x => x.EmployeeID == employee.id);
                        foreach (var item in prooves)
                        {
                            db.Prooves.Remove(item);
                        }
                        db.Employees.Remove(employee);
                        try
                        {
                            db.SaveChanges();
                            if (Directory.Exists(path + "\\" + employee.id))
                            {


                                try
                                {
                                    Directory.Delete(path + "\\" + employee.id, true);
                                }
                                catch (IOException)
                                {
                                    Directory.Delete(path + "\\" + employee.id, true);
                                }
                                catch (UnauthorizedAccessException)
                                {
                                    Directory.Delete(path + "\\" + employee.id, true);
                                }
                            }
                            if (System.IO.File.Exists((path + "ZipFolder\\" + employee.id + ".zip")))
                            {
                                System.IO.File.Delete((path + "ZipFolder\\" + employee.id + ".zip"));
                            }

                        }
                        catch
                        {
                            ViewBag.error = "يوجد مدخلات اخرى متعلقة بهذا الموظف يرجى تغييرها قبل الحذف";
                            return View(employee);
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

                        var path = Server.MapPath("~\\App_Data\\Employees\\");
                        var prooves = db.Prooves.Where(x => x.EmployeeID == employee.id);
                        foreach (var item in prooves)
                        {
                            db.Prooves.Remove(item);
                        }
                        db.Employees.Remove(employee);
                        try
                        {
                            db.SaveChanges();
                            if (Directory.Exists(path + "\\" + employee.id))
                            {


                                try
                                {
                                    Directory.Delete(path + "\\" + employee.id, true);
                                }
                                catch (IOException)
                                {
                                    Directory.Delete(path + "\\" + employee.id, true);
                                }
                                catch (UnauthorizedAccessException)
                                {
                                    Directory.Delete(path + "\\" + employee.id, true);
                                }
                            }
                            if (System.IO.File.Exists(path + "ZipFolder\\" + employee.id + ".zip"))
                            {
                                System.IO.File.Delete(path + "ZipFolder\\" + employee.id + ".zip");
                            }

                        }
                        catch
                        {
                            ViewBag.error = "يوجد مدخلات اخرى متعلقة بهذا الموظف يرجى تغييرها قبل الحذف";
                            return View(employee);
                        }
                        return RedirectToAction("Default", "Home");

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

                        var path = Server.MapPath("~\\App_Data\\Employees\\");
                        var prooves = db.Prooves.Where(x => x.EmployeeID == employee.id);
                        foreach (var item in prooves)
                        {
                            db.Prooves.Remove(item);
                        }
                        db.Employees.Remove(employee);
                        try
                        {
                            db.SaveChanges();
                            if (Directory.Exists(path + "\\" + employee.id))
                            {


                                try
                                {
                                    Directory.Delete(path + "\\" + employee.id, true);
                                }
                                catch (IOException)
                                {
                                    Directory.Delete(path + "\\" + employee.id, true);
                                }
                                catch (UnauthorizedAccessException)
                                {
                                    Directory.Delete(path + "\\" + employee.id, true);
                                }
                            }
                            if (System.IO.File.Exists((path + "ZipFolder\\" + employee.id + ".zip")))
                            {
                                System.IO.File.Delete((path + "ZipFolder\\" + employee.id + ".zip"));
                            }

                        }
                        catch
                        {
                            ViewBag.error = "يوجد مدخلات اخرى متعلقة بهذا الموظف يرجى تغييرها قبل الحذف";
                            return View(employee);
                        }
                        return RedirectToAction("Default", "Home");

                    }
                }

                if (type.AddSchoolEmployees == true || type.AddManagers == true || type.AddCOManagers == true || type.AddSchoolManagers == true)
                {
                    Employee employee = db.Employees.Find(id);
                    if (employee == null)
                    {
                        return HttpNotFound();
                    }
                    if ((employee.EmployeeType.NormalEmployee == null || employee.EmployeeType.NormalEmployee == false) &&
                        (employee.EmployeeType.Manager == null || employee.EmployeeType.Manager == false) &&
                        (employee.EmployeeType.CoManager == null || employee.EmployeeType.CoManager == false) &&
                        (employee.EmployeeType.SchoolManager == null || employee.EmployeeType.SchoolManager == false))
                    {

                        var path = Server.MapPath("~\\App_Data\\Employees\\");
                        var prooves = db.Prooves.Where(x => x.EmployeeID == employee.id);
                        foreach (var item in prooves)
                        {
                            db.Prooves.Remove(item);
                        }
                        db.Employees.Remove(employee);
                        try
                        {
                            db.SaveChanges();
                            if (Directory.Exists(path + "\\" + employee.id))
                            {


                                try
                                {
                                    Directory.Delete(path + "\\" + employee.id, true);
                                }
                                catch (IOException)
                                {
                                    Directory.Delete(path + "\\" + employee.id, true);
                                }
                                catch (UnauthorizedAccessException)
                                {
                                    Directory.Delete(path + "\\" + employee.id, true);
                                }
                            }
                            if (System.IO.File.Exists(path + "ZipFolder\\" + employee.id + ".zip"))
                            {
                                System.IO.File.Delete(path + "ZipFolder\\" + employee.id + ".zip");
                            }

                        }
                        catch
                        {
                            ViewBag.error = "يوجد مدخلات اخرى متعلقة بهذا الموظف يرجى تغييرها قبل الحذف";
                            return View(employee);
                        }
                        return RedirectToAction("Default", "Home");
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

                        var path = Server.MapPath("~\\App_Data\\Employees\\");
                        var prooves = db.Prooves.Where(x => x.EmployeeID == employee.id);
                        foreach (var item in prooves)
                        {
                            db.Prooves.Remove(item);
                        }
                        db.Employees.Remove(employee);
                        try
                        {
                            db.SaveChanges();
                            if (Directory.Exists(path + "\\" + employee.id))
                            {


                                try
                                {
                                    Directory.Delete(path + "\\" + employee.id, true);
                                }
                                catch (IOException)
                                {
                                    Directory.Delete(path + "\\" + employee.id, true);
                                }
                                catch (UnauthorizedAccessException)
                                {
                                    Directory.Delete(path + "\\" + employee.id, true);
                                }
                            }
                            if (System.IO.File.Exists(path + "ZipFolder\\" + employee.id + ".zip"))
                            {
                                System.IO.File.Delete(path + "ZipFolder\\" + employee.id + ".zip");
                            }

                        }
                        catch
                        {
                            ViewBag.error = "يوجد مدخلات اخرى متعلقة بهذا الموظف يرجى تغييرها قبل الحذف";
                            return View(employee);
                        }
                        return RedirectToAction("Default", "Home");
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
