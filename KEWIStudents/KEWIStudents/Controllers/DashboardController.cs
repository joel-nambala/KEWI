using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using KEWIStudents.Models;
using KEWIStudents.NAVWS;

namespace KEWIStudents.Controllers
{
    public class DashboardController : Controller
    {
        Studentportal webportals = Components.ObjNav;
        string[] strLimiters = new string[] { "::" };
        public ActionResult Index()
        {
            if (Session["username"] == null) return RedirectToAction("index", "login");
            try
            {
            string path = Server.MapPath("~/Downloads/");
            if(!Directory.Exists(path))Directory.CreateDirectory(path);
            ViewBag.StudentName = Session["studentName"].ToString();

                string username = Session["username"].ToString();
                string courseData = webportals.GetStudentCourseData(username);
                if (!string.IsNullOrEmpty(courseData))
                {
                    string[] courseDataArr = courseData.Split(strLimiters, StringSplitOptions.None);
                    string program = courseDataArr[0];
                    string programName = courseDataArr[1];
                    string stage = courseDataArr[2];
                    string settlementType = courseDataArr[3];
                    string semester = courseDataArr[4];
                    string academicYear = courseDataArr[5];
                    string yearOfStudy = courseDataArr[6];

                    Session["program"] = program;
                    Session["stage"] = stage;
                    Session["settlementType"] = settlementType;
                    Session["semester"] = semester;
                    Session["academicYear"] = academicYear;
                    Session["yearOfStudy"] = yearOfStudy;                    
                }
            }
            catch(Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return View();
        }

        public ActionResult MyProfile()
        {
            if (Session["username"] == null) return RedirectToAction("index", "login");
            Account account = new Account();
            try
            {
                string username = Session["username"].ToString();
                string response = webportals.GetStudentDetails(username);
                if(!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(strLimiters,StringSplitOptions.None);
                    account.StudentNo = responseArr[0];
                    account.StudentName = responseArr[1];
                    account.Email= responseArr[2];
                    account.IdNumber = responseArr[3];
                    account.PhoneNo = responseArr[4];
                    account.DateOfBirth= string.IsNullOrEmpty(responseArr[5]) ? DateTime.Now : Convert.ToDateTime(responseArr[5]);
                    account.PostalAddress = responseArr[6];
                }

                string courseData = webportals.GetStudentCourseData(username);
                if(!string.IsNullOrEmpty (courseData))
                {
                    string[] courseDataArr = courseData.Split(strLimiters, StringSplitOptions.None);
                    string program = courseDataArr[0];
                    string programName = courseDataArr[1];
                    string stage = courseDataArr[2];
                    string settlementType = courseDataArr[3];
                    string semester = courseDataArr[4];
                    string academicYear= courseDataArr[5];
                    string yearOfStudy = courseDataArr[6];

                    account.Program = program;
                    account.ProgramName = programName;
                    account.Stage = stage;
                    account.SettlementType = settlementType;
                    account.Semester = semester;
                }
                else
                {
                    TempData["Error"] = "You have insufficient data. Please contact the system administrator!";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return View(account);
        }

        public ActionResult Settings()
        {
            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();
            return RedirectToAction("index", "login");
        }

        public JsonResult GetStudentFees()
        {
            string studentNo = Session["username"].ToString();
            var item = FinanceHelper.GetStudentFees(studentNo);
            return Json(item,JsonRequestBehavior.AllowGet);
        }
    }
}