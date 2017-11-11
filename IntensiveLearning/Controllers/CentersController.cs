using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IntensiveLearning.Database;
using IntensiveLearning.Models;
using System.IO.Compression;
using System.Threading;

namespace IntensiveLearning.Controllers
{
    public class CentersController : Controller
    {
        private TaalimEntities db = new TaalimEntities();
        // GET: Centers
        public ActionResult Index()
        {
            if (Session["ID"] != null)
            {
                var typeName = (string)Session["Type"]; var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
                if (type.SeeAll == true || type.SeeAllButFinance == true)
                {
                    var centers = db.Centers;

                    foreach (var item in centers)
                    {

                        if (DateTime.Now.Month == 1 && item.Month1 == true)
                        {
                            item.State = "متوفر";
                        }

                        else if (DateTime.Now.Month == 2 && item.Month2 == true)
                        {
                            item.State = "متوفر";
                        }

                        else if (DateTime.Now.Month == 4 && item.Month3 == true)
                        {

                            item.State = "متوفر";

                        }
                        else if (DateTime.Now.Month == 5 && item.Month5 == true)
                        {


                            item.State = "متوفر";

                        }

                        else if (DateTime.Now.Month == 7 && item.Month6 == true)
                        {

                            item.State = "متوفر";

                        }

                        else if (DateTime.Now.Month == 8 && item.Month8 == true)
                        {

                            item.State = "متوفر";

                        }
                        else if (DateTime.Now.Month == 9 && item.Month9 == true)
                        {


                            item.State = "متوفر";

                        }

                        else if (DateTime.Now.Month == 10 && item.Month10 == true)
                        {

                            item.State = "متوفر";

                        }

                        else if (DateTime.Now.Month == 11 && item.Month11 == true)
                        {

                            item.State = "متوفر";

                        }
                        else if (DateTime.Now.Month == 12 && item.Month12 == true)
                        {


                            item.State = "متوفر";

                        }
                        else
                        {
                            item.State = "خارج الخدمة";
                        }

                    }


                    if (TempData["Message"] != null)
                    {
                        ViewBag.StateMessage = TempData["Message"];
                    }
                    return View(centers.ToList());
                }
                else if (type.SeeAccToCity == true)
                {
                    var id = Convert.ToInt32(Session["ID"]);
                    var emp = db.Employees.FirstOrDefault(x => x.id == id);
                    var centers = db.Centers.Where(x => x.Cityid == emp.CityID);
                    foreach (var item in centers)
                    {

                        if (DateTime.Now.Month == 1 && item.Month1 == true)
                        {
                            item.State = "متوفر";
                        }

                        else if (DateTime.Now.Month == 2 && item.Month2 == true)
                        {
                            item.State = "متوفر";
                        }

                        else if (DateTime.Now.Month == 4 && item.Month3 == true)
                        {

                            item.State = "متوفر";

                        }
                        else if (DateTime.Now.Month == 5 && item.Month5 == true)
                        {


                            item.State = "متوفر";

                        }

                        else if (DateTime.Now.Month == 7 && item.Month6 == true)
                        {

                            item.State = "متوفر";

                        }

                        else if (DateTime.Now.Month == 8 && item.Month8 == true)
                        {

                            item.State = "متوفر";

                        }
                        else if (DateTime.Now.Month == 9 && item.Month9 == true)
                        {


                            item.State = "متوفر";

                        }

                        else if (DateTime.Now.Month == 10 && item.Month10 == true)
                        {

                            item.State = "متوفر";

                        }

                        else if (DateTime.Now.Month == 11 && item.Month11 == true)
                        {

                            item.State = "متوفر";

                        }
                        else if (DateTime.Now.Month == 12 && item.Month12 == true)
                        {


                            item.State = "متوفر";

                        }
                        else
                        {
                            item.State = "خارج الخدمة";
                        }

                    }
                    return View(centers.ToList());
                }
                return RedirectToAction("Default", "Home");

            }
            return RedirectToAction("Index", "Home");
        }

        // GET: Centers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Default", "Home");
            }
            if (Session["ID"] != null)
            {
                var typeName = (string)Session["Type"]; var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
                if (type.SeeAll == true || type.SeeAllButFinance == true || type.SeeAccToCity == true)
                {
                    Center center = db.Centers.Find(id);
                    if (center == null)
                    {
                        return HttpNotFound();
                    }
                    if (type.SeeAccToCity == true)
                    {
                        var empid = Convert.ToInt32(Session["ID"]);
                        var emp = db.Employees.FirstOrDefault(x => x.id == empid);
                        if (emp.CityID != center.Cityid)
                        {
                            return RedirectToAction("Default", "Home");
                        }
                    }
                    return View(center);
                }
                return RedirectToAction("Default", "Home");
            }
            return RedirectToAction("Index", "Home");
        }

        // GET: Centers/Create
        public ActionResult Create()
        {
            if (Session["ID"] != null)
            {
                var typeName = (string)Session["Type"];
                var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
                var id = Convert.ToInt32(Session["ID"]);
                Employee employee = db.Employees.Find(id);
                if (type.AddCitesAndCenters == true)
                {
                    if (type.SeeAccToCity == true)
                    {
                        ViewBag.Periodid = new SelectList(db.Periods, "id", "Name");

                        ViewBag.Cityid = new SelectList(db.Cities.Where(x => x.id == employee.CityID), "id", "Name");

                        ViewBag.DependedOn = new SelectList(db.Centers.Where(x => x.Cityid == employee.CityID && x.CenterType == "رئيسي"), "id", "Name");
                    }
                    else
                    {
                        ViewBag.Periodid = new SelectList(db.Periods, "id", "Name");

                        ViewBag.Cityid = new SelectList(db.Cities, "id", "Name");

                        ViewBag.DependedOn = new SelectList(db.Centers.Where(x=> x.CenterType == "رئيسي"), "id", "Name");
                    }
                    return View();
                }
                return RedirectToAction("Default", "Home");
            }
            return RedirectToAction("Index", "Home");
        }

        // POST: Centers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(/*[Bind(Include = "Name,Place,Desc,State,HolesN,Cityid")]*/ CenterModel centerModel, IEnumerable<HttpPostedFileBase> file)
        {
            if (Session["ID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (centerModel.DependedOn == null && centerModel.Cityid == null)
            {
                ViewBag.error = "الرجاء التأكد من اتمام معلومات المركز ومدينته";
                return View(centerModel);
            }
            bool sendImageError = false;
            bool proceed = true;
            bool NumberValidation = false;
            var typeName = (string)Session["Type"];
            var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
            var empid = Convert.ToInt32(Session["ID"]);
            var employee = db.Employees.Find(empid);
            if (centerModel.firstMonth > 9)
            {
                if (centerModel.LastMonth > 9 && centerModel.LastMonth < centerModel.firstMonth)
                {
                    NumberValidation = true;
                }
            }
            else if (centerModel.firstMonth > centerModel.LastMonth)
            {
                NumberValidation = true;
            }

            if (NumberValidation)
            {
                ViewBag.Periodid = new SelectList(db.Periods, "id", "Name");
                ViewBag.error = "يرجى التأكد من الأشهر";
                ViewBag.Cityid = new SelectList(db.Cities, "id", "Name");
                return View(centerModel);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    centerModel.center.id = db.Centers.OrderByDescending(x => x.id).FirstOrDefault().id + 1;
                }
                catch (Exception)
                {
                    centerModel.center.id = 1;
                }

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
                    if ((Directory.Exists(Server.MapPath("~/App_Data/Centers") + "\\" + centerModel.center.id)))
                    {
                        try
                        {
                            Directory.Delete(Server.MapPath("~/App_Data/Centers") + "\\" + centerModel.center.id, true);
                        }
                        catch (IOException)
                        {
                            Directory.Delete(Server.MapPath("~/App_Data/Centers") + "\\" + centerModel.center.id, true);
                        }
                        catch (UnauthorizedAccessException)
                        {
                            Directory.Delete(Server.MapPath("~/App_Data/Centers") + "\\" + centerModel.center.id, true);
                        }
                    }
                    foreach (var item in file)
                    {
                        if (item.ContentLength > 0)
                        {
                            var fileName = Path.GetFileName(item.FileName);


                            if (!Directory.Exists(Server.MapPath("~/App_Data/Centers") + "\\" + centerModel.center.id))
                            {
                                Directory.CreateDirectory(Server.MapPath("~/App_Data/Centers") + "\\" + centerModel.center.id);
                            }
                            var path = Path.Combine(Server.MapPath("~/App_Data/Centers/" + centerModel.center.id), fileName);
                            item.SaveAs(path);
                            Proove proove = new Proove();
                            proove.Path = path;
                            proove.id = prooveid;
                            proove.CenterID = centerModel.center.id;
                            db.Prooves.Add(proove);
                            prooveid++;

                        }
                        else
                        {

                        }
                    }

                    var startPath = Server.MapPath("~/App_Data/Centers" + "\\" + centerModel.center.id);

                    if (!Directory.Exists(Server.MapPath("~/App_Data/Centers" + "\\" + "ZipFolder")))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/App_Data/Centers" + "\\" + "ZipFolder"));
                    }
                    var zipPath = Server.MapPath("~/App_Data/Centers" + "\\" + "ZipFolder") + "\\" + centerModel.center.id + ".zip";

                    if (System.IO.File.Exists(zipPath))
                    {
                        System.IO.File.Delete(zipPath);
                    }
                    try
                    {
                        ZipFile.CreateFromDirectory(startPath, zipPath);
                    }
                    catch (Exception wx) { }
                    centerModel.center.Proof = zipPath;

                }
                catch (Exception ex)
                {


                }
                centerModel.center.ProjectID = 1;
                if (centerModel.firstMonth > centerModel.LastMonth)
                {
                    for (int i = centerModel.firstMonth; i <= 12; i++)
                    {
                        switch (i)
                        {
                            case 10:
                                centerModel.center.Month10 = true;
                                break;
                            case 11:
                                centerModel.center.Month11 = true;
                                break;
                            case 12:
                                centerModel.center.Month12 = true;
                                break;

                        }
                    }
                    centerModel.firstMonth = 1;
                }

                for (int i = centerModel.firstMonth; i <= centerModel.LastMonth; i++)
                {
                    switch (i)
                    {
                        case 1:
                            centerModel.center.Month1 = true;
                            break;
                        case 2:
                            centerModel.center.Month2 = true;
                            break;
                        case 3:
                            centerModel.center.Month3 = true;
                            break;
                        case 4:
                            centerModel.center.Month4 = true;
                            break;
                        case 5:
                            centerModel.center.Month5 = true;
                            break;
                        case 6:
                            centerModel.center.Month6 = true;
                            break;
                        case 7:
                            centerModel.center.Month7 = true;
                            break;
                        case 8:
                            centerModel.center.Month8 = true;
                            break;
                        case 9:
                            centerModel.center.Month9 = true;
                            break;
                        case 10:
                            centerModel.center.Month10 = true;
                            break;
                        case 11:
                            centerModel.center.Month11 = true;
                            break;
                        case 12:
                            centerModel.center.Month12 = true;
                            break;
                    }
                }

                if (centerModel.Cityid == null)
                {
                    centerModel.center.Cityid = db.Centers.Find(centerModel.DependedOn).Cityid;
                }
                else
                {
                    centerModel.center.Cityid = centerModel.Cityid;
                }
                centerModel.center.AddedBy = employee.id;
                centerModel.center.AddingDate = DateTime.Now.Date;
                centerModel.center.AddingTime = DateTime.Now.TimeOfDay;
                centerModel.center.DependedOn = centerModel.DependedOn;
                if (type.SeeAccToCity == true)
                {
                    var id = Convert.ToInt32(Session["ID"]);
                    var emp = db.Employees.FirstOrDefault(x => x.id == id);
                    centerModel.center.Cityid = emp.CityID;
                }
                if (proceed)
                {
                    db.Centers.Add(centerModel.center);
                    db.SaveChanges();
                    TempData["Message"] = "تم الادخال بنجاح";
                    return RedirectToAction("Index");
                }
            }
            if (sendImageError)
            {
                ViewBag.error = "يرجى ارفاق الاثبات كملف خارجي";
            }
            if (type.SeeAccToCity == true)
            {
                ViewBag.Periodid = new SelectList(db.Periods, "id", "Name");

                ViewBag.Cityid = new SelectList(db.Cities.Where(x => x.id == employee.CityID), "id", "Name");

                ViewBag.DependedOn = new SelectList(db.Centers.Where(x => x.Cityid == employee.CityID && x.CenterType == "رئيسي"), "id", "Name");
            }
            else
            {
                ViewBag.Periodid = new SelectList(db.Periods, "id", "Name");

                ViewBag.Cityid = new SelectList(db.Cities, "id", "Name");

                ViewBag.DependedOn = new SelectList(db.Centers.Where(x => x.CenterType == "رئيسي"), "id", "Name");
            }
            return View(centerModel);

        }

        // GET: Centers/Edit/5
        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                return RedirectToAction("Default", "Home");
            }
            if (Session["ID"] != null)
            {
                var typeName = (string)Session["Type"];
                var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
                if (type.AddCitesAndCenters == true)
                {
                    CenterModel centerModel = new CenterModel();

                    centerModel.center = db.Centers.Find(id);


                    if (centerModel.center.Month10 == true)
                    {
                        centerModel.firstMonth = 10;
                    }
                    else if (centerModel.center.Month11 == true)
                    {
                        centerModel.firstMonth = 11;
                    }
                    else if (centerModel.center.Month12 == true)
                    {
                        centerModel.firstMonth = 12;
                    }
                    else if (centerModel.center.Month1 == true)
                    {
                        centerModel.firstMonth = 1;

                    }
                    else if (centerModel.center.Month2 == true)
                    {
                        centerModel.firstMonth = 2;

                    }
                    else if (centerModel.center.Month3 == true)
                    {
                        centerModel.firstMonth = 3;

                    }
                    else if (centerModel.center.Month4 == true)
                    {
                        centerModel.firstMonth = 4;
                    }
                    else if (centerModel.center.Month5 == true)
                    {
                        centerModel.firstMonth = 5;

                    }
                    else if (centerModel.center.Month6 == true)
                    {
                        centerModel.firstMonth = 6;

                    }
                    else if (centerModel.center.Month7 == true)
                    {
                        centerModel.firstMonth = 7;

                    }
                    else if (centerModel.center.Month8 == true)
                    {
                        centerModel.firstMonth = 8;

                    }
                    else if (centerModel.center.Month9 == true)
                    {
                        centerModel.firstMonth = 9;

                    }





                    if (centerModel.center.Month9 == true)
                    {
                        centerModel.LastMonth = 9;
                    }
                    else if (centerModel.center.Month8 == true)
                    {
                        centerModel.LastMonth = 8;
                    }
                    else if (centerModel.center.Month7 == true)
                    {
                        centerModel.LastMonth = 7;
                    }
                    else if (centerModel.center.Month6 == true)
                    {
                        centerModel.LastMonth = 6;

                    }
                    else if (centerModel.center.Month5 == true)
                    {
                        centerModel.LastMonth = 5;

                    }
                    else if (centerModel.center.Month4 == true)
                    {
                        centerModel.LastMonth = 4;

                    }
                    else if (centerModel.center.Month3 == true)
                    {
                        centerModel.LastMonth = 3;
                    }
                    else if (centerModel.center.Month2 == true)
                    {
                        centerModel.LastMonth = 2;

                    }
                    else if (centerModel.center.Month1 == true)
                    {
                        centerModel.LastMonth = 1;

                    }
                    else if (centerModel.center.Month12 == true)
                    {
                        centerModel.LastMonth = 12;

                    }
                    else if (centerModel.center.Month11 == true)
                    {
                        centerModel.LastMonth = 11;

                    }
                    else if (centerModel.center.Month10 == true)
                    {
                        centerModel.LastMonth = 10;

                    }


                    if (centerModel == null)
                    {
                        return HttpNotFound();
                    }
                    if (type.SeeAccToCity == true)
                    {
                        var empid = Convert.ToInt32(Session["ID"]);
                        var emp = db.Employees.FirstOrDefault(x => x.id == empid);
                        if (emp.CityID != centerModel.center.Cityid)
                        {
                            return RedirectToAction("Default", "Home");
                        }
                        ViewBag.Periodid = new SelectList(db.Periods, "id", "Name", centerModel.center.Periodid);
                        ViewBag.Cityid = new SelectList(db.Cities.Where(x => x.id == emp.CityID), "id", "Name", centerModel.center.Cityid);
                        ViewBag.DependedOn = new SelectList(db.Centers.Where(x => x.Cityid == emp.CityID && x.CenterType == "رئيسي" && x.id != id), "id", "Name", centerModel.center.DependedOn);

                    }
                    else
                    {
                        ViewBag.Periodid = new SelectList(db.Periods, "id", "Name", centerModel.center.Periodid);
                        ViewBag.Cityid = new SelectList(db.Cities, "id", "Name", centerModel.center.Cityid);
                        ViewBag.DependedOn = new SelectList(db.Centers.Where(x => x.CenterType == "رئيسي" && x.id != id), "id", "Name", centerModel.center.DependedOn);
                    }
                    return View(centerModel);
                }
                return RedirectToAction("Default", "Home");
            }
            return RedirectToAction("Index", "Home");

        }

        // POST: Centers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(/*[Bind(Include = "id,Name,Place,Desc,State,HolesN,Cityid")]*/ CenterModel centerModel, IEnumerable<HttpPostedFileBase> file)
        {
            if (Session["ID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            bool proceed = true;
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
                        var oldImages = db.Prooves.Where(x => x.CenterID == centerModel.center.id).ToList();

                        foreach (var image in oldImages)
                        {

                            db.Prooves.Remove(image);
                        }

                        if ((Directory.Exists(Server.MapPath("~/App_Data/Centers") + "\\" + centerModel.center.id)))
                        {
                            try
                            {
                                Directory.Delete(Server.MapPath("~/App_Data/Centers") + "\\" + centerModel.center.id, true);
                            }
                            catch (IOException)
                            {
                                Directory.Delete(Server.MapPath("~/App_Data/Centers") + "\\" + centerModel.center.id, true);
                            }
                            catch (UnauthorizedAccessException)
                            {
                                Directory.Delete(Server.MapPath("~/App_Data/Centers") + "\\" + centerModel.center.id, true);
                            }
                        }

                        if (!Directory.Exists(Server.MapPath("~/App_Data/Centers") + "\\" + centerModel.center.id))
                        {
                            Directory.CreateDirectory(Server.MapPath("~/App_Data/Centers") + "\\" + centerModel.center.id);
                        }

                        foreach (var item in file)
                        {
                            if (item.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(item.FileName);

                                var path = Path.Combine(Server.MapPath("~/App_Data/Centers/" + centerModel.center.id), fileName);
                                item.SaveAs(path);
                                Proove proove = new Proove();
                                proove.Path = path;
                                proove.id = prooveid;
                                proove.CenterID = centerModel.center.id;
                                db.Prooves.Add(proove);
                                prooveid++;

                            }
                            else
                            {

                            }
                        }
                        try
                        {

                            var startPath = Server.MapPath("~/App_Data/Centers" + "\\" + centerModel.center.id);

                            if (!Directory.Exists(Server.MapPath("~/App_Data/Centers" + "\\" + "ZipFolder")))
                            {
                                Directory.CreateDirectory(Server.MapPath("~/App_Data/Centers" + "\\" + "ZipFolder"));
                            }
                            var zipPath = Server.MapPath("~/App_Data/Centers" + "\\" + "ZipFolder") + "\\" + centerModel.center.id + ".zip";

                            if (System.IO.File.Exists(zipPath))
                            {
                                System.IO.File.Delete(zipPath);
                            }
                            try
                            {
                                ZipFile.CreateFromDirectory(startPath, zipPath);
                            }
                            catch { }
                            centerModel.center.Proof = zipPath;
                        }
                        catch { }
                    }

                }
                catch
                {
                }

                db.SaveChanges();
                if (centerModel.Cityid == null)
                {
                    centerModel.center.Cityid = db.Centers.Find(centerModel.DependedOn).Cityid;
                }
                else
                {
                    centerModel.center.Cityid = centerModel.Cityid;
                }
                centerModel.center.DependedOn = centerModel.DependedOn;

                for (int i = 0; i <= 12; i++)
                {
                    switch (i)
                    {
                        case 1:
                            centerModel.center.Month1 = false;
                            break;
                        case 2:
                            centerModel.center.Month2 = false;
                            break;
                        case 3:
                            centerModel.center.Month3 = false;
                            break;
                        case 4:
                            centerModel.center.Month4 = false;
                            break;
                        case 5:
                            centerModel.center.Month5 = false;
                            break;
                        case 6:
                            centerModel.center.Month6 = false;
                            break;
                        case 7:
                            centerModel.center.Month7 = false;
                            break;
                        case 8:
                            centerModel.center.Month8 = false;
                            break;
                        case 9:
                            centerModel.center.Month9 = false;
                            break;
                        case 10:
                            centerModel.center.Month10 = false;
                            break;
                        case 11:
                            centerModel.center.Month11 = false;
                            break;
                        case 12:
                            centerModel.center.Month12 = false;
                            break;
                    }

                }

                if (centerModel.firstMonth > centerModel.LastMonth)
                {
                    for (int i = centerModel.firstMonth; i <= 12; i++)
                    {
                        switch (i)
                        {
                            case 10:
                                centerModel.center.Month10 = true;
                                break;
                            case 11:
                                centerModel.center.Month11 = true;
                                break;
                            case 12:
                                centerModel.center.Month12 = true;
                                break;

                        }
                    }
                    centerModel.firstMonth = 1;
                }

                for (int i = centerModel.firstMonth; i <= centerModel.LastMonth; i++)
                {
                    switch (i)
                    {
                        case 1:
                            centerModel.center.Month1 = true;
                            break;
                        case 2:
                            centerModel.center.Month2 = true;
                            break;
                        case 3:
                            centerModel.center.Month3 = true;
                            break;
                        case 4:
                            centerModel.center.Month4 = true;
                            break;
                        case 5:
                            centerModel.center.Month5 = true;
                            break;
                        case 6:
                            centerModel.center.Month6 = true;
                            break;
                        case 7:
                            centerModel.center.Month7 = true;
                            break;
                        case 8:
                            centerModel.center.Month8 = true;
                            break;
                        case 9:
                            centerModel.center.Month9 = true;
                            break;
                        case 10:
                            centerModel.center.Month10 = true;
                            break;
                        case 11:
                            centerModel.center.Month11 = true;
                            break;
                        case 12:
                            centerModel.center.Month12 = true;
                            break;
                    }
                }
                if (proceed)
                {
                    db.Entry(centerModel.center).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["Message"] = "تم التعديل بنجاح";
                    return RedirectToAction("Index");
                }
            }
            var typeName = (string)Session["Type"]; var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();

            if (type.SeeAccToCity == true)
            {
                var id = Convert.ToInt32(Session["ID"]);
                var emp = db.Employees.Find(id);
                ViewBag.Periodid = new SelectList(db.Periods, "id", "Name", centerModel.center.Periodid);
                ViewBag.Cityid = new SelectList(db.Cities.Where(x => x.id == emp.CityID), "id", "Name", centerModel.center.Cityid);
                ViewBag.DependedOn = new SelectList(db.Centers.Where(x => x.Cityid == emp.CityID && x.CenterType == "رئيسي" && x.id != centerModel.center.id), "id", "Name", centerModel.center.DependedOn);

            }
            else
            {
                ViewBag.Periodid = new SelectList(db.Periods, "id", "Name", centerModel.center.Periodid);
                ViewBag.Cityid = new SelectList(db.Cities, "id", "Name", centerModel.center.Cityid);
                ViewBag.DependedOn = new SelectList(db.Centers.Where(x=> x.CenterType == "رئيسي" && x.id != centerModel.center.id), "id", "Name", centerModel.center.DependedOn);

            }


            return View(centerModel);
        }

        // GET: Centers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Default", "Home");
            }
            if (Session["ID"] != null)
            {
                var typeName = (string)Session["Type"]; var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
                if (type.AddCitesAndCenters == true)
                {
                    Center center = db.Centers.Find(id);
                    if (center == null)
                    {
                        return HttpNotFound();
                    }
                    if (type.SeeAccToCity == true)
                    {
                        var empid = Convert.ToInt32(Session["ID"]);
                        var emp = db.Employees.FirstOrDefault(x => x.id == empid);
                        if (emp.CityID != center.Cityid)
                        {
                            return RedirectToAction("Default", "Home");
                        }
                    }
                    return View(center);
                }
                return RedirectToAction("Default", "Home");
            }
            return RedirectToAction("Index", "Home");

        }

        // POST: Centers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["ID"] != null)
            {
                var typeName = (string)Session["Type"];
                var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
                if (type.AddCitesAndCenters == true)
                {
                    Center center = db.Centers.Find(id);
                    var path = Server.MapPath("~\\App_Data\\Centers\\");
                    var prooves = db.Prooves.Where(x => x.CenterID == center.id);
                    foreach (var item in prooves)
                    {
                        db.Prooves.Remove(item);
                    }

                    db.Centers.Remove(center);

                    try
                    {
                        db.SaveChanges();
                        if (Directory.Exists(path + "\\" + center.id))
                        {


                            try
                            {
                                Directory.Delete(path + "\\" + center.id, true);
                            }
                            catch (IOException)
                            {
                                Directory.Delete(path + "\\" + center.id, true);
                            }
                            catch (UnauthorizedAccessException)
                            {
                                Directory.Delete(path + "\\" + center.id, true);
                            }
                        }

                        if (System.IO.File.Exists(path + "ZipFolder\\" + center.id + ".zip"))
                        {
                            System.IO.File.Delete(path + "ZipFolder\\" + center.id + ".zip");
                        }


                    }
                    catch
                    {
                        ViewBag.error = "يوجد مدخلات اخرى متعلقة بهذا المركز يرجى تغييرها قبل الحذف";
                        return View(center);
                    }
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Default", "Home");
            }
            return RedirectToAction("Index", "Home");

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
