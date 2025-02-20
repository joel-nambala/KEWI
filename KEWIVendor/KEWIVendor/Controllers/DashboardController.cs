using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KEWIVendor.Models;
using KEWIVendor.NAVWS;

namespace KEWIVendor.Controllers
{
    public class DashboardController : Controller
    {
        Procurement webportals = Components.ObjNav;
        string[] strLimiters = new string[] { "::" };
        public ActionResult Index()
        {
            if (Session["VendorNo"] == null) return RedirectToAction("index", "login");
            Account account = new Account();
            try
            {
                string path = Server.MapPath("~/Downloads/");
                if(!Directory.Exists(path)) Directory.CreateDirectory(path);
                string vendorNo = Session["VendorNo"].ToString();
                string response = webportals.GetVendorDetails(vendorNo);
                if (!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                    account.VendorName = responseArr[0];
                    account.EmailAddress = responseArr[1];
                    account.PhoneNo=responseArr[2];
                    account.Address = responseArr[3];
                    account.VATNo = responseArr[4];
                    account.ContactPerson = responseArr[5];
                }
            }
            catch(Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return View(account);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();
            return RedirectToAction("index", "login");
        }
    }
}