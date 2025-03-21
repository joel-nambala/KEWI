using System;
using System.Web.Mvc;
using KEWIStaff.Models;
using KEWIStaff.NAVWS;

namespace KEWIStaff.Controllers
{
    public class ExaminationController : Controller
    {
        Staffportal webportals = Components.ObjNav;
        string[] strLimiters = new string[] { "::" };
        string[] strLimiters2 = new string[] { "[]" };
        public ActionResult SummativeAssessment()
        {
            if (Session["username"] == null) return RedirectToAction("index", "login");            
            return View();
        }

        [HttpPost]
        public ActionResult StudentMarks(Examination examination)
        {
            try
            {
                string program = examination.Programme;
                string stage = examination.Stage;
                string semester = examination.Semester;
                string unit = examination.Unit;
                string campus = examination.Campus;
                Session["program"] = program;
                Session["stage"] = stage;
                Session["semester"] = semester;
                Session["unit"] = unit;
                Session["campus"] = campus;
                return RedirectToAction("entermarks", "examination");
            }
            catch(Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("assignmarks", "examination");
            }
        }

        public ActionResult EnterMarks()
        {
            if (Session["username"] == null) return RedirectToAction("index", "login");
            Examination exam = new Examination();
            try
            {
                string program = Session["program"].ToString();
                string stage = Session["stage"].ToString();
                string semester= Session["semester"].ToString();
                string unit = Session["unit"].ToString();
                string campus =Session["campus"].ToString();

                ViewBag.markEntryDateline = true;
                exam.UnitDescription = webportals.GetUnitDescription(program,stage,semester,unit);
                exam.Semester = semester;
                exam.Stage = stage;
                exam.Unit = unit;
                exam.Programme = program;
                exam.MaxCatScore = "30";
                exam.MaxPracticalScore = "30";
                exam.MaxExamScore = webportals.GetFinalExamMaxScore(unit);
                exam.LockCatEditing = "No";
                exam.LockExamEditing = "No";
                var students = ExamHelper.GetLecturerStudents(semester, program, unit, stage);
                exam.LecturerStudents = students;
            }
            catch( Exception ex )
            {
                TempData["Error"] = ex.Message;
            }
            return View(exam);
        }

        [HttpPost]
        public ActionResult SubmitMarks(Examination exam)
        {
            try
            {     
                string examScores = exam.ExamMarks;
                string[] examScoresArr = examScores.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                int counter = 0;
                foreach(var examScore in  examScoresArr)
                {
                    string[] responseArr = examScore.Split(strLimiters, StringSplitOptions.None);
                    string studentNo = responseArr[0];
                    string unit = responseArr[1];
                    string score = responseArr[2];
                    string stage = responseArr[3];
                    string semester = responseArr[4];
                    string program = responseArr[5];
                    string lecturer = Session["username"].ToString();
                    if (!string.IsNullOrEmpty(score))
                    {
                        webportals.SubmitSummativeResults(studentNo, unit, Convert.ToInt32(score), program,lecturer,stage,semester);
                        counter++;
                    }
                }
                TempData["Success"]= $"You have successfully submitted the marks for {counter} students!";
            }
            catch(Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("entermarks", "examination");
        }

        public ActionResult FormativeAssessment()
        {
            if (Session["username"] == null) return RedirectToAction("index", "login");
            return View();
        }

        [HttpPost]
        public ActionResult SubmitFormativeMarks(Examination exam)
        {
            try
            {

            }
            catch(Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("formativeassessment", "examination");
        }
    }
}