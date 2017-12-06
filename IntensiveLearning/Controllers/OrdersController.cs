using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IntensiveLearning.Database;
using System.IO.Compression;
using System.Web.Script.Serialization;

namespace IntensiveLearning.Controllers
{
    public class OrdersController : Controller
    {
        TaalimEntities db = new TaalimEntities();
        public ActionResult Index()
        {
            if (Session["ID"] != null)
            {
                if (TempData["BndOverload"] != null)
                {
                    ViewBag.BndOverload = TempData["BndOverload"];
                }

                return View();
            }
            return RedirectToAction("Index", "Home");
        }
        // GET: Orders
        public JsonResult GetOrders()
        {
            var typeName = (string)Session["Type"];
            var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();

            var empid = Convert.ToInt32(Session["ID"]);
            var order = db.Orders.ToList();
            if (type.SeeAll == true || type.SeeAllButFinance == true)
            {
                order = db.Orders.ToList();
            }
            else if (type.SeeAccToCity == true)
            {
                var emp = db.Employees.Find(empid);
                order = db.Orders.Where(x => x.Center.Cityid == emp.CityID || x.Employeeid == empid).ToList();
            }
            else if (type.SeeAccToCenter == true)
            {
                var emp = db.Employees.Find(empid);
                order = db.Orders.Where(x => x.CenterID == emp.Centerid || x.Employeeid == empid).ToList();
            }
            else if (type.SeeTeachers == true)
            {
                order = db.Orders.Where(x => x.Employeeid == empid || x.Employeeid == empid).ToList();
            }
            var Bnd = db.Bnds.ToList();
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
                if (o.Center == null)
                {
                    o.Center = new Center();
                }
                if (o.SubBnd == null)
                {
                    o.SubBnd = new SubBnd();
                }
            }

            var Orders = order.Select(x => new
            {
                Bnd = x.Bnd.Name,
                Center = x.Center.Name,
                Necessity = x.Necessity,
                OrderType = x.OrderType,
                SumPrice = x.SumPrice,
                Paymentid = x.Paymentid,
                PaymentApprove = x.PaymentApprove,
                PaymentApprovalDate = x.PaymentApprovalDate,
                BuyingApprovalDate = x.BuyingApprovalDate,
                ProofAcceptanceDate = x.ProofAcceptanceDate,
                BuyingApprove = x.BuyingApprove,
                ProofAcceptance = x.ProofAcceptance,
                Date = x.Date,
                Subject = x.Subject,
                EmployeeName = x.Employee.name,
                EmployeeSurname = x.Employee.surname,
                PeacePrice = x.PeacePrice,
                Quantity = x.Quantity,
                State = x.State,
                Time = x.Time,
                id = x.id,
                FirstLevelSign = x.FirstLevelSign,
                SecondLevelSign = x.SecondLevelSign,
                ThirdLevelSign = x.ThirdLevelSign,
                EmployeeID = x.Employeeid,
                Bndid = x.Bndid,
                Centerid = x.CenterID,
                PaymentOrderDate = x.PaymentOrderDate,
                proof = x.proof,
                QuantityChanged = x.QuantityChanged,
                Comment = x.CanclationReason
            }).ToList();

            var Bnds = Bnd.Select(x => new
            {
                id = x.id,
                Name = x.Name,
            }).ToList();
            var Payment = db.PaymentsRecords.ToList();

            var Payemnts = Payment.Select(x => new
            {
                id = x.id,
            }).ToList();
            List<object> toSendList = new List<object>();
            toSendList.Add(Orders);
            toSendList.Add(empid);
            toSendList.Add(Bnds);
            toSendList.Add(Payemnts);
            return Json(toSendList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Default", "Home");

            }
            if (Session["ID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var typeName = (string)Session["Type"];
            var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();

            var order = db.Orders.Find(id);
            var empid = Convert.ToInt32(Session["ID"]);
            if (type.SeeAll == true || order.Employeeid == empid)
            {
                ViewBag.type = type;
                return View(order);

            }
            return RedirectToAction("Index");
        }

        public ActionResult Create()
        {
            if (Session["ID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var empid = Convert.ToInt16(Session["ID"]);
            var emp = db.Employees.Find(empid);
            var typeName = (string)Session["Type"];
            var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
            if (type.AddBuyingRequest == true)
            {
                if (type.SeeAccToCity ==true)
                {
                    ViewBag.CenterID = new SelectList(db.Centers.Where(x=>x.Cityid == emp.CityID), "id", "Name");

                }
                else if (type.SeeAccToCenter == true || type.SeeTeachers ==  true)
                {
                    ViewBag.CenterID = new SelectList(db.Centers.Where(x => x.id == emp.Centerid), "id", "Name");

                }
                else
                {
                    ViewBag.CenterID = new SelectList(db.Centers, "id", "Name");
                }
                ViewBag.type = type;
                return View();
            }
            return RedirectToAction("Default", "Index");
        }
        [HttpPost]
        public ActionResult Create(Order order)
        {
            if (Session["ID"] != null)
            {


                int empid = Convert.ToInt32(Session["ID"]);
                Employee emp = db.Employees.Find(empid);
                var typeName = (string)Session["Type"];
                var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
                try
                {
                    order.id = db.Orders.OrderByDescending(x => x.id).FirstOrDefault().id + 1;
                }
                catch (Exception)
                {
                    order.id = 1;
                }
                order.Date = DateTime.Now.Date;
                order.Time = DateTime.Now.TimeOfDay;
                order.Employeeid = empid;
                order.SumPrice = order.Quantity * order.PeacePrice;


                if (type.AddBuyingRequest == true)
                {
                    if (emp.Centerid == null)
                    {
                        //مالي
                        if (type.Finance == true && type.CoManager == true)
                        {
                            order.SecondLevelSign = true;
                        }
                        //مدير تنفيذي
                        else if (type.Manager == true && type.AddNewProject == true)
                        {
                            order.ThirdLevelSign = true;
                        }
                        //مراقبة وتقييم
                        //else if (type.CoManager == true && type.HighAcceptance == true && type.BuyingAcceptance == true)
                        //{
                        //    order.ThirdLevelSign = true;
                        //}
                        //متابعة
                        else if (type.CoManager == true && type.SeeAll == true)
                        {
                            order.FirstLevelSign = true;
                        }
                    }
                    //مدير مركز
                    else
                    {
                        order.CenterID = emp.Centerid;
                    }
                    if (ModelState.IsValid)
                    {
                        db.Orders.Add(order);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                if (type.SeeAccToCity == true)
                {
                    ViewBag.CenterID = new SelectList(db.Centers.Where(x => x.Cityid == emp.CityID), "id", "Name",order.CenterID);

                }
                else if (type.SeeAccToCenter == true || type.SeeTeachers == true)
                {
                    ViewBag.CenterID = new SelectList(db.Centers.Where(x => x.id == emp.Centerid), "id", "Name", order.CenterID);

                }
                else
                {
                    ViewBag.CenterID = new SelectList(db.Centers, "id", "Name", order.CenterID);
                }
                ViewBag.type = type;
                ViewBag.error = "حصل خطأ أثناء عملية التخزين";
                return View(order);

            }
            return RedirectToAction("Index", "Home");
        }



        public ActionResult SaveProof(FormCollection formCollection)
        {
            string o = formCollection["Order"];
            int id = new JavaScriptSerializer().Deserialize<int>(o);
            Order order = db.Orders.Find(id);
            var files = Request.Files;
            int proovesNum = 0;
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

                if (Request.Files.Count > 0)
                {
                    var oldImages = db.Prooves.Where(x => x.OrderID == id).ToList();

                    foreach (var image in oldImages)
                    {

                        db.Prooves.Remove(image);
                    }

                    if ((Directory.Exists(Server.MapPath("~/App_Data/Orders") + "\\" + id)))
                    {
                        try
                        {
                            Directory.Delete(Server.MapPath("~/App_Data/Orders") + "\\" + id, true);
                        }
                        catch (IOException)
                        {
                            Directory.Delete(Server.MapPath("~/App_Data/Orders") + "\\" + id, true);
                        }
                        catch (UnauthorizedAccessException)
                        {
                            Directory.Delete(Server.MapPath("~/App_Data/Orders") + "\\" + id, true);
                        }
                    }

                    if (!Directory.Exists(Server.MapPath("~/App_Data/Orders") + "\\" + id))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/App_Data/Orders") + "\\" + id);
                    }

                }
                if (Request.Files.Count == 0)
                {
                    return Json(false);
                }
                foreach (string file in Request.Files)
                {
                    var fileContent = Request.Files[file];
                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
                        if (!Directory.Exists(Server.MapPath("~/App_Data/Orders/" + id)))
                        {
                            Directory.CreateDirectory(Server.MapPath("~/App_Data/Orders/" + id));
                        }
                        var inputStream = fileContent.InputStream;
                        var fileName = fileContent.FileName;
                        var path = Path.Combine(Server.MapPath("~/App_Data/Orders/" + id), fileName);
                        using (var fileStream = System.IO.File.Create(path))
                        {
                            inputStream.CopyTo(fileStream);
                            proovesNum++;
                            Proove proove = new Proove();
                            proove.Path = path;
                            proove.id = prooveid;
                            proove.OrderID = id;
                            db.Prooves.Add(proove);

                            prooveid++;
                        }
                    }
                    else
                    {
                    }


                }
                try
                {
                    var startPath = Server.MapPath("~/App_Data/Orders" + "\\" + id);

                    if (!Directory.Exists(Server.MapPath("~/App_Data/Orders" + "\\" + "ZipFolder")))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/App_Data/Orders" + "\\" + "ZipFolder"));
                    }
                    var zipPath = Server.MapPath("~/App_Data/Orders" + "\\" + "ZipFolder") + "\\" + id + ".zip";

                    if (System.IO.File.Exists(zipPath))
                    {
                        System.IO.File.Delete(zipPath);
                    }
                    try
                    {
                        ZipFile.CreateFromDirectory(startPath, zipPath);
                    }
                    catch { }
                    db.SaveChanges();
                    prooveid--;
                    for (int i = 0; i < proovesNum; i++)
                    {
                        var proovetozip = db.Prooves.Find(prooveid - i);
                        proovetozip.ZipFilePath = zipPath;
                        db.Entry(proovetozip).State = EntityState.Modified;
                    }
                    order.proof = prooveid;
                    db.Entry(order).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch { }

                ViewBag.Message = "Upload successful";
            }
            catch
            {
                //ViewBag.Message = "Upload failed";
                //return Json(false);
            }

            return RedirectToAction("Index");
        }



        public ActionResult ConfirmOrder(int id)
        {
            int empid = Convert.ToInt32(Session["ID"]);
            Employee emp = db.Employees.Find(empid);

            var typeName = (string)Session["Type"];
            var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();

            Order order = db.Orders.Find(id);

            if (emp.Centerid == null)
            {
                //مالي
                if (type.Finance == true && type.CoManager == true)
                {
                    order.SecondLevelSign = true;
                }
                //مدير تنفيذي
                else if (type.Manager == true && type.AddNewProject == true)
                {
                    order.ThirdLevelSign = true;
                }
                //مراقبة وتقييم
                //else if (type.CoManager == true && type.HighAcceptance == true && type.BuyingAcceptance == true)
                //{
                //    order.ThirdLevelSign = true;
                //}
                //متابعة
                else if (type.CoManager == true && type.SeeAll == true)
                {
                    order.FirstLevelSign = true;
                }
                order.CanclationReason = null;
            }
            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult RefuseOrder(int id, string comment)
        {
            int empid = Convert.ToInt32(Session["ID"]);
            Employee emp = db.Employees.Find(empid);

            var typeName = (string)Session["Type"];
            var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();

            Order order = db.Orders.Find(id);

            if (emp.Centerid == null)
            {
                //مالي
                if (type.Finance == true && type.CoManager == true)
                {
                    order.SecondLevelSign = false;
                }
                //مدير تنفيذي
                else if (type.Manager == true && type.AddNewProject == true)
                {
                    order.ThirdLevelSign = false;
                }
                //مراقبة وتقييم
                //else if (type.CoManager == true && type.HighAcceptance == true && type.BuyingAcceptance == true)
                //{
                //    order.ThirdLevelSign = true;
                //}
                //متابعة
                else if (type.CoManager == true && type.SeeAll == true)
                {
                    order.FirstLevelSign = false;
                }
                order.CanclationReason = comment;
            }
            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult AssignBnd(int Bndid, int id)
        {
            Order order = db.Orders.Find(id);
            Bnd bnd = db.Bnds.Find(Bndid);
            if (bnd.AfterReductionNum - order.SumPrice <= 0)
            {
                TempData["BndOverload"] = "لا يمكنك تجاوز ميزانية البند... الميزانية المتبقية " + bnd.AfterReductionNum;
                return RedirectToAction("Index");
            }
            order.Bndid = Bndid;
            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Quantity(int id, int AcceptedQuantity)
        {
            if (Session["ID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var typeName = (string)Session["Type"];
            var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
            Order order = db.Orders.Find(id);
            Order newOrder = new Order();
            newOrder.Bndid = order.Bndid;
            newOrder.CenterID = order.CenterID;
            newOrder.Date = order.Date;
            newOrder.Employeeid = order.Employeeid;
            newOrder.FirstLevelSign = order.FirstLevelSign;
            newOrder.SecondLevelSign = order.SecondLevelSign;
            newOrder.ThirdLevelSign = order.ThirdLevelSign;
            newOrder.Time = order.Time;
            newOrder.Subject = order.Subject;
            newOrder.Quantity = order.Quantity;
            newOrder.Necessity = order.Necessity;
            newOrder.OrderType = order.OrderType;
            newOrder.PeacePrice = order.PeacePrice;
            newOrder.State = order.State;
            if (order.Quantity != AcceptedQuantity)
            {
                if (type.Finance == true)
                {
                    if (order.Bnd.AfterReductionNum - (order.PeacePrice * AcceptedQuantity) < 0)
                    {
                        TempData["BndOverload"] = "لا يمكنك تجاوز ميزانية البند... الميزانية المتبقية " + order.Bnd.AfterReductionNum;
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        order.Quantity = AcceptedQuantity;
                        order.SumPrice = order.PeacePrice * AcceptedQuantity;
                        order.QuantityChanged = true;
                        try
                        {
                            newOrder.id = db.Orders.OrderByDescending(x => x.id).FirstOrDefault().id + 1;
                        }
                        catch (Exception)
                        {
                            newOrder.id = 1;
                        }
                        newOrder.Quantity = newOrder.Quantity - AcceptedQuantity;
                        newOrder.SumPrice = newOrder.PeacePrice * newOrder.Quantity;

                        db.Entry(order).State = EntityState.Modified;
                        db.Orders.Add(newOrder);
                        db.SaveChanges();
                    }
                }
            }
            else
            {
                order.QuantityChanged = true;
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();

            }
            return RedirectToAction("Index");
        }




        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Default", "Home");
            }
            if (Session["ID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Order order = db.Orders.Find(id);
            var typeName = (string)Session["Type"];
            var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
            if (order.Paymentid == null)
            {
                int empid = Convert.ToInt32(Session["ID"]);
                Employee emp = db.Employees.Find(empid);
                if (type.SeeAccToCity == true)
                {
                    ViewBag.CenterID = new SelectList(db.Centers.Where(x => x.Cityid == emp.CityID), "id", "Name", order.CenterID);

                }
                else if (type.SeeAccToCenter == true || type.SeeTeachers == true)
                {
                    ViewBag.CenterID = new SelectList(db.Centers.Where(x => x.id == emp.Centerid), "id", "Name", order.CenterID);

                }
                else
                {
                    ViewBag.CenterID = new SelectList(db.Centers, "id", "Name", order.CenterID);
                }
                ViewBag.type = type;
                return View(order);

            }
            else
            {
                TempData["BndOverload"] = "لا يمكنك التعديل بعد اصدار امر الدفع";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult Edit(Order order)
        {
            if (Session["ID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var typeName = (string)Session["Type"];
            var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
            if (ModelState.IsValid)
            {
                order.FirstLevelSign = null;
                order.SecondLevelSign = null;
                order.ThirdLevelSign = null;
                order.CanclationReason = null;

                order.Date = DateTime.Now.Date;
                order.Time = DateTime.Now.TimeOfDay;
                order.SumPrice = order.PeacePrice * order.Quantity;
                order.Bndid = null;

                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            int empid = Convert.ToInt32(Session["ID"]);
            Employee emp = db.Employees.Find(empid);
            if (type.SeeAccToCity == true)
            {
                ViewBag.CenterID = new SelectList(db.Centers.Where(x => x.Cityid == emp.CityID), "id", "Name", order.CenterID);

            }
            else if (type.SeeAccToCenter == true || type.SeeTeachers == true)
            {
                ViewBag.CenterID = new SelectList(db.Centers.Where(x => x.id == emp.Centerid), "id", "Name", order.CenterID);

            }
            else
            {
                ViewBag.CenterID = new SelectList(db.Centers, "id", "Name", order.CenterID);
            }
            ViewBag.type = type;
            return View(order);
        }


        public ActionResult CreatePayment(int id)
        {
            if (Session["ID"] == null)
            {
                return RedirectToAction("Index", "Home");

            }
            var typeName = (string)Session["Type"];
            var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
            Order order = db.Orders.Find(id);

            if (type.Finance == true)
            {
                if (order.Bnd.AfterReductionNum - order.SumPrice < 0)
                {
                    TempData["BndOverload"] = "لا يمكنك تجاوز ميزانية البند... الميزانية المتبقية " + order.Bnd.AfterReductionNum;
                    return RedirectToAction("Index");
                }
                else
                {
                    PaymentsRecord payment = new PaymentsRecord();
                    try
                    {
                        payment.id = db.PaymentsRecords.OrderByDescending(x => x.id).FirstOrDefault().id + 1;
                    }
                    catch (Exception)
                    {
                        payment.id = 1;
                    }
                    order.Paymentid = payment.id;
                    order.PaymentOrderDate = DateTime.Now.Date;
                    db.PaymentsRecords.Add(payment);
                    db.Entry(order).State = EntityState.Modified;

                    db.SaveChanges();
                }

            }
            return RedirectToAction("Index");
        }

        public ActionResult ChoosePayment(int id, int Paymentid)
        {
            if (Session["ID"] == null)
            {
                return RedirectToAction("Index", "Home");

            }
            var typeName = (string)Session["Type"];
            var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
            Order order = db.Orders.Find(id);
            if (type.Finance == true)
            {
                if (order.Bnd.AfterReductionNum - order.SumPrice < 0)
                {
                    TempData["BndOverload"] = "لا يمكنك تجاوز ميزانية البند... الميزانية المتبقية " + order.Bnd.AfterReductionNum;
                    return RedirectToAction("Index");
                }
                else
                {
                    order.Paymentid = Paymentid;
                    db.Entry(order).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index");

        }

        //public ActionResult CheckBought(int id) {

        //    var order = db.Orders.Find(id);
        //    var empid = Convert.ToInt16(Session["ID"]);
        //    var emp = db.Employees.Find(empid);
        //    if (order.Employeeid == empid)
        //    {
        //        if (order.FirstLevelSign ==true && order.SecondLevelSign == true && order.ThirdLevelSign == true)
        //        {
        //            order.BuyingSign = true;
        //            db.Entry(order).State = EntityState.Modified;
        //            db.SaveChanges();
        //        }
        //    }


        //    return RedirectToAction("Index");
        //}

        public ActionResult PaymentRefuse(int? id, string comment)
        {
            if (id == null)
            {
                return RedirectToAction("Default", "Home");
            }

            if (Session["ID"] == null)
            {
                return RedirectToAction("Index", "Home");

            }
            var typeName = (string)Session["Type"];
            var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();

            Order order = db.Orders.Find(id);

            if (type.Finance == true)
            {
                order.CanclationReason = comment;
                order.PaymentApprove = false;
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
            }


            return RedirectToAction("Index");
        }

        public ActionResult BuyingRefuse(int? id, string comment)
        {
            if (id == null)
            {
                return RedirectToAction("Default", "Home");
            }

            if (Session["ID"] == null)
            {
                return RedirectToAction("Index", "Home");

            }
            var typeName = (string)Session["Type"];
            var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();

            Order order = db.Orders.Find(id);

            if (type.Finance == true)
            {
                order.CanclationReason = comment;

                order.BuyingApprove = false;
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
            }


            return RedirectToAction("Index");
        }


        public ActionResult ProofRefuse(int? id, string comment)
        {
            if (id == null)
            {
                return RedirectToAction("Default", "Home");
            }

            if (Session["ID"] == null)
            {
                return RedirectToAction("Index", "Home");

            }
            var typeName = (string)Session["Type"];
            var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();

            Order order = db.Orders.Find(id);

            if (type.Finance == true)
            {
                order.CanclationReason = comment;

                order.ProofAcceptance = false;
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
            }


            return RedirectToAction("Index");
        }


        public ActionResult PaymentApprove(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Default", "Home");
            }
            if (Session["ID"] == null)
            {
                return RedirectToAction("Index", "Home");

            }
            Order order = db.Orders.Find(id);
            var typeName = (string)Session["Type"];
            var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
            if (type.Finance == true)
            {
                if (order.Bnd.AfterReductionNum - order.SumPrice < 0)
                {
                    TempData["BndOverload"] = "لا يمكنك تجاوز ميزانية البند... الميزانية المتبقية " + order.Bnd.AfterReductionNum;
                    return RedirectToAction("Index");
                }
                else
                {
                    try
                    {
                        Center ce = db.Centers.Find(order.CenterID);
                        if (ce.DependedOn != null)
                        {
                            Center center = db.Centers.Find(ce.DependedOn);
                            try
                            {
                                if (center.SpentBudget != null)
                                {
                                    center.SpentBudget += order.SumPrice;
                                }
                                else
                                {
                                    center.SpentBudget = order.SumPrice;
                                }
                            }
                            catch
                            {
                                center.SpentBudget = order.SumPrice;
                            }
                            db.Entry(center).State = EntityState.Modified;
                        }
                        else
                        {
                            try
                            {
                                if (ce.SpentBudget != null)
                                {
                                    ce.SpentBudget += order.SumPrice;
                                }
                                else
                                {
                                    ce.SpentBudget = order.SumPrice;
                                }
                            }
                            catch
                            {
                                ce.SpentBudget = order.SumPrice;
                            }
                            db.Entry(ce).State = EntityState.Modified;
                        }
                    }
                    catch { }
                    SubBnd subBnd = new SubBnd();
                    try
                    {
                        subBnd.id = db.SubBnds.OrderByDescending(x => x.id).FirstOrDefault().id + 1;
                    }
                    catch (Exception)
                    {
                        subBnd.id = 1;
                    }
                    subBnd.Orderid = order.id;
                    subBnd.BndId = order.Bndid;
                    subBnd.CenterID = order.CenterID;
                    subBnd.CreatedBy = order.Employeeid;
                    subBnd.Date = order.Date;
                    subBnd.PeacePrice = order.PeacePrice;
                    subBnd.Quantity = order.Quantity;
                    subBnd.Subject = order.Subject;
                    subBnd.SumPrice = order.SumPrice;
                    Bnd bnd = db.Bnds.Find(order.Bndid);
                    bnd.AfterReductionNum -= order.SumPrice;
                    order.SubBndid = subBnd.id;
                    order.PaymentApprove = true;
                    order.PaymentApprovalDate = DateTime.Now.Date;
                    order.CanclationReason = null;
                    db.SubBnds.Add(subBnd);
                    db.Entry(bnd).State = EntityState.Modified;
                    db.Entry(order).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult BuyingApprove(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Default", "Home");
            }
            if (Session["ID"] == null)
            {
                return RedirectToAction("Index", "Home");

            }
            Order order = db.Orders.Find(id);
            var empid = Convert.ToInt32(Session["ID"]);
            var typeName = (string)Session["Type"];
            var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();

            if (empid == order.Employeeid)
            {
                order.BuyingApprove = true;
                order.BuyingApprovalDate = DateTime.Now.Date;
                order.CanclationReason = null;
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public ActionResult ProofAcceptance(int? id)
        {

            if (id == null)
            {
                return RedirectToAction("Default", "Home");
            }
            if (Session["ID"] == null)
            {
                return RedirectToAction("Index", "Home");

            }
            Order order = db.Orders.Find(id);
            var typeName = (string)Session["Type"];
            var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
            if (type.Finance == true)
            {
                order.ProofAcceptance = true;
                order.ProofAcceptanceDate = DateTime.Now.Date;
                order.State = "منتهية";
                order.CanclationReason = null;
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }



        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Default", "Home");
            }
            if (Session["ID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Order order = db.Orders.Find(id);
            var empid = Convert.ToInt32(Session["ID"]);
            if (empid == order.Employeeid)
            {
                if (order.FirstLevelSign != true && order.SecondLevelSign != true && order.ThirdLevelSign != true)
                {
                    return View(db.Orders.Find(id));

                }
                else
                {
                    TempData["BndOverload"] = "لا يمكنك حذف طلب تم الموافقة عليه";
                }
            }
            return RedirectToAction("Index");
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var order = db.Orders.Find(id);
            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}