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

        public JsonResult GetSubmittedExamMarks(string studentNo, string unit, string semester, string stage, string program)
        {
            string username = Session["username"].ToString();
            var item = ExamHelper.GetExamScore(studentNo, unit, semester, username, stage, program);
            return Json(item, JsonRequestBehavior.AllowGet);
        }
    }
}