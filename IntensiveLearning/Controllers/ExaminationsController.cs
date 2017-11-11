using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using IntensiveLearning.Database;
using System.IO.Compression;

namespace IntensiveLearning.Controllers
{
    public class ExaminationsController : Controller
    {
        private TaalimEntities db = new TaalimEntities();

        // GET: Examinations
        public ActionResult Index()
        {
            if (Session["ID"] != null)
            {
                var typeName = (string)Session["Type"];
                var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
                int od = Convert.ToInt32(Session["ID"]);
                Employee emp = db.Employees.Find(od);
                var examinations = db.Examinations.ToList();
                if (type.SeeAll == true || type.SeeAccToCity == true || type.SeeAccToCenter == true || type.SeeAllButFinance == true || type.SeeTeachers == true)
                {
                    if (type.SeeAccToCenter == true || type.SeeTeachers == true)
                    {
                         examinations = db.Examinations.Where(x=>x.Student.Centerid == emp.Centerid).Include(e => e.Stage).Include(e => e.Student).Include(e => e.Study_subject).Include(e => e.ExamType).ToList();

                    }
                    else if (type.SeeAccToCity==true)
                    {
                         examinations = db.Examinations.Where(x => x.Student.Center.Cityid == emp.CityID).Include(e => e.Stage).Include(e => e.Student).Include(e => e.Study_subject).Include(e => e.ExamType).ToList();

                    }
                    else
                    {
                         examinations = db.Examinations.Include(e => e.Stage).Include(e => e.Student).Include(e => e.Study_subject).Include(e => e.ExamType).ToList();

                    }
                    if (TempData["Message"] != null)
                    {
                        ViewBag.StateMessage = TempData["Message"];
                    }
                    return View(examinations);
                }
                return RedirectToAction("Default", "Home");
            }
            return RedirectToAction("Index", "Home");
        }

        // GET: Examinations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Default", "Home");
            }
            if (Session["ID"] != null)
            {
                var typeName = (string)Session["Type"]; var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
                if (type.SeeAll == true || type.SeeAccToCity == true || type.SeeAccToCenter == true || type.SeeAllButFinance == true || type.SeeTeachers == true)
                {
                    Examination examination = db.Examinations.Find(id);
                    if (examination == null)
                    {
                        return HttpNotFound();
                    }
                    return View(examination);
                }
                return RedirectToAction("Default", "Home");
            }
            return RedirectToAction("Index", "Home");
        }

        // GET: Examinations/Create
        public ActionResult Create()
        {
            if (Session["ID"] != null)
            {
                var typeName = (string)Session["Type"]; var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
                if (type.AddExam == true)
                {
                    ViewBag.Stageid = new SelectList(db.Stages, "id", "StageName");
                    ViewBag.Studentid = new SelectList(db.Students, "id", "Name");
                    ViewBag.Subjectid = new SelectList(db.Study_subject, "id", "Name");
                    ViewBag.ExamTypeid = new SelectList(db.ExamTypes, "id", "Type");

                    return View();
                }
                return RedirectToAction("Default", "Home");
            }
            return RedirectToAction("Index", "Home");


        }

        // POST: Examinations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult Create(FormCollection formCollection)
        {

            var name = formCollection["Exam"];
            Examination examination = new JavaScriptSerializer().Deserialize<Examination>(name);
            try
            {
                examination.id = db.Examinations.OrderByDescending(x => x.id).FirstOrDefault().id + 1;
            }
            catch (Exception)
            {
                examination.id = 1;
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
            examination.AddedBy = Convert.ToInt32(Session["ID"]);
            examination.AddingDate = DateTime.Now.Date;
            examination.AddingTime = DateTime.Now.TimeOfDay;

            try
            {

                if (Request.Files.Count > 0)
                {
                    var oldImages = db.Prooves.Where(x => x.ExaminationID == examination.id).ToList();

                    foreach (var image in oldImages)
                    {

                        db.Prooves.Remove(image);
                    }

                    if ((Directory.Exists(Server.MapPath("~/App_Data/Examinations") + "\\" + examination.id)))
                    {
                        try
                        {
                            Directory.Delete(Server.MapPath("~/App_Data/Examinations") + "\\" + examination.id, true);
                        }
                        catch (IOException)
                        {
                            Directory.Delete(Server.MapPath("~/App_Data/Examinations") + "\\" + examination.id, true);
                        }
                        catch (UnauthorizedAccessException)
                        {
                            Directory.Delete(Server.MapPath("~/App_Data/Examinations") + "\\" + examination.id, true);
                        }
                    }

                    if (!Directory.Exists(Server.MapPath("~/App_Data/Examinations") + "\\" + examination.id))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/App_Data/Examinations") + "\\" + examination.id);
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
                        if (!Directory.Exists(Server.MapPath("~/App_Data/Examinations/" + examination.id)))
                        {
                            Directory.CreateDirectory(Server.MapPath("~/App_Data/Examinations/" + examination.id));
                        }
                        var inputStream = fileContent.InputStream;
                        var fileName = fileContent.FileName;
                        var path = Path.Combine(Server.MapPath("~/App_Data/Examinations/" + examination.id), fileName);
                        using (var fileStream = System.IO.File.Create(path))
                        {
                            inputStream.CopyTo(fileStream);

                            Proove proove = new Proove();
                            proove.Path = path;
                            proove.id = prooveid;
                            proove.ExaminationID = examination.id;
                            db.Prooves.Add(proove);
                            prooveid++;
                        }
                    }
                    else
                    {
                        return Json(false);
                    }


                }
                try
                {
                    var startPath = Server.MapPath("~/App_Data/Examinations" + "\\" + examination.id);

                    if (!Directory.Exists(Server.MapPath("~/App_Data/Examinations" + "\\" + "ZipFolder")))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/App_Data/Examinations" + "\\" + "ZipFolder"));
                    }
                    var zipPath = Server.MapPath("~/App_Data/Examinations" + "\\" + "ZipFolder") + "\\" + examination.id + ".zip";

                    if (System.IO.File.Exists(zipPath))
                    {
                        System.IO.File.Delete(zipPath);
                    }
                    try
                    {
                        ZipFile.CreateFromDirectory(startPath, zipPath);
                    }
                    catch (Exception wx) { }
                    examination.Proof = zipPath;
                }
                catch { }
                ViewBag.Message = "Upload successful";
            }
            catch
            {
                ViewBag.Message = "Upload failed";
                return Json(false);
            }

            if (ModelState.IsValid)
            {
                db.Examinations.Add(examination);
                db.SaveChanges();
                return Json(true);
            }

            //ViewBag.Stageid = new SelectList(db.Stages, "id", "StageName", examination.Stageid);
            //ViewBag.Studentid = new SelectList(db.Students, "id", "Name", examination.Studentid);
            //ViewBag.Subjectid = new SelectList(db.Study_subject, "id", "Name", examination.Subjectid);
            //ViewBag.ExamTypeid = new SelectList(db.ExamTypes, "id", "Type", examination.ExamTypeid);
            return Json(false);
        }

        // GET: Examinations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Default", "Home");
            }
            if (Session["ID"] != null)
            {
                var typeName = (string)Session["Type"]; var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
                if (type.AddExam == true)
                {

                    Examination examination = db.Examinations.Find(id);
                    if (examination == null)
                    {
                        return HttpNotFound();
                    }
                    ViewBag.Stageid = new SelectList(db.Stages, "id", "StageName", examination.Stageid);
                    ViewBag.Studentid = new SelectList(db.Students, "id", "Name", examination.Studentid);
                    ViewBag.Subjectid = new SelectList(db.Study_subject, "id", "Name", examination.Subjectid);
                    ViewBag.ExamTypeid = new SelectList(db.ExamTypes, "id", "Type", examination.ExamTypeid);
                    return View(examination);
                }
                return RedirectToAction("Default", "Home");
            }
            return RedirectToAction("Index", "Home");

        }

        // POST: Examinations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(/*[Bind(Include = "id,Desc,Subjectid,Studentid,Stageid,ExamTypeid,Mark,Date")]*/ Examination examination, IEnumerable<HttpPostedFileBase> file)
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

                if (file.Count() > 1 || file.Count() > 0 && file.FirstOrDefault() != null)
                {
                    var oldImages = db.Prooves.Where(x => x.ExaminationID == examination.id).ToList();

                    foreach (var image in oldImages)
                    {

                        db.Prooves.Remove(image);
                    }

                    if ((Directory.Exists(Server.MapPath("~/App_Data/Examinations") + "\\" + examination.id)))
                    {
                        try
                        {
                            Directory.Delete(Server.MapPath("~/App_Data/Examinations") + "\\" + examination.id, true);
                        }
                        catch (IOException)
                        {
                            Directory.Delete(Server.MapPath("~/App_Data/Examinations") + "\\" + examination.id, true);
                        }
                        catch (UnauthorizedAccessException)
                        {
                            Directory.Delete(Server.MapPath("~/App_Data/Examinations") + "\\" + examination.id, true);
                        }
                    }

                    if (!Directory.Exists(Server.MapPath("~/App_Data/Examinations") + "\\" + examination.id))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/App_Data/Examinations") + "\\" + examination.id);
                    }

               


                foreach (var item in file)
                {
                    if (item.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(item.FileName);
                        if (!Directory.Exists(Server.MapPath("~/App_Data/Examinations") + "\\" + examination.id))
                        {
                            Directory.CreateDirectory(Server.MapPath("~/App_Data/Examinations") + "\\" + examination.id);
                        }
                        var path = Path.Combine(Server.MapPath("~/App_Data/Examinations/" + examination.id), fileName);
                        item.SaveAs(path);
                        Proove proove = new Proove();
                        proove.Path = path;
                        proove.id = prooveid;
                        proove.ExaminationID = examination.id;
                        db.Prooves.Add(proove);
                        prooveid++;

                    }
                    else
                    {

                    }
                }
                try
                {
                    var startPath = Server.MapPath("~/App_Data/Examinations" + "\\" + examination.id);

                    if (!Directory.Exists(Server.MapPath("~/App_Data/Examinations" + "\\" + "ZipFolder")))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/App_Data/Examinations" + "\\" + "ZipFolder"));
                    }
                    var zipPath = Server.MapPath("~/App_Data/Examinations" + "\\" + "ZipFolder") + "\\" + examination.id + ".zip";

                    if (System.IO.File.Exists(zipPath))
                    {
                        System.IO.File.Delete(zipPath);
                    }
                    try
                    {
                        ZipFile.CreateFromDirectory(startPath, zipPath);
                    }
                    catch (Exception wx) { }
                    examination.Proof = zipPath;
                }
                catch { }

                }
            }
            catch (Exception ex)
            {
                ViewBag.error = "يرجى ارفاق الاثبات كملف خارجي";
                proceed = false;
            }
            if (proceed)
            {
                if (ModelState.IsValid)
                {
                    db.Entry(examination).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["Message"] = "تم التعديل بنجاح";
                    return RedirectToAction("Index");
                }
            }

            ViewBag.Stageid = new SelectList(db.Stages, "id", "StageName", examination.Stageid);
            ViewBag.Studentid = new SelectList(db.Students, "id", "Name", examination.Studentid);
            ViewBag.Subjectid = new SelectList(db.Study_subject, "id", "Name", examination.Subjectid);
            ViewBag.ExamTypeid = new SelectList(db.ExamTypes, "id", "Type", examination.ExamTypeid);
            return View(examination);
        }

        // GET: Examinations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Default", "Home");
            }
            if (Session["ID"] != null)
            {
                var typeName = (string)Session["Type"]; var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
                if (type.AddExam == true)
                {
                    Examination examination = db.Examinations.Find(id);
                    if (examination == null)
                    {
                        return HttpNotFound();
                    }
                    return View(examination);
                }
                return RedirectToAction("Default", "Home");
            }
            return RedirectToAction("Index", "Home");


        }

        // POST: Examinations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["ID"] != null)
            {
                var typeName = (string)Session["Type"]; var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
                if (type.AddExam == true)
                {
                    Examination examination = db.Examinations.Find(id);
                    var path = Server.MapPath("~\\App_Data\\Examinations\\");
                    var prooves = db.Prooves.Where(x => x.ExaminationID == examination.id);
                    foreach (var item in prooves)
                    {
                        db.Prooves.Remove(item);
                    }

                    db.Examinations.Remove(examination);

                    try
                    {
                        db.SaveChanges();
                        if (Directory.Exists(path + "\\" + examination.id))
                        {


                            try
                            {
                                Directory.Delete(path + "\\" + examination.id, true);
                            }
                            catch (IOException)
                            {
                                Directory.Delete(path + "\\" + examination.id, true);
                            }
                            catch (UnauthorizedAccessException)
                            {
                                Directory.Delete(path + "\\" + examination.id, true);
                            }
                        }
                        if (System.IO.File.Exists(path + "ZipFolder\\" + examination.id + ".zip"))
                        {
                            System.IO.File.Delete(path + "ZipFolder\\" + examination.id + ".zip");
                        }

                    }
                    catch
                    {
                        ViewBag.error = "يوجد مدخلات اخرى متعلقة بهذاالامتحان يرجى تغييرها قبل الحذف";
                        return View(examination);
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
