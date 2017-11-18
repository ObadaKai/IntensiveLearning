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
        // GET: Orders
        public ActionResult Index()
        {
            if (Session["ID"] != null)
            {
                var typeName = (string)Session["Type"];
                var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

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
                order = db.Orders.Where(x => x.Center.Cityid == emp.CityID).ToList();
            }
            else if (type.SeeAccToCenter == true)
            {
                var emp = db.Employees.Find(empid);
                order = db.Orders.Where(x => x.CenterID == emp.Centerid).ToList();
            }
            else if (type.SeeTeachers == true)
            {
                order = db.Orders.Where(x => x.Employeeid == empid).ToList();
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
                Bought = x.BuyingSign,
                proof = x.SubBnd.proof,
            }).ToList();

            var Bnds = Bnd.Select(x => new
            {
                id = x.id,
                Name = x.Name,
            }).ToList();

            List<object> toSendList = new List<object>();
            toSendList.Add(Orders);
            toSendList.Add(empid);
            toSendList.Add(Bnds);
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
                    order.SubBnd.proof = zipPath;
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



                if (type.AddBuyingRequest == true)
                {
                    if (emp.Centerid == null)
                    {
                        order.Date = DateTime.Now.Date;
                        order.Employeeid = empid;
                        order.Time = DateTime.Now.TimeOfDay;
                        //مالي
                        if (type.Finance == true && type.CoManager == true && type.BuyingAcceptance == true)
                        {
                            order.SecondLevelSign = true;
                        }
                        //مدير تنفيذي
                        else if (type.Manager == true && type.AddNewProject == true && type.BuyingAcceptance == true)
                        {
                            order.ThirdLevelSign = true;
                        }
                        //مراقبة وتقييم
                        //else if (type.CoManager == true && type.HighAcceptance == true && type.BuyingAcceptance == true)
                        //{
                        //    order.ThirdLevelSign = true;
                        //}
                        //متابعة
                        else if (type.CoManager == true && type.SeeAll == true && type.BuyingAcceptance == true)
                        {
                            order.FirstLevelSign = true;
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
            return Json(order, JsonRequestBehavior.AllowGet);
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


            }
            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();
            if (order.FirstLevelSign == true && order.SecondLevelSign == true && order.ThirdLevelSign == true && order.Bndid != null)
            {
                SubBnd subBnd = new SubBnd();

                try
                {
                    subBnd.id = db.SubBnds.OrderByDescending(x => x.id).FirstOrDefault().id + 1;
                }
                catch (Exception)
                {
                    subBnd.id = 1;
                }
                subBnd.BndId = order.Bndid;
                subBnd.CenterId = order.CenterID;
                subBnd.Date = order.Date;
                order.SubBndid = subBnd.id;
                order.State = "تم قبول الطلب";
                subBnd.PeacePrice = order.PeacePrice;
                subBnd.Quantity = (int)order.Quantity;
                subBnd.Subject = order.Subject;
                subBnd.SumPrice = subBnd.PeacePrice * subBnd.Quantity;



                PaymentsRecord paymentsRecord = new PaymentsRecord();
                try
                {
                    paymentsRecord.id = db.PaymentsRecords.OrderByDescending(x => x.id).FirstOrDefault().id + 1;
                }
                catch (Exception)
                {
                    paymentsRecord.id = 1;
                }
                paymentsRecord.Orderid = order.id;
                db.PaymentsRecords.Add(paymentsRecord);
                subBnd.Paymentid = paymentsRecord.id;
                db.SubBnds.Add(subBnd);
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();

            }
            return RedirectToAction("Index");
        }

        public JsonResult AssignBnd(int Bndid, int id)
        {
            var order = db.Orders.Find(id);
            order.Bndid = Bndid;
            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();
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
                    var path = Path.Combine(Server.MapPath("~/App_Data/Orders"), fileName);
                    Order mystd = db.Orders.Find(order.id);
                    string EmpProof = mystd.SubBnd.proof;
                    if ((System.IO.File.Exists(EmpProof)))
                    {
                        System.IO.File.Delete(EmpProof);
                    }
                    file.SaveAs(path);
                    order.SubBnd.proof = path;
                    db.Entry(mystd).State = EntityState.Detached;
                }
                ViewBag.Message = "Upload successful";
            }
            catch
            {

            }
            return View();
        }


        public ActionResult CheckBought(int id) {

            var order = db.Orders.Find(id);
            var empid = Convert.ToInt16(Session["ID"]);
            var emp = db.Employees.Find(empid);
            if (order.Employeeid == empid)
            {
                if (order.FirstLevelSign ==true && order.SecondLevelSign == true && order.ThirdLevelSign == true)
                {
                    order.BuyingSign = true;
                    db.Entry(order).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }


            return RedirectToAction("Index");
        }

    }
}