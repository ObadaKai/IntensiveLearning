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
                if (type.AddCitesAndCenters == true)
                {
                    ViewBag.Cityid = new SelectList(db.Cities, "id", "Name");
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
            bool proceed = true;
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
                try
                {
                    var startPath = Server.MapPath("~/App_Data/Centers" + "/" + centerModel.center.id);
                    var zipPath = Server.MapPath("~/App_Data/Centers" + "/" + centerModel.center.id) + "\\" + centerModel.center.id + ".zip";
                    var proove = db.Prooves.Where(x => x.CenterID == centerModel.center.id).Select(x => x.Path);
                    try
                    {
                        ZipFile.CreateFromDirectory(startPath, zipPath, CompressionLevel.Fastest, true);
                    }
                    catch (Exception wx) { }
                    centerModel.center.Proof = zipPath;
                }
                catch { }


            }
            catch (Exception ex)
            {
                ViewBag.error = "يرجى ارفاق الاثبات كملف خارجي";
                proceed = false;
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


            centerModel.center.Cityid = centerModel.Cityid;
            var typeName = (string)Session["Type"];
            var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
            if (type.SeeAccToCity == true)
            {
                var id = Convert.ToInt32(Session["ID"]);
                var emp = db.Employees.FirstOrDefault(x => x.id == id);
                centerModel.center.Cityid = emp.CityID;
            }
            if (proceed)
            {
                if (ModelState.IsValid)
                {
                    db.Centers.Add(centerModel.center);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            ViewBag.Cityid = new SelectList(db.Cities, "id", "Name");
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

                    centerModel.center.Cityid = centerModel.Cityid;
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
                    }
                    ViewBag.Cityid = new SelectList(db.Cities, "id", "Name");
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


            bool proceed = true;
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

                if (file.Count() > 0)
                {
                    var oldImages = db.Prooves.Where(x => x.CenterID == centerModel.center.id).ToList();

                    foreach (var image in oldImages)
                    {
                        var path = Path.GetDirectoryName(image.Path);
                        if ((Directory.Exists(path)))
                        {
                            try
                            {
                                Directory.Delete(path, true);
                            }
                            catch (IOException)
                            {
                                Directory.Delete(path, true);
                            }
                            catch (UnauthorizedAccessException)
                            {
                                Directory.Delete(path, true);
                            }
                        }
                        db.Prooves.Remove(image);
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
                try
                {
                    var startPath = Server.MapPath("~/App_Data/Centers" + "/" + centerModel.center.id);
                    var zipPath = Server.MapPath("~/App_Data/Centers" + "/" + centerModel.center.id) + "\\" + centerModel.center.id + ".zip";
                    var proove = db.Prooves.Where(x => x.CenterID == centerModel.center.id).Select(x => x.Path);
                    try
                    {
                        ZipFile.CreateFromDirectory(startPath, zipPath, CompressionLevel.Fastest, true);
                    }
                    catch (Exception wx) { }
                    centerModel.center.Proof = zipPath;
                }
                catch { }


            }
            catch (Exception ex)
            {
                ViewBag.error = "يرجى ارفاق الاثبات كملف خارجي";
                proceed = false;
            }


            centerModel.center.Cityid = centerModel.Cityid;
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
                if (ModelState.IsValid)
                {
                    db.Entry(centerModel.center).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            ViewBag.Cityid = new SelectList(db.Cities, "id", "Name");
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
                    var pathW = center.Proof;
                    var prooves = db.Prooves.Where(x => x.CenterID == center.id);
                    foreach (var item in prooves)
                    {
                        db.Prooves.Remove(item);
                    }

                    db.Centers.Remove(center);
                    try
                    {
                        db.SaveChanges();
                        var path = Path.GetDirectoryName(pathW);
                        try
                        {
                            Directory.Delete(path, true);
                        }
                        catch (IOException)
                        {
                            Directory.Delete(path, true);
                        }
                        catch (UnauthorizedAccessException)
                        {
                            Directory.Delete(path, true);
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
