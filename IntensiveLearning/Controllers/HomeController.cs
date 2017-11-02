using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IntensiveLearning.Database;
using IntensiveLearning.Models;

namespace IntensiveLearning.Controllers
{
    public class HomeController : Controller
    {
        private TaalimEntities db = new TaalimEntities();
        [HttpGet]
        public ActionResult Index()
        {
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


                        if (employee.EmployeeType.AddManagers == true)
                        {
                            return RedirectToAction("Index", "Employees");
                        }
                        if (employee.EmployeeType.NormalEmployee == true)
                        {
                            return RedirectToAction("Create", "DailyReport");
                        }
                        else if (employee.EmployeeType.Manager == true)
                        {
                            return RedirectToAction("Index", "Employees");
                        }
                        else
                        {
                            return RedirectToAction("Index", "Complains");
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
                else if (employee.Manager == true || employee.AddNewEmployeeType ==true)
                {
                    return RedirectToAction("Index", "Employees");
                }
                else
                {
                    return RedirectToAction("Index", "Complains");
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
    }
}