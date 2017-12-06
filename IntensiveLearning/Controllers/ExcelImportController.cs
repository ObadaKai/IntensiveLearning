using IntensiveLearning.Database;
using IntensiveLearning.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace IntensiveLearning.Views.TryOut
{
    public class ExcelImportController : Controller
    {
        TaalimEntities db = new TaalimEntities();
        // GET: Default
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ImportEmployees(HttpPostedFileBase file)
        {
            if (Session["ID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            DataSet ds = new DataSet();
            if (Request.Files["file"].ContentLength > 0)
            {
                string fileExtension = System.IO.Path.GetExtension(Request.Files["file"].FileName);
                if (fileExtension == ".xls" || fileExtension == ".xlsx")
                {
                    string fileLocation = Server.MapPath("~/App_Data/Test/" + Request.Files["file"].FileName);
                    if (System.IO.File.Exists(fileLocation))
                    {
                        System.IO.File.Delete(fileLocation);
                    }
                    Request.Files["file"].SaveAs(fileLocation);
                    string excelConnectionString = string.Empty;
                    excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    //connection String for xls file format.
                    if (fileExtension == ".xls")
                    {
                        excelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                    }
                    //connection String for xlsx file format.
                    else if (fileExtension == ".xlsx")
                    {
                        excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    }
                    //Create Connection to Excel work book and add oledb namespace
                    OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
                    excelConnection.Open();
                    DataTable dt = new DataTable();

                    dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    if (dt == null)
                    {
                        return null;
                    }
                    String[] excelSheets = new String[dt.Rows.Count];
                    int t = 0;
                    //excel data saves in temp file here.
                    foreach (DataRow row in dt.Rows)
                    {
                        excelSheets[t] = row["TABLE_NAME"].ToString();
                        t++;
                    }
                    OleDbConnection excelConnection1 = new OleDbConnection(excelConnectionString);
                    string query = string.Format("Select * from [{0}]", excelSheets[0]);
                    using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection1))
                    {
                        dataAdapter.Fill(ds);
                    }
                    excelConnection.Close();
                    excelConnection1.Close();
                }
                if (fileExtension.ToString().ToLower().Equals(".xml"))
                {
                    string fileLocation = Server.MapPath("~/Test/") + Request.Files["FileUpload"].FileName;
                    if (System.IO.File.Exists(fileLocation))
                    {
                        System.IO.File.Delete(fileLocation);
                    }

                    Request.Files["FileUpload"].SaveAs(fileLocation);
                    XmlTextReader xmlreader = new XmlTextReader(fileLocation);
                    // DataSet ds = new DataSet();
                    ds.ReadXml(xmlreader);
                    xmlreader.Close();
                }

                int counter;
                try
                {
                    counter = db.Employees.OrderByDescending(x => x.id).FirstOrDefault().id + 1;
                }
                catch (Exception)
                {
                    counter = 1;
                }


                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Employee employee = new Employee();
                    employee.id = counter;
                    counter++;
                    var Myid = Convert.ToInt16(Session["ID"]);
                    var emp = db.Employees.Find(Myid);
                    employee.AddedBy = Myid;
                    employee.AddingDate = DateTime.Now.Date;
                    employee.AddingTime = DateTime.Now.TimeOfDay;
                    employee.State = "متوفر";
                    employee.name = ds.Tables[0].Rows[i]["الاسم"].ToString();
                    employee.surname = ds.Tables[0].Rows[i]["الكنية"].ToString();
                    employee.FathersName = ds.Tables[0].Rows[i]["اسم الأب"].ToString();

                    try
                    {
                        employee.BDate = DateTime.Parse(ds.Tables[0].Rows[i]["تاريخ الولادة"].ToString(), CultureInfo.CurrentCulture);

                    }
                    catch
                    {

                        employee.BDate = null;

                    }
                    employee.OldJob = ds.Tables[0].Rows[i]["الوظيفة القديمة"].ToString();
                    try
                    {
                        employee.ExpYears = Convert.ToInt32(ds.Tables[0].Rows[i]["عدد سنوات الخبرة"].ToString());
                    }
                    catch
                    {
                        employee.ExpYears = null;
                    }
                    employee.InsideOrOutside = ds.Tables[0].Rows[i]["داخل أو خارج الملاك"].ToString();

                    employee.Certificate = ds.Tables[0].Rows[i]["الشهادة"].ToString();
                    if (ds.Tables[0].Rows[i]["نوع الشهادة"].ToString() == "ابتدائي" || ds.Tables[0].Rows[i]["نوع الشهادة"].ToString() == "اعدادي" || ds.Tables[0].Rows[i]["نوع الشهادة"].ToString() == "ثانوي" || ds.Tables[0].Rows[i]["نوع الشهادة"].ToString() == "جامعي 4 سنوات" || ds.Tables[0].Rows[i]["نوع الشهادة"].ToString() == "جامعي 5 سنوات" || ds.Tables[0].Rows[i]["نوع الشهادة"].ToString() == "جامعي 6 سنوات" || ds.Tables[0].Rows[i]["نوع الشهادة"].ToString() == "ماجستير" || ds.Tables[0].Rows[i]["نوع الشهادة"].ToString() == "دكتوراة")
                    {
                        employee.CType = ds.Tables[0].Rows[i]["نوع الشهادة"].ToString();

                    }
                    if (ds.Tables[0].Rows[i]["الجنس"].ToString() == "ذكر" || ds.Tables[0].Rows[i]["الجنس"].ToString() == "انثى" || ds.Tables[0].Rows[i]["الجنس"].ToString() == "أنثى")
                    {
                        if (ds.Tables[0].Rows[i]["الجنس"].ToString() == "أنثى")
                        {
                            employee.Sex = "انثى";
                        }
                        else
                        {
                            employee.Sex = ds.Tables[0].Rows[i]["الجنس"].ToString();
                        }
                    }
                    try
                    {
                        employee.SDate = DateTime.Parse(ds.Tables[0].Rows[i]["تاريخ البدء"].ToString(), CultureInfo.CurrentCulture);

                    }
                    catch
                    {

                        if (DateTime.Now.Date == DateTime.Parse("30/11/2017", CultureInfo.CurrentCulture))
                        {
                            employee.SDate = DateTime.Parse("10/10/2017", CultureInfo.CurrentCulture);
                        }
                        else
                        {
                            employee.SDate = null;
                        }

                    }
                    if (emp.EmployeeType.SeeAccToCity == true)
                    {
                        string CenterName = ds.Tables[0].Rows[i]["اسم المركز"].ToString();
                        var jobname = ds.Tables[0].Rows[i]["المهمة"].ToString();
                        var periodname = ds.Tables[0].Rows[i]["الفترة"].ToString();

                        try
                        {
                            employee.Centerid = db.Centers.Where(c => c.Cityid == emp.CityID).FirstOrDefault(x => x.Name == CenterName).id;
                        }
                        catch
                        {
                            employee.Centerid = null;
                        }
                        try
                        {
                            employee.CityID = emp.CityID;
                        }
                        catch
                        {
                            employee.CityID = null;
                        }
                        try
                        {
                            employee.Job = db.EmployeeTypes.FirstOrDefault(x => x.Type == jobname).id;
                            if (employee.Job < 8)
                            {
                                employee.Job = 16;
                            }
                        }
                        catch
                        {
                            employee.Job = 16;
                        }

                        try
                        {
                            employee.Periodid = db.Periods.FirstOrDefault(x => x.Name == periodname).id;
                        }
                        catch
                        {
                            employee.Periodid = null;
                        }
                    }
                    else if (emp.EmployeeType.SeeAccToCenter == true)
                    {
                        var jobname = ds.Tables[0].Rows[i]["المهمة"].ToString();
                        var periodname = ds.Tables[0].Rows[i]["الفترة"].ToString();

                        try
                        {
                            employee.Centerid = emp.Centerid;
                        }
                        catch
                        {
                            employee.Centerid = null;
                        }

                        try
                        {
                            employee.Job = db.EmployeeTypes.FirstOrDefault(x => x.Type == jobname).id;
                            if (employee.Job < 7)
                            {
                                employee.Job = 16;
                            }
                        }
                        catch
                        {
                            employee.Job = 16;
                        }

                        try
                        {
                            employee.Periodid = db.Periods.FirstOrDefault(x => x.Name == periodname).id;
                        }
                        catch
                        {
                            employee.Periodid = null;
                        }

                    }
                    else if (emp.EmployeeType.CoManager == true && emp.EmployeeType.SeeAccToCity != true || emp.EmployeeType.Manager == true)
                    {
                        string CityName = ds.Tables[0].Rows[i]["اسم المدينة"].ToString();
                        string CenterName = ds.Tables[0].Rows[i]["اسم المركز"].ToString();
                        var jobname = ds.Tables[0].Rows[i]["المهمة"].ToString();
                        var periodname = ds.Tables[0].Rows[i]["الفترة"].ToString();

                        try
                        {
                            employee.Centerid = db.Centers.FirstOrDefault(x => x.Name == CenterName).id;
                        }
                        catch
                        {
                            employee.Centerid = null;
                        }
                        try
                        {
                            employee.CityID = db.Cities.FirstOrDefault(x => x.Name == CityName).id;
                        }
                        catch
                        {
                            employee.CityID = null;
                        }
                        try
                        {
                            employee.Job = db.EmployeeTypes.FirstOrDefault(x => x.Type == jobname).id;
                            if (employee.Job < 8)
                            {
                                employee.Job = 16;
                            }
                        }
                        catch
                        {
                            employee.Job = 16;
                        }

                        try
                        {
                            employee.Periodid = db.Periods.FirstOrDefault(x => x.Name == periodname).id;
                        }
                        catch
                        {
                            employee.Periodid = null;
                        }
                    }

                    try
                    {
                        employee.Username = ds.Tables[0].Rows[i]["اسم المستخدم"].ToString();
                        if (db.Employees.Where(x => x.Username == employee.Username).Count() > 0)
                        {
                            employee.Username = null;
                        }
                    }
                    catch
                    {
                        employee.Username = null;
                    }
                    employee.Proof = null;
                    employee.Password = Helper.ComputeHash("123", "SHA512", null);
                    db.Employees.Add(employee);
                    //string conn = ConfigurationManager.ConnectionStrings["Data Source=DESKTOP-N6HJU8V;Initial Catalog=TaalimCopyOnline;Integrated Security=True"].ConnectionString;
                    //SqlConnection con = new SqlConnection(conn);
                    //string query = "Insert into Employees(id,name,surname,BDate,Certificate,CType,SDate,Job,Sex,FathersName,Centerid,Periodid,CityID) Values('" + ds.Tables[0].Rows[i][0].ToString() + "','" + ds.Tables[0].Rows[i][1].ToString() + "','" + ds.Tables[0].Rows[i][2].ToString() + "')";
                    //con.Open();
                    //SqlCommand cmd = new SqlCommand(query, con);
                    //cmd.ExecuteNonQuery();
                    //con.Close();
                }
                try
                {
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();
                }
                catch
                {
                    ViewBag.Importerror = "حصل خطأ اثناء الحفظ";
                }
            }
            return RedirectToAction("Index", "Employees");
        }

        [HttpPost]
        public ActionResult ImportStudents(HttpPostedFileBase file)
        {
            if (Session["ID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            DataSet ds = new DataSet();
            if (Request.Files["file"].ContentLength > 0)
            {
                string fileExtension = System.IO.Path.GetExtension(Request.Files["file"].FileName);
                if (fileExtension == ".xls" || fileExtension == ".xlsx")
                {
                    string fileLocation = Server.MapPath("~/App_Data/Test/") + Request.Files["file"].FileName;
                    if (System.IO.File.Exists(fileLocation))
                    {
                        System.IO.File.Delete(fileLocation);
                    }
                    Request.Files["file"].SaveAs(fileLocation);
                    string excelConnectionString = string.Empty;
                    excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    //connection String for xls file format.
                    if (fileExtension == ".xls")
                    {
                        excelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                    }
                    //connection String for xlsx file format.
                    else if (fileExtension == ".xlsx")
                    {
                        excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    }
                    //Create Connection to Excel work book and add oledb namespace
                    OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
                    excelConnection.Open();
                    DataTable dt = new DataTable();

                    dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    if (dt == null)
                    {
                        return null;
                    }
                    String[] excelSheets = new String[dt.Rows.Count];
                    int t = 0;
                    //excel data saves in temp file here.
                    foreach (DataRow row in dt.Rows)
                    {
                        excelSheets[t] = row["TABLE_NAME"].ToString();
                        t++;
                    }
                    OleDbConnection excelConnection1 = new OleDbConnection(excelConnectionString);
                    string query = string.Format("Select * from [{0}]", excelSheets[0]);
                    using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection1))
                    {
                        dataAdapter.Fill(ds);
                    }

                    excelConnection.Close();
                    excelConnection1.Close();

                }
                if (fileExtension.ToString().ToLower().Equals(".xml"))
                {
                    string fileLocation = Server.MapPath("~/Test/") + Request.Files["FileUpload"].FileName;
                    if (System.IO.File.Exists(fileLocation))
                    {
                        System.IO.File.Delete(fileLocation);
                    }

                    Request.Files["FileUpload"].SaveAs(fileLocation);
                    XmlTextReader xmlreader = new XmlTextReader(fileLocation);
                    // DataSet ds = new DataSet();
                    ds.ReadXml(xmlreader);
                    xmlreader.Close();
                }

                int counter;
                try
                {
                    counter = db.Students.OrderByDescending(x => x.id).FirstOrDefault().id + 1;
                }
                catch (Exception)
                {
                    counter = 1;
                }

                int studentnumber;
                try
                {
                    studentnumber = (int)db.Students.OrderByDescending(x => x.StudentNumber).FirstOrDefault().StudentNumber + 1;
                    int yearnum = studentnumber / 100000;
                    var yearS = DateTime.Now;
                    string lastTwoDigitsOfYear = yearS.ToString("yy");
                    var ExactYear = Convert.ToInt16(lastTwoDigitsOfYear);
                    if (yearnum != ExactYear)
                    {
                        var studentNumberString = ExactYear.ToString() + "00001";
                        var NewstudentNumber = Convert.ToInt32(studentNumberString);
                        studentnumber = NewstudentNumber;
                    }
                }
                catch
                {
                    var yearS = DateTime.Now;
                    string lastTwoDigitsOfYear = yearS.ToString("yy");
                    var ExactYear = Convert.ToInt16(lastTwoDigitsOfYear);
                    var studentNumberString = ExactYear.ToString() + "00001";
                    var studentNumber = Convert.ToInt32(studentNumberString);
                    studentnumber = studentNumber;
                }

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Student student = new Student();
                    student.id = counter;
                    counter++;
                    var Myid = Convert.ToInt16(Session["ID"]);
                    student.AddedBy = Myid;
                    student.AddingDate = DateTime.Now.Date;
                    student.AddingTime = DateTime.Now.TimeOfDay;
                    student.State = "متوفر";
                    student.Name = ds.Tables[0].Rows[i]["الاسم"].ToString();
                    student.Surname = ds.Tables[0].Rows[i]["الكنية"].ToString();
                    student.FathersName = ds.Tables[0].Rows[i]["اسم الأب"].ToString();
                    student.Mothersname = ds.Tables[0].Rows[i]["اسم الأم"].ToString();

                    try
                    {
                        student.BDate = DateTime.Parse(ds.Tables[0].Rows[i]["تاريخ الولادة"].ToString(), CultureInfo.CreateSpecificCulture("fr-FR"));
                    }
                    catch
                    {

                        student.BDate = null;

                    }
                    student.Certificate = ds.Tables[0].Rows[i]["الشهادة المتحصل عليها"].ToString();
                    try
                    {
                        student.Mark = Convert.ToInt32(ds.Tables[0].Rows[i]["العلامة"].ToString());
                    }
                    catch
                    {
                        student.Mark = null;
                    }
                    student.OldSchool = ds.Tables[0].Rows[i]["المدرسة التي كان يدرس فيها"].ToString();
                    string mutassarreb = ds.Tables[0].Rows[i]["وضع التلميذ متسرب/راسب/منقطع"].ToString();
                    if (mutassarreb == "متسرب" || mutassarreb == "راسب" || mutassarreb == "منقطع")
                    {
                        student.StudentState = ds.Tables[0].Rows[i]["وضع التلميذ متسرب/راسب/منقطع"].ToString();

                    }

                    if (ds.Tables[0].Rows[i]["الجنس"].ToString() == "ذكر" || ds.Tables[0].Rows[i]["الجنس"].ToString() == "انثى" || ds.Tables[0].Rows[i]["الجنس"].ToString() == "أنثى")
                    {
                        if (ds.Tables[0].Rows[i]["الجنس"].ToString() == "أنثى")
                        {
                            student.Sex = "انثى";
                        }
                        else
                        {
                            student.Sex = ds.Tables[0].Rows[i]["الجنس"].ToString();
                        }
                    }
                    var sdate = ds.Tables[0].Rows[i]["تاريخ البدء"].ToString();
                    try
                    {
                        student.SDate = DateTime.Parse(ds.Tables[0].Rows[i]["تاريخ البدء"].ToString(), CultureInfo.CreateSpecificCulture("fr-FR"));
                    }
                    catch
                    {

                        if (DateTime.Now.Date == DateTime.Parse("30/11/2017", CultureInfo.CreateSpecificCulture("fr-FR")))
                        {
                            student.SDate = DateTime.Parse("14/10/2017", CultureInfo.CreateSpecificCulture("fr-FR"));
                        }
                        else
                        {
                            student.SDate = null;
                        }

                    }
                    var empid = Convert.ToInt32(Session["ID"]);
                    var emp = db.Employees.Find(empid);
                    if (emp.EmployeeType.SeeAccToCenter == true)
                    {
                        student.Centerid = emp.Centerid;
                    }
                    else
                    {
                        string CenterName = ds.Tables[0].Rows[i]["اسم المركز"].ToString();
                        try
                        {
                            student.Centerid = db.Centers.FirstOrDefault(x => x.Name == CenterName).id;
                        }
                        catch
                        {
                            student.Centerid = null;
                        }

                    }
                    var Regimentname = ds.Tables[0].Rows[i]["اسم الشعبة"].ToString();
                    var StageName = ds.Tables[0].Rows[i]["اسم المرحلة"].ToString();


                    try
                    {
                        student.Regimentid = db.Regiments.FirstOrDefault(x => x.Name == Regimentname).id;
                    }
                    catch
                    {
                        student.Regimentid = null;
                    }
                    try
                    {
                        student.Stageid = db.Stages.FirstOrDefault(x => x.StageName == StageName).id;
                    }
                    catch
                    {
                        student.Stageid = null;
                    }

                    student.StudentNumber = studentnumber;
                    studentnumber++;

                    db.Students.Add(student);
                    //string conn = ConfigurationManager.ConnectionStrings["Data Source=DESKTOP-N6HJU8V;Initial Catalog=TaalimCopyOnline;Integrated Security=True"].ConnectionString;
                    //SqlConnection con = new SqlConnection(conn);
                    //string query = "Insert into Employees(id,name,surname,BDate,Certificate,CType,SDate,Job,Sex,FathersName,Centerid,Periodid,CityID) Values('" + ds.Tables[0].Rows[i][0].ToString() + "','" + ds.Tables[0].Rows[i][1].ToString() + "','" + ds.Tables[0].Rows[i][2].ToString() + "')";
                    //con.Open();
                    //SqlCommand cmd = new SqlCommand(query, con);
                    //cmd.ExecuteNonQuery();
                    //con.Close();
                }
                try
                {
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();
                }
                catch
                {
                    ViewBag.Importerror = "حصل خطأ اثناء الحفظ";
                }
            }
            return RedirectToAction("Index", "Students");
        }





        [HttpPost]
        public ActionResult ImportBnds(HttpPostedFileBase file)
        {

            if (Session["ID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            DataSet ds = new DataSet();
            if (Request.Files["file"].ContentLength > 0)
            {
                string fileExtension = System.IO.Path.GetExtension(Request.Files["file"].FileName);
                if (fileExtension == ".xls" || fileExtension == ".xlsx")
                {
                    string fileLocation = Server.MapPath("~/App_Data/Test/" + Request.Files["file"].FileName);
                    if (System.IO.File.Exists(fileLocation))
                    {
                        System.IO.File.Delete(fileLocation);
                    }
                    Request.Files["file"].SaveAs(fileLocation);
                    string excelConnectionString = string.Empty;
                    excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    //connection String for xls file format.
                    if (fileExtension == ".xls")
                    {
                        excelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                    }
                    //connection String for xlsx file format.
                    else if (fileExtension == ".xlsx")
                    {
                        excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    }
                    //Create Connection to Excel work book and add oledb namespace
                    OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
                    excelConnection.Open();
                    DataTable dt = new DataTable();

                    dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    if (dt == null)
                    {
                        return null;
                    }
                    String[] excelSheets = new String[dt.Rows.Count];
                    int t = 0;
                    //excel data saves in temp file here.
                    foreach (DataRow row in dt.Rows)
                    {
                        excelSheets[t] = row["TABLE_NAME"].ToString();
                        t++;
                    }
                    OleDbConnection excelConnection1 = new OleDbConnection(excelConnectionString);
                    string query = string.Format("Select * from [{0}]", excelSheets[0]);
                    using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection1))
                    {
                        dataAdapter.Fill(ds);
                    }

                    excelConnection.Close();
                    excelConnection1.Close();

                }
                if (fileExtension.ToString().ToLower().Equals(".xml"))
                {
                    string fileLocation = Server.MapPath("~/Test/") + Request.Files["FileUpload"].FileName;
                    if (System.IO.File.Exists(fileLocation))
                    {
                        System.IO.File.Delete(fileLocation);
                    }

                    Request.Files["FileUpload"].SaveAs(fileLocation);
                    XmlTextReader xmlreader = new XmlTextReader(fileLocation);
                    // DataSet ds = new DataSet();
                    ds.ReadXml(xmlreader);
                    xmlreader.Close();
                }

                int counter;
                try
                {
                    counter = db.Bnds.OrderByDescending(x => x.id).FirstOrDefault().id + 1;
                }
                catch (Exception)
                {
                    counter = 1;
                }

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Bnd bnd = new Bnd();
                    bnd.id = counter;
                    counter++;
                    bnd.Simbol = ds.Tables[0].Rows[i][0].ToString();
                    bnd.BudgetLineItem = ds.Tables[0].Rows[i][1].ToString();
                    bnd.Name = ds.Tables[0].Rows[i][2].ToString();
                    bnd.Unit = ds.Tables[0].Rows[i][3].ToString();

                    try
                    {
                        if (ds.Tables[0].Rows[i][22].ToString() == "Indirect")
                        {
                            double duble = Convert.ToDouble(ds.Tables[0].Rows[i][4].ToString()) * 100;
                            bnd.NumberOfUnits = (int)duble;

                        }
                        else
                        {
                            double duble = Convert.ToDouble(ds.Tables[0].Rows[i][4].ToString()) * 100;
                            bnd.NumberOfUnits = (int)duble;
                        }
                    }
                    catch
                    {

                        bnd.NumberOfUnits = null;

                    }
                    try
                    {
                        bnd.PerUnitCost = Convert.ToDouble(ds.Tables[0].Rows[i][5].ToString());
                    }
                    catch
                    {

                        bnd.PerUnitCost = null;

                    }
                    try
                    {
                        bnd.PeriodOnMonth = Convert.ToDouble(ds.Tables[0].Rows[i][6].ToString());
                    }
                    catch
                    {
                        bnd.PeriodOnMonth = null;
                    }



                    try
                    {
                        bnd.PercentOfCostChargedtoProject = Convert.ToDouble(ds.Tables[0].Rows[i][7].ToString()) * 100;
                    }
                    catch
                    {
                        bnd.PercentOfCostChargedtoProject = null;
                    }
                    try
                    {
                        bnd.TotalNum = Convert.ToDouble(ds.Tables[0].Rows[i][8].ToString());
                    }
                    catch
                    {
                        bnd.TotalNum = null;
                    }
                    try
                    {
                        bnd.AfterReductionNum = Convert.ToDouble(ds.Tables[0].Rows[i][8].ToString());
                    }
                    catch
                    {
                        bnd.AfterReductionNum = null;
                    }

                    try
                    {
                        bnd.Month13Share = Convert.ToDouble(ds.Tables[0].Rows[i][9].ToString());
                    }
                    catch
                    {
                        bnd.Month13Share = null;
                    }
                    try
                    {
                        bnd.Month10Share = Convert.ToDouble(ds.Tables[0].Rows[i][10].ToString());
                    }
                    catch
                    {
                        bnd.Month10Share = null;
                    }
                    try
                    {
                        bnd.Month11Share = Convert.ToDouble(ds.Tables[0].Rows[i][11].ToString());
                    }
                    catch
                    {
                        bnd.Month11Share = null;
                    }
                    try
                    {
                        bnd.Month12Share = Convert.ToDouble(ds.Tables[0].Rows[i][12].ToString());
                    }
                    catch
                    {
                        bnd.Month12Share = null;
                    }
                    try
                    {
                        bnd.Month1Share = Convert.ToDouble(ds.Tables[0].Rows[i][13].ToString());
                    }
                    catch
                    {
                        bnd.Month1Share = null;
                    }
                    try
                    {
                        bnd.Month2Share = Convert.ToDouble(ds.Tables[0].Rows[i][14].ToString());
                    }
                    catch
                    {
                        bnd.Month2Share = null;
                    }
                    try
                    {
                        bnd.Month3Share = Convert.ToDouble(ds.Tables[0].Rows[i][15].ToString());
                    }
                    catch
                    {
                        bnd.Month3Share = null;
                    }
                    try
                    {
                        bnd.Month4Share = Convert.ToDouble(ds.Tables[0].Rows[i][16].ToString());
                    }
                    catch
                    {
                        bnd.Month4Share = null;
                    }
                    try
                    {
                        bnd.Month5Share = Convert.ToDouble(ds.Tables[0].Rows[i][17].ToString());
                    }
                    catch
                    {
                        bnd.Month5Share = null;
                    }
                    try
                    {
                        bnd.Month6Share = Convert.ToDouble(ds.Tables[0].Rows[i][18].ToString());
                    }
                    catch
                    {
                        bnd.Month6Share = null;
                    }
                    try
                    {
                        bnd.Month7Share = Convert.ToDouble(ds.Tables[0].Rows[i][19].ToString());
                    }
                    catch
                    {
                        bnd.Month7Share = null;
                    }
                    try
                    {
                        bnd.Month8Share = Convert.ToDouble(ds.Tables[0].Rows[i][20].ToString());
                    }
                    catch
                    {
                        bnd.Month8Share = null;
                    }

                    try
                    {
                        bnd.Month9Share = Convert.ToDouble(ds.Tables[0].Rows[i][21].ToString());
                    }
                    catch
                    {
                        bnd.Month9Share = null;
                    }
                    bnd.WhichTable = ds.Tables[0].Rows[i][22].ToString();
                    db.Bnds.Add(bnd);
                    //string conn = ConfigurationManager.ConnectionStrings["Data Source=DESKTOP-N6HJU8V;Initial Catalog=TaalimCopyOnline;Integrated Security=True"].ConnectionString;
                    //SqlConnection con = new SqlConnection(conn);
                    //string query = "Insert into Employees(id,name,surname,BDate,Certificate,CType,SDate,Job,Sex,FathersName,Centerid,Periodid,CityID) Values('" + ds.Tables[0].Rows[i][0].ToString() + "','" + ds.Tables[0].Rows[i][1].ToString() + "','" + ds.Tables[0].Rows[i][2].ToString() + "')";
                    //con.Open();
                    //SqlCommand cmd = new SqlCommand(query, con);
                    //cmd.ExecuteNonQuery();
                    //con.Close();
                }
                try
                {
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();
                }
                catch
                {
                    ViewBag.Importerror = "حصل خطأ اثناء الحفظ";
                }
            }
            return RedirectToAction("Index", "Students");
        }
    }
}