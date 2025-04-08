using System;
using System.Web.Mvc;
using System.Web.UI.WebControls;
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
            catch (Exception ex)
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
                string semester = Session["semester"].ToString();
                string unit = Session["unit"].ToString();
                string campus = Session["campus"].ToString();

                ViewBag.markEntryDateline = true;
                exam.UnitDescription = webportals.GetUnitDescription(program, stage, semester, unit);
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
            catch (Exception ex)
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
                foreach (var examScore in examScoresArr)
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
                        webportals.SubmitSummativeResults(studentNo, unit, Convert.ToInt32(score), program, lecturer, stage, semester);
                        counter++;
                    }
                }
                TempData["Success"] = $"You have successfully submitted the marks for {counter} students!";
            }
            catch (Exception ex)
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
                string program = exam.Programme;
                string stage = exam.Stage;
                string semester = exam.Semester;
                string unit = exam.Unit;
                string campus = exam.Campus;
                string learningOutcome = exam.LearningOutcome;
                string assessmentCategory = exam.AssessmentCategory;

                Session["program"] = program;
                Session["stage"] = stage;
                Session["semester"] = semester;
                Session["unit"] = unit;
                Session["campus"] = campus;
                Session["learningOutcome"] = learningOutcome;
                Session["assessmentCategory"] = assessmentCategory;
                return RedirectToAction("formativeassessmentmarks", "examination");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("formativeassessment", "examination");
        }

        public ActionResult FormativeAssessmentMarks()
        {
            if (Session["username"] == null) return RedirectToAction("index", "login");
            Examination exam = new Examination();
            try
            {
                string program = Session["program"].ToString();
                string stage = Session["stage"].ToString();
                string semester = Session["semester"].ToString();
                string unit = Session["unit"].ToString();
                string campus = Session["campus"].ToString();
                string learningOutcome = Session["learningOutcome"].ToString();
                string assessmentCategory = Session["assessmentCategory"].ToString();
                var students = ExamHelper.GetLoStudents(program, unit, learningOutcome);
                exam.LecturerStudents = students;
                exam.UnitDescription = webportals.GetUnitName(unit);
                exam.Stage = stage;
                exam.Semester = semester;
                exam.Unit = unit;
                exam.Programme = program;
                ViewBag.LearningOutcome = webportals.GetLearningOutcomeName(unit, learningOutcome);
                exam.LockCatEditing = "No";
                exam.LockExamEditing = "No";
                ViewBag.markEntryDateline = true;
                exam.AssessmentCategory = assessmentCategory;
                exam.LearningOutcome = learningOutcome;
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return View(exam);
        }

        [HttpPost]
        public ActionResult SubmitAssessmentMarks(Examination exam)
        {
            try
            {
                string results = exam.MarksCategory;
                string learningOutcome = exam.LearningOutcome;
                string[] resultsArr = results.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                int counter = 0;
                foreach (string s in resultsArr)
                {
                    string[] responseArr = s.Split(strLimiters, StringSplitOptions.None);
                    string studentNo = responseArr[0];
                    string program = responseArr[1];
                    string stage = responseArr[2];
                    string semester = responseArr[3];
                    string unit = responseArr[4];
                    int assessmentCategory = Convert.ToInt32(responseArr[5]);
                    string score = responseArr[6];
                    string lecturer = Session["username"].ToString();
                    if (!string.IsNullOrEmpty(score))
                    {
                        webportals.SubmitFormativeResults(studentNo, unit, Convert.ToDecimal(score), program, lecturer, stage, semester, learningOutcome, assessmentCategory);
                        counter++;
                    }
                }
                TempData["Success"] = $"You have successfully submitted marks for {counter} students!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("formativeassessmentmarks", "examination");
        }

        public ActionResult AssignMarks()
        {
            if (Session["username"] == null) return RedirectToAction("index", "login");
            return View();
        }

        [HttpPost]
        public ActionResult SubmitOldMarks(OldExam examination)
        {
            try
            {
                string program = examination.Programme;
                string stage = examination.Stage;
                string semester = examination.Semester;
                string unit = examination.Unit;
                string campus = examination.Campus;
                string academicYear = examination.AcademicYear;
                Session["program"] = program;
                Session["stage"] = stage;
                Session["semester"] = semester;
                Session["unit"] = unit;
                Session["campus"] = campus;
                Session["academicYear"] = academicYear;
                return RedirectToAction("enteroldmarks", "examination");
            }
            catch(Exception ex)
            {
                TempData["Error"] = ex.Message;
            return RedirectToAction("assignmarks", "examination");
            }
        }

        public ActionResult EnterOldMarks()
        {
            if (Session["username"] == null) return RedirectToAction("index", "login");
            OldExam exam =   new OldExam();
            try
            {
                string program = Session["program"].ToString();
                string stage = Session["stage"].ToString();
                string semester = Session["semester"].ToString();
                string unit = Session["unit"].ToString();
                string campus = Session["campus"].ToString();
                string programCategory = webportals.GetProgramCategory(program);
                string academicYear = Session["academicYear"].ToString();                

                string semesterDetails = webportals.GetSemesterDetails(semester, academicYear);
                if(!string.IsNullOrEmpty(programCategory))
                {
                    bool marksDateline = false;
                    string[] semesterDetailsArr = semesterDetails.Split(strLimiters, StringSplitOptions.None);
                    exam.LockCatEditing = semesterDetailsArr[0];
                    exam.LockExamEditing = semesterDetailsArr[1];
                    if (string.IsNullOrEmpty(semesterDetailsArr[2])) marksDateline = false;
                    if(!string.IsNullOrEmpty (semesterDetailsArr[2]))
                    {
                        if (DateTime.Now <= Convert.ToDateTime(semesterDetailsArr[2])) marksDateline = true;
                        else marksDateline = false;
                    }
                        ViewBag.markEntryDateline = marksDateline;
                }

                exam.UnitDescription = webportals.GetUnitName(unit);
                exam.Semester = semester;
                exam.Stage = stage;
                exam.Unit = unit;
                exam.Programme = program;
                exam.MaxCatScore = webportals.GetMaxScores(programCategory, 1);
                exam.MaxPracticalScore = webportals.GetMaxScores(programCategory, 6);
                exam.MaxExamScore = webportals.GetMaxScores(programCategory,2);
                var students = ExamHelper.GetLecturerStudentsOld(semester, program, unit, stage);
                exam.LecturerStudents = students;
            }
            catch(Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return View(exam);
        }

        [HttpPost]
        public ActionResult SubmitOldExamMarks(OldExam exam)
        {
            try
            {
                string username = Session["username"].ToString();
                string lectName = Session["staffName"].ToString();
                string programme = Session["programme"].ToString();
                string stage = Session["stage"].ToString();
                string term = Session["term"].ToString();
                string unit = Session["unit"].ToString();
                string campus = Session["campus"].ToString();
                string acadYear = webportals.GetAcademicYear();

                // CAT 1
                string assignmentCategory = exam.CatMarks;
                string[] assignmentCategoryArr = assignmentCategory.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in assignmentCategoryArr)
                {
                    string[] responseArr = item.Split(strLimiters, StringSplitOptions.None);
                    string studentNo = responseArr[0];
                    string transId = responseArr[1];
                    if (!string.IsNullOrEmpty(responseArr[2]))
                    {
                        decimal score = Convert.ToDecimal(responseArr[2]);
                        webportals.SubmitCATMarks(programme,stage,term,unit,studentNo,score,transId,acadYear,username,lectName);
                    }
                }

                // Practical
                string catCategory = exam.PracticalMarks;
                string[] catCategoryArr = catCategory.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in catCategoryArr)
                {
                    string[] responseArr = item.Split(strLimiters, StringSplitOptions.None);
                    string studentNo = responseArr[0];
                    string transId = responseArr[1];
                    if (!string.IsNullOrEmpty(responseArr[2]))
                    {
                        decimal score = Convert.ToDecimal(responseArr[2]);
                        webportals.SubmitPracticalmarks(programme, stage, term, unit, studentNo, score, transId, acadYear, username, lectName);
                    }
                }

                // Final Exam
                string examCategory = exam.ExamMarks;
                string[] examCategoryArr = examCategory.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in examCategoryArr)
                {
                    string[] responseArr = item.Split(strLimiters, StringSplitOptions.None);
                    string studentNo = responseArr[0];
                    string transId = responseArr[1];
                    if (!string.IsNullOrEmpty(responseArr[2]))
                    {
                        decimal score = Convert.ToDecimal(responseArr[2]);
                        webportals.SubmitExamMarks(programme, stage, term, unit, studentNo, score, transId, acadYear, username, lectName);
                    }
                }
                TempData["Success"] = "You have successfully submitted the marks";
                return RedirectToAction("enteroldmarks", "examination");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("enteroldmarks", "examination");
            }
        }
    }
}