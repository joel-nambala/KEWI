using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KEWIVendor.Models;
using KEWIVendor.NAVWS;

namespace KEWIVendor.Controllers
{
    public class LoginController : Controller
    {
        Procurement webportals = Components.ObjNav;
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
                string vatNo = account.VATNo;
                string password = account.Password;

                if (!webportals.CheckValidVatNo(vatNo))
                {
                    TempData["Error"] = "Invalid VAT Registration No";
                    return RedirectToAction("index", "login");
                }

                if (webportals.CheckVendorPasswordChanged(vatNo))
                {
                    return RedirectToAction("loginforchangedpassword", "login", new { vatNo, password });
                }
                else
                {
                    return RedirectToAction("loginforunchangedpassword", "login", new { vatNo });
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("index", "login");
            }
        }

        public ActionResult LoginForChangedPassword(string vatNo, string password)
        {
            try
            {
                string response = webportals.LoginForChangedPassword(vatNo, password);
                if (!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                    string vendorNo = responseArr[0];
                    string vendorName = responseArr[1];
                    string email= responseArr[3];
                    Session["VendorNo"] = vendorNo;
                    Session["VATNo"] = vatNo;
                    Session["VendorName"] = vendorName;
                    Session["VendorEmail"] = email;
                    return RedirectToAction("index", "dashboard");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("index", "login");
            }
            return View();
        }

        public ActionResult LoginForUnchangedPassword(string vatNo)
        {
            try
            {
                string response = webportals.LoginForUnchangedPassword(vatNo);
                if (!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                    string vendorNo = responseArr[0];
                    Session["vendorNo"] = vendorNo;
                    return RedirectToAction("resetpassword", "login");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("index", "login");
            }
            return View();
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
                string vatNo = account.VATNo;
                return RedirectToAction("loginforunchangedpassword", "login", new { vatNo });
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
                string vendorNo = Session["vendorNo"].ToString();
                string genpass = account.NewPassword;
                webportals.UpdateVendorPassword(vendorNo, genpass);
                TempData["Success"] = "Password has been updated successfully!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("index", "login");
        }
    }
}