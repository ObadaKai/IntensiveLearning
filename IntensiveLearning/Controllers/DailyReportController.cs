using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IntensiveLearning.Database;

namespace IntensiveLearning.Controllers
{
    public class DailyReportController : Controller
    {
        TaalimEntities db = new TaalimEntities();
        // GET: DailyReport
        public ActionResult Index()
        {
            if (Session["ID"] != null)
            {
                var typeName = (string)Session["Type"];
                var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
                if (type.SeeAll == true || type.SeeAllButFinance == true )
                {
                    return View(db.DailyActivities.ToList());
                }
                if ( type.SeeAccToCenter == true)
                {
                    var empid = Convert.ToInt32(Session["ID"]);
                    var empCityid = db.Employees.FirstOrDefault(x => x.id == empid).Centerid;
                    return View(db.DailyActivities.Where(x=>x.Employee.Centerid == empCityid).ToList());
                }
                if (type.SeeAccToCity == true)
                {
                    var empid = Convert.ToInt32(Session["ID"]);
                    var empCityid = db.Employees.FirstOrDefault(x => x.id == empid).CityID;
                    return View(db.DailyActivities.Where(x => x.Employee.CityID == empCityid).ToList());
                }
                
                return RedirectToAction("Default", "Home");
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Create()
        {
            if (Session["ID"] != null)
            {
                var typeName = (string)Session["Type"];
                var id = Convert.ToInt32(Session["ID"]);
                var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
                if (type.AddDaileyReport == true)
                {
                    if (!(db.DailyActivities.OrderByDescending(x=>x.Employee.id == id).FirstOrDefault().date == DateTime.Now.Date))
                    {
                        return View();
                    }

                }
                return RedirectToAction("Default", "Home");
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Create(DailyActivity daily)
        {
            try
            {
                daily.id = db.DailyActivities.OrderByDescending(x => x.id).FirstOrDefault().id + 1;
            }
            catch (Exception)
            {
                daily.id = 1;
            }
            if (ModelState.IsValid)
            {
                daily.Managerid = Convert.ToInt32(Session["ID"]);
                daily.date = DateTime.Now.Date;
                db.DailyActivities.Add(daily);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
    }
}