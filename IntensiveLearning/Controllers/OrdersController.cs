using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IntensiveLearning.Database;

namespace IntensiveLearning.Controllers
{
    public class OrdersController : Controller
    {
        TaalimEntities db = new TaalimEntities();
        // GET: Orders
        public ActionResult Index()
        {
            if (Session["ID"]!=null)
            {
                var typeName = (string)Session["Type"];
                var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();

                if (type.BuyingAcceptance != null)
                {
                    if (type.BuyingAcceptance == true)
                    {
                        return View();
                    }

                }
                return RedirectToAction("Default", "Home");

            }
            return RedirectToAction("Index", "Home");
        }

        public JsonResult GetOrders()
        {
            var empid = Convert.ToInt32(Session["ID"]);
            var order = db.Orders.ToList();
            foreach (var o in order)
            {
                if (o.Bnd == null)
                {
                    o.Bnd = new Bnd();
                }
                if (o.Employee == null)
                {
                    o.Employee = new Employee();
                }
            }

            var Orders = order.Select(x => new
            {
                Bnd = x.Bndid,
                Center = x.CenterID,
                Date = x.Date,
                Desc = x.Desc,
                EmployeeName = x.Employee.name,
                EmployeeSurname = x.Employee.surname,
                PeacePrice = x.PeacePrice,
                Quantity = x.Quantity,
                Proof = x.Proof,
                State = x.State,
                Time = x.Time,
                id = x.id,
                FirstLevelSign = x.FirstLevelSign,
                SecondLevelSign = x.SecondLevelSign,
                ThirdLevelSign = x.ThirdLevelSign,
                FourthLevelSign = x.FourthLevelSign,
                FifthLevelSign = x.FifthLevelSign,
                SixthLevelSign = x.SixthLevelSign,
                SeventhLevelSign = x.SeventhLevelSign,
                SendingLevelSign = x.SendingLevelSign,
                RecievingLevelSign = x.RecievingLevelSign,
                AfterRecieveFirstLevelSign = x.AfterRecieveFirstLevelSign,
                AfterRecieveSecondLevelSign = x.AfterRecieveSecondLevelSign,
                EmployeeID = x.Employeeid,
            }).ToList();

            List<object> toSendList = new List<object>();
            toSendList.Add(Orders);
            toSendList.Add(empid);
            return Json(toSendList, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Create()
        {
            var typeName = (string)Session["Type"];
            var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();

            if (type.AddBuyingRequest == true)
            {
                return View();
            }
            return RedirectToAction("Default", "Index");
        }

        [HttpPost]
        public ActionResult Create(Order order, HttpPostedFileBase file)
        {
            if (Session["ID"] != null)
            {
                try
                {

                    if (file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var path = Path.Combine(Server.MapPath("~/App_Data/Orders"), fileName);
                        file.SaveAs(path);
                        order.Proof = path;

                    }
                    else
                    {
                        ViewBag.error = "يرجى ارفاق الاثبات كملف خارجي";
                        return View(order);
                    }
                }
                catch
                {

                }

                int empid = Convert.ToInt32(Session["ID"]);
                Employee emp = db.Employees.Find(empid);
                var typeName = (string)Session["Type"];
                var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
                if (type.AddBuyingRequest == true)
                {
                    if (emp.Centerid == null)
                    {
                        order.Date = DateTime.Now.Date;
                        order.Employeeid = empid;
                        order.Time = DateTime.Now.TimeOfDay;
                        //منسق محافظة
                        if (type.CoManager == true && type.SeeAccToCity == true)
                        {
                            order.FirstLevelSign = true;
                        }
                        //منسق عام
                        else if (type.CoManager == true && type.AddCitesAndCenters == true)
                        {
                            order.SecondLevelSign = true;
                        }
                        //مراقبة وتقييم
                        else if (type.CoManager == true && type.HighAcceptance == true)
                        {
                            order.ThirdLevelSign = true;
                        }
                        //متابعة
                        else if (type.CoManager == true && type.SeeAll == true)
                        {
                            order.FourthLevelSign = true;
                        }
                        //مدير المشروع
                        else if (type.Manager == true && type.AddNewProject != true)
                        {
                            order.FifthLevelSign = true;
                        }
                        //مدير تنفيذي
                        else if (type.Manager == true && type.AddNewProject == true)
                        {
                            order.SeventhLevelSign = true;
                        }
                        //مالي
                        else if (type.Finance == true && type.CoManager == true)
                        {
                            order.SixthLevelSign = true;
                        }
                    }
                    //مدير مركز
                    else
                    {
                        order.CenterID = emp.Centerid;
                        order.Date = DateTime.Now.Date;
                        order.Employeeid = empid;
                        order.Time = DateTime.Now.TimeOfDay;
                    }

                    db.Orders.Add(order);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

            }
            ViewBag.error = "حصل خطأ أثناء عملية التخزين";
            return View(order);
        }


        public JsonResult ConfirmOrder(int id)
        {
            int empid = Convert.ToInt32(Session["ID"]);
            Employee emp = db.Employees.Find(empid);

            var typeName = (string)Session["Type"];
            var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();

            Order order = db.Orders.Find(id);

            if (emp.Centerid == null)
            {
                order.Date = DateTime.Now.Date;
                order.Employeeid = empid;
                order.Time = DateTime.Now.TimeOfDay;

                //منسق محافظة
                if (type.CoManager == true && type.SeeAccToCity == true && type.BuyingAcceptance == true)
                {
                    order.FirstLevelSign = true;
                }
                //منسق عام
                else if (type.CoManager == true && type.AddCitesAndCenters == true && type.BuyingAcceptance == true)
                {
                    order.SecondLevelSign = true;
                }
                //مراقبة وتقييم
                else if (type.CoManager == true && type.HighAcceptance == true && type.BuyingAcceptance == true)
                {
                    order.ThirdLevelSign = true;
                }
                //متابعة
                else if (type.CoManager == true && type.SeeAll == true && type.BuyingAcceptance == true)
                {
                    order.FourthLevelSign = true;
                }
                //مدير المشروع
                else if (type.Manager == true && type.AddNewProject != true && type.BuyingAcceptance == true)
                {
                    order.FifthLevelSign = true;
                }
                //مدير تنفيذي
                else if (type.Manager == true && type.AddNewProject == true && type.BuyingAcceptance == true)
                {
                    order.SeventhLevelSign = true;
                }
                //مالي
                else if (type.Finance == true && type.CoManager == true && type.BuyingAcceptance == true)
                {
                    order.SixthLevelSign = true;
                }
            }
            return Json(true);
        }



        public ActionResult Edit()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Edit(Order order, HttpPostedFileBase file)
        {
            try
            {
                if (file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/App_Data/Employees"), fileName);
                    Order mystd = db.Orders.Find(order.id);
                    string EmpProof = mystd.Proof;
                    if ((System.IO.File.Exists(EmpProof)))
                    {
                        System.IO.File.Delete(EmpProof);
                    }
                    file.SaveAs(path);
                    order.Proof = path;
                    db.Entry(mystd).State = EntityState.Detached;
                }
                ViewBag.Message = "Upload successful";
            }
            catch
            {

            }
            return View();
        }
    }
}