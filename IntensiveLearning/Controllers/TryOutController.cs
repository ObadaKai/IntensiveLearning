using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IntensiveLearning.Database;

namespace IntensiveLearning.Controllers
{
    public class TryOutController : Controller
    {
        TaalimEntities db = new TaalimEntities();
        // GET: TryOut
        public ActionResult Index()
        {
            //var students = db.Students.Where(x => x.id >= 5163).ToList();
            //foreach (var item in students)
            //{
            //    item.StudentNumber = item.StudentNumber - 1284;
            //    db.Entry(item).State = System.Data.Entity.EntityState.Modified;
            //}


            //var Orders = db.Orders.ToList();
            //foreach (var Order in Orders)
            //{
            //    if (Order.PeacePriceSyrian != null)
            //    {
            //        Order.PeacePrice = Order.PeacePriceSyrian / Order.CommissionPrice;
            //        Order.SumPrice = Order.PeacePrice * Order.Quantity;
            //    }
            //    db.Entry(Order).State = System.Data.Entity.EntityState.Modified;
            //}


            //db.SaveChanges();

            return View();
        }
        public ActionResult AddStudent(Student st)
        {
           return RedirectToAction("Index", "Students");
        }
    }
}