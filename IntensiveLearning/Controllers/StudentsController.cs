using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using IntensiveLearning.Database;
using IntensiveLearning.Models;
using System.IO.Compression;

namespace IntensiveLearning.Controllers
{
    public class StudentsController : Controller
    {
        private TaalimEntities db = new TaalimEntities();

        // GET: Students
        public ActionResult Index()
        {
            if (Session["ID"] != null)
            {
                var typeName = (string)Session["Type"];
                var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
                if (TempData["Message"] != null)
                {
                    ViewBag.StateMessage = TempData["Message"];
                }
                if (type.SeeAll == true || type.SeeAllButFinance == true)
                {

                    var students = db.Students.Include(s => s.Center).Include(s => s.Regiment).Include(s => s.Stage);
                    return View(students.ToList());
                }
                if (type.SeeAccToCity == true)
                {
                    var empid = Convert.ToInt32(Session["ID"]);
                    var emp = db.Employees.Find(empid);
                    var students = db.Students.Where(x => x.Center.Cityid == emp.CityID).Include(s => s.Center).Include(s => s.Regiment).Include(s => s.Stage);
                    return View(students.ToList());
                }
                if (type.SeeAccToCenter == true)
                {
                    var empid = Convert.ToInt32(Session["ID"]);
                    var emp = db.Employees.Find(empid);
                    var students = db.Students.Where(x => x.Centerid == emp.Centerid).Include(s => s.Center).Include(s => s.Regiment).Include(s => s.Stage); return View(students.ToList());
                }
                if (type.SeeTeachers == true)
                {
                    var empid = Convert.ToInt32(Session["ID"]);
                    var emp = db.Employees.Find(empid);
                    var students = db.Students.Where(x => x.Centerid == emp.Centerid).Include(s => s.Center).Include(s => s.Regiment).Include(s => s.Stage); return View(students.ToList());
                }
                return RedirectToAction("Default", "Home");
            }
            return RedirectToAction("Index", "Home");


        }

        // GET: Students/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Default", "Home");
            }
            if (Session["ID"] != null)
            {
                var typeName = (string)Session["Type"]; var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
                if (type.SeeAll == true || type.SeeAllButFinance == true)
                {
                    Student student = db.Students.Find(id);
                    if (student == null)
                    {
                        return HttpNotFound();
                    }
                    return View(student);
                }

                if (type.SeeAccToCity == true)
                {
                    var empid = Convert.ToInt32(Session["ID"]);
                    var emp = db.Employees.Find(empid);
                    Student student = db.Students.Find(id);
                    if (student == null)
                    {
                        return HttpNotFound();
                    }
                    if (student.Center.Cityid == emp.CityID)
                    {
                        return View(student);
                    }

                }
                if (type.SeeAccToCenter == true)
                {
                    var empid = Convert.ToInt32(Session["ID"]);
                    var emp = db.Employees.Find(empid);
                    Student student = db.Students.Find(id);
                    if (student == null)
                    {
                        return HttpNotFound();
                    }
                    if (student.Centerid == emp.Centerid)
                    {
                        return View(student);
                    }
                }
                if (type.SeeTeachers == true)
                {
                    var empid = Convert.ToInt32(Session["ID"]);
                    var emp = db.Employees.Find(empid);
                    Student student = db.Students.Find(id);
                    if (student == null)
                    {
                        return HttpNotFound();
                    }

                    if (student.Centerid == emp.Centerid)
                    {
                        return View(student);
                    }
                }
                return RedirectToAction("Default", "Home");
            }
            return RedirectToAction("Index", "Home");


        }

        // GET: Students/Create
        public ActionResult Create()
        {
            if (Session["ID"] != null)
            {
                var typeName = (string)Session["Type"]; var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
                if (type.AddStudent == true)
                {
                    var Sid = Convert.ToInt32(Session["ID"]);
                    var emp = db.Employees.Where(x => x.id == Sid).FirstOrDefault().Centerid;
                    ViewBag.Centerid = new SelectList(db.Centers.Where(x => x.id == emp), "id", "Name");
                    ViewBag.Regimentid = new SelectList(db.Regiments, "id", "Name");
                    ViewBag.Stageid = new SelectList(db.Stages, "id", "StageName");
                    ViewBag.Certificate = new SelectList(db.Stages, "StageName", "StageName");

                    return View();



                }
                return RedirectToAction("Default", "Home");
            }
            return RedirectToAction("Index", "Home");


        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult Create(/*[Bind(Include = "BDate,Name,Surname,Certificate,Mark,State,Centerid,Stageid,Regimentid,SDate,EDate")]*/ FormCollection formCollection)
        {
            var name = formCollection["student"];
            Student student = new JavaScriptSerializer().Deserialize<Student>(name);

            var files = Request.Files;
            try
            {
                student.StudentNumber = db.Students.OrderByDescending(x => x.StudentNumber).FirstOrDefault().StudentNumber + 1;

            }
            catch
            {
                var yearS = DateTime.Now;
                string lastTwoDigitsOfYear = yearS.ToString("yy");
                var ExactYear = Convert.ToInt16(lastTwoDigitsOfYear);
                var studentNumberString = ExactYear.ToString() + "00001";
                var studentNumber = Convert.ToInt32(studentNumberString);
                student.StudentNumber = studentNumber;
            }




            try
            {
                student.id = db.Students.OrderByDescending(x => x.id).FirstOrDefault().id + 1;
            }
            catch (Exception)
            {
                student.id = 1;
            }
            student.State = "متوفر";

            int prooveid;
            try
            {
                prooveid = db.Prooves.OrderByDescending(x => x.id).FirstOrDefault().id + 1;
            }
            catch (Exception)
            {
                prooveid = 1;
            }
            student.AddedBy = Convert.ToInt32(Session["ID"]);
            student.AddingDate = DateTime.Now.Date;
            student.AddingTime = DateTime.Now.TimeOfDay;

            try
            {

                if (Request.Files.Count > 0)
                {
                    var oldImages = db.Prooves.Where(x => x.StudentID == student.id).ToList();

                    foreach (var image in oldImages)
                    {

                        db.Prooves.Remove(image);
                    }

                    if ((Directory.Exists(Server.MapPath("~/App_Data/Students") + "\\" + student.id)))
                    {
                        try
                        {
                            Directory.Delete(Server.MapPath("~/App_Data/Students") + "\\" + student.id, true);
                        }
                        catch (IOException)
                        {
                            Directory.Delete(Server.MapPath("~/App_Data/Students") + "\\" + student.id, true);
                        }
                        catch (UnauthorizedAccessException)
                        {
                            Directory.Delete(Server.MapPath("~/App_Data/Students") + "\\" + student.id, true);
                        }
                    }

                    if (!Directory.Exists(Server.MapPath("~/App_Data/Students") + "\\" + student.id))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/App_Data/Students") + "\\" + student.id);
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
                        if (!Directory.Exists(Server.MapPath("~/App_Data/Students/" + student.id)))
                        {
                            Directory.CreateDirectory(Server.MapPath("~/App_Data/Students/" + student.id));
                        }
                        var inputStream = fileContent.InputStream;
                        var fileName = fileContent.FileName;
                        var path = Path.Combine(Server.MapPath("~/App_Data/Students/" + student.id), fileName);
                        using (var fileStream = System.IO.File.Create(path))
                        {
                            inputStream.CopyTo(fileStream);

                            Proove proove = new Proove();
                            proove.Path = path;
                            proove.id = prooveid;
                            proove.StudentID = student.id;
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
                    var startPath = Server.MapPath("~/App_Data/Students" + "\\" + student.id);

                    if (!Directory.Exists(Server.MapPath("~/App_Data/Students" + "\\" + "ZipFolder")))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/App_Data/Students" + "\\" + "ZipFolder"));
                    }
                    var zipPath = Server.MapPath("~/App_Data/Students" + "\\" + "ZipFolder") + "\\" + student.id + ".zip";

                    if (System.IO.File.Exists(zipPath))
                    {
                        System.IO.File.Delete(zipPath);
                    }
                    try
                    {
                        ZipFile.CreateFromDirectory(startPath, zipPath);
                    }
                    catch (Exception wx) { }
                    student.Proof = zipPath;
                }
                catch { }

                ViewBag.Message = "Upload successful";
            }
            catch
            {
                //ViewBag.Message = "Upload failed";
                //return Json(false);
            }

            if (ModelState.IsValid)
            {
                db.Students.Add(student);
                try
                {
                    db.SaveChanges();
                    return Json(true);
                }
                catch (Exception ex)
                {
                    return Json(false);
                }
            }
            else
            {
                return Json(false);
            }
        }

        // GET: Students/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Default", "Home");
            }
            if (Session["ID"] != null)
            {
                var typeName = (string)Session["Type"]; var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
                if (type.AddStudent == true)
                {

                    Student student = db.Students.Find(id);
                    if (student == null)
                    {
                        return HttpNotFound();
                    }
                    var Sid = Convert.ToInt32(Session["ID"]);
                    var emp = db.Employees.Where(x => x.id == Sid).FirstOrDefault().Centerid;
                    ViewBag.Centerid = new SelectList(db.Centers.Where(x => x.id == emp), "id", "Name");
                    ViewBag.Regimentid = new SelectList(db.Regiments, "id", "Name", student.Regimentid);
                    ViewBag.Stageid = new SelectList(db.Stages, "id", "StageName", student.Stageid);
                    return View(student);



                }
                return RedirectToAction("Default", "Home");
            }
            return RedirectToAction("Index", "Home");


        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(/*[Bind(Include = "id,BDate,Name,Surname,Certificate,Mark,State,Centerid,Stageid,Regimentid,SDate,EDate")]*/ Student student, IEnumerable<HttpPostedFileBase> file)
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

                    var oldImages = db.Prooves.Where(x => x.StudentID == student.id).ToList();

                    foreach (var image in oldImages)
                    {

                        db.Prooves.Remove(image);
                    }

                    if ((Directory.Exists(Server.MapPath("~/App_Data/Students") + "\\" + student.id)))
                    {
                        try
                        {
                            Directory.Delete(Server.MapPath("~/App_Data/Students") + "\\" + student.id, true);
                        }
                        catch (IOException)
                        {
                            Directory.Delete(Server.MapPath("~/App_Data/Students") + "\\" + student.id, true);
                        }
                        catch (UnauthorizedAccessException)
                        {
                            Directory.Delete(Server.MapPath("~/App_Data/Students") + "\\" + student.id, true);
                        }
                    }

                    if (!Directory.Exists(Server.MapPath("~/App_Data/Students") + "\\" + student.id))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/App_Data/Students") + "\\" + student.id);
                    }



                    foreach (var item in file)
                    {
                        if (item.ContentLength > 0)
                        {
                            var fileName = Path.GetFileName(item.FileName);
                            if (!Directory.Exists(Server.MapPath("~/App_Data/Students") + "\\" + student.id))
                            {
                                Directory.CreateDirectory(Server.MapPath("~/App_Data/Students") + "\\" + student.id);
                            }
                            var path = Path.Combine(Server.MapPath("~/App_Data/Students/" + student.id), fileName);
                            item.SaveAs(path);
                            Proove proove = new Proove();
                            proove.Path = path;
                            proove.id = prooveid;
                            proove.StudentID = student.id;
                            db.Prooves.Add(proove);
                            prooveid++;

                        }
                        else
                        {

                        }
                    }
                    try
                    {
                        var startPath = Server.MapPath("~/App_Data/Students" + "\\" + student.id);

                        if (!Directory.Exists(Server.MapPath("~/App_Data/Students" + "\\" + "ZipFolder")))
                        {
                            Directory.CreateDirectory(Server.MapPath("~/App_Data/Students" + "\\" + "ZipFolder"));
                        }
                        var zipPath = Server.MapPath("~/App_Data/Students" + "\\" + "ZipFolder") + "\\" + student.id + ".zip";

                        if (System.IO.File.Exists(zipPath))
                        {
                            System.IO.File.Delete(zipPath);
                        }
                        try
                        {
                            ZipFile.CreateFromDirectory(startPath, zipPath);
                        }
                        catch { }
                        student.Proof = zipPath;
                    }
                    catch { }

                }
            }
            catch { }



            if (student.EDate != null)
            {
                if (student.EDate < DateTime.Now.Date)
                {
                    student.State = "خارج الخدمة";
                }
                else
                {
                    student.State = "متوفر";
                }
            }
            if (proceed)
            {
                if (ModelState.IsValid)
                {

                    db.Entry(student).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["Message"] = "تم التعديل بنجاح";
                    return RedirectToAction("Index");
                }
            }


            //catch (DbEntityValidationException ex)
            //{
            //    foreach (DbEntityValidationResult item in ex.EntityValidationErrors)
            //    {
            //        // Get entry

            //        DbEntityEntry entry = item.Entry;
            //        string entityTypeName = entry.Entity.GetType().Name;

            //        // Display or log error messages

            //        foreach (DbValidationError subItem in item.ValidationErrors)
            //        {
            //            string message = string.Format("Error '{0}' occurred in {1} at {2}",
            //                     subItem.ErrorMessage, entityTypeName, subItem.PropertyName);
            //            Console.WriteLine(message);
            //        }
            //    }
            //}

            var Sid = Convert.ToInt32(Session["ID"]);
            var emp = db.Employees.Where(x => x.id == Sid).FirstOrDefault().Centerid;
            ViewBag.Centerid = new SelectList(db.Centers.Where(x => x.id == emp), "id", "Name");
            ViewBag.Regimentid = new SelectList(db.Regiments, "id", "Name", student.Regimentid);
            ViewBag.Stageid = new SelectList(db.Stages, "id", "StageName", student.Stageid);
            return View(student);
        }

        // GET: Students/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Default", "Home");
            }
            if (Session["ID"] != null)
            {
                var typeName = (string)Session["Type"]; var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
                if (type.AddStudent == true)
                {

                    Student student = db.Students.Find(id);
                    if (student == null)
                    {
                        return HttpNotFound();
                    }
                    return View(student);



                }
                return RedirectToAction("Default", "Home");
            }
            return RedirectToAction("Index", "Home");


        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["ID"] != null)
            {
                var typeName = (string)Session["Type"]; var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
                if (type.AddStudent == true)
                {

                    Student student = db.Students.Find(id);
                    var path = Server.MapPath("~\\App_Data\\Students\\");
                    var prooves = db.Prooves.Where(x => x.StudentID == student.id);
                    foreach (var item in prooves)
                    {
                        db.Prooves.Remove(item);
                    }

                    db.Students.Remove(student);

                    try
                    {
                        db.SaveChanges();
                        if (Directory.Exists(path + "\\" + student.id))
                        {


                            try
                            {
                                Directory.Delete(path + "\\" + student.id, true);
                            }
                            catch (IOException)
                            {
                                Directory.Delete(path + "\\" + student.id, true);
                            }
                            catch (UnauthorizedAccessException)
                            {
                                Directory.Delete(path + "\\" + student.id, true);
                            }
                        }
                        if (System.IO.File.Exists(path + "ZipFolder\\" + student.id))
                        {
                            System.IO.File.Delete(path + "ZipFolder\\" + student.id);
                        }

                    }
                    catch (Exception ex)
                    {
                        ViewBag.error = "يوجد مدخلات اخرى متعلقة بهذا الطالب يرجى تغييرها قبل الحذف";
                        return View(student);
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
