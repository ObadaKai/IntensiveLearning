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
using System.Data.Entity;

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
            var Students = db.Students.Where(c => c.Centerid == emp.Centerid).Select(x => new { x.id, x.Name, x.Surname, x.FathersName }).ToList();
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
            var Students = db.Students.Where(c => c.Centerid == emp.Centerid).Select(x => new { x.id, x.Name, x.Surname, x.FathersName }).ToList();
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

        //Mission Part Goes 5 Methods
        public ActionResult Missions()
        {
            var empid = Convert.ToInt32(Session["ID"]);
            var emp = db.Employees.Find(empid);
            var toChangeMissions = new List<Mission>();
            var toChangeResponses = new List<MissionResponse>();
            var tosendList = new List<object>();
            if (emp.EmployeeType.SeeAccToCenter == true || emp.EmployeeType.SeeAccToCity == true || emp.EmployeeType.SeeTeachers == true)
            {
                var PIC = db.MissionPersonInCharges.Where(x => x.EmployeeID == empid).ToList();

                foreach (var item in PIC)
                {
                    Mission ms = db.Missions.Where(x => x.id == item.MissionId).Where(x => x.Closed != true).FirstOrDefault();
                    if (ms != null)
                    {
                        toChangeMissions.Add(ms);
                    }

                }
                var Missions = db.Missions.Where(x => x.Manager == empid).Where(x => x.Closed != true).ToList();
                var MissionsWithEntries = db.Missions.Where(x => x.PersonEntered == empid).Where(x => x.Closed != true).ToList();
                toChangeMissions = toChangeMissions.Union(Missions).ToList();
                toChangeMissions = toChangeMissions.Union(MissionsWithEntries).ToList();
                toChangeResponses = db.MissionResponses.ToList();
            }
            else
            {
                toChangeMissions = db.Missions.Where(x => x.Closed != true).ToList();
                toChangeResponses = db.MissionResponses.ToList();
            }

            foreach (var item in toChangeMissions)
            {
                if (item.Employee == null) item.Employee = new Employee();
                if (item.Employee1 == null) item.Employee1 = new Employee();
                if (item.MissionResponses.Count == 0) item.MissionResponses = new List<MissionResponse>();
                if (item.MissionPersonInCharges.Count == 0) item.MissionPersonInCharges = new List<MissionPersonInCharge>();

            }
            if (true)
            {

            }


            var ToSendMissions = toChangeMissions.Select(c => new
            {
                ID = c.id,
                Checked = c.Checked,
                Closed = c.Closed,
                MissionText = c.MissionText,
                DateOfEntry = c.DateOfEntry,
                DateOfFinish = c.DateOfFinish,
                DateOfLastModification = c.DateOfLastModification,
                TimeOfEntry = c.TimeOfEntry,
                TimeOfFinish = c.TimeOfFinish,
                TimeOfLastModification = c.TimeOfLastModification,
                ManagerName = c.Employee.name,
                ManagerSurname = c.Employee.surname,
                ManagerID = c.Manager,
                PersonEntered = c.PersonEntered,
                NumberOfModifications = c.NumberOfModifications,
                PersonCheckedName = c.Employee1.name,
                PersonCheckedSurname = c.Employee1.surname,
                Comments = c.MissionResponses.Select(x => new
                {
                    EmployeeName = x.Employee.name,
                    EmployeeSurname = x.Employee.surname,
                    MissionID = x.MissionID,
                    Response = x.Response,
                    NestedID = x.NestedID
                }),
                PeopleInCharge = c.MissionPersonInCharges.Select(x => new
                {
                    EmployeeID = x.EmployeeID,
                    EmployeeName = x.Employee.name,
                    EmployeeSurname = x.Employee.surname
                }),
            }).ToList();

            var toSendResponses = toChangeResponses.Select(c => new
            {
                ID = c.id,
                MissionID = c.MissionID,
                Response = c.Response,
                NestedID = c.NestedID,
                type = c.type,
                EmployeeName = c.Employee.name,
                EmployeeSurname = c.Employee.surname,
            }).ToList();
            tosendList.Add(ToSendMissions);
            tosendList.Add(toSendResponses);
            tosendList.Add(empid);
            return Json(tosendList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult MissionsShowHistory()
        {
            var empid = Convert.ToInt32(Session["ID"]);
            var emp = db.Employees.Find(empid);
            var toChangeMissions = new List<Mission>();
            var toChangeResponses = new List<MissionResponse>();
            var tosendList = new List<object>();
            if (emp.EmployeeType.SeeAccToCenter == true || emp.EmployeeType.SeeAccToCity == true || emp.EmployeeType.SeeTeachers == true)
            {
                var PIC = db.MissionPersonInCharges.Where(x => x.EmployeeID == empid).ToList();

                foreach (var item in PIC)
                {
                    Mission ms = db.Missions.Where(x => x.id == item.MissionId).Where(x => x.Closed == true).FirstOrDefault();
                    if (ms != null)
                    {
                        toChangeMissions.Add(ms);
                    }

                }
                var Missions = db.Missions.Where(x => x.Manager == empid).Where(x => x.Closed == true).ToList();
                var MissionsWithEntries = db.Missions.Where(x => x.PersonEntered == empid).Where(x => x.Closed == true).ToList();
                toChangeMissions = toChangeMissions.Union(Missions).ToList();
                toChangeMissions = toChangeMissions.Union(MissionsWithEntries).ToList();
                toChangeResponses = db.MissionResponses.ToList();
            }
            else
            {
                toChangeMissions = db.Missions.Where(x => x.Closed == true).ToList();
                toChangeResponses = db.MissionResponses.ToList();
            }

            foreach (var item in toChangeMissions)
            {
                if (item.Employee == null) item.Employee = new Employee();
                if (item.Employee1 == null) item.Employee1 = new Employee();
                if (item.MissionResponses.Count == 0) item.MissionResponses = new List<MissionResponse>();
                if (item.MissionPersonInCharges.Count == 0) item.MissionPersonInCharges = new List<MissionPersonInCharge>();

            }
            if (true)
            {

            }


            var ToSendMissions = toChangeMissions.Select(c => new
            {
                ID = c.id,
                Checked = c.Checked,
                Closed = c.Closed,
                MissionText = c.MissionText,
                DateOfEntry = c.DateOfEntry,
                DateOfFinish = c.DateOfFinish,
                DateOfLastModification = c.DateOfLastModification,
                TimeOfEntry = c.TimeOfEntry,
                TimeOfFinish = c.TimeOfFinish,
                PersonEntered = c.PersonEntered,
                TimeOfLastModification = c.TimeOfLastModification,
                ManagerName = c.Employee.name,
                ManagerSurname = c.Employee.surname,
                ManagerID = c.Manager,
                NumberOfModifications = c.NumberOfModifications,
                PersonCheckedName = c.Employee1.name,
                PersonCheckedSurname = c.Employee1.surname,
                Comments = c.MissionResponses.Select(x => new
                {
                    EmployeeName = x.Employee.name,
                    EmployeeSurname = x.Employee.surname,
                    MissionID = x.MissionID,
                    Response = x.Response,
                    NestedID = x.NestedID
                }),
                PeopleInCharge = c.MissionPersonInCharges.Select(x => new
                {
                    EmployeeID = x.EmployeeID,
                    EmployeeName = x.Employee.name,
                    EmployeeSurname = x.Employee.surname
                }),
            }).ToList();

            var toSendResponses = toChangeResponses.Select(c => new
            {
                ID = c.id,
                MissionID = c.MissionID,
                Response = c.Response,
                NestedID = c.NestedID,
                type = c.type,
                EmployeeName = c.Employee.name,
                EmployeeSurname = c.Employee.surname,
            }).ToList();
            tosendList.Add(ToSendMissions);
            tosendList.Add(toSendResponses);
            tosendList.Add(empid);
            return Json(tosendList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddComment(string comment, string MissionID, string nested, string ResponseID)
        {

            if (Session["ID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            MissionResponse response = new MissionResponse();
            try
            {
                response.id = db.MissionResponses.OrderByDescending(x => x.id).FirstOrDefault().id + 1;
            }
            catch (Exception)
            {
                response.id = 1;
            }
            response.MissionID = Convert.ToInt32(MissionID);
            response.Response = comment;
            response.WriterID = Convert.ToInt32(Session["ID"]);
            if (nested == "Yes")
            {
                response.NestedID = Convert.ToInt32(ResponseID);
            }

            db.MissionResponses.Add(response);
            db.SaveChanges();


            return Redirect(Request.UrlReferrer.ToString());
        }

        public JsonResult CheckMission(string id)
        {
            var mID = Convert.ToInt32(id);
            var empid = Convert.ToInt32(Session["ID"]);
            if (db.MissionPersonInCharges.Where(x => x.MissionId == mID && x.EmployeeID == empid).Count() > 0)
            {
                Mission mission = db.Missions.Find(mID);
                mission.Checked = true;
                mission.DateOfChecking = DateTime.Now.Date;
                mission.TimeOFChecking = DateTime.Now.TimeOfDay;
                mission.PersonChecked = Convert.ToInt32(Session["ID"]);
                db.SaveChanges();
                return Json(true);
            }
            return Json(false);
        }

        public ActionResult CloseMission(int id)
        {
            var empid = Convert.ToInt32(Session["ID"]);
            Mission mission = db.Missions.Find(id);
            if (empid == mission.Manager)
            {
                mission.Closed = true;
                mission.DateOfFinish = DateTime.Now.Date;
                mission.TimeOfFinish = DateTime.Now.TimeOfDay;
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Missions");
        }
        //Mission Part finishes




        [HttpGet]
        public ActionResult Exams()
        {
            var empid = Convert.ToInt32(Session["ID"]);
            var emp = db.Employees.Find(empid);
            var Exams = db.Examinations.ToList();

            if (emp.EmployeeType.SeeAccToCenter == true || emp.EmployeeType.SeeTeachers == true)
            {
                Exams = Exams.Where(x => x.Student.Centerid == emp.Centerid).ToList();
            }
            else if (emp.EmployeeType.SeeAccToCity == true)
            {
                Exams = Exams.Where(x => x.Student.Center.Cityid == emp.CityID).ToList();
            }
            foreach (var item in Exams)
            {
                if (item.Student.Center == null)
                {
                    item.Student.Center = new Center();
                }
            }
            var ToSendList = Exams.Select(c => new
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
                Center = c.Student.Center.Name,
                Approval = c.Approval,
            }).ToList();
            return Json(ToSendList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Exams(ExamTextBoxes textBox)
        {

            if (textBox.SearchBoxData == null && textBox.SearchBoxDate == null && textBox.SearchBoxNumber == null || textBox.SearchBoxData == "" && textBox.SearchBoxDate == null && textBox.SearchBoxNumber == null)
            {
                return RedirectToAction("Employees");
            }

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
                Center = c.Student.Center.Name,
                Approval = c.Approval,
            }).ToList();
            return Json(ToSendList, JsonRequestBehavior.AllowGet);
        }






        [HttpGet]
        public ActionResult Employees()
        {
            if (Session["ID"] == null)
            {
                return null;
            }
            var empid = Convert.ToInt32(Session["ID"]);
            var emp = db.Employees.Find(empid);


            var Cities = db.Cities.Select(x => new { id = x.id, Name = x.Name }).ToList();

            var Centers = db.Centers.Select(x => new { id = x.id, Name = x.Name, Cityid = x.Cityid }).ToList();

            var EmployeeTypes = db.EmployeeTypes.Select(x => new { id = x.id, Type = x.Type, x.SeeAccToCenter, x.SeeAccToCity, x.SeeAll, x.SeeAllButFinance, x.SeeTeachers, x.Manager, x.CoManager, x.SchoolManager, x.NormalEmployee }).ToList();

            var Periods = db.Periods.Select(x => new { id = x.id, Name = x.Name }).ToList();

            List<object> allSelections = new List<object>();

            if (emp.EmployeeType.SeeAccToCenter == true || emp.EmployeeType.SeeTeachers == true)
            {
                Centers = null;
                Cities = null;

            }
            else if (emp.EmployeeType.SeeAccToCity == true)
            {
                Centers = Centers.Where(x => x.Cityid == emp.CityID).ToList();
                Cities = null;

            }

            var typeName = (string)Session["Type"];
            var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
            var employees = db.Employees.ToList();
            if (type.AddNewEmployeeType == true)
            {
                EmployeeTypes = EmployeeTypes.Where(x => x.Manager == true).ToList();
                employees = db.Employees.Where(x => x.EmployeeType.Manager == true).Include(e => e.Center).Include(e => e.EmployeeType).Include(e => e.Period).ToList();
            }
            else if (type.AddManagers == true)
            {
                employees = null;
            }
            else if (type.SeeAll == true)
            {

                employees = null;

            }
            else if (type.SeeAllButFinance == true)
            {

                EmployeeTypes = EmployeeTypes.Where(x => x.SeeTeachers == true || x.SeeAccToCenter == true || x.SeeAccToCity == true).ToList();

                employees = null;

            }
            else if (type.SeeAccToCity == true)
            {

                EmployeeTypes = EmployeeTypes.Where(x => x.SeeTeachers == true || x.SeeAccToCenter == true).ToList();

                employees = db.Employees.Where(x => ((x.Centerid == emp.Centerid) && x.EmployeeType.NormalEmployee == true) || (x.EmployeeType.Manager != true && x.EmployeeType.CoManager != true && x.EmployeeType.SchoolManager != true && x.EmployeeType.NormalEmployee != true && x.Centerid == emp.Centerid)).Include(e => e.Center).Include(e => e.EmployeeType).Include(e => e.Period).ToList();
            }
            else if (type.SeeAccToCenter == true || type.SeeTeachers == true)
            {
                EmployeeTypes = EmployeeTypes.Where(x => x.SeeTeachers == true).ToList();

                employees = db.Employees.Where(x => (x.EmployeeType.SeeTeachers == true && x.Centerid == emp.Centerid) || (x.EmployeeType.Manager != true && x.EmployeeType.CoManager != true && x.EmployeeType.SchoolManager != true && x.EmployeeType.NormalEmployee != true)).Include(e => e.Center).Include(e => e.EmployeeType).Include(e => e.Period).ToList();
            }
            if (employees != null)
            {


                foreach (var Emp in employees)
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



                var ToSendList = employees.Select(c => new
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
                    City = c.City.Name,
                    Approval = c.Approval,
                    OldJob = c.OldJob,
                    ExpYears = c.ExpYears,
                    InsideOrOutside = c.InsideOrOutside
                }).ToList();

                allSelections.Add(ToSendList);
            }
            else
            {
                allSelections.Add(employees);
            }
            allSelections.Add(Centers);
            allSelections.Add(Cities);
            allSelections.Add(Periods);
            allSelections.Add(EmployeeTypes);

            return Json(allSelections, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Employees(TextBox textBox)
        {
            if (textBox.SearchBoxData == null && textBox.SearchBoxDate == null || textBox.SearchBoxData == "" && textBox.SearchBoxDate == null)
            {
                return RedirectToAction("Employees");
            }

            string searchbox = textBox.SearchBoxData;
            List<Employee> EmployeeBDate = new List<Employee>();
            List<Employee> EmployeeSDate = new List<Employee>();
            List<Employee> EmployeeEDate = new List<Employee>();
            List<Employee> StringDate = new List<Employee>();
            List<Employee> Salaries = new List<Employee>();
            List<Employee> Expyears = new List<Employee>();
            List<Employee> Cities = new List<Employee>();
            List<Employee> Centers = new List<Employee>();
            List<Employee> EmployeeTypes = new List<Employee>();
            List<Employee> Periods = new List<Employee>();
            if (textBox.CitiesChange != null)
            {
                if (textBox.CitiesChange == 0)
                {
                    Cities = db.Employees.Where(x => x.CityID == null && x.Centerid == null).ToList();
                }
                else
                {
                    Cities = db.Employees.Where(x => x.Center.Cityid == textBox.CitiesChange || x.CityID == textBox.CitiesChange).ToList();
                }
            }
            if (textBox.CentersChange != null)
            {
                if (textBox.CentersChange == 0)
                {
                    Centers = db.Employees.Where(x => x.Centerid == null).ToList();
                }
                else
                {
                    if (db.Centers.Find(textBox.CentersChange).CenterType == "رئيسي")
                    {
                        Centers = db.Employees.Where(x => x.Centerid == textBox.CentersChange || x.Center.DependedOn == textBox.CentersChange).ToList();
                    }
                    else
                    {
                        Centers = db.Employees.Where(x => x.Centerid == textBox.CentersChange).ToList();
                    }
                }
            }
            if (textBox.EmployeeTypesChange != null)
            {
                EmployeeTypes = db.Employees.Where(x => x.Job == textBox.EmployeeTypesChange).ToList();
            }
            if (textBox.PeriodsChange != null)
            {
                Periods = db.Employees.Where(x => x.Periodid == textBox.PeriodsChange).ToList();
            }

            try
            {
                if (textBox.SearchBoxData != null)
                {
                    var salary = Convert.ToDouble(textBox.SearchBoxData);
                    Salaries = db.Employees.Where(x => x.Salary == salary).ToList();

                }
            }
            catch (Exception)
            {


            }
            try
            {
                if (textBox.SearchBoxData != null)
                {
                    var Expyear = Convert.ToDouble(textBox.SearchBoxData);
                    Expyears = db.Employees.Where(x => x.Salary == Expyear).ToList();

                }
            }
            catch (Exception)
            {


            }
            try
            {
                if (textBox.SearchBoxData != null)
                {
                    var stringdate = DateTime.Parse(textBox.SearchBoxData);
                    StringDate = db.Employees.Where(x => x.BDate == stringdate || x.EDate == stringdate || x.SDate == stringdate).ToList();
                }
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
            || x.surname.Contains(searchbox) || x.State.Contains(searchbox) || x.EmployeeType.Type.Contains(searchbox) || x.Period.Name.Contains(searchbox) || x.InsideOrOutside.Contains(searchbox) || x.OldJob.Contains(searchbox)).ToList();

            EmployeeStrings.AddRange(Salaries);
            EmployeeStrings.AddRange(Expyears);
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

                else if (EmployeeEDate != null)
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

            if (EmployeeStrings.Count() == 0)
            {
                if (Cities.Count() != 0)
                {
                    EmployeeStrings = Cities;
                }
                else if (Centers.Count() != 0)
                {
                    EmployeeStrings = Centers;
                }

                else if (Periods.Count() != 0)
                {
                    EmployeeStrings = Periods;
                }
                else if (EmployeeTypes.Count() != 0)
                {
                    EmployeeStrings = EmployeeTypes;
                }

            }

            if (EmployeeStrings.Count() > 0)
            {
                if (textBox.CitiesChange != null)
                {
                    EmployeeStrings = EmployeeStrings.Intersect(Cities).ToList();
                }
                if (textBox.CentersChange != null)
                {
                    EmployeeStrings = EmployeeStrings.Intersect(Centers).ToList();
                }

                if (textBox.PeriodsChange != null)
                {
                    EmployeeStrings = EmployeeStrings.Intersect(Periods).ToList();
                }
                if (textBox.EmployeeTypesChange != null)
                {
                    EmployeeStrings = EmployeeStrings.Intersect(EmployeeTypes).ToList();
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
                City = c.City.Name,
                Approval = c.Approval,
                OldJob = c.OldJob,
                ExpYears = c.ExpYears,
                InsideOrOutside = c.InsideOrOutside
            }).ToList();
            var typeName = (string)Session["Type"];
            var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
            var empid = Convert.ToInt32(Session["ID"]);
            var emp = db.Employees.Find(empid);
            try
            {
                if (type.AddNewEmployeeType == true && type.SeeAll != true && type.SeeAllButFinance != true && type.SeeAccToCity != true && type.SeeAccToCenter != true && type.SeeTeachers != true)
                {
                    ToSendList = ToSendList.Where(x => x.EmpManager == true).ToList();
                }
                else if (type.SeeAll == true)
                {

                }
                else if (type.SeeAllButFinance == true)
                {


                    ToSendList = ToSendList.Where(x => (x.EmpCoManager == true || x.EmpSchoolManager == true || x.EmpNormal == true) || (x.EmpManager != true && x.EmpNormal != true && x.EmpCoManager != true && x.EmpSchoolManager != true)).ToList();
                }

                else if (type.SeeAccToCity == true)
                {

                    ToSendList = ToSendList.Where(x => ((x.City == emp.City.Name) && (x.EmpSchoolManager == true || x.EmpNormal == true)) || (x.EmpManager != true && x.EmpNormal != true && x.EmpCoManager != true && x.EmpSchoolManager != true && x.City == emp.City.Name)).ToList();
                }
                else if (type.SeeAccToCenter == true)
                {
                    ToSendList = ToSendList.Where(x => (x.Center == emp.Center.Name && (x.EmpSchoolManager == true || x.EmpNormal == true)) || (x.EmpManager != true && x.EmpNormal != true && x.EmpCoManager != true && x.EmpSchoolManager != true && x.Center == emp.Center.Name)).ToList();
                }

                else if (type.SeeTeachers == true)
                {
                    ToSendList = ToSendList.Where(x => (x.EmpSeeTeacher == true && x.Center == emp.Center.Name) || (x.EmpManager != true && x.EmpNormal != true && x.EmpCoManager != true && x.EmpSchoolManager != true)).ToList();
                }
                else
                {
                    ToSendList = null;

                }
            }
            catch
            {

            }
            //.Select(c => new { c.Mark, c.Stage.StageName, c.Student.Name, c.Student.Surname, c.Study_subject.Name, c.Date, c.Desc, c.ExamType.Type })
            return Json(ToSendList, JsonRequestBehavior.AllowGet);
        }






        [HttpGet]
        public ActionResult Presences()
        {
            var empid = Convert.ToInt32(Session["ID"]);
            var emp = db.Employees.Find(empid);
            var Presences = db.Presences.ToList();

            if (emp.EmployeeType.SeeAccToCenter == true || emp.EmployeeType.SeeTeachers == true)
            {
                Presences = Presences.Where(x => x.Student.Centerid == emp.Centerid).ToList();
            }
            else if (emp.EmployeeType.SeeAccToCity == true)
            {
                Presences = Presences.Where(x => x.Student.Center.Cityid == emp.CityID).ToList();
            }

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
                StudentName = c.Student.Name,
                Approval = c.Approval
            }).ToList();
            return Json(ToSendList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Presences(TextBox textBox)
        {
            if (textBox.SearchBoxData == null && textBox.SearchBoxDate == null || textBox.SearchBoxData == "" && textBox.SearchBoxDate == null)
            {
                return RedirectToAction("Presences");
            }


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
                StudentName = c.Student.Name,
                Approval = c.Approval
            }).ToList();
            //.Select(c => new { c.Mark, c.Stage.StageName, c.Student.Name, c.Student.Surname, c.Study_subject.Name, c.Date, c.Desc, c.ExamType.Type })
            return Json(ToSendList, JsonRequestBehavior.AllowGet);
        }





        [HttpGet]
        public ActionResult SearchStudents()
        {
            if (Session["ID"] == null)
            {
                return null;
            }
            var empid = Convert.ToInt32(Session["ID"]);
            var emp = db.Employees.Find(empid);


            var student = db.Students.ToList();

            var Cities = db.Cities.Select(x => new { id = x.id, Name = x.Name }).ToList();

            var Centers = db.Centers.Select(x => new { id = x.id, Name = x.Name, Cityid = x.Cityid }).ToList();

            var Regiments = db.Regiments.Select(x => new { id = x.id, Name = x.Name }).ToList();

            var Stages = db.Stages.Select(x => new { id = x.id, StageName = x.StageName }).ToList();

            var Periods = db.Periods.Select(x => new { id = x.id, Name = x.Name }).ToList();

            List<object> allSelections = new List<object>();

            if (emp.EmployeeType.SeeAccToCenter == true || emp.EmployeeType.SeeTeachers == true)
            {
                student = student.Where(x => x.Centerid == emp.Centerid).ToList();
                Centers = null;
                Cities = null;

            }
            else if (emp.EmployeeType.SeeAccToCity == true)
            {
                student = null;
                Centers = Centers.Where(x => x.Cityid == emp.CityID).ToList();
                Cities = null;

            }
            else
            {
                student = null;
            }



            if (student != null)
            {


                foreach (var item in student)
                {
                    if (item.Center == null)
                    {
                        item.Center = new Center();
                    }
                    if (item.Center.City == null)
                    {
                        item.Center.City = new City();
                    }
                    if (item.Regiment == null)
                    {
                        item.Regiment = new Regiment();
                    }
                    if (item.Stage == null)
                    {
                        item.Stage = new Stage();
                    }
                    if (item.Regiment.Period == null)
                    {
                        item.Regiment.Period = new Period();
                    }
                }
                var ToSendStudents = student.Select(c => new
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
                    City = c.Center.City.Name,
                    Fathersname = c.FathersName,
                    Mothersname = c.Mothersname,
                    StudentNumber = c.StudentNumber,
                    StudentState = c.StudentState,
                    OldSchool = c.OldSchool,
                }).OrderBy(x => x.StudentNumber);
                allSelections.Add(ToSendStudents);

            }
            else
            {
                allSelections.Add(student);
            }
            allSelections.Add(Centers);
            allSelections.Add(Cities);
            allSelections.Add(Regiments);
            allSelections.Add(Stages);
            allSelections.Add(Periods);

            return Json(allSelections, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult SearchStudents(TextBox textBox)
        {
            if (Session["ID"] == null)
            {
                return null;
            }

            var empid = Convert.ToInt32(Session["ID"]);
            var emp = db.Employees.Find(empid);


            string searchbox = textBox.SearchBoxData;
            List<Student> examNum = new List<Student>();
            List<Student> StudentNumber = new List<Student>();
            List<Student> BDate = new List<Student>();
            List<Student> SDate = new List<Student>();
            List<Student> EDate = new List<Student>();
            List<Student> DateTextBox = new List<Student>();


            List<Student> Cities = new List<Student>();
            List<Student> Centers = new List<Student>();
            List<Student> Regiments = new List<Student>();
            List<Student> Stages = new List<Student>();
            List<Student> Periods = new List<Student>();
            if (textBox.CitiesChange != null)
            {
                if (textBox.CitiesChange == 0)
                {
                    Cities = db.Students.ToList();
                }
                else
                {
                    Cities = db.Students.Where(x => x.Center.Cityid == textBox.CitiesChange).ToList();
                }
            }
            if (textBox.CentersChange != null)
            {
                if (db.Centers.Find(textBox.CentersChange).CenterType == "رئيسي")
                {
                    Centers = db.Students.Where(x => x.Centerid == textBox.CentersChange || x.Center.DependedOn == textBox.CentersChange).ToList();
                }
                else
                {
                    Centers = db.Students.Where(x => x.Centerid == textBox.CentersChange).ToList();
                }
            }
            if (textBox.RegimentsChange != null)
            {
                Regiments = db.Students.Where(x => x.Regimentid == textBox.RegimentsChange).ToList();
            }
            if (textBox.StagesChange != null)
            {
                Stages = db.Students.Where(x => x.Stageid == textBox.StagesChange).ToList();
            }
            if (textBox.PeriodsChange != null)
            {
                Periods = db.Students.Where(x => x.Regiment.Periodid == textBox.PeriodsChange).ToList();
            }

            try
            {
                if (searchbox!=null)
                {
                    var num = Convert.ToInt32(searchbox);
                    examNum = db.Students.Where(x => x.Mark == num).ToList();

                }
            }
            catch (Exception)
            {

            }
            try
            {
                if (searchbox != null)
                {
                    var num = Convert.ToInt32(searchbox);
                    StudentNumber = db.Students.Where(x => x.StudentNumber == num).ToList();

                }
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

            var Studentstrings = db.Students.Where(x => x.Center.Name.Contains(searchbox) || x.Name.Contains(searchbox) || x.Surname.Contains(searchbox) || x.FathersName.Contains(searchbox) || x.Mothersname.Contains(searchbox) || x.OldSchool.Contains(searchbox) || x.StudentState.Contains(searchbox) || x.Regiment.Name.Contains(searchbox) || x.Stage.StageName.Contains(searchbox)
            || x.Certificate.Contains(searchbox) || x.State.Contains(searchbox) || x.Sex.Contains(searchbox)).ToList();

            Studentstrings.AddRange(DateTextBox);
            Studentstrings.AddRange(examNum);
            Studentstrings.AddRange(StudentNumber);
            Studentstrings.AddRange(BDate);
            Studentstrings.AddRange(SDate);
            Studentstrings.AddRange(EDate);
            Studentstrings.RemoveAll(item => item == null);
            Studentstrings.Distinct();

            if (Studentstrings.Count() == 0)
            {
                if (Cities.Count() != 0)
                {
                    Studentstrings = Cities;
                }
                else if (Centers.Count() != 0)
                {
                    Studentstrings = Centers;
                }
                else if (Regiments.Count() != 0)
                {
                    Studentstrings = Regiments;
                }
                else if (Periods.Count() != 0)
                {
                    Studentstrings = Periods;
                }
                else if (Stages.Count() != 0)
                {
                    Studentstrings = Stages;
                }

            }

            if (Studentstrings.Count() > 0)
            {
                if (textBox.CitiesChange != null)
                {
                    Studentstrings = Studentstrings.Intersect(Cities).ToList();
                }
                if (textBox.CentersChange != null)
                {
                    Studentstrings = Studentstrings.Intersect(Centers).ToList();
                }
                if (textBox.RegimentsChange != null)
                {
                    Studentstrings = Studentstrings.Intersect(Regiments).ToList();
                }
                if (textBox.PeriodsChange != null)
                {
                    Studentstrings = Studentstrings.Intersect(Periods).ToList();
                }
                if (textBox.StagesChange != null)
                {
                    Studentstrings = Studentstrings.Intersect(Stages).ToList();
                }
            }

            var typeName = (string)Session["Type"];
            var type = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();
            try
            {
                if (type.SeeAccToCity != null)
                {
                    if (type.SeeAccToCity == true)
                    {
                        Studentstrings = Studentstrings.Where(x => x.Center.Cityid == emp.CityID).ToList();
                    }
                }
                else if (type.SeeAccToCenter != null || type.SeeTeachers != null)
                {
                    if (type.SeeAccToCenter == true || type.SeeTeachers == true)
                    {
                        Studentstrings = Studentstrings.Where(x => x.Centerid == emp.Centerid).ToList();
                    }
                }
            }
            catch
            {

            }

            foreach (var item in Studentstrings)
            {
                if (item.Center == null)
                {
                    item.Center = new Center();
                }
                if (item.Center.City == null)
                {
                    item.Center.City = new City();
                }
                if (item.Regiment == null)
                {
                    item.Regiment = new Regiment();
                }
                if (item.Stage == null)
                {
                    item.Stage = new Stage();
                }
                if (item.Regiment.Period == null)
                {
                    item.Regiment.Period = new Period();
                }
            }

            var ToSendStudents = Studentstrings.Select(c => new
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
                City = c.Center.City.Name,
                Fathersname = c.FathersName,
                Mothersname = c.Mothersname,
                StudentNumber = c.StudentNumber,
                StudentState = c.StudentState,
                OldSchool = c.OldSchool,
            }).ToList();


            return Json(ToSendStudents, JsonRequestBehavior.AllowGet);
        }







        public FileResult DownloadProof(string ImageName, int? id)
        {
            if (ImageName == null)
            {
                Proove proove = db.Prooves.Find(id);
                ImageName = proove.ZipFilePath;
            }
            var exe = Path.GetExtension(ImageName);
            var c = Path.GetDirectoryName(ImageName);
            string lastFolderName = Path.GetFileName(Path.GetDirectoryName(ImageName));
            return File(ImageName, System.Net.Mime.MediaTypeNames.Application.Octet, DateTime.Now.Month + "/" + DateTime.Now.Day + "__" + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "__" + lastFolderName + exe);
        }





        public ActionResult Approvals(int id, string type, bool acceptance)
        {
            if (Session["ID"] != null)
            {
                var Myid = Convert.ToInt32(Session["ID"]);
                var typeName = (string)Session["Type"];
                var Emptype = db.EmployeeTypes.Where(x => x.Type == typeName).FirstOrDefault();

                if (type == "Employees")
                {
                    var emp = db.Employees.Find(id);
                    if (Emptype.HighAcceptance == true)
                    {
                        emp.Approval = acceptance;
                        emp.ApprovedBy = Myid;
                        db.Entry(emp).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    return RedirectToAction("Index", "Centers");

                }
                else if (type == "Students")
                {
                    var student = db.Students.Find(id);

                    if (Emptype.AddSchoolManagers == true)
                    {
                        student.Approval = acceptance;
                        student.ApprovedBy = Myid;
                        db.Entry(student).State = EntityState.Modified;
                        db.SaveChanges();

                    }
                    return RedirectToAction("Index", "Centers");

                }
                else if (type == "Examinations")
                {
                    var exam = db.Examinations.Find(id);

                    if (Emptype.AddSchoolEmployees == true)
                    {
                        exam.Approval = acceptance;
                        exam.ApprovedBy = Myid;
                        db.Entry(exam).State = EntityState.Modified;
                        db.SaveChanges();


                    }
                    return RedirectToAction("Index", "Centers");

                }
                else if (type == "Centers")
                {
                    var center = db.Centers.Find(id);

                    if (Emptype.HighAcceptance == true)
                    {
                        center.Approval = acceptance;
                        center.ApprovedBy = Myid;
                        db.Entry(center).State = EntityState.Modified;
                        db.SaveChanges();


                    }
                    return RedirectToAction("Index", "Centers");
                }

                else if (type == "Presences")
                {
                    var presence = db.Presences.Find(id);

                    if (Emptype.HighAcceptance == true)
                    {
                        presence.Approval = acceptance;
                        presence.ApprovedBy = Myid;
                        db.Entry(presence).State = EntityState.Modified;
                        db.SaveChanges();


                    }
                    return RedirectToAction("Index", "Centers");
                }
            }
            return RedirectToAction("Index", "Home");
        }
    }
}