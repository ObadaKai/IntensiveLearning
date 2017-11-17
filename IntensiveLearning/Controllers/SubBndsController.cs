using IntensiveLearning.Database;
using IntensiveLearning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IntensiveLearning.Controllers
{
    public class SubBndsController : Controller
    {
        private static int bndid; 
        TaalimEntities db = new TaalimEntities();
        // GET: SubBnd
        public ActionResult Index(int? id)
        {
            
            if (Session["ID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return RedirectToAction("Index", "Bnds");
            }

            bndid = (int)id;
            return View();
        }


        public JsonResult GetSubBnds()
        {
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
            var Subbnd = db.SubBnds.Where(x=>x.BndId == bndid).ToList();
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
                proof = x.proof
            }).ToList();
            List<object> o = new List<object>();
            foreach (var item in SubBnds)
            {
                var list = new List<object>();
                try { 
                var xxx = db.Centers.Find(item.Centerid).DependedOn;
                    if (xxx != null)
                    {
                        var RealCenter = db.Centers.Find(xxx);
                        list.Add(item.Subject);
                        list.Add(item.PeacePrice);
                        list.Add(item.Quantity);
                        list.Add(item.Center);
                        list.Add(RealCenter.Name);
                        list.Add(item.SumPrice);
                        list.Add(item.Date);
                        list.Add(item.PayymentApprove);
                        list.Add(item.BuyingApprove);
                        list.Add(item.ProofAcceptance);
                        list.Add(item.proof);

                        list.Add(item.Bndid);
                        list.Add(item.Bnd);
                        list.Add(item.Centerid);
                        list.Add(item.id);

                    }
                    else
                    {
                        list.Add(item.Subject);
                        list.Add(item.PeacePrice);
                        list.Add(item.Quantity);
                        list.Add(item.Center);
                        list.Add(item.Center);
                        list.Add(item.SumPrice);
                        list.Add(item.Date);
                        list.Add(item.PayymentApprove);
                        list.Add(item.BuyingApprove);
                        list.Add(item.ProofAcceptance);
                        list.Add(item.proof);

                        list.Add(item.Bndid);
                        list.Add(item.Bnd);
                        list.Add(item.Centerid);
                        list.Add(item.id);
                    }
                }
                catch
                {
                    list.Add(item.Subject);
                    list.Add(item.PeacePrice);
                    list.Add(item.Quantity);
                    list.Add(item.Center);
                    list.Add(item.Center);
                    list.Add(item.SumPrice);
                    list.Add(item.Date);
                    list.Add(item.PayymentApprove);
                    list.Add(item.BuyingApprove);
                    list.Add(item.ProofAcceptance);
                    list.Add(item.proof);

                    list.Add(item.Bndid);
                    list.Add(item.Bnd);
                    list.Add(item.Centerid);
                    list.Add(item.id);
                }
                o.Add(list);


                
            }

            List<object> toSendList = new List<object>();
            toSendList.Add(o);
            toSendList.Add(empid);
            toSendList.Add(ToSendBnd);
            toSendList.Add(ToSendCenters);
            return Json(toSendList, JsonRequestBehavior.AllowGet);
        }
    }
}