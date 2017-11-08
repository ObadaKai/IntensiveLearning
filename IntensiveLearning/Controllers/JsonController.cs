using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using IntensiveLearning.Database;
using IntensiveLearning.Models;

namespace IntensiveLearning.Controllers
{
    public class JsonController : Controller
    {
        private TaalimEntities db = new TaalimEntities();
        // GET: Json
        public JsonResult FetchStudentsOfDate()
        {
            var id = Convert.ToInt16(Session["ID"]);
            var emp = db.Employees.Find(id);
            var AlreadyDone = db.Examinations.Select(x => new { x.Studentid, x.Date }).ToList();
            var Students = db.Students.Where(c=>c.Centerid == emp.Centerid).Select(x => new { x.id, x.Name, x.Surname,x.FathersName }).ToList();
            foreach (var item in AlreadyDone)
            {
                if (item.Date == DateTime.Now.Date)
                {
                    var c = Students.Where(x => x.id == item.Studentid).FirstOrDefault();
                    Students.Remove(c);
                }
            }
            return Json(Students, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult FetchStudentsOfDate(Date date)
        {
            var id = Convert.ToInt16(Session["ID"]);
            var emp = db.Employees.Find(id);
            var AlreadyDone = db.Examinations.Select(x => new { x.Studentid, x.Date, x.Subjectid }).ToList();
            var Students = db.Students.Where(c => c.Centerid == emp.Centerid).Select(x => new { x.id, x.Name, x.Surname,x.FathersName }).ToList();
            foreach (var item in AlreadyDone)
            {
                if (item.Date == date.Fielpate.Date && item.Subjectid == date.Subjectid)
                {
                    var c = Students.Where(x => x.id == item.Studentid).FirstOrDefault();
                    Students.Remove(c);
                }
            }
            return Json(Students, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult SearchExams(ExamTextBoxes textBox)
        {

            string searchbox = textBox.SearchBoxData;
            List<Examination> examNum = new List<Examination>();
            List<Examination> examDate = new List<Examination>();
            List<Examination> examDateTextbox = new List<Examination>();
            List<Examination> MoreThanNumber = new List<Examination>();


            try
            {
                MoreThanNumber = db.Examinations.Where(x => x.Mark >= textBox.SearchBoxNumber).ToList();
            }
            catch (Exception)
            {

                throw;
            }


            try
            {
                var num = Convert.ToInt32(searchbox);
                examNum = db.Examinations.Where(x => x.Mark == num).ToList();
            }
            catch (Exception)
            {

            }

            try
            {
                examDate = db.Examinations.Where(x => x.Date == textBox.SearchBoxDate).ToList();
            }
            catch (Exception)
            {

            }
            try
            {
                var textb = DateTime.Parse(textBox.SearchBoxData);
                examDateTextbox = db.Examinations.Where(x => x.Date == textb).ToList();
            }
            catch (Exception)
            {

            }
            var examstrings = db.Examinations.Where(x => x.Desc.Contains(searchbox) || x.ExamType.Type.Contains(searchbox) || x.Stage.StageName.Contains(searchbox) || x.Student.Name.Contains(searchbox)
            || x.Student.Surname.Contains(searchbox) || x.Study_subject.Name.Contains(searchbox)).ToList();
            examstrings.AddRange(examNum);
            examstrings.AddRange(examDateTextbox);

            if (textBox.SearchBoxData == null)
            {
                if (MoreThanNumber != null)
                {
                    examstrings.AddRange(MoreThanNumber);
                }
                else
                {
                    examstrings.AddRange(examDate);
                }

            }
            else
            {
                if (MoreThanNumber.Count > 0)
                {
                    examstrings = examstrings.Intersect(MoreThanNumber).ToList();
                }
                if (examDate.Count > 0)
                {
                    examstrings = examstrings.Intersect(examDate).ToList();
                }

            }
            examstrings.RemoveAll(item => item == null);
            examstrings.Distinct();
            foreach (var item in examstrings)
            {
                if (item.Student.Center == null)
                {
                    item.Student.Center = new Center();
                }
            }
            var ToSendList = examstrings.Select(c => new
            {
                ID = c.id,
                Mark = c.Mark,
                StageName = c.Stage.StageName,
                StudentName = c.Student.Name,
                StudentSurname = c.Student.Surname,
                Subject = c.Study_subject.Name,
                Date = c.Date,
                Desc = c.Desc,
                ExamType = c.ExamType.Type,
                Proof = c.Proof,
                Center= c.Student.Center.Name,
            }).ToList();
            //.Select(c => new { c.Mark, c.Stage.StageName, c.Student.Name, c.Student.Surname, c.Study_subject.Name, c.Date, c.Desc, c.ExamType.Type })
            return Json(ToSendList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SearchEmployees(TextBox textBox)
        {
            string searchbox = textBox.SearchBoxData;
            List<Employee> EmployeeBDate = new List<Employee>();
            List<Employee> EmployeeSDate = new List<Employee>();
            List<Employee> EmployeeEDate = new List<Employee>();
            List<Employee> StringDate = new List<Employee>();
            List<Employee> Salaries = new List<Employee>();
            try
            {
                var salary = Convert.ToDouble(textBox.SearchBoxData);
                Salaries = db.Employees.Where(x => x.Salary == salary).ToList();
            }
            catch (Exception)
            {


            }
            try
            {
                var stringdate = DateTime.Parse(textBox.SearchBoxData);
                StringDate = db.Employees.Where(x => x.BDate == stringdate || x.EDate == stringdate || x.SDate == stringdate).ToList();
            }
            catch (Exception)
            {

            }
            try
            {
                EmployeeBDate = db.Employees.Where(x => x.BDate == textBox.SearchBoxDate).ToList();
            }
            catch (Exception)
            {

            }
            try
            {
                EmployeeSDate = db.Employees.Where(x => x.SDate == textBox.SearchBoxDate).ToList();
            }
            catch (Exception)
            {

            }
            try
            {
                EmployeeEDate = db.Employees.Where(x => x.EDate == textBox.SearchBoxDate).ToList();
            }
            catch (Exception)
            {

            }
            var EmployeeStrings = db.Employees.Where(x => x.name.Contains(searchbox) || x.surname.Contains(searchbox) || x.Center.Name.Contains(searchbox) || x.City.Name.Contains(searchbox) || x.Center.City.Name.Contains(searchbox) || x.Certificate.Contains(searchbox) || x.CType.Contains(searchbox)
            || x.surname.Contains(searchbox) || x.State.Contains(searchbox) || x.EmployeeType.Type.Contains(searchbox) || x.Period.Name.Contains(searchbox)).ToList();

            EmployeeStrings.AddRange(Salaries);
            EmployeeStrings.AddRange(StringDate);

            if (textBox.SearchBoxData == null)
            {
                if (EmployeeBDate != null)
                {
                    EmployeeStrings.AddRange(EmployeeBDate);
                }
                else if (EmployeeSDate != null)
                {
                    EmployeeStrings.AddRange(EmployeeSDate);

                }

                else
                {
                    EmployeeStrings.AddRange(EmployeeEDate);

                }
            }
            else
            {
                if (EmployeeBDate.Count > 0)
                {
                    EmployeeStrings = EmployeeStrings.Intersect(EmployeeBDate).ToList();
                }
                if (EmployeeSDate.Count > 0)
                {
                    EmployeeStrings = EmployeeStrings.Intersect(EmployeeSDate).ToList();
                }
                if (EmployeeEDate.Count > 0)
                {
                    EmployeeStrings = EmployeeStrings.Intersect(EmployeeEDate).ToList();
                }


            }
            EmployeeStrings.RemoveAll(item => item == null);
            EmployeeStrings.Distinct();

            foreach (var Emp in EmployeeStrings)
            {
                if (Emp.Center == null)
                {
                    Emp.Center = new Center();
                }
                if (Emp.Period == null)
                {
                    Emp.Period = new Period();
                }
                if (Emp.EmployeeType == null)
                {
                    Emp.EmployeeType = new EmployeeType();
                }
                if (Emp.City == null)
                {
                    if (Emp.Center.Cityid != null)
                    {
                        Emp.City = Emp.Center.City;

                    }
                    else
                    {
                        Emp.City = new City();
                    }
                }
            }

            var ToSendList = EmployeeStrings.Select(c => new
            {
                ID = c.id,
                name = c.name,
                Center = c.Center.Name,
                Certificate = c.Certificate,
                CType = c.CType,
                surname = c.surname,
                State = c.State,
                EmployeeType = c.EmployeeType.Type,
                Period = c.Period.Name,
                EmployeeEDate = c.EDate,
                EmployeeSDate = c.SDate,
                EmployeeBDate = c.BDate,
                EmpManager = c.EmployeeType.Manager,
                EmpCoManager = c.EmployeeType.CoManager,
                EmpSchoolManager = c.EmployeeType.SchoolManager,
                EmpNormal = c.EmployeeType.NormalEmployee,
                EmpSeeTeacher = c.EmployeeType.SeeTeachers,
                Salary = c.Salary,
                Proof = c.Proof,
                Sex = c.Sex,
                SeeAccToCity = c.EmployeeType.SeeAccToCity,
                SeeAll = c.EmployeeType.SeeAll,
                City = c.City.Name
            }).ToList();
            var typeName = (string)Session["Type"];
            var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
            var empid = Convert.ToInt32(Session["ID"]);
            var emp = db.Employees.Find(empid);
            try
            {
                if (type.AddNewEmployeeType == true)
                {

                    ToSendList = ToSendList.Where(x => x.EmpManager == true).ToList();
                }
                else if (type.AddManagers == true)
                {

                }
                else if (type.AddCOManagers == true)
                {


                    ToSendList = ToSendList.Where(x => (x.EmpCoManager == true || x.EmpSchoolManager == true || x.EmpNormal == true) || (x.EmpManager != true && x.EmpNormal != true && x.EmpCoManager != true && x.EmpSchoolManager != true)).ToList();
                }
                else if (type.AddSchoolManagers == true)
                {
                    if (type.SeeAccToCity == true)
                    {

                        ToSendList = ToSendList.Where(x => ((x.City == emp.City.Name || x.City == emp.Center.City.Name) && (x.EmpSchoolManager == true || x.EmpNormal == true)) || (x.EmpManager != true && x.EmpNormal != true && x.EmpCoManager != true && x.EmpSchoolManager != true && x.City == emp.City.Name)).ToList();
                    }
                    else if (type.SeeAccToCenter == true)
                    {
                        ToSendList = ToSendList.Where(x => (x.Center == emp.Center.Name && (x.EmpSchoolManager == true || x.EmpNormal == true)) || (x.EmpManager != true && x.EmpNormal != true && x.EmpCoManager != true && x.EmpSchoolManager != true && x.Center == emp.Center.Name)).ToList();
                    }
                    else
                    {
                        ToSendList = ToSendList.Where(x => x.EmpSchoolManager == true || x.EmpNormal == true || x.City != null || (x.EmpManager != true && x.EmpNormal != true && x.EmpCoManager != true && x.EmpSchoolManager != true)).ToList();
                    }
                }
                else if (type.AddSchoolEmployees == true)
                {
                    if (type.SeeAccToCity == true)
                    {

                        ToSendList = ToSendList.Where(x => ((x.City == emp.City.Name || x.City == emp.Center.City.Name) && x.EmpNormal == true) || (x.EmpManager != true && x.EmpNormal != true && x.EmpCoManager != true && x.EmpSchoolManager != true && x.City == emp.City.Name)).ToList();
                    }
                    else if (type.SeeAccToCenter == true)
                    {
                        ToSendList = ToSendList.Where(x => ((x.Center == emp.Center.Name) && x.EmpNormal == true) || (x.EmpManager != true && x.EmpNormal != true && x.EmpCoManager != true && x.EmpSchoolManager != true && x.Center == emp.Center.Name)).ToList();
                    }
                    else
                    {
                        ToSendList = ToSendList.Where(x => x.EmpNormal == true || (x.EmpManager != true && x.EmpNormal != true && x.EmpCoManager != true && x.EmpSchoolManager != true)).ToList();
                    }
                }


                else if (type.SeeTeachers == true)
                {
                    ToSendList = ToSendList.Where(x => (x.EmpSeeTeacher == true && x.Center == emp.Center.Name) || (x.EmpManager != true && x.EmpNormal != true && x.EmpCoManager != true && x.EmpSchoolManager != true)).ToList();
                }
            }
            catch
            {

            }
            //.Select(c => new { c.Mark, c.Stage.StageName, c.Student.Name, c.Student.Surname, c.Study_subject.Name, c.Date, c.Desc, c.ExamType.Type })
            return Json(ToSendList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SearchPresence(TextBox textBox)
        {
            string searchbox = textBox.SearchBoxData;
            List<Presence> examDate = new List<Presence>();
            List<Presence> SearchboxExamDate = new List<Presence>();

            try
            {
                examDate = db.Presences.Where(x => x.Date == textBox.SearchBoxDate).ToList();
            }
            catch (Exception)
            {

            }
            try
            {
                var date = DateTime.Parse(textBox.SearchBoxData);
                SearchboxExamDate = db.Presences.Where(x => x.Date == date).ToList();
            }
            catch (Exception)
            {

            }
            var Presences = db.Presences.Where(x => x.Student.Name.Contains(searchbox) || x.Desc.Contains(searchbox)).ToList();

            Presences.AddRange(SearchboxExamDate);


            if (textBox.SearchBoxData == null)
            {
                Presences.AddRange(examDate);
            }
            else
            {
                if (examDate.Count > 0)
                {
                    Presences = Presences.Intersect(examDate).ToList();
                }
            }
            Presences.RemoveAll(item => item == null);
            Presences.Distinct();

            var ToSendList = Presences.Select(c => new
            {
                ID = c.id,
                Date = c.Date,
                Desc = c.Desc,
                Lesson1 = c.Lesson1,
                Lesson2 = c.Lesson2,
                Lesson3 = c.Lesson3,
                Lesson4 = c.Lesson4,
                Lesson5 = c.Lesson5,
                Lesson6 = c.Lesson6,
                Lesson7 = c.Lesson7,
                StudentSurname = c.Student.Surname,
                StudentName = c.Student.Name
            }).ToList();
            //.Select(c => new { c.Mark, c.Stage.StageName, c.Student.Name, c.Student.Surname, c.Study_subject.Name, c.Date, c.Desc, c.ExamType.Type })
            return Json(ToSendList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SearchStudents(TextBox textBox)
        {
            string searchbox = textBox.SearchBoxData;
            List<Student> examNum = new List<Student>();
            List<Student> BDate = new List<Student>();
            List<Student> SDate = new List<Student>();
            List<Student> EDate = new List<Student>();
            List<Student> DateTextBox = new List<Student>();
            try
            {
                var num = Convert.ToInt32(searchbox);
                examNum = db.Students.Where(x => x.Mark == num).ToList();
            }
            catch (Exception)
            {

            }

            try
            {
                var stringDate = DateTime.Parse(textBox.SearchBoxData);
                DateTextBox = db.Students.Where(x => x.BDate == stringDate).ToList();
            }
            catch (Exception)
            {

            }
            try
            {
                BDate = db.Students.Where(x => x.BDate == textBox.SearchBoxDate).ToList();
            }
            catch (Exception)
            {

            }
            try
            {
                SDate = db.Students.Where(x => x.SDate == textBox.SearchBoxDate).ToList();
            }
            catch (Exception)
            {

            }
            try
            {
                EDate = db.Students.Where(x => x.EDate == textBox.SearchBoxDate).ToList();
            }
            catch (Exception)
            {

            }

            var Studentstrings = db.Students.Where(x => x.Center.Name.Contains(searchbox) || x.Name.Contains(searchbox) || x.Surname.Contains(searchbox) || x.Regiment.Name.Contains(searchbox) || x.Stage.StageName.Contains(searchbox)
            || x.Certificate.Contains(searchbox) || x.State.Contains(searchbox) || x.Sex.Contains(searchbox)).ToList();

            Studentstrings.AddRange(DateTextBox);
            Studentstrings.AddRange(examNum);
            Studentstrings.AddRange(BDate);
            Studentstrings.AddRange(SDate);
            Studentstrings.AddRange(EDate);
            Studentstrings.RemoveAll(item => item == null);
            Studentstrings.Distinct();

            foreach (var student in Studentstrings)
            {
                if (student.Center == null)
                {
                    student.Center = new Center();
                }
                if (student.Center.City == null)
                {
                    student.Center.City = new City();
                }
            }

            var ToSendList = Studentstrings.Select(c => new
            {
                ID = c.id,
                Mark = c.Mark,
                StageName = c.Stage.StageName,
                Name = c.Name,
                Surname = c.Surname,
                Regiment = c.Regiment.Name,
                Center = c.Center.Name,
                Certificate = c.Certificate,
                BDate = c.BDate,
                EDate = c.EDate,
                SDate = c.SDate,
                Period = c.Regiment.Period.Name,
                Sex = c.Sex,
                Proof = c.Proof,
                City = c.Center.City.Name
            }).ToList();
            var typeName = (string)Session["Type"];
            var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
            var empid = Convert.ToInt32(Session["ID"]);
            var emp = db.Employees.Find(empid);
            try
            {
                if (type.SeeAccToCity != null)
                {
                    if (type.SeeAccToCity == true)
                    {
                        ToSendList = ToSendList.Where(x => x.City == emp.City.Name).ToList();
                    }
                }
                else if (type.SeeAccToCenter != null || type.SeeTeachers != null)
                {
                    if (type.SeeAccToCenter == true || type.SeeTeachers == true)
                    {
                        ToSendList = ToSendList.Where(x => x.Center == emp.Center.Name).ToList();
                    }
                }
            }
            catch
            {

            }
            //.Select(c => new { c.Mark, c.Stage.StageName, c.Student.Name, c.Student.Surname, c.Study_subject.Name, c.Date, c.Desc, c.ExamType.Type })
            return Json(ToSendList, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public JsonResult StudentUpload(FormCollection formCollection)
        //{
        //    try
        //    {
        //        var name = formCollection["student"];
        //        Student student = new JavaScriptSerializer().Deserialize<Student>(name);
        //    }
        //    catch
        //    {

        //    }
        //    try
        //    {
        //        foreach (string file in Request.Files)
        //        {
        //            var fileContent = Request.Files[file];
        //            if (fileContent != null && fileContent.ContentLength > 0)
        //            {
        //                var inputStream = fileContent.InputStream;
        //                var fileName = fileContent.FileName;
        //                var path = Path.Combine(Server.MapPath("~/App_Data/Students/"), fileName);
        //                using (var fileStream = System.IO.File.Create(path))
        //                {
        //                    inputStream.CopyTo(fileStream);
        //                }
        //            }

        //        }

        //        ViewBag.Message = "Upload successful";
        //        return Json("");
        //    }
        //    catch (Exception x)
        //    {
        //        ViewBag.Message = "Upload failed";
        //        return null;
        //    }
        //}


        public FileResult DownloadProof(string ImageName)
        {
            var exe = Path.GetExtension(ImageName);
            var c = Path.GetDirectoryName(ImageName);
            string lastFolderName = Path.GetFileName(Path.GetDirectoryName(ImageName));
            return File(ImageName, System.Net.Mime.MediaTypeNames.Application.Octet, DateTime.Now.Month + "/" + DateTime.Now.Day + "__" + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "__" + lastFolderName + exe);
        }

    }
}