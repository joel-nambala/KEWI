using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KEWIStudents.Controllers
{
    public class ApiController : Controller
    {
        public JsonResult GetFeeStatement()
        {
            string username = Session["username"].ToString();
            var item = FinanceHelper.GetStudentFeesStatement(username);
            return Json(item,JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPaymentReceipts()
        {
            string username = Session["username"].ToString();
            var item = FinanceHelper.GetPaymentReceipts(username);
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProgramStages()
        {
            string program = Session["program"].ToString();
            var item = AcademicsHelper.GetProgramStages(program);
            return Json(item, JsonRequestBehavior.AllowGet);
        }
    }
}