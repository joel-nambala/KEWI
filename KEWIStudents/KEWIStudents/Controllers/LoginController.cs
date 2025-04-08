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
    public class LoginController : Controller
    {
        Studentportal webportals = Components.ObjNav;
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

                webportals.CheckValidStudentNo(username);

                if (webportals.CheckPasswordChanged(username))
                {
                    return RedirectToAction("loginforchangedpassword", "login", new { username, password });
                }
                else
                {
                    return RedirectToAction("loginforunchangedpassword", "login", new { username });
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("index", "login");
        }

        public ActionResult LoginForChangedPassword(string username, string password)
        {
            try
            {
                string response = webportals.LoginForChangedPassword(username, password);
                if (!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                    string studentNo = responseArr[0];
                    string studentName = responseArr[1];
                    Session["username"] = studentNo;
                    Session["studentName"] = studentName;
                    return RedirectToAction("index", "dashboard");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("index", "login");
        }

        public ActionResult LoginForUnchangedPassword(string username)
        {
            try
            {
                string response = webportals.LoginForUnchangedPassword(username);
                if (!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                    string studentNo = responseArr[0];
                    string studentEmail = responseArr[1];
                    return RedirectToAction("forgotpassword", "login", new { studentNo });
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("index", "login");
        }

        public ActionResult ForgotPassword(string studentNo)
        {
            ViewBag.StudentNo = studentNo ?? string.Empty;
            return View();
        }

        [HttpPost]
        public ActionResult ResetPassword(Account account)
        {
            string studentNo = account.Username;
            try
            {
                string password = account.Password;
                webportals.UpdateStudentPassword(studentNo, password);
                TempData["Success"] = "Password has been updated successfully!";
                return RedirectToAction("index", "login");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("forgotpassword", "login", new { studentNo });
            }
        }
    }
}