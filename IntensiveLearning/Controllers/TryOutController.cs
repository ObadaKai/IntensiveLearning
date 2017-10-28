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
        // GET: TryOut
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AddStudent(Student st)
        {
           return RedirectToAction("Index", "Students");
        }
    }
}