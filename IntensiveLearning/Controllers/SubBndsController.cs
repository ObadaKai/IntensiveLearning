using IntensiveLearning.Database;
using IntensiveLearning.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IntensiveLearning.Controllers
{
    public class SubBndsController : Controller
    {
        TaalimEntities db = new TaalimEntities();
        // GET: SubBnd
        public ActionResult Index(int? id)
        {
            if (Session["ID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (id != null)
            {
                Session["Bndid"] = id;
            }


            return View();
        }


        public JsonResult GetSubBnds()
        {
            var bndid = Convert.ToInt32(Session["Bndid"]);
            var bnd = db.Bnds.Find(bndid);
            var sum = db.SubBnds.Where(x => x.BndId == bndid).Sum(c => c.SumPrice);

            var ToSendBnd = new
            {
                Bndid = bnd.id,
                Bndname = bnd.Name,
                BndTotalNum = bnd.TotalNum,
                BndPaidAmount = sum,
                BndRemaining = bnd.TotalNum - sum,
            };

            List<SubBndCenters> AllCenters = new List<SubBndCenters>();
            List<SubBndCenters> ToSendCenters = new List<SubBndCenters>();
            foreach (var item in db.Centers)
            {
                var center = db.SubBnds.Where(x => x.CenterId == item.id).Sum(x => x.SumPrice);
                SubBndCenters Allc = new SubBndCenters();
                Allc.id = item.id;
                Allc.Name = item.Name;
                Allc.Center = center;
                AllCenters.Add(Allc);
            }

            foreach (var item in AllCenters)
            {
                Center center = db.Centers.Find(item.id);
                if (center.DependedOn != null)
                {
                    AllCenters.Find(x => x.id == center.DependedOn).Center += item.Center;
                }

            }


            foreach (var item in AllCenters)
            {
                Center center = db.Centers.Find(item.id);
                if (center.DependedOn == null)
                {
                    SubBndCenters Allc = new SubBndCenters();
                    Allc.id = item.id;
                    Allc.Name = item.Name;
                    Allc.Center = item.Center;
                    ToSendCenters.Add(Allc);
                }

            }

            var empid = Convert.ToInt32(Session["ID"]);
            var Subbnd = db.SubBnds.Where(x => x.BndId == bndid).ToList();

            foreach (var CC in Subbnd)
            {
                if (CC.Bnd == null)
                {
                    CC.Bnd = new Bnd();
                }
                if (CC.Center == null)
                {
                    CC.Center = new Center();
                }

            }


            var SubBnds = Subbnd.Select(x => new
            {
                Subject = x.Subject,
                PeacePrice = x.PeacePrice,
                Quantity = x.Quantity,
                Center = x.Center.Name,

                Bndid = x.BndId,
                Bnd = x.Bnd.Name,
                Centerid = x.CenterId,
                CenterDepended = x.Center.Name,
                Date = x.Date,
                id = x.id,
                SumPrice = x.SumPrice,
                PayymentApprove = x.PayymentApprove,
                BuyingApprove = x.BuyingApprove,
                ProofAcceptance = x.ProofAcceptance,
                proof = x.proof,
                Payment = x.Paymentid,
            }).ToList();
            List<SubBndItems> o = new List<SubBndItems>();
            foreach (var item in SubBnds)
            {
                SubBndItems subBndItems = new SubBndItems();
                try
                {
                    var xxx = db.Centers.Find(item.Centerid).DependedOn;
                    if (xxx != null)
                    {
                        var RealCenter = db.Centers.Find(xxx);
                        subBndItems.Subject = item.Subject;
                        subBndItems.PeacePrice = item.PeacePrice;
                        subBndItems.Quantity = item.Quantity;
                        subBndItems.Center = item.Center;
                        subBndItems.CenterDepended = RealCenter.Name;
                        subBndItems.SumPrice = item.SumPrice;
                        subBndItems.Date = item.Date;
                        subBndItems.BuyingApprove = item.BuyingApprove;
                        subBndItems.PayymentApprove = item.PayymentApprove;
                        subBndItems.ProofAcceptance = item.ProofAcceptance;
                        subBndItems.proof = item.proof;
                        subBndItems.Bndid = item.Bndid;
                        subBndItems.Bnd = item.Bnd;
                        subBndItems.Centerid = item.Centerid;
                        subBndItems.id = item.id;
                        subBndItems.Payment = item.Payment;
                        subBndItems.Bought = db.Orders.FirstOrDefault(x => x.SubBndid == item.id).BuyingSign;
                    }
                    else
                    {
                        subBndItems.Subject = item.Subject;
                        subBndItems.PeacePrice = item.PeacePrice;
                        subBndItems.Quantity = item.Quantity;
                        subBndItems.Center = item.Center;
                        subBndItems.CenterDepended = item.Center;
                        subBndItems.SumPrice = item.SumPrice;
                        subBndItems.Date = item.Date;
                        subBndItems.BuyingApprove = item.BuyingApprove;
                        subBndItems.PayymentApprove = item.PayymentApprove;
                        subBndItems.ProofAcceptance = item.ProofAcceptance;
                        subBndItems.proof = item.proof;
                        subBndItems.Bndid = item.Bndid;
                        subBndItems.Bnd = item.Bnd;
                        subBndItems.Centerid = item.Centerid;
                        subBndItems.id = item.id;
                        subBndItems.Payment = item.Payment;
                        subBndItems.Bought = db.Orders.FirstOrDefault(x => x.SubBndid == item.id).BuyingSign;

                    }
                }
                catch
                {
                    subBndItems.Subject = item.Subject;
                    subBndItems.PeacePrice = item.PeacePrice;
                    subBndItems.Quantity = item.Quantity;
                    subBndItems.Center = item.Center;
                    subBndItems.CenterDepended = item.Center;
                    subBndItems.SumPrice = item.SumPrice;
                    subBndItems.Date = item.Date;
                    subBndItems.BuyingApprove = item.BuyingApprove;
                    subBndItems.PayymentApprove = item.PayymentApprove;
                    subBndItems.ProofAcceptance = item.ProofAcceptance;
                    subBndItems.proof = item.proof;
                    subBndItems.Bndid = item.Bndid;
                    subBndItems.Bnd = item.Bnd;
                    subBndItems.Centerid = item.Centerid;
                    subBndItems.id = item.id;
                    subBndItems.Payment = item.Payment;
                    subBndItems.Bought = db.Orders.FirstOrDefault(x => x.SubBndid == item.id).BuyingSign;

                }
                o.Add(subBndItems);



            }

            List<object> toSendList = new List<object>();
            toSendList.Add(o);
            toSendList.Add(empid);
            toSendList.Add(ToSendBnd);
            toSendList.Add(ToSendCenters);
            return Json(toSendList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Bnds");
            }
            if (Session["ID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var typeName = (string)Session["Type"];
            var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();

            ViewBag.CenterId = new SelectList(db.Centers, "id", "Name");
            ViewBag.bndid = id;
            if (type.Finance == true)
            {
                return View();

            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SubBnd subBnd)
        {
            if (Session["ID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            int empid = Convert.ToInt32(Session["ID"]);

            if (ModelState.IsValid)
            {
                try
                {
                    subBnd.id = db.SubBnds.OrderByDescending(x => x.id).FirstOrDefault().id + 1;
                }
                catch (Exception)
                {
                    subBnd.id = 1;
                }
                subBnd.CreatedBy = empid;
                subBnd.Date = DateTime.Now.Date;
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
                subBnd.Paymentid = paymentsRecord.id;
                db.SubBnds.Add(subBnd);
                db.PaymentsRecords.Add(paymentsRecord);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CenterId = new SelectList(db.Centers, "id", "Name");

            return View(subBnd);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                return RedirectToAction("Index", "Bnds");
            }
            if (Session["ID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            int empid = Convert.ToInt32(Session["ID"]);
            SubBnd subBnd = db.SubBnds.Find(id);
            var typeName = (string)Session["Type"];
            var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();

            if (empid == subBnd.CreatedBy && type.Finance == true)
            {
                ViewBag.CenterId = new SelectList(db.Centers, "id", "Name", subBnd.CenterId);
                return View(subBnd);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(SubBnd subBnd)
        {
            if (Session["ID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                subBnd.Date = DateTime.Now.Date;
                subBnd.SumPrice = subBnd.PeacePrice * subBnd.Quantity;
                subBnd.BuyingApprove = null;
                subBnd.ProofAcceptance = null;
                subBnd.PayymentApprove = null;
                db.Entry(subBnd).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CenterId = new SelectList(db.Centers, "id", "Name", subBnd.CenterId);

            return View(subBnd);
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Bnds");
            }
            if (Session["ID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var typeName = (string)Session["Type"];
            var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
            int empid = Convert.ToInt32(Session["ID"]);
            SubBnd subBnd = db.SubBnds.Find(id);
            var Bndid = subBnd.BndId;
            if (type.Finance == true && subBnd.CreatedBy == empid)
            {
                return View(subBnd);
            }
            return RedirectToAction("Index");

        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            if (Session["ID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var typeName = (string)Session["Type"];
            var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
            SubBnd subBnd = db.SubBnds.Find(id);
            var Bndid = subBnd.BndId;
            if (type.Finance == true)
            {
                db.SubBnds.Remove(subBnd);
                db.SaveChanges();
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
            SubBnd subBnd = db.SubBnds.Find(id);
            var typeName = (string)Session["Type"];
            var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();

            if (type.Finance == true)
            {
                subBnd.BuyingApprove = true;
                db.Entry(subBnd).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public ActionResult PayymentApprove(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Default", "Home");
            }
            if (Session["ID"] == null)
            {
                return RedirectToAction("Index", "Home");

            }
            SubBnd subBnd = db.SubBnds.Find(id);
            var typeName = (string)Session["Type"];
            var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();

            if (type.Finance == true)
            {
                subBnd.PayymentApprove = true;
                db.Entry(subBnd).State = EntityState.Modified;
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
            SubBnd subBnd = db.SubBnds.Find(id);
            var typeName = (string)Session["Type"];
            var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();

            if (type.Finance == true)
            {
                try
                {
                    Center ce = db.Centers.Find(subBnd.CenterId);
                    if (ce.DependedOn != null)
                    {
                        Center center = db.Centers.Find(ce.DependedOn);
                        try
                        {
                            if (center.SpentBudget != null)
                            {
                                center.SpentBudget += subBnd.SumPrice;
                            }
                            else
                            {
                                center.SpentBudget = subBnd.SumPrice;
                            }
                        }
                        catch
                        {
                            center.SpentBudget = subBnd.SumPrice;
                        }
                        db.Entry(center).State = EntityState.Modified;
                    }
                    else
                    {
                        try
                        {
                            if (ce.SpentBudget != null)
                            {
                                ce.SpentBudget += subBnd.SumPrice;
                            }
                            else
                            {
                                ce.SpentBudget = subBnd.SumPrice;
                            }
                        }
                        catch
                        {
                            ce.SpentBudget = subBnd.SumPrice;
                        }
                        db.Entry(ce).State = EntityState.Modified;
                    }
                }
                catch { }

                subBnd.ProofAcceptance = true;
                Order order = db.Orders.FirstOrDefault(x => x.SubBndid == id);
                order.State = "منتهية";
                db.Entry(subBnd).State = EntityState.Modified;
                db.Entry(order).State = EntityState.Modified;

                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

    }
}