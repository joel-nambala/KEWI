using System;
using System.Web.Mvc;

namespace KEWIStaff.Controllers
{
    public class ApiController : Controller
    {
        public JsonResult GetLecturerProgrammes()
        {
            string username = Session["username"].ToString();
            var item = ExamHelper.GetLecturerProgrammes(username);
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLecturerStages(string programme)
        {
            string username = Session["username"].ToString();
            var item = ExamHelper.GetLecturerStages(username, programme);
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLecturerTerms(string programme, string stage)
        {
            string username = Session["username"].ToString();
            var item = ExamHelper.GetLecturerSemesters(username, programme, stage);
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLecturerUnits(string programme, string stage, string term)
        {
            string username = Session["username"].ToString();
            var item = ExamHelper.GetLecturerUnits(username, programme, stage, term);
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCampus()
        {
            var item = ExamHelper.GetCampus();
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAcademicYears()
        {
            var item = ExamHelper.GetAcademicYears();
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSubmittedExamMarks(string studentNo, string unit, string semester, string stage, string program)
        {
            string username = Session["username"].ToString();
            var item = ExamHelper.GetExamScore(studentNo, unit, semester, username, stage, program);
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSubmittedFormativeExamMarks(string studentNo, string unit, string semester, string stage, string program, string learningOutcome,string assessmentCategory)
        {
            string username = Session["username"].ToString();
            var item = ExamHelper.GetFormativeExamScore(studentNo, unit, semester, username, stage, program,learningOutcome,Convert.ToInt32(assessmentCategory));
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLoPrograms()
        {
            string username = Session["username"].ToString();
            var item = ExamHelper.GetLoPrograms(username);
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLoStage(string program)
        {
            string username = Session["username"].ToString();
            var item = ExamHelper.GetLoStage(username,program);
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLoSemester(string program, string stage)
        {
            string username = Session["username"].ToString();
            var item = ExamHelper.GetLoSemester(username, program, stage);
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLoUnits(string program, string stage,string semester)
        {
            string username = Session["username"].ToString();
            var item = ExamHelper.GetLoUnits(username, program, stage,semester);
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLoLearningOutcomes(string program, string stage, string semester,string unit)
        {
            string username = Session["username"].ToString();
            var item = ExamHelper.GetLoLearningOutcomes(username, program, stage, semester,unit);
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAssessmentCategories(string unit)
        {
            var item = ExamHelper.GetAssessmentCategories(unit);
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetMaxScore(string unit, string assessmentCategory)
        {
            var item = ExamHelper.GetMaxScore(unit, Convert.ToInt32(assessmentCategory));
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAssignedScores(string studentNo, string unit, string stage, string term, string examType)
        {
            string item = Components.ObjNav.GetAssignedScores(studentNo, unit, stage, term, Convert.ToInt32(examType));
            return Json(item, JsonRequestBehavior.AllowGet);
        }
    }
}