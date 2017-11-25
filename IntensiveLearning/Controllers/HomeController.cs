using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IntensiveLearning.Database;
using IntensiveLearning.Models;
using System.IO;
using System.IO.Compression;

namespace IntensiveLearning.Controllers
{
    public class HomeController : Controller
    {
        private TaalimEntities db = new TaalimEntities();
        [HttpGet]
        public ActionResult Index()
        {
            if (Session["ID"] != null)
            {
                return RedirectToAction("Default", "Home");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Index(string Username, string Password)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var employee = db.Employees.Where(x => x.Username == Username).FirstOrDefault();
                    var passwordHashed = Helper.VerifyHash(Password, "SHA512", employee.Password);
                    if (passwordHashed)
                    {


                        Session["Username"] = employee.Username;
                        Session["ID"] = employee.id;
                        Session["Manager"] = employee.EmployeeType.Manager;
                        Session["CoManager"] = employee.EmployeeType.CoManager;
                        Session["SchoolManager"] = employee.EmployeeType.SchoolManager;
                        Session["NormalEmployee"] = employee.EmployeeType.NormalEmployee;
                        Session["Type"] = employee.EmployeeType.Type;
                        Session["AddDaileyReport"] = employee.EmployeeType.AddDaileyReport;
                        Session["AddBuyingRequest"] = employee.EmployeeType.AddBuyingRequest;
                        Session["AddCitesAndCenters"] = employee.EmployeeType.AddCitesAndCenters;
                        Session["AddCOManagers"] = employee.EmployeeType.AddCOManagers;
                        Session["AddExam"] = employee.EmployeeType.AddExam;
                        Session["AddManagers"] = employee.EmployeeType.AddManagers;
                        Session["AddNewEmployeeType"] = employee.EmployeeType.AddNewEmployeeType;
                        Session["AddNewProject"] = employee.EmployeeType.AddNewProject;
                        Session["AddPresence"] = employee.EmployeeType.AddPresence;
                        Session["AddSchoolEmployees"] = employee.EmployeeType.AddSchoolEmployees;
                        Session["AddSchoolManagers"] = employee.EmployeeType.AddSchoolManagers;
                        Session["AddSchoolManagingTools"] = employee.EmployeeType.AddSchoolManagingTools;
                        Session["AddStudent"] = employee.EmployeeType.AddStudent;
                        Session["ApproveExam"] = employee.EmployeeType.ApproveExam;
                        Session["BuyingAcceptance"] = employee.EmployeeType.BuyingAcceptance;
                        Session["HighAcceptance"] = employee.EmployeeType.HighAcceptance;
                        Session["Finance"] = employee.EmployeeType.Finance;
                        Session["SeeAll"] = employee.EmployeeType.SeeAll;
                        Session["SeeAccToCenter"] = employee.EmployeeType.SeeAccToCenter;
                        Session["SeeAccToCity"] = employee.EmployeeType.SeeAccToCity;
                        Session["SeeAllButFinance"] = employee.EmployeeType.SeeAllButFinance;
                        Session["SeeTeachers"] = employee.EmployeeType.SeeTeachers;
                        if (employee.Center != null)
                        {
                            Session["Markaz"] = employee.Center.Name;
                        }


                        if (employee.EmployeeType.NormalEmployee == true)
                        {
                            return RedirectToAction("Create", "DailyReport");
                        }
                        else
                        {
                            return RedirectToAction("Index", "Employees");
                        }

                    }

                }
                catch (Exception)
                {
                    ViewBag.error = "تعذر العثور على المسخدم";
                }

            }
            return View();
        }



        public ActionResult Default()
        {
            if (Session["ID"] != null)
            {
                var typeName = (string)Session["Type"];
                var employee = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
                if (employee.NormalEmployee == true)
                {
                    return RedirectToAction("Create", "DailyReport");
                }
                else
                {
                    return RedirectToAction("Index", "Employees");
                }


            }
            return RedirectToAction("Index");

        }


        public ActionResult Logout()
        {
            Session["Username"] = null;
            Session["ID"] = null;
            Session["Type"] = null;
            Session["Manager"] = null;
            Session["CoManager"] = null;
            Session["SchoolManager"] = null;
            Session["NormalEmployee"] = null;
            Session["AddDaileyReport"] = null;
            Session["AddBuyingRequest"] = null;
            Session["AddCitesAndCenters"] = null;
            Session["AddCOManagers"] = null;
            Session["AddExam"] = null;
            Session["AddManagers"] = null;
            Session["AddNewEmployeeType"] = null;
            Session["AddNewProject"] = null;
            Session["AddPresence"] = null;
            Session["AddSchoolEmployees"] = null;
            Session["AddSchoolManagers"] = null;
            Session["AddSchoolManagingTools"] = null;
            Session["AddStudent"] = null;
            Session["ApproveExam"] = null;
            Session["BuyingAcceptance"] = null;
            Session["HighAcceptance"] = null;
            Session["Finance"] = null;
            Session["SeeAll"] = null;
            Session["SeeAccToCenter"] = null;
            Session["SeeAccToCity"] = null;
            Session["SeeAllButFinance"] = null;
            Session["SeeTeachers"] = null;
            Session["Markaz"] = null;
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            if (Session["ID"] != null)
            {
                ChangePassword c = new ChangePassword();
                return View(c);
            }
            return RedirectToAction("Index", "Login");
        }
        [HttpPost]
        public ActionResult ChangePassword(ChangePassword chP)
        {
            int id = Convert.ToInt32(Session["ID"]);

            Employee fromDbEmp = db.Employees.Find(id);
            if (ModelState.IsValid)
            {

                if (chP.password == chP.confPassword)
                {
                    bool checkPass = Helper.VerifyHash(chP.password, "SHA512", fromDbEmp.Password);

                    if (checkPass)
                    {
                        if (chP.Newpassword != chP.password)
                        {
                            chP.Newpassword = Helper.ComputeHash(chP.Newpassword, "SHA512", null);
                            fromDbEmp.Password = chP.Newpassword;
                            db.Entry(fromDbEmp).State = EntityState.Modified;
                            db.SaveChanges();
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            ViewBag.Error = "كلمة المرور لم تتغير";
                        }


                    }
                    else
                    {
                        ViewBag.Error = "كلمة المرور خاظئة";
                    }
                }
                else
                {
                    ViewBag.Error = "كلمة المرور وتأكيدها مختلفتين";
                }
            }
            return View();
        }



        public ActionResult ResetDefaultPass(int id)
        {
            var NewPass = Helper.ComputeHash("123", "SHA512", null);
            var emp = db.Employees.Find(id);
            emp.Password = NewPass;
            db.Entry(emp).State = EntityState.Modified;
            db.Configuration.ValidateOnSaveEnabled = false;
            db.SaveChanges();
            return RedirectToAction("Index", "Employees");
        }


        public ActionResult EditOwnUser(int? id)
        {
            if (Session["ID"] == null)
            {
                return RedirectToAction("Index", "Home");

            }
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var empid = Convert.ToInt32(Session["ID"]);
            if (id == empid)
            {
                var employee = db.Employees.Find(id);
                return View(employee);

            }
            return RedirectToAction("Default", "Home");
        }

        [HttpPost]
        public ActionResult EditOwnUser(Employee employee, IEnumerable<HttpPostedFileBase> file)
        {
            if (Session["ID"] == null)
            {
                return RedirectToAction("Index", "Home");

            }

            var empid = Convert.ToInt32(Session["ID"]);

            if (empid == employee.id)
            {
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
                    employee.Password = Helper.ComputeHash("123", "SHA512", null);
                    db.Entry(employee).State = EntityState.Modified;
                    db.SaveChanges();

                    TempData["Message"] = "تم التعديل بنجاح";
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(employee);

                }
            }
            return RedirectToAction("Default", "Home");
        }


    }
}