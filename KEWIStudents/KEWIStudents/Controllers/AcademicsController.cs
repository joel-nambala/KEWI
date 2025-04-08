using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KEWIStudents.Models;
using KEWIStudents.NAVWS;
using Microsoft.Ajax.Utilities;

namespace KEWIStudents.Controllers
{
    public class AcademicsController : Controller
    {
        Studentportal webportals = Components.ObjNav;
        string[] strLimiters = new string[] { "::" };
        string[] strLimiters2 = new string[] { "[]" };

        public ActionResult SemesterRegistration()
        {
            if (Session["username"] == null) return RedirectToAction("index", "login");
            Academics academics=new Academics();
            try
            {
                string username = Session["username"].ToString();
                string studentStatus = AcademicsHelper.StudentStatus(username);
                if (studentStatus == "Current" || studentStatus == "Registration") { }
                else
                {
                    TempData["Error"] = $"You are not eligible for registration at this time. Your current status is {studentStatus}";
                    return RedirectToAction("index", "dashboard");
                }

                string studentName = Session["studentName"].ToString();
                string stage = Session["stage"].ToString();
                string settlementType = Session["SettlementType"].ToString();
                string program = Session["program"].ToString();
                string nextStage = webportals.GetNextStage(program, stage);

                academics.StudentNo = username;
                academics.StudentName = studentName;
                academics.Stage = stage;
                academics.SettlementType = settlementType;
                academics.Program = program;
                academics.NextStage = nextStage;
                academics.ProgramName = webportals.GetProgramName(program);
            }
            catch(Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return View(academics);
        }

        [HttpPost]
        public ActionResult SemesterRegistration(Academics academics)
        {
            try
            {
                string username = Session["username"].ToString();
                string program = academics.Program;
                string stage = academics.Stage;
                string nextStage = academics.NextStage;
                string settlementType = academics.SettlementType;
                string semester = webportals.GetCurrentSemester();
                string acadYear = webportals.GetCurrentAcademicYear();
                webportals.PreRegisterStudent(username,program,nextStage,semester,acadYear,settlementType);
                TempData["Success"] = $"You have successfully registered for the semester {semester} and academic year {acadYear}";
                return RedirectToAction("unitregistration", "academics");
            }
            catch(Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("semesterregistration", "academics");
            }
        }

        public ActionResult UnitRegistration()
        {
            if (Session["username"] == null) return RedirectToAction("index", "login");
            Academics academics = new Academics();
            try
            {

            }
            catch(Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return View(academics);
        }
    }
}