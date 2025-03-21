using System;
using System.Web.Mvc;
using KEWIStaff.Models;
using KEWIStaff.NAVWS;

namespace KEWIStaff.Controllers
{
    public class LoginController : Controller
    {
        Staffportal webportals = Components.ObjNav;
        string[] strLimiters = new string[] { "::" };
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Account account)
        {
            try
            {
                string username = account.Username;
                string password = account.Password;
                webportals.CheckValidStaffNo(username);
                if (webportals.CheckStaffPasswordChanged(username))
                {
                    return RedirectToAction("loginforchangedpassword", "login", new { username, password });
                }
                else
                {
                    return RedirectToAction("loginforunchangedpassword", "login", new { username });
                }
            }
            catch(Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("index", "login");
            }
        }

        public ActionResult LoginForChangedPassword(string username, string password)
        {
            try
            {
                string response = webportals.LoginForChangedPassword(username, password);
                if(!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                    string staffNo = responseArr[0];
                    string staffName = responseArr[1];
                    Session["username"] = staffNo;
                    Session["staffName"] = staffName;
                    return RedirectToAction("index", "dashboard");
                }
            }
            catch( Exception ex )
            {
                TempData["Error"] = ex.Message;
            }
                return RedirectToAction("index", "login");
        }

        public ActionResult LoginForUnChangedPassword(string username)
        {
            try
            {
                string response = webportals.LoginForUnchangedPassword(username);
                if (!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                    string staffNo = responseArr[0];
                    string staffEmail = responseArr[1];
                    Session["staffNo"] = staffNo;
                    Session["staffEmail"] = staffEmail;
                    return RedirectToAction("resetpassword", "login");
                }
            }
            catch(Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("index", "login");
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(Account account)
        {
            try
            {
                string username = account.Username;
                webportals.CheckValidStaffNo(username);
                return RedirectToAction("loginforunchangedpassword", "login", new { username });
            }
            catch(Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("forgotpassword", "login");
            }
        }

        public ActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ResetPassword(Account account)
        {
            try
            {
                string username = Session["staffNo"].ToString();
                string email = Session["staffEmail"].ToString();
                string genpass = account.Password;
                if (Components.ValidPassword(genpass))
                {
                    webportals.UpdateStaffPassword(username, genpass);
                    TempData["Success"] = "Password has been updated successfully!";
                    return RedirectToAction("index", "login");
                }
                else
                {
                    TempData["Error"]= "Password must contain altleast one uppercase, one lowercase, one number and one special character!";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
                return RedirectToAction("resetpassword", "login");
        }
    }
}