using System;
using System.Web.Mvc;
using KEWIStaff.Models;
using KEWIStaff.NAVWS;

namespace KEWIStaff.Controllers
{
    public class DashboardController : Controller
    {
        Staffportal webportals = Components.ObjNav;
        string[] strLimiters = new string[] { "::" };
        public ActionResult Index()
        {
            if (Session["username"] == null) return RedirectToAction("index", "login");
            Account account = new Account();
            try
            {
                string username = Session["username"].ToString();
                string response = webportals.GetStaffDetails(username);
                if (!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                    account.StaffNo = responseArr[0];
                    account.StaffName = responseArr[1];
                    account.JobTitle = responseArr[2];
                    account.Gender = responseArr[3];
                    account.IdNumber = responseArr[4];
                    account.EmailAddress = responseArr[5];
                    account.PhoneNumber = responseArr[6]; account.PostalAddress = responseArr[7];
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return View(account);
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            Session.RemoveAll();
            Session.Clear();
            return RedirectToAction("index", "login");
        }
    }
}